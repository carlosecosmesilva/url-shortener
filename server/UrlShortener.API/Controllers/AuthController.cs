using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Data;
using UrlShortener.API.Models;
using UrlShortener.API.Services;
using UrlShortener.API.DTOs;
using MassTransit;
using UrlShortener.API.Messaging.Messages;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AppDbContext context, JwtService jwtService, IPublishEndpoint publishEndpoint) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly JwtService _jwtService = jwtService;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        /// <summary>
        /// Logs in a user and returns an access token and refresh token.
        /// </summary>
        /// <param name="request">Login request containing username and password.</param>
        /// <returns>Returns an access token and refresh token if successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                await _publishEndpoint.Publish(new LoginEvent
                {
                    Username = request.Username,
                    Success = false,
                    Timestamp = DateTime.UtcNow,
                    Reason = "User not found"
                });
                return Unauthorized(new { message = "Invalid username or password." });
            }

            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Password))
            );

            if (user.PasswordHash != passwordHash)
            {
                await _publishEndpoint.Publish(new LoginEvent
                {
                    Username = request.Username,
                    Success = false,
                    Timestamp = DateTime.UtcNow,
                    Reason = "Invalid password"
                });
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            var refresh = new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };
            _context.Add(refresh);
            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish(new LoginEvent
            {
                Username = request.Username,
                Success = true,
                Timestamp = DateTime.UtcNow
            });

            return Ok(new { accessToken, refreshToken });
        }

        /// <summary>
        /// Refreshes the access token using a valid refresh token.
        /// </summary>
        /// <param name="request">Refresh request containing the refresh token.</param>
        /// <returns>Returns a new access token if the refresh token is valid.</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var refresh = await _context.RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == request.RefreshToken && !r.IsExpired);
            if (refresh == null)
                return Unauthorized("Invalid refresh token");

            var accessToken = _jwtService.GenerateAccessToken(refresh.User);
            return Ok(new { accessToken });
        }

        /// <summary>
        /// Registers a new user with a username and password.
        /// </summary>
        /// <param name="request">Register request containing username and password.</param>
        /// <returns>Returns a success message if registration is successful.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return Conflict("Username already exists.");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(request.Password)))
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User registered successfully." });
        }

        /// <summary>
        /// Verifies the password against the stored hash.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <param name="hash">The stored password hash to compare against.</param>
        /// <returns>Returns true if the password matches the hash, otherwise false.</returns>
        private static bool VerifyPassword(string password, string hash)
        {
            return hash == Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        }
        /// <summary>
        /// Generates a new refresh token.
        /// </summary>
        /// <returns>Returns a new refresh token as a base64 string.</returns>
        private static string GenerateRefreshToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}