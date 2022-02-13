using System.Text.Json.Serialization;
using MicroserviceExample.Entities;
using MicroserviceExample.EntityFramework;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

static IEdmModel GetModel()
{
    ODataConventionModelBuilder builder = new ();
    builder.EntitySet<Article>("Articles");
    builder.EntitySet<Review>("Reviews");
    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDbContext<MicroserviceExampleContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddControllers()
    .AddOData(options => options.AddRouteComponents("v1", GetModel()).Select().OrderBy().Count().Filter().Expand())
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MicroserviceExample", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroserviceExample v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }