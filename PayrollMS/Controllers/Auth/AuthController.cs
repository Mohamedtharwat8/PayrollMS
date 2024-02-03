using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollMS.Dtos;
using PayrollMS.Services.Interface;

namespace PayrollMS.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Route For Seeding my roles to DB
    [HttpPost]
    [Route("seed-roles")]
    public async Task<IActionResult> SeedRoles()
    {
        var seerRoles = await _authService.SeedRolesAsync();

        return Ok(seerRoles);
    }


    // Route -> Register
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var registerResult = await _authService.RegisterAsync(registerDto);

        if (registerResult.IsSucceed)
            return Ok(registerResult);

        return BadRequest(registerResult);
    }


    // Route -> Login
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var loginResult = await _authService.LoginAsync(loginDto);

        if (loginResult.IsSucceed)
            return Ok(loginResult);

        return Unauthorized(loginResult);
    }



    // Route -> make user -> Manager
    [HttpPost]
    [Route("make-Manager")]
    public async Task<IActionResult> MakeManager([FromBody] UpdatePermissionDto updatePermissionDto)
    {
        var operationResult = await _authService.MakeManagerAsync(updatePermissionDto);

        if (operationResult.IsSucceed)
            return Ok(operationResult);

        return BadRequest(operationResult);
    }

    // Route -> make user -> Accountant
    [HttpPost]
    [Route("make-Accountant")]
    public async Task<IActionResult> MakeAccountant([FromBody] UpdatePermissionDto updatePermissionDto)
    {
        var operationResult = await _authService.MakeAccountantAsync(updatePermissionDto);

        if (operationResult.IsSucceed)
            return Ok(operationResult);

        return BadRequest(operationResult);
    }
}
