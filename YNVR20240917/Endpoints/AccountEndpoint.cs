using AutenticationJWTMinimalAPI;
using YNVR20240917.Auth;

namespace AutenticationJWTMinimalAPI.Endpoints
{
    public static class AcountEndpoint
    {
        public static void AddAcountEndpoints(this WebApplication app)
        {
            app.MapPost("/account/loggin", (string login, string password, IJwtAuthenticationService authService) =>
            {
                if (login == "admin" && password == "12345")
                {

                    var token = authService.Authenticate(login);

                    return Results.Ok(token);
                }

                else
                {
                    return Results.Unauthorized();
                }

            });
        }
    }
}
