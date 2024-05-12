using System.Text;
using Medic.DataAccess.Repositories;
using Medic.DataAccess.UnitOfWorks;
using Medic.Service.Interfaces;
using Medic.Service.Mappers;
using Medic.Service.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Medic.WebApi.Extensions;

public static class ServiceCollections
{
    public static void AddServices(this IServiceCollection services)
    { 
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IHospitalService, HospitalService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMailSender, MailSender>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
    }
    
    
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Medic API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }
}
