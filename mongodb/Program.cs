using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using mongodb.Models;
using mongodb.Repository.Implementation;
using mongodb.Repository.Interface;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
/*builder.Services.AddSwaggerGen();*/
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder
  .Services
  .Configure<SchoolDatabaseSettings>(
    builder.Configuration.GetSection("SchoolDatabaseSettings")
  );
builder.Services.AddSingleton<IMongoClient>(_ => {
    var connectionString =
        builder
            .Configuration
            .GetSection("SchoolDatabaseSettings:ConnectionString")?
            .Value;

    return new MongoClient(connectionString);
});

//get database directly by Imongodatabse
/*builder.Services.AddScoped<IMongoDatabase>(resolver =>
{
    var options = resolver.GetRequiredService<IOptions<SchoolDatabaseSettings>>();
    var client = resolver.GetRequiredService<IMongoClient>();
    return client.GetDatabase(options.Value.DatabaseName);
});*/

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

    });


builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
//Adding contacts and interfaces services 
builder.Services.AddScoped<IRole, IRoleImplementation>();
builder.Services.AddScoped<IStudent, IStudentImplementation>();
builder.Services.AddScoped<ILogin,ILoginImplementation>();
builder.Services.AddTransient<IEmail, EmailImplementation>();
var app = builder.Build();

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

