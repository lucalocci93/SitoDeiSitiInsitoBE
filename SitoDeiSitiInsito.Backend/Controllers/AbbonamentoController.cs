using DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitoDeiSiti.Backend.Services;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Validators;

namespace Identity.Controllers
{
    [Route("[controller]")]
    public class AbbonamentoController : BaseController
    {
        private readonly GetUserSubscriptionsValidator GetUserSubscriptionsValidator;
        private readonly UpdateSubscriptionValidator UpdateSubscriptionValidator;

        public AbbonamentoController(UserManager UserService, AbbonamentoManager AbbonamentoService,
            DocumentoManager DocumentoManager, EventiManager eventiManager)
            :base(UserService, AbbonamentoService, DocumentoManager, eventiManager)
        {
            GetUserSubscriptionsValidator = new();
            UpdateSubscriptionValidator = new();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetListSubscription")]
        public async Task<ActionResult> GetListSubscription()
        {
            var resp = await abbonamentoManager.GetAllAbbonamenti();

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
        [HttpGet("GetUserSubscriptions")]
        public async Task<ActionResult> GetUserSubscriptions(Guid Utente)
        {
            var validationResult = await GetUserSubscriptionsValidator.ValidateAsync(Utente).ConfigureAwait(false);

            if (validationResult != null && validationResult.IsValid)
            {
                var resp = await abbonamentoManager.GetAbbonamentiByUser(Utente);

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
            else
            {
                return BadRequest(validationResult != null ? validationResult.Errors.FirstOrDefault()?.ErrorMessage : string.Empty);
            }
        }

        [Authorize]
        [HttpGet("GetUserSubscription")]
        public async Task<ActionResult> GetUserSubscription(Guid Utente, int id)
        {
            var resp = await abbonamentoManager.GetAbbonamento(Utente, id);

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
        [HttpPost("AddUserSubscription")]
        public async Task<ActionResult> AddUserSubscription([FromBody] Subscription subscription)
        {
            var resp = await abbonamentoManager.AddAbbonamentoUser(subscription);

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
        [HttpGet("GetSubscriptionType")]
        public async Task<ActionResult> GetSubscriptionType()
        {
            var resp = await abbonamentoManager.GetTipiAbbonamento();

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

        //[Authorize(Roles = "Admin")]
        [HttpPut("UpdateSubscription/{operation}")]
        public async Task<ActionResult> UpdateSubscription(DbOperationsAbbonamentoEnums operation, [FromBody] Subscription subscription)
        {
            var validationResult = await UpdateSubscriptionValidator.ValidateAsync(subscription).ConfigureAwait(false);

            if (validationResult != null && validationResult.IsValid)
            {
                var resp = await abbonamentoManager.UpdateAbbonamentoUser(operation, subscription).ConfigureAwait(false);

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
            else
            {
                return BadRequest(validationResult != null ? validationResult.Errors.FirstOrDefault()?.ErrorMessage : string.Empty);
            }
        }
    }
}
