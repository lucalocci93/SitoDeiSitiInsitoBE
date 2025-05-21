using DAL.Enums;
using FluentValidation;
using FluentValidation.Results;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;
using SitoDeiSiti.Validators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Controllers
{
    public class UserController : BaseController
    {
        private readonly CreateNewUserValidator NewUserValidator;
        private readonly AuthValidator authValidator;

        public UserController(UserManager UserService, AbbonamentoManager AbbonamentoService,
            DocumentoManager documentoManager, EventiManager eventiManager)
            : base(UserService, AbbonamentoService, documentoManager, eventiManager)
        {
            NewUserValidator = new();
            authValidator = new();
        }

        [AllowAnonymous]
        [HttpGet("Authenticate")]
        public async Task<ActionResult> Authenticate([FromQuery] string Username, string Password)
        {
            var res = await authValidator.ValidateAsync(new User() { Email = Username, Password = Password }).ConfigureAwait(false);

            if (res.IsValid)
            { 
                Response<JWT> response = await userManager.GenerateToken(Username, Password);

                if (response != null)
                {
                    if (response.success)
                    {
                        if (string.IsNullOrEmpty(response.Data.Token))
                        {
                            return Unauthorized();
                        }
                        else
                        {
                            return Ok(response);
                        }
                    }
                    else
                    {
                        return BadRequest(response.Error.Message);
                    }
                }
                else
                {
                    return Problem();
                }
            }
            else
            {
                return BadRequest(res.Errors.FirstOrDefault()?.ErrorMessage);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> Users()
        {
            var resp = await userManager.GetAllUser();
            
            if(resp != null)
            {
                if (resp.success)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Error.Message);
                }
            }
            else
            {
                return Problem();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUser")]
        public async Task<ActionResult> User(Guid RowGuid)
        {
            var resp = await userManager.GetUser(RowGuid);

            if (resp != null)
            {
                if (resp.success)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Error.Message);
                }
            }
            else
            {
                return Problem();
            }
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<ActionResult> Create([FromBody] User user)
        {
            if (user != null)
            {
                ValidationResult res = NewUserValidator.Validate(user);

                if (res.IsValid)
                {
                    var resp = await userManager.CreateUser(user);

                    if (resp != null)
                    {
                        if (resp.success)
                        {
                            return Ok();
                        }
                        else
                        {
                            return BadRequest(resp.Error.Message);
                        }
                    }
                    else
                    {
                        return Problem();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser/{operation}")]
        public async Task<ActionResult> UpdateUser(UserDbOperationEnum operation, [FromBody] User user)
        {
            var resp = await userManager.UpdateUser(operation, user);

            if (resp != null)
            {
                if (resp.success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(resp.Error.Message);
                }
            }
            else
            {
                return Problem();
            }

        }
    }
}
