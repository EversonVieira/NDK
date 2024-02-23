namespace NDK.Core.Interfaces
{
    public interface INDKUser
    {
        string? Email { get; set; }
        string? FirstName { get; set; }
        string? LastName { get; set; }
        string? UserName { get; set; }
        string? Password { get; set; }
    }
}