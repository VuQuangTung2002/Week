using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Week;
using Week.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
// để kiểm tra các dữ kiện của các claim nhập vào cần phải có thêm dòng
// dưới để có thể kết nối đến TokenHander.cs để kiểm tra 
builder.Services.AddSingleton<TokenHander>();
// Configure JWT Authentication service
builder.Services.AddAuthentication(
    option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5010",
            ValidAudience = "http://localhost:5010",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero,
        };
        options.Events = new JwtBearerEvents()
        {
            // chạy đầu tiên
            OnMessageReceived = context =>
            {
                return Task.CompletedTask;
            },
            // chạy thứ 2
            OnTokenValidated = context =>
            {
                // kiem tra cac du lieu claim nhap vao
                var TokenHander = context.HttpContext.RequestServices.GetRequiredService<TokenHander>();
                return TokenHander.ValidateToken(context);
                
            },
            // sử lý lỗi token chạy không thành công 
            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            },
            
            // chạy thứ 4
            OnChallenge = context =>
            {
                string error = context.ErrorDescription;
                return Task.CompletedTask;
            }
        };
    });
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
//    options.AddPolicy("UserOnly", policy => policy.RequireRole("user"));
//});

builder.Services.AddControllers();
builder.Services.AddDbContext<MyDbConnect>(options =>
{
    // Đọc connection string từ appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("MyConnectionString");
    options.UseNpgsql(connectionString);
});
builder.Services.AddDefaultIdentity<ItemUsers>(option =>
{
    option.SignIn.RequireConfirmedAccount = true;
    option.Password.RequireDigit = true;
    option.Password.RequireLowercase = true;
    option.Password.RequireUppercase = true;
    option.Password.RequireNonAlphanumeric = true;
    option.Password.RequiredLength = 5;
})
.AddRoles<ItemRole>()
.AddEntityFrameworkStores<MyDbConnect>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Admin/Account/Login";
    options.LogoutPath = $"/Admin/Account/Logout";
    options.AccessDeniedPath = "/Admin/Account/AccessDenied";
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
//app.UseRouting();
//app.UseMiddleware<PermissionMiddleware>();
//app.UseAuthentication();
//app.UseAuthorization();
//app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
//app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Đặt sau UseRouting
app.UseAuthorization();  // Đặt sau UseAuthentication

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
