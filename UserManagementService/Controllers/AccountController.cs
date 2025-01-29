using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using UserManagementService.Dtos;
using Microsoft.AspNetCore.Identity;
using UserManagementService.Models;
using UserManagementService.MailService.Services;
using UserManagementService.MailService.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using UserManagementService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace UserManagementService.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly DataContext _dataContext;
        public AccountController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IEmailService emailService,
            DataContext dataContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _dataContext = dataContext;

        }

        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var userExist = await _userManager.FindByEmailAsync(resetPasswordDto.Email!);
            if (userExist != null && await _userManager.CheckPasswordAsync(userExist, resetPasswordDto.CurrentPassword!))
            {
                await _userManager.ChangePasswordAsync(userExist, resetPasswordDto.CurrentPassword!, resetPasswordDto.NewPassword!);
                return Ok("password reset successfull.");

            }
            return BadRequest("Either user doesn't exist or invalid credentials");
        }

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var clientRequest = HttpContext.Request.Headers["Referer"];
            var userExist = await _userManager.FindByEmailAsync(email);
            if (userExist != null) 
            {
                // check for email verification
                if (userExist.EmailConfirmed)
                {
                    // add token to verify email
                    var token = await _userManager.GeneratePasswordResetTokenAsync(userExist);
                    // this will be used without frontend
                    //var confirmationLink = Url.Action(nameof(ResetPassword), "Account", new { token, email = userExist.Email, currentPassword =currentPassword, newPassword = newPassword}, Request.Scheme);
                    // confirmation link for frontend
                    string confirmationLink = $"{clientRequest}reset-password?token={token}&email={userExist.Email}";
                    var message = new Message(new string[] { userExist.Email! }, "Password Reset Link", confirmationLink!);
                    _emailService.SendEmail(message);
                    return StatusCode(StatusCodes.Status200OK, new {Message="A password reset link is sent to the provided email."});
                }
                else
                {
                    return BadRequest(new { Message = "Email is not verified!" });
                }

            }
            return BadRequest(new { Message = "User doesn't exist for this email" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserRegistrationDto userRegistrationDto, string role)
        {
            //check for use exist
            var userExist = await _userManager.FindByEmailAsync(userRegistrationDto.Email!);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists!" });
            }
            //add user to database
            IdentityUser user = new()
            {
                Email = userRegistrationDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userRegistrationDto.Username
            };
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, userRegistrationDto.Password!);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                       new Response { Status = "Error", Message = "User failed to create!" });
                }
                // add role to user
                await _userManager.AddToRoleAsync(user, role);

                // add token to verify email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Email Confirmation Link", confirmationLink!);
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status201Created,
                      new Response { Status = "Success", Message = $"User created and email sent to {user.Email} successfully." });

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new Response { Status = "Error", Message = "Provided role doesn't exist!" });
            }


        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                       new Response { Status = "Success", Message = "Email verified successfully." });
                }

            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                      new Response { Status = "Error", Message = "This user doesn't exist!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            // check for existing user

            var user = await _userManager.FindByEmailAsync(loginDto.Email!);
            if (user==null)
            {
                return BadRequest("User doesn't exist!");
                
            }
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password!);
            if (!checkPassword)
            {
                return BadRequest("Invalid credentials!");

            }

            if (user != null && checkPassword)
            {

                // check the password
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // claim list creation
                var userRole = await _userManager.GetRolesAsync(user);

                foreach (var role in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));

                }

                // generate token with claim
                var jwtToken = GetToken(authClaims);
                var encryptedToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                HttpContext.Response.Cookies.Append("jwt", encryptedToken,
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(3),
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None,
                    }) ;


                // return token
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo
                });
            }
            return Unauthorized();
        }



        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (jwt != null)
                {
                    var email = User.FindFirstValue(ClaimTypes.Email);
                    if (email == "" || email == null) return BadRequest("Bad request or you are not logged in!");
                    var user = await _dataContext.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
                    return Ok(user);
                }

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
            return BadRequest("You are not logged in");
        }

    
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "Success"
            });
        }

    }
}
