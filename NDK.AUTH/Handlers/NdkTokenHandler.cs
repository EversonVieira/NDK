using NDK.Auth.Interfaces;
using NDK.Core.Models;
using NDK.PublicAuth.Interfaces;
using NDK.PublicAuth.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace NDK.Auth.Handlers
{
    public class NdkTokenHandler : INdkTokenHandler
    {
        private NdkTokenConfiguration _configuration;
        private IPublicNdkTokenHandler _publicNdkTokenHandler;
        public NdkTokenHandler(NdkTokenConfiguration configuration, IPublicNdkTokenHandler publicNdkTokenHandler)
        {
            _configuration = configuration;
            _publicNdkTokenHandler = publicNdkTokenHandler;
        }

        public NdkToken CreateToken(NdkTokenPayload payload)
        {
            if (payload is null)
            {
                throw new InvalidOperationException("Payload can't be null");
            }
            var date = DateTime.UtcNow;
            return new NdkToken
            {
                Header = new NdkTokenHeader
                {
                    TokenType = _configuration.TokenType,
                    TokenPublicKey = GetTokenPublicKey(payload),
                    ExpirationDate = date.AddMinutes(_configuration.ExpirationInMinutes)
                },
                Payload = payload,
                Signature = new NdkTokenSignature
                {
                    SignedAt = date,
                    SignedBy = _configuration.Signer,
                    TwoWaySignature = new Guid().ToString().Replace("-", ""),
                }
            };
        }

        public NdkResponse<bool> ValidateToken(string token)
        {

            NdkResponse<bool> response = new NdkResponse<bool>();

            NdkToken tokenData = RetrieveTokenByString(token);

            if (tokenData == null || tokenData.Header is null || tokenData.Payload is null || tokenData.Signature is null)
            {
                response.AddMessage(new NdkMessage
                {
                    Code = "NDKAUTH01",
                    Text = "UNATHOURIZED",
                    Type = NdkMessageType.VALIDATION
                });

                return response;
            }

            string tokenAsString = RetrieveTokenAsString(tokenData);

            response.Result = tokenAsString.Equals(token);

            if (!response.Result || tokenData.Header.ExpirationDate.AddMinutes(_configuration.ExpirationInMinutes) < DateTime.UtcNow)
            {
                response.AddMessage(new NdkMessage
                {
                    Code = "NDKAUTH01",
                    Text = "UNATHOURIZED",
                    Type = NdkMessageType.VALIDATION
                });
            }

            return response;
        }


        public NdkToken RetrieveTokenByString(string token)
        {
           return this._publicNdkTokenHandler.RetrieveTokenByString(token);
        }

        public string RetrieveTokenAsString(NdkToken token)
        {
            StringBuilder sb = new StringBuilder();

            string header = JsonSerializer.Serialize(token.Header);
            string body = JsonSerializer.Serialize(token.Payload);
            string signature = JsonSerializer.Serialize(token.Signature);

            sb.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(header)).Replace("=", "").Replace("+", "-").Replace(@"//", "_"));
            sb.Append('.');
            sb.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(body)).Replace("=", "").Replace("+", "-").Replace(@"//", "_"));
            sb.Append('.');
            sb.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(signature)).Replace("=", "").Replace("+", "-").Replace(@"//", "_"));
            sb.Append('.');
            sb.Append(RetrieveTokenTailString(token));


            return sb.ToString();
        }

        private string RetrieveTokenTailString(NdkToken token)
        {

            StringBuilder sb = new StringBuilder();

            string start = JsonSerializer.Serialize(token);

            using (HMACSHA512 sha = new HMACSHA512(GetTokenPrivateKey()))
            {
                byte[] p1 = sha.ComputeHash(Encoding.UTF8.GetBytes(start));

                foreach (byte b in p1)
                {
                    sb.Append(b.ToString("X2"));
                }

                byte[] p2 = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));

                foreach (byte b in p2)
                {
                    sb.Append(b.ToString("X2"));
                }

                byte[] p3 = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                sb.Clear();

                foreach (byte b in p3)
                {
                    sb.Append(b.ToString("X2"));
                }
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString())).Replace("=", "").Replace("+", "-").Replace(@"//", "_");
        }

        private string GetTokenPublicKey(NdkTokenPayload payload)
        {
            StringBuilder sb = new StringBuilder();

            string start = JsonSerializer.Serialize(payload);

            using (HMACMD5 md5 = new HMACMD5(Encoding.UTF8.GetBytes(_configuration.Gen1)))
            {

                byte[] p1 = md5.ComputeHash(Encoding.UTF8.GetBytes(start));

                foreach (byte b in p1)
                {
                    sb.Append(b.ToString("X2"));
                }

                byte[] p2 = md5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));

                foreach (byte b in p2)
                {
                    sb.Append(b.ToString("X2"));
                }

                byte[] p3 = md5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                sb.Clear();

                foreach (byte b in p3)
                {
                    sb.Append(b.ToString("X2"));
                }

            }

            return sb.ToString();
        }

        private byte[] GetTokenPrivateKey()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(_configuration.Gen2);
            sb.AppendLine("╔────╦-───┼");
            sb.AppendLine(_configuration.PrivateKey);


            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
