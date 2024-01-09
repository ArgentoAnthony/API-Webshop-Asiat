using API_Webshop_Asiat.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Text;
using Webshop_DAL.Interfaces;
using Webshop_DAL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<SqlConnection>(pc => new(builder.Configuration.GetConnectionString("default")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<TokenManager>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICommentaireEvaluationService, CommentaireEvaluationService>();
builder.Services.AddScoped<ICommandeService, CommandeService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", o => o.RequireRole("Admin"));
    options.AddPolicy("IsModo", o => o.RequireRole("Admin","Modo"));
    options.AddPolicy("IsVendeur", o => o.RequireRole("Admin","Vendeur"));
    options.AddPolicy("IsConnected", o => o.RequireAuthenticatedUser());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        options => options.TokenValidationParameters = new()
        {
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidIssuer = "webshop.com",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(TokenManager._secretKey)
                ),
            ValidateAudience = false,
        }
    );

var app = builder.Build();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
