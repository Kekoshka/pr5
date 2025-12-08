using Microsoft.Extensions.Hosting;
using pr5.Common.Extensions;
using pr5.Interfaces;
using System.CommandLine;




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();


var app = builder.Build();
app.UseHttpsRedirection();
app.UseExceptionHandling();
app.UseAuthorization();
app.MapControllers();
app.Run();


Option
var root = new RootCommand("CLI");
root.Add(settings);
root.Add(discon);
root.Add(addblack);
