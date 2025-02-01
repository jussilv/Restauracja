using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restauracja.Data;
using Restauracja.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=restauracja.db"));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddRazorPages();

var app = builder.Build();

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    SeedData(context);
}

app.MapRazorPages();
app.Run();


void SeedData(ApplicationDbContext context)
{
    if (!context.MenuItems.Any())
    {
        var menuItems = new List<MenuItem>
        {
            
            new MenuItem { Name = "Burger Classic", Description = "Wo³owina, sa³ata, pomidor, sos BBQ", Price = 19.99m, IsAvailable = true },
            new MenuItem { Name = "Burger Cheese", Description = "Wo³owina, cheddar, ogórek, sos majonezowy", Price = 21.99m, IsAvailable = true },
            new MenuItem { Name = "Burger Double", Description = "Podwójna wo³owina, bekon, cheddar", Price = 24.99m, IsAvailable = true },
            new MenuItem { Name = "Pizza Margherita", Description = "Sos pomidorowy, mozzarella, bazylia", Price = 29.99m, IsAvailable = true },
            new MenuItem { Name = "Pizza Pepperoni", Description = "Mozzarella, pepperoni, sos pomidorowy", Price = 32.99m, IsAvailable = true },
            new MenuItem { Name = "Pizza Wiejska", Description = "Boczek, cebula, ser, sos czosnkowy", Price = 34.99m, IsAvailable = true },
            new MenuItem { Name = "Spaghetti Bolognese", Description = "Makaron, miêso wo³owe, sos pomidorowy", Price = 27.99m, IsAvailable = true },
            new MenuItem { Name = "Carbonara", Description = "Makaron, boczek, jajko, ser pecorino", Price = 26.99m, IsAvailable = true },
            new MenuItem { Name = "Lasagne", Description = "Makaron, sos miêsny, beszamel", Price = 28.99m, IsAvailable = true },

           
            new MenuItem { Name = "Frytki", Description = "Porcja chrupi¹cych frytek", Price = 9.99m, IsAvailable = true },
            new MenuItem { Name = "Onion Rings", Description = "Cebulowe kr¹¿ki w panierce", Price = 11.99m, IsAvailable = true },
            new MenuItem { Name = "Nachos z serem", Description = "Nachos zapiekane z serem", Price = 13.99m, IsAvailable = true },
            new MenuItem { Name = "Mozzarella Sticks", Description = "Paluszki serowe z sosem pomidorowym", Price = 14.99m, IsAvailable = true },
            new MenuItem { Name = "Krewetki w tempurze", Description = "Chrupi¹ce krewetki w cieœcie", Price = 18.99m, IsAvailable = true },

            new MenuItem { Name = "Sa³atka Cezar", Description = "Kurczak, sa³ata, parmezan, grzanki", Price = 21.99m, IsAvailable = true },
            new MenuItem { Name = "Sa³atka Grecka", Description = "Feta, ogórek, oliwki, pomidory", Price = 20.99m, IsAvailable = true },
            new MenuItem { Name = "Sa³atka z krewetkami", Description = "Krewetki, rukola, mango, orzechy", Price = 24.99m, IsAvailable = true },

            new MenuItem { Name = "Cola", Description = "Napój gazowany 0,5L", Price = 6.99m, IsAvailable = true },
            new MenuItem { Name = "Fanta", Description = "Napój gazowany 0,5L", Price = 6.99m, IsAvailable = true },
            new MenuItem { Name = "Sprite", Description = "Napój gazowany 0,5L", Price = 6.99m, IsAvailable = true },
            new MenuItem { Name = "Sok pomarañczowy", Description = "Naturalny sok 0,5L", Price = 7.99m, IsAvailable = true },
            new MenuItem { Name = "Sok jab³kowy", Description = "Naturalny sok 0,5L", Price = 7.99m, IsAvailable = true },
            new MenuItem { Name = "Woda niegazowana", Description = "Butelka 0,5L", Price = 5.99m, IsAvailable = true },
            new MenuItem { Name = "Woda gazowana", Description = "Butelka 0,5L", Price = 5.99m, IsAvailable = true },
            new MenuItem { Name = "Kawa Americano", Description = "Klasyczna kawa czarna", Price = 8.99m, IsAvailable = true },
            new MenuItem { Name = "Latte", Description = "Espresso z mlekiem", Price = 10.99m, IsAvailable = true },
            new MenuItem { Name = "Herbata", Description = "Herbata czarna lub zielona", Price = 7.99m, IsAvailable = true },
            new MenuItem { Name = "Mojito (bezalkoholowe)", Description = "Limonka, miêta, cukier, gazowana woda", Price = 9.99m, IsAvailable = true }
        };

        context.MenuItems.AddRange(menuItems);
        context.SaveChanges();
    }
}
