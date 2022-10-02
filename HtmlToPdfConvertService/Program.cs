using HtmlToPdfConvertService.Models;
using HtmlToPdfConvertService.Services.Implementations;
using HtmlToPdfConvertService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<FilesDatabaseSettings>(
    builder.Configuration.GetSection("PdfFilesDatabase"));
builder.Services.AddTransient<IFileConverter, HtmlToPdfConverter>();
builder.Services.AddSingleton<IFileProvider, MongoFileProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ConvertFile/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ConvertFile}/{action=Index}/{id?}");

app.Run();
