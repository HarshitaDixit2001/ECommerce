using AppModels;
using AppServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// Services
// =====================================================

builder.Services.AddScoped<DbConnection>(provider =>
    new SqlConnection(builder.Configuration.GetConnectionString("DbCon"))
);

builder.Services.AddScoped<IDapperContext, DapperContext>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IAddServices, AddServices>();
builder.Services.AddScoped<IGetServices, GetServices>();

// ─────────────────────────────────────────────────────
// 🔐 JWT Authentication Setup
// ─────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/home/login";
    options.AccessDeniedPath = "/home/accessdenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Important for HTTP
    options.Cookie.SameSite = SameSiteMode.Lax; // Can also be None if cross-site needed
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Must be false for HTTP
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});



builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization();

var app = builder.Build();

// =====================================================
// Error Handling
// =====================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// =====================================================
// Middleware
// =====================================================

app.UseHttpsRedirection();

app.UseStaticFiles();

// User Images Folder
var imagePath = builder.Configuration["FilePaths:UserImages"];

if (!string.IsNullOrEmpty(imagePath))
{
    if (!Directory.Exists(imagePath))
    {
        Directory.CreateDirectory(imagePath);
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(imagePath)
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// =====================================================
// Routes
// =====================================================

// Sitemap Route
app.MapControllerRoute(
    name: "sitemap",
    pattern: "sitemap.xml",
    defaults: new { controller = "Sitemap", action = "Index" }
);

// Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Attribute Routing
app.MapControllers();

app.Run();







/*
using AppModels;
using AppServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────
// 🔗 Connection & Dependency Injection
// ─────────────────────────────────────────────────────
builder.Services.AddScoped<DbConnection>(provider =>
    new SqlConnection(builder.Configuration.GetConnectionString("DbCon"))
);

builder.Services.AddScoped<IDapperContext, DapperContext>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IAddServices, AddServices>();
builder.Services.AddScoped<IGetServices, GetServices>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

// ─────────────────────────────────────────────────────
// 🔐 JWT Authentication Setup
// ─────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/home/login";
    options.AccessDeniedPath = "/home/accessdenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Important for HTTP
    options.Cookie.SameSite = SameSiteMode.Lax; // Can also be None if cross-site needed
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Must be false for HTTP
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});



builder.Services.AddAuthorization();
var app = builder.Build();

// ─────────────────────────────────────────────────────
// 🌐 Middleware Pipeline
// ─────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var imagePath = builder.Configuration["FilePaths:UserImages"];
if (!Directory.Exists(imagePath))
{
    Directory.CreateDirectory(imagePath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagePath),
    //RequestPath = "/user-images"
});


app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=31536000";
    }
});

app.UseRouting();
app.UseAuthentication(); // 👈 Must be before UseAuthorization
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.Run();
*/

 



