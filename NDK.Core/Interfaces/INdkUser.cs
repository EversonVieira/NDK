namespace NDK.Core.Interfaces
{
    public interface INdkUser
    {
        string? Email { get; set; }
        string? FirstName { get; set; }
        string? LastName { get; set; }
    }
}