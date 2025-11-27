using DAL.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SitoDeiSiti.Backend.Services;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;
using SitoDeiSiti.Validators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Controllers
{
    [Route("[controller]")]
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
                return BadRequest(res.Errors.Any() ? res.Errors.FirstOrDefault()?.ErrorMessage :
                    "Errore, riptovare piu tardi, se il problema persiste contattare un amministratore");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> Users()
        {
            var resp = await userManager.GetAllUser();

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
                    return BadRequest(res.Errors.Any() ? res.Errors.FirstOrDefault()?.ErrorMessage 
                        : "Errore, riptovare piu tardi, se il problema persiste contattare un amministratore");
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

        [AllowAnonymous]
        [HttpGet("GetCinture")]
        public async Task<ActionResult> GetCinture()
        {
            var resp = await userManager.GetCinture().ConfigureAwait(false);
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
        [HttpGet("GetOrganizzazioni")]
        public async Task<ActionResult> GetOrganizzazioni()
        {
            var resp = await userManager.GetOrganizzazioni().ConfigureAwait(false);
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

        [Authorize]
        [HttpGet("GetAtletiOrganizzazione")]
        public async Task<ActionResult> GetAtletiOrganizzazione(Guid Org)
        {
            var resp = await userManager.GetAtletiOrganizzazione(Org).ConfigureAwait(false);
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
    }

}
