using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Scripts")),
//RequestPath = "/Scripts"
//});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
