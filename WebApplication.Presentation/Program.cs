using ClassLibrary.Domain.Services;
using ClassLibrary.DataAccess.Repositories;
using ClassLibrary.Domain.Interfaces;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepo>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string conn = config.GetConnectionString("Default");
    return new UserRepo(conn);
});
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ICookbookRepo>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string conn = config.GetConnectionString("Default");
    return new CookbookRepo(conn);
});
builder.Services.AddScoped<CookbookService>();
builder.Services.AddScoped<IRecipeRepo>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string conn = config.GetConnectionString("Default");
    return new RecipeRepo(conn);
});
builder.Services.AddScoped<RecipeService>();
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
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=User}/{action=Login}/{id?}")
  .WithStaticAssets();

app.Run();
