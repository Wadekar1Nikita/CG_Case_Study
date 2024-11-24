using System.Text;
using adminrepository;
using ContextFile;
using LatestUpdate.Repositories;
using LatestUpdate.Repository;
using LatestUpdate.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(
options=>options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ICropRepository, CropRepository>();

builder.Services.AddScoped<ISoldHistoryRepository, SoldHistoryRepository>();

builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();

builder.Services.AddScoped<IClaimInsuranceRepository, ClaimInsuranceRepository>();

builder.Services.AddScoped<IBidderRepository, BidderRepository>();

builder.Services.AddScoped<IFarmerRepository, FarmerRepository>();

builder.Services.AddScoped<IBiddingRepository,BiddingRepository>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();




builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Farm Scheme System",
        Version = "v1",
        Description = "API documentation for Farm Scheme System"
    });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please Enter Token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
 
    });
});


builder.Services.AddAuthentication(options=>
{
    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; 
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"], 

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    ValidateLifetime = true, 
                    ClockSkew = TimeSpan.Zero, 

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])) 
                };
            });


//builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
 //       builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

