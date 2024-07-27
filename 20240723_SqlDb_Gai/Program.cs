using _20240723_SqlDb_Gai.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using _20240723_SqlDb_Gai.Filter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CarContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDbConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddSwaggerGen(configure =>
{
    configure.SwaggerDoc("v2", new OpenApiInfo { 
        Version = "v2",
        Title = "CarApi", 
        Description = "Api request to db cars",
        Contact = new OpenApiContact
        { 
            Name = "KostiantynPolishko",
            Email = "polxs_wp31@student.itstep.org",
            Url = new Uri("https://habr.com/ru/companies/simbirsoft/articles/707108/")
        },
        License = new OpenApiLicense
        {
            Name = "ApiITSTEP",
            Url = new Uri("https://mystat.itstep.org/")
        }
    });
    configure.SchemaFilter<SwaggerSkipPropertyFilter>();
    string basePath = AppContext.BaseDirectory;
    string xmlPath = Path.Combine(basePath, "CarApi.xml");
    configure.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2"); });
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
