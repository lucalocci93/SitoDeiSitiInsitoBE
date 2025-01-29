using Identity.DTOs;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AbbonamentoController : BaseController
    {
        public AbbonamentoController(UserManager UserService, AbbonamentoManager AbbonamentoService,
            DocumentoManager DocumentoManager, EventiManager eventiManager)
            :base(UserService, AbbonamentoService, DocumentoManager, eventiManager)
        {
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
        [Authorize]
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

    }
}
