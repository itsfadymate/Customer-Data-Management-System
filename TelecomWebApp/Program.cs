using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using TelecomWebApp;
Debug.WriteLine("entry point");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the database context with dependency injection
builder.Services.AddDbContext<TelecomContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Security settings
    options.Cookie.IsEssential = true; // Required for GDPR compliance
});

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
app.UseSession();
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
    name: "profile",
    pattern: "{controller=GenericCustomer}/{action=RedirectToLogin}/{id?}");
app.MapControllerRoute(
    name: "ViewLast5MonthsServicePlans",
    pattern: "{controller=AccountController}/{action=ViewLast5MonthsServicePlans}/{id?}");

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
app.MapControllerRoute(
    name: "getSMSOffers",
    pattern: "{controller=Admin}/{action=GetSMSOffers}/{id?}");
app.MapControllerRoute(
    name:"cashbackFunction",
    pattern:"{controller=Admin}/{action=CashbackFunction}/{id?}");
app.MapControllerRoute(
    name:"averageFunction",
    pattern: "{controller=Admin}/{action=TransactionAverage}/{id?}");
app.MapControllerRoute(
    name: "mobileNoLinked",
    pattern: "{controller=Admin}/{action=IsWalletLinked}/{id?}");
app.MapControllerRoute(
    name:"accountPaymentPoints",
    pattern:"{controller=Admin}/{action=AccountPaymentPoints}/{id?}");
app.MapControllerRoute(
    name:"updatePoints",
    pattern: "{controller=Admin}/{action=UpdateAccountPoints}/{id?}");

app.Run();
