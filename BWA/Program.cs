using BWA.APIInfrastructure.Attributes;
using BWA.APIInfrastructure.Extension;
using BWA.APIInfrastructure.Filters;
using BWA.APIInfrastructure.Middlewares;
using BWA.Utility;
using FluentValidation;
using FluentValidation.AspNetCore;

Utils.RootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.GetAppSettingSection(builder.Configuration, out var appSetting);

// Load languages
ContentLoader.LanguageLoader();

// Antiforgery
services.AddAntiforgery();

// Kestrel TLS
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12
                                    | System.Security.Authentication.SslProtocols.Tls13;
    });
});

services.AddControllers(options =>
{    
    options.Filters.Add(typeof(ValidateModelStateAttribute));
    options.Filters.Add(typeof(ActionFilter));
    options.EnableEndpointRouting = false;
});

services.AddValidatorsFromAssemblyContaining<Program>(includeInternalTypes: true);

services.AddFluentValidationAutoValidation(opt =>
{
    opt.ImplicitlyValidateChildProperties = true;
});

services.RegisterServices();
services.RegisterRepositories();
services.ConfigureAuthorization();
services.ConfigureDatabase(builder.Configuration);
services.ConfigureSwagger();
services.ConfigureCors(appSetting);
services.ConfigureJwtToken(appSetting);
services.AddAutoMapper(typeof(Program));
services.AddAuthorization();
services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("BWACors");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    string swaggerJsonBasePath = string.IsNullOrEmpty(c.RoutePrefix) ? "." : "..";
    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Blog Web System API V1");
});
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseCsp(csp =>
    {
        csp.Defaults.AllowSelf();
        csp.Styles.AllowSelf().AllowUnsafeInline();
        csp.Scripts.AllowSelf().AllowUnsafeInline();
        csp.Fonts.AllowSelf().AllowUnsafeInline();
        csp.Images.AllowSelf().AllowData();
        csp.Connect.AllowAny();
    });
}
else
{
    app.UseCsp(builder =>
    {
        builder.Defaults.AllowSelf();
        builder.Scripts.AllowSelf().Allow(appSetting.ClientAppUrl);
        builder.Styles.AllowSelf().Allow(appSetting.ClientAppUrl);
        builder.Fonts.AllowSelf().Allow(appSetting.ClientAppUrl);
        builder.Images.AllowSelf().Allow(appSetting.ClientAppUrl);
    });
}
app.Run();
