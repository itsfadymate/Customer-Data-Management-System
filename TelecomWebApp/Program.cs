using Microsoft.EntityFrameworkCore;
using TelecomWebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the database context with dependency injection
builder.Services.AddDbContext<TelecomContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

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
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Dashboard}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Dashboard}/{id}",
    defaults: new {controller = "Admin"});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=GenericCustomer}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=CustomerProfilesWithActiveAccounts}/{id?}");

app.MapControllerRoute(
        name: "physicalStoreVoucherDetails",
        pattern: "{controller=Admin}/{action=PhysicalStoreVoucherDetails}/{id?}");

app.MapControllerRoute(
        name: "resolvedTickets",
        pattern: "{controller=Admin}/{action=ResolvedTickets}/{id?}");



app.MapControllerRoute(
    name:"name",
    pattern: "{controller=Admin}/{action=E_shopVouchers}/{id?}");

app.MapControllerRoute(
        name: "customerAccountWithPlanDetails",
        pattern: "{controller=Admin}/{action=CustomerAccountWithPlanDetails}/{id?}");

app.MapControllerRoute(
        name: "customerAccountsByPlanDate",
        pattern: "{controller=Admin}/{action=CustomerAccountsByPlanDate}/{id?}");

app.MapControllerRoute(
    name:"cashbacksnum",
    pattern:"{controller=Admin}/{action=Num_of_cashbacks}/{id?}");

app.MapControllerRoute(
       name: "accountUsagePlan",
       pattern: "{controller=Admin}/{action=AccountUsagePlan}/{id?}");

app.MapControllerRoute(
            name: "removeBenefits",
            pattern: "{controller=Admin}/{action=RemoveBenefits}/{id?}");

app.Run();
