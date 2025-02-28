using Ecommerce.Infrastructure;
using Ecommerce.Application;
using Ecommerce.WebApi.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
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
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseExceptionHandler();

    app.UseCors("default");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
