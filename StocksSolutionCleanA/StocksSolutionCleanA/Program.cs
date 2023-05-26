using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Repositories;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using Services.FinnhubService;
using Services.StocksService;
using Stocks.Core.Domain.IdentityEntities;
using Stocks.Core.Domain.RepositoryContracts;
using StocksApp;
using StocksApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
// Supply an object of TradingOptions with TradingOptions section as a service
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

// Add services into IoC Container
builder.Services.AddTransient<IStocksSellOrderService, StocksSellOrderService>();
builder.Services.AddTransient<IStocksBuyOrderService, StocksBuyOrderService>();
builder.Services.AddTransient<IFinnhubStocksService, FinnhubStocksService>();
builder.Services.AddTransient<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
builder.Services.AddTransient<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
builder.Services.AddTransient<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();

builder.Services.AddTransient<IStocksRepository, StocksRepository>();
builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();

//By default is designed as scoped service 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Enables Identity in this project
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders()
  /*Configuring the repository layer that interacts with the DbContext to manipulate the user's data and	 user's role data*/
  .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
  //Guid is the data type of Id property of the role
  .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

builder.Services.AddAuthorization(options =>
{
    //enforces authorization policy (user must be authenticated) for all the action methods
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});
builder.Services.ConfigureApplicationCookie(options =>
{
    //if the user is not logged in automatically it has to redirect to this URL
    options.LoginPath = "/Account/Login";
});

builder.Services.AddLocalization();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();// custom middleware

var app = builder.Build();



if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseMiddleware<ExceptionHandlingMiddleware>();
}
app.UseRequestLocalization(opciones =>
{
    opciones.DefaultRequestCulture = new RequestCulture("es-PE");
});


Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();

//Identifying action method-based route
app.UseRouting();
//Reading Identity cookie
app.UseAuthentication();
//Validates access permissions of the user
app.UseAuthorization();
//Execute the filter pipeline  (action + filters)
app.MapControllers();
app.Run();
