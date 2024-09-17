namespace AutenticationJWTMinimalAPI.Endpoints;

public static class BodegaEndpoint
{
    static List<object> data = new List<object>();

    public static void AddBodegaEndpoint(this WebApplication app)
    {
        app.MapGet("/protected", () =>
        {
            return data;
        }).RequireAuthorization();

        app.MapPost("protected/", (string name, string lastname) =>
        {
            data.Add(new { name, lastname });

            return Results.Ok();
        }).RequireAuthorization();
    }
}

