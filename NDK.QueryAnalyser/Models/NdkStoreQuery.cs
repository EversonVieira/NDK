using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NDK.QueryAnalyser.Core.Models
{
    public class NdkStoreQuery
    {
        public long Id { get; set; }
        public string? QueryMD5 { get; set; }
        public string? TriggeredBy { get; set; }
        public string? FullQuery { get; set; }
        public string? BaseQuery { get; set; }
        public string? RequestJson { get; set; }
        public DateTime RunnedAt { get; set; }
        public DateTime RunnedUntil { get; set; }

        public void SetQueryMd5(string sourceQuery)
        {
            using (MD5 md5 = MD5.Create())
            {
                StringBuilder sb = new StringBuilder();
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(sourceQuery));

                foreach (var byt in bytes)
                {
                    sb.Append(byt.ToString());
                }

                QueryMD5 = sb.ToString();
            }
        }
    }
}
