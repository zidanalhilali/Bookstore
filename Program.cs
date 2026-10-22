using Bookstore.Models;
using Bookstore.Models.Repositories;
using Bookstore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBookstoreRepository<Author>, AuthorDbRepository>();
builder.Services.AddScoped<IBookstoreRepository<Book>, BookDbRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddDbContext<BookstoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookstoreDb")));

var app = builder.Build();

// Middleware pipeline (الترتيب مهم)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

// في بيئة التطوير فقط: طبّق الـ Migrations وأضف بيانات تجريبية
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<BookstoreDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.Run();