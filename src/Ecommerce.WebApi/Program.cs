using Ecommerce.Infrastructure;
using Ecommerce.Application;
using Ecommerce.WebApi.Extensions;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    // Adding Serilog
    builder.Host.UseSerilog((host, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(host.Configuration));

    // Add services to the container.
    builder.Services
           .AddPresentation(builder.Environment)
           .AddInfrastructure(builder.Environment)
           .AddApplication();

    builder.Configuration.AddUserSecrets<Program>(optional:false,reloadOnChange:true);
}


WebApplication app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            if (app.Environment.IsDevelopment())
            {
                // Persist the token even if the page is reloaded or the project itself
                options.EnablePersistAuthorization();
            }            
        });
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseExceptionHandler();

    app.UseCors("default");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
