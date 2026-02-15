using System.Reflection;
using System.Text;
using BWA.APIInfrastructure.Filters;
using BWA.APIInfrastructure.Headers;
using BWA.APIInfrastructure.Middlewares;
using BWA.Database.Contexts;
using BWA.Database.Infrastructure;
using BWA.Database.Interfaces;
using BWA.Database.Repositories;
using BWA.Services.interfaces;
using BWA.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;

namespace BWA.APIInfrastructure.Extension
{
    public static class IServiceCollections
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IRepository<>), typeof(Repository<>))
                .AddTransient(typeof(IUserRepository), typeof(UserRepository))
                .AddTransient(typeof(IBlogRepository), typeof(BlogRepository))
                .AddTransient(typeof(ICategoryRepository), typeof(CategoryRepository))
                .AddTransient(typeof(IRoleRepository), typeof(RoleRepository))
                .AddTransient(typeof(ICommentsRepository), typeof(CommentsRepository))
                .AddTransient(typeof(ICountryRepository), typeof(CountryRepository))
                .AddTransient(typeof(IConnectionRepository), typeof(ConnectionRepository))
                .AddTransient(typeof(IConnectionHistoryRepository), typeof(ConnectionHistoryRepository))
                ;
        }
        public static void RegisterServices(this IServiceCollection services)
        {

            //services
            //    .AddTransient(typeof(IUserService), typeof(UserService))
            //    .AddTransient(typeof(IBlogService), typeof(BlogService))
            //.AddTransient(typeof(IRoleService), typeof(RoleService))
            //.AddTransient(typeof(ICOmme), typeof(CommentsService)
            //;

            var assembliesToScan = new[]
            {
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(IBaseService))
            };

            // Register UnitOfWork
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register all services
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();
        }
        public static void ConfigureDatabase(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<BWAContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("connection")));
        }
        public static void GetAppSettingSection(this IServiceCollection service, IConfiguration configuration, out AppSettings appSetting)
        {
            var appSetingSection = configuration.GetSection("appSettings");
            service.Configure<AppSettings>(appSetingSection);
            appSetting = appSetingSection.Get<AppSettings>();
        }
        public static void ConfigureCors(this IServiceCollection services, AppSettings appSettings)
        {
            var urls = appSettings.ClientAppUrl.Split(",");
            services.AddCors(options =>
            {
                options.AddPolicy("BWACors", b =>
                {
                    b.WithOrigins(urls)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }
        public static IApplicationBuilder UseCsp(this IApplicationBuilder app, Action<CspOptionsBuilder> builder)
        {
            var newBuilder = new CspOptionsBuilder();
            builder(newBuilder);

            var options = newBuilder.Build();
            return app.UseMiddleware<CSPMiddleware>(options);
        }
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new BWAAuthorizationRequirement());
                });
            });
            services.AddScoped<IAuthorizationHandler, BWAAuthorizationHandler>();
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blog Web Application API",
                    Version = "v1"
                });

                // JWT Security Definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter JWT token in this format: Bearer {your token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,     // 👈 IMPORTANT
                    Scheme = "Bearer",                  // 👈 IMPORTANT
                    BearerFormat = "JWT"
                });

                // Enable Authorization using JWT
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",          // 👈 correct scheme
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                // optional custom header filter
                c.OperationFilter<SwaggerHeader>();
            });
        }
        public static void ConfigureJwtToken(this IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

    }
}
