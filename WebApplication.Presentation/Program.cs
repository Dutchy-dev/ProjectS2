using ClassLibrary.Domain.Services;
using ClassLibrary.DataAccess.Repositories;
using ClassLibrary.Domain.Interfaces;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductRepo>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string conn = config.GetConnectionString("Default");
    return new ProductRepo(conn);
});
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IShoppingListRepo>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string conn = config.GetConnectionString("Default");
    return new ShoppingListRepo(conn);
});
builder.Services.AddScoped<ShoppingListService>();
builder.Services.AddScoped<IProductListRepo>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string conn = config.GetConnectionString("Default");
    return new ProductListRepo(conn);
});
builder.Services.AddScoped<ProductListService>();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}")
  .WithStaticAssets();

app.Run();
