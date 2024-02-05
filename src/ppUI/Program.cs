
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelApi;
 
var builder = WebApplication.CreateBuilder(args);
var jwtIssuer = builder.Configuration.GetSection("Token:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Token:SecurityKey").Get<string>();
var audience = builder.Configuration.GetSection("Token:Audience").Get<string>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });




// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCustomDependenciesConfigure(builder.Configuration); // Burada configuration'ý geçiyoruz

builder.Services.AddCustomValdiatorConfigure();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/Account/Login"; // Set your login path
           options.AccessDeniedPath = "/Account/AccessDenied"; // Set your access denied path
       });

//builder.Services.AddAuthorization();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.WithOrigins("https://localhost:7151/", "http://localhost:4200", "https://localhost:4200", "http://localhost:50139").AllowAnyHeader().AllowAnyMethod()
));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseMiddleware<AuthenticationMiddleware>();

app.UseStaticFiles();
app.UseCors();



app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
