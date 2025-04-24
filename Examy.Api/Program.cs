using System.Text;
using Examy.Api.Data;
using Examy.Api.Data.Entities;
using Examy.Api.Endpoints;
using Examy.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddDbContext<QuizContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Quiz");
    options.UseSqlServer(connectionString);
}, optionsLifetime:ServiceLifetime.Singleton);

builder.Services.AddDbContextFactory<QuizContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Quiz");
    options.UseSqlServer(connectionString);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var secretKey = builder.Configuration.GetValue<string>("Jwt:Secret");
    var symmetriKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
        IssuerSigningKey = symmetriKey
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p =>
    {
        var allowedOriginsStr = builder.Configuration.GetValue<string>("AllowedOrigins");
        var allowedOrigins = allowedOriginsStr.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        p.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Ensure credentials are allowed if needed
    });
});

builder.Services.AddTransient<AuthService>()
    .AddTransient<CategoryService>()
    .AddTransient<QuizService>()
    .AddTransient<AdminService>()
    .AddTransient<StudentQuizService>();

//builder.Services.AddSingleton<Func<QuizContext>>(sp => () =>
//{
//    var scope = sp.CreateScope();
//    return scope.ServiceProvider.GetRequiredService<QuizContext>();
//}

//);


builder.Services.AddAuthorization();
var app = builder.Build();

#if DEBUG
ApplyDbMigrations(app.Services);
#endif
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication()
    .UseAuthorization();

app.MapAuthEndpoints()
    .MapCatgoryEndpoints()
    .MapQuizEndpoints()
    .MapAdminEndpoints()
    .MapStudentQuizEndpoints();

app.Run();

static void ApplyDbMigrations(IServiceProvider sp)
{
    var scope = sp.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<QuizContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}
