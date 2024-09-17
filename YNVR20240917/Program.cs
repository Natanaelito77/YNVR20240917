using AutenticationJWTMinimalAPI.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using YNVR20240917.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// Configurar Swagger para documentar la API
builder.Services.AddSwaggerGen(c =>
{
    // Definir informacion basica de la API en Swagger
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    // cConfigurar un esquema de seguridad para JWT en Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresar tu token JWT Authentication",

        // Hacer referencia al esquema de seguridad JWT definido anteriormente
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    // Agresar un requesito de seguridad para JWT en Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoggedInPolicy", policy =>
    {
        // Requerir que el usuario este autenticado para acceder a recursos protegidos
        policy.RequireAuthenticatedUser();
    });
});

// Definir la clave secreta utilizada para firmar y verificar tokens JWT
// Esta clave se puede modificar lo ideal seria una clave diferente
// para cada proyecto
var key = "Key.JWTMinimal2024.API";

// Configurar la autentication con JWT
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Configurar la autenticationJWT utilizando el esquema JWTBearer
.AddJwtBearer(x =>
{
    // Indicar si se requiere metadata HTTPS al validar el token
    // En un entorno de desarrollo, generalmente se establece en false
    x.RequireHttpsMetadata = false;

    // Indicar que se debe guardar el token JWT recibido del cliente
    x.SaveToken = true;

    // configurar los parametros de validacion del token JWT
    x.TokenValidationParameters = new TokenValidationParameters
    {
        // Establecer la clave secreta utilizada para firmar y verificar el token
        // en este caso, la clave es una cadena codificada en ASCII
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),

        // Indicar si se debe validar el audience(audiencia) del token
        // en este caso, se establece en false, lo que significa que no es valida la audiencia
        ValidateAudience = false,

        // Indicar si se debe validar la firma del token utilizado la clave espcificada
        // En este caso, se establece en true para validar la firma
        ValidateIssuerSigningKey = true,

        // Indicar si se debe validar el issuer (emisor) del token
        // en este caso, se establece en false, lo que significa que no es valida el emisor
        ValidateIssuer = false
    };
});

// Agregar una instancia unica del servicio de autenticacion JWT
// al inyector para poder obtenerla al momento de generar el token
builder.Services.AddSingleton<IJwtAuthenticationService>(new JwtAuthenticationService(key));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Agregar los endpoints ded la aplicacion
app.AddAcountEndpoints();
app.AddBodegaEndpoint();
app.AddCategoriaProductoEndpoint();   

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();