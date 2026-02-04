using FurryFriends.Web.Service;
using FurryFriends.Web.Service.IService;
using FurryFriends.Web.Services;
using FurryFriends.Web.Services.Handlers;
using FurryFriends.Web.Services.IService;
using FurryFriends.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllersWithViews();



// Đăng ký HttpMessageHandler
builder.Services.AddScoped<AuthHeaderHandler>();

// Đăng ký các service với HttpClient
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
builder.Services.AddScoped<DiscountCalculationService>();
builder.Services.AddScoped<IPhieuHoanTraService, PhieuHoanTraService>();

//builder.Services.AddHttpClient<IPhieuHoanTraService, PhieuHoanTraService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7289/"); // URL API của bạn
//});

builder.Services.AddHttpClient<IHoaDonService, HoaDonService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IGiamGiaService, GiamGiaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IDiaChiKhachHangService, DiaChiKhachHangService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IKhachHangService, KhachHangService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IChucVuService, ChucVuService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<ITaiKhoanService, TaiKhoanService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<INhanVienService, NhanVienService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IVoucherService, VoucherService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IChatLieuService, ChatLieuService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});
builder.Services.AddHttpClient<IThanhPhanService, ThanhPhanService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});
builder.Services.AddHttpClient<IThuongHieuService, ThuongHieuService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});
builder.Services.AddHttpClient<IMauSacService, MauSacService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});
builder.Services.AddHttpClient<IKichCoService, KichCoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});
builder.Services.AddHttpClient<IThongTinCaNhanService, ThongTinCaNhanService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});
builder.Services.AddHttpClient<IGioHangService, GioHangService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});
builder.Services.AddHttpClient<IHinhThucThanhToanService, HinhThucThanhToanService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});
builder.Services.AddHttpClient<IAnhService, AnhService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});

builder.Services.AddHttpClient<ISanPhamChiTietService, SanPhamChiTietService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});

builder.Services.AddHttpClient<ISanPhamService, SanPhamService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
});

builder.Services.AddHttpClient<IThongBaoService, ThongBaoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});


// Sử dụng AddHttpMessageHandler để thêm AuthHeaderHandler
builder.Services.AddHttpClient<IBanHangService, BanHangService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7289/");
}).AddHttpMessageHandler<AuthHeaderHandler>();

// Thêm cấu hình xác thực Google và Facebook
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
        .AddCookie(options =>
        {
            options.Cookie.Name = "FurryFriends.Auth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.ExpireTimeSpan = TimeSpan.FromHours(2);
            options.SlidingExpiration = true;
        });
// .AddGoogle(options => ... removed

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// Đăng ký filter


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Middleware kiểm tra quyền truy cập Area Admin
app.Use(async (context, next) =>
{
    var area = context.Request.RouteValues["area"]?.ToString();
    var controller = context.Request.RouteValues["controller"]?.ToString();
    var role = context.Session.GetString("Role");
    
    // Nếu đang truy cập Area Admin
    if (area?.ToLower() == "admin")
    {
        // Danh sách controller được phép cho nhân viên
        var allowedControllersForEmployee = new[] { "BanHang", "DonHang", "HoaDon", "KhachHangs", "SanPham" };
        
        // Kiểm tra xem có phải admin không
        if (string.IsNullOrEmpty(role) || role.ToLower() != "admin")
        {
            // Nếu không phải admin, kiểm tra xem có được phép truy cập controller này không
            if (role?.ToLower() == "nhanvien" && controller != null && allowedControllersForEmployee.Contains(controller))
            {
                // Nhân viên được phép truy cập các controller này
                await next();
                return;
            }
            else
            {
                // Không có quyền truy cập
                context.Response.Redirect("/Auth/DangNhap?error=unauthorized");
                return;
            }
        }
    }
    
    await next();
});

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.SameAsRequest
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Areas",
        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
