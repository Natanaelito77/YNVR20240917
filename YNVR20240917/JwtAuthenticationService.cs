using YNVR20240917.Auth;

internal class JwtAuthenticationService : IJwtAuthenticationService
{
    public JwtAuthenticationService(string key)
    {
    }

    string IJwtAuthenticationService.Authenticate(string username)
    {
        throw new NotImplementedException();
    }
}