namespace YNVR20240917.Auth
{
    public interface IJwtAuthenticationService
    {
        string Authenticate(string username);
    }
}
