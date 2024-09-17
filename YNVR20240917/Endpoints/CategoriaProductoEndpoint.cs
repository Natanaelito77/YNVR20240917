namespace AutenticationJWTMinimalAPI.Endpoints
{
    public static class CategoriaProductoEndpoint
    {
        static List<object> data = new List<object>();

        public static void AddCategoriaProductoEndpoint(this WebApplication app)
        {
            app.MapGet("/test", () =>
            {
                return data;
            }).AllowAnonymous();

            app.MapPost("/test", (string name, string lastname) =>
            {
                data.Add(new { name, lastname });

                return Results.Ok();
            }).AllowAnonymous();

            app.MapDelete("/test", ()=>
            {
                data = new List<object>();

                return Results.Ok(); 
            }).RequireAuthorization();
        }
    }

}
