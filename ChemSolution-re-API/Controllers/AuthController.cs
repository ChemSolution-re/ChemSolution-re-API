using AutoMapper;
using ChemSolution_re_API.Data;
using ChemSolution_re_API.DTO.Request;
using ChemSolution_re_API.DTO.Response;
using ChemSolution_re_API.Entities;
using ChemSolution_re_API.Services.Email;
using ChemSolution_re_API.Services.JWT;
using ChemSolution_re_API.Services.JWT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ChemSolution_re_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthController(
            DataContext context,
            IJwtService jwtService,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper,
            IEmailService emailService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (await _context.Users.AnyAsync(x => x.UserEmail == model.UserEmail))
            {
                return BadRequest("User with such Email exists");
            }

            var newUser = _mapper.Map<User>(model);

            newUser.VerificationToken = await GenerateVerificationTokenAsync();

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            // send email
            await SendVerificationEmailAsync(newUser, Request.Headers["origin"]);

            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserEmail == model.Email);

            if (user == null || !user.IsVerified)
            {
                return BadRequest("Email or password is incorrect");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Email or password is incorrect");
            }
            else if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                await _context.SaveChangesAsync();
            }

            var token = _jwtService.GetToken(_mapper.Map<JwtUser>(user));

            var response = _mapper.Map<AuthorizeResponse>(user);
            response.AccessToken = token;

            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.VerificationToken == model.Token);

            if (user == null)
            {
                return BadRequest("Invalid token");
            }

            user.Verified = DateTime.UtcNow;
            user.VerificationToken = "";

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Verification successful, you can now login" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserEmail == model.Email);

            if(user == null)
            {
                return Ok();
            }

            user.ResetToken = await GenerateResetTokenAsync();
            user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // send email
            await SendPasswordResetEmailAsync(user, Request.Headers["origin"]);

            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x =>
                    x.ResetToken == model.Token && x.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                return BadRequest("Invalid token");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
            user.PasswordReset = DateTime.UtcNow;
            user.ResetToken = "";
            user.ResetTokenExpires = null;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password reset successful, you can now login" });
        }

        //helper methods
        private async Task<string> GenerateVerificationTokenAsync()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            // ensure token is unique by checking against db
            var tokenIsUnique = !await _context.Users.AnyAsync(x => x.VerificationToken == token);
            if (!tokenIsUnique)
                return await GenerateVerificationTokenAsync();

            return token;
        }

        private async Task<string> GenerateResetTokenAsync()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            // ensure token is unique by checking against db
            var tokenIsUnique = !await _context.Users.AnyAsync(x => x.ResetToken == token);
            if (!tokenIsUnique)
                return await GenerateResetTokenAsync();

            return token;
        }

        private async Task SendVerificationEmailAsync(User account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                // origin exists if request sent from browser single page app (e.g. Angular or React)
                // so send link to verify via single page app
                var verifyUrl = $"{origin}/account/verify-email?token={account.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                            <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                // origin missing if request sent directly to api (e.g. from Postman)
                // so send instructions to verify directly with api
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                            <p><code>{account.VerificationToken}</code></p>";
            }

            await _emailService.SendEmailAsync(
                to: account.UserEmail,
                subject: "ChemSolution-re-API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                        <p>Thanks for registering!</p>
                        {message}"
            );
        }

        private async Task SendPasswordResetEmailAsync(User account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                            <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                            <p><code>{account.ResetToken}</code></p>";
            }

            await _emailService.SendEmailAsync(
                to: account.UserEmail,
                subject: "ChemSolution-re-API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                        {message}"
            );
        }
    }
}
