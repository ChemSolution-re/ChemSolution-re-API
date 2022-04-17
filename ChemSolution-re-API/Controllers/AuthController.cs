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

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(newUser.UserEmail, "ChemSolution", "<h1>You are registered in ChemSolution</h1>");

            var token = _jwtService.GetToken(_mapper.Map<JwtUser>(newUser));

            var response = _mapper.Map<AuthorizeResponse>(newUser);
            response.Access_token = token;

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserEmail == model.Email);

            if (user == null)
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
            response.Access_token = token;

            return Ok(response);
        }
    }
}
