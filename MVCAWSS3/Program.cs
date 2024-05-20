using Amazon.S3;
using MVCAWSS3.Helpers;
using MVCAWSS3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddSingleton<PathProvider>();
builder.Services.AddSingleton<UploadService>();
builder.Services.AddTransient<AWSS3BucketHelper>();
builder.Services.AddTransient<AWSS3Service>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
