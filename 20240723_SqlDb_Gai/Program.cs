using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using _20240723_SqlDb_Gai.Filter;
using _20240723_SqlDb_Gai.Models;
using _20240723_SqlDb_Gai.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CarContext>(configure => configure.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDbConnection")));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(configure =>
{
    //Add a custom operation filter which sets default values
    configure.OperationFilter<SwaggerDefaultValues>();

    //Connected attribute [SwaggerIgnore] to hide requested fields from SwaggerUI
    configure.SchemaFilter<SwaggerSkipPropertyFilter>();

    //Connected service of display comments in SwaggerUI. Comments done in XML format inside code.
    string basePath = AppContext.BaseDirectory;
    string xmlPath = Path.Combine(basePath, "CarApi.xml");
    configure.IncludeXmlComments(xmlPath);
});

//Add services of versioning in Swagger
builder.Services.AddApiVersioning(configure =>
{
    configure.DefaultApiVersion = new ApiVersion(1, 0);
    configure.AssumeDefaultVersionWhenUnspecified = true;
    configure.ReportApiVersions = true;
    configure.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddApiVersioning().AddApiExplorer(configure =>
{
    configure.GroupNameFormat = "'v'VVV";
    configure.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(configure => { 
        configure.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

        var descriptions = app.DescribeApiVersions();

        //Build a swagger endpoint for each dicovered API version
        foreach(var desctiption in descriptions)
        {
            var url = $"/swagger/{desctiption.GroupName}/swagger.json";
            var name = desctiption.GroupName.ToUpperInvariant();
            configure.SwaggerEndpoint(url, name);
        }
    });

    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "/{controller=Car}/{action=Get}/{Number?}"
            );
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();