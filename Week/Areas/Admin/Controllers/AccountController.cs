using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Week.Models;
using Week.Areas.Admin.Attributes;
using BC = BCrypt.Net;
namespace Week.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        public readonly MyDbConnect db;
        private DateTime acesstoken;
        private DateTime datetimeRefresh; private readonly SignInManager<ItemUsers> _signInManager;
        private readonly UserManager<ItemUsers> _userManager;
        public AccountController(SignInManager<ItemUsers> signInManager, UserManager<ItemUsers> userManager, IConfiguration configuration, MyDbConnect db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            this.db = db;
        }
        private (string token, DateTime acesstoken) IssAccesstoken(ItemUsers user)
        {
            acesstoken = DateTime.UtcNow.AddMinutes(1);
            // chuyen doi du lieu tu chuoi key sang dang byte vaf kieu symme
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var cretials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);

            // cai dat cau hinh claim 
            var claim = new List<Claim>
            {
                new Claim("my_app_week1", user.Id.ToString().Trim()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString().Trim()),
                new Claim(ClaimTypes.Email, user.Email.ToString().Trim()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString().Trim()),
                
                new Claim(JwtRegisteredClaimNames.Exp, acesstoken.ToString("yyyy/mm/dd HH:mm:ss").Trim())
            };
      
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: cretials);
            return (new JwtSecurityTokenHandler().WriteToken(token), acesstoken);
        }
        private (string token, DateTime datetimeRefresh, string Code) IssRefreshtoken(ItemUsers user)
        {
            datetimeRefresh = DateTime.UtcNow.AddHours(3);
            // chuyen doi du lieu tu chuoi key sang dang byte vaf kieu symme
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var cretials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
           var Code = Guid.NewGuid().ToString();
            // cai dat cau hinh claim 
            var claim = new List<Claim>
            {
                
                new Claim("my_app_week1", user.Id.ToString().Trim()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString().Trim()),
                new Claim(ClaimTypes.Email, user.Email.ToString().Trim()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString().Trim()),
               
                new Claim(ClaimTypes.SerialNumber, Code.Trim()),
                new Claim(JwtRegisteredClaimNames.Exp, datetimeRefresh.ToString("yyyy/mm/dd HH:mm:ss").Trim()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claim,
                expires: datetimeRefresh,
                signingCredentials: cretials);
            return (new JwtSecurityTokenHandler().WriteToken(token), datetimeRefresh, Code);
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectToAction("Login","Account");
        }
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.register = TempData["register"];
            ViewBag.action = "/Admin/Account/LoginPost";
            return View();
        }
        private static readonly HttpClient client = new HttpClient();
        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult LoginPost(IFormCollection fc, string returnUrl = null)
        {
            try
            {
                string email = fc["email"].ToString().Trim();
                string _password = fc["password"].ToString().Trim();
                var user = db.Users.FirstOrDefault(s => s.Email == email);
                if (user != null && BC.BCrypt.Verify(_password, user.PasswordHash.Trim()))
                {

                    HttpContext.Session.SetString("user_email", email);
                    HttpContext.Session.SetString("user_password", _password);

                    // Generate token
                    (var token, DateTime acesstoken) = IssAccesstoken(user);
                    (var refreshtoken, DateTime datetimeRefresh, string Code) = IssRefreshtoken(user);

                    // lưu dữ diệu ào local storage
                    // khoi tao mot UserToken
                    ItemUserToken x = new ItemUserToken();
                    x.Id = Convert.ToInt32(user.Id);
                    x.AccessToken = token;
                    x.RefreshToken = refreshtoken;
                    x.ExpriredDateRefreshToken = datetimeRefresh;
                    x.ExpriredDateAccessToken = acesstoken;
                    x.IsActive = 1;
                    x.CodeRefreshToken = Code;

                    // luu du lieu vao trong token 
                    db.UserTokens.Add(x);
                    try
                    {
                        db.SaveChanges();
                        ValidateTokenRefresh(refreshtoken);


                    }
                    catch (DbUpdateException ex)
                    {
                        // Lấy thông tin lỗi chi tiết
                        var innerException = ex.InnerException?.Message;
                        return Ok(innerException);

                        // Hoặc throw lại exception nếu cần
                        throw;
                    }


                    return Ok(new JwtModel() { AccessToken = token, RefreshToken = refreshtoken, Email = user.Email, UserName = user.UserName });



                    // Redirect to Home/Index
                    //return RedirectToAction("Index", "Home");
                }

                // Return failure response
                return Json(new { success = false, message = "Đăng nhập thất bại" });
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> ValidateTokenRefresh(string refreshtoken)
        {
            try
            {
                var tokenHander = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                // Kiểm tra token
                var principal = tokenHander.ValidateToken(refreshtoken, validationParameters, out var validatedToken);
                if (!(validatedToken is JwtSecurityToken jwtToken))
                {
                    return BadRequest(new { message = "Token Not Suit" });
                }

                // Lấy mã số của refresh token từ claims
                var codenumberRefresh = principal.Claims.FirstOrDefault(s => s.Type == ClaimTypes.SerialNumber)?.Value;
                if (string.IsNullOrEmpty(codenumberRefresh))
                {
                    return BadRequest(new { Message = "Error Code Number Refresh" });
                }

                // Tìm UserToken với mã code refresh token và đang hoạt động
                var useToken = await db.UserTokens.FirstOrDefaultAsync(s => s.CodeRefreshToken == codenumberRefresh && s.IsActive == 1);
                if (useToken != null)
                {
                    var user = await db.Users.FirstOrDefaultAsync(s => Convert.ToInt32(s.Id) == useToken.Id);
                    if (user != null)
                    {
                        // Đặt trạng thái IsActive = 0 cho tất cả các bản ghi của người dùng
                        var x = await db.UserTokens.Where(s => s.Id == useToken.Id).ToListAsync();
                        if (x.Count != 0)
                        {
                            foreach (var token in x)
                            {
                                db.Entry(token).State = EntityState.Modified;
                                token.IsActive = 0;
                              
                            }
                            await db.SaveChangesAsync(); // Lưu tất cả các thay đổi một lần

                            // Tạo mới access token và refresh token
                            var (newAccessToken, newAccessExpiry) = IssAccesstoken(user);
                            var (newRefreshToken, newRefreshExpiry, newCode) = IssRefreshtoken(user);
                            var newUserToken = new ItemUserToken
                            {
                                Id = Convert.ToInt32(user.Id),
                                AccessToken = newAccessToken,
                                RefreshToken = newRefreshToken,
                                ExpriredDateRefreshToken = newRefreshExpiry,
                                ExpriredDateAccessToken = newAccessExpiry,
                                IsActive = 1,
                                CodeRefreshToken = newCode
                            };
                             db.UserTokens.Add(newUserToken);
                            await db.SaveChangesAsync(); // Lưu bản ghi mới
                            //try
                            //{
                            //    // Lấy token từ session storage (giả sử token được lưu trữ trong biến)
                            //     // Thay thế bằng token thực tế

                            //    // Thiết lập token vào header Authorization
                            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);

                            //    // Gửi yêu cầu GET đến server
                            //    HttpResponseMessage response = await client.GetAsync("https://example.com/api/protected");

                            //    // Kiểm tra xem yêu cầu có thành công không
                            //    if (response.IsSuccessStatusCode)
                            //    {
                            //        // Đọc dữ liệu trả về từ server
                            //        string responseData = await response.Content.ReadAsStringAsync();
                            //        return Ok(responseData);
                            //    }
                            //    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            //    {
                            //        // Xử lý lỗi xác thực 401
                            //        throw new Exception("Yêu cầu bị từ chối: Token không hợp lệ hoặc không có quyền truy cập.");
                            //    }
                            //    else
                            //    {
                            //        // Xử lý các lỗi khác
                            //        throw new Exception("Yêu cầu không thành công: " + response.ReasonPhrase);
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    // Xử lý lỗi
                            //    Console.WriteLine("Lỗi: " + ex.Message);
                            //    throw;
                            //}
                        }
                        else
                        {
                            return Ok("không tìm thấy IsActive nào thảo mãn ");
                        }
                    }
                }
                return BadRequest(new { message = "Invalid refresh token or user not found" });
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new
                {
                    message = "Invalid token",
                    details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", details = ex.Message });
            }
        }
        public async Task<IActionResult> Profile()
        {
            ItemUsers pro = new ItemUsers(); 
            if (Request.Query.TryGetValue("email", out var email))
            {
                string user_email = email.ToString();
                if (String.IsNullOrEmpty(user_email))
                {
                    return BadRequest("No find user_email");
                }
              
             
                    pro = await db.Users.Where(s => s.Email == user_email.Trim()).FirstOrDefaultAsync();
                if (pro == null)
                {
                    return NotFound("No exist pro's data");
                }
               
                    ViewBag.errorpass = TempData["errorpassword"];
                
                ViewBag.profile = "/Admin/Account/ProfilePost/" + user_email;
               
            }
            return View(pro);
        }
        [HttpPost("Admin/Account/ProfilePost/{user_email}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilePost(string user_email, IFormCollection fc)
        {
            if (String.IsNullOrEmpty(user_email))
            {
                return BadRequest("No find user_email");
            }
            if (fc.TryGetValue("oldpassword", out var oldpassword))
            {
                var pro = await db.Users.Where(s => s.Email.Trim() == user_email.Trim()).FirstOrDefaultAsync();
                if (pro == null)
                {
                    return NotFound("No exist pro's data");
                }

                if (BC.BCrypt.Verify(oldpassword.ToString().Trim(), pro.PasswordHash.Trim()))
                    {
                    if (fc.TryGetValue("name", out var name))
                    {
                        pro.UserName = name;
                    }
                    if (fc.TryGetValue("email", out var email))
                    {
                        pro.Email = email;
                    }
                    if (fc.TryGetValue("password", out var password))
                    {
                        pro.PasswordHash = BC.BCrypt.HashPassword(password);
                    }
                    pro.AccessFailedCount = 0;
                    pro.ConcurrencyStamp = "9491a763-745a-47c2-8d14-79acb0722d05";
                    pro.PhoneNumber = "0";
                    pro.PhoneNumberConfirmed = false;
                    pro.EmailConfirmed = false;
                    pro.LockoutEnabled = false;
                    pro.LockoutEnd = null;
                    pro.TwoFactorEnabled = false;
                    pro.NormalizedEmail = "";
                    pro.NormalizedUserName = "";
                    pro.SecurityStamp = "MGYZTW6AH4XKCBDFSHJH6DMSMOXSODTA";

                    TempData["errorpassword"] = "Change susscess password";
                    db.SaveChanges();
                }
                else
                {
                    TempData["errorpassword"] = "No true password";
                }
            }

            return RedirectToAction("Profile", "Account", new { email = user_email});
        }

        public IActionResult Logout()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("user_email")))
            {
                HttpContext.Session.Remove("user_email");
                HttpContext.Session.Remove("user_password");
            }
            return RedirectToAction("Login", "Account");
        }
       
    }
}
