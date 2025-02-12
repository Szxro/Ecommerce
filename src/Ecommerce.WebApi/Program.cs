using Ecommerce.Infrastructure;
using Ecommerce.Application;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{

    // Add services to the container.
    builder.Services
           .AddInfrastructure(builder.Environment)
           .AddApplication();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
