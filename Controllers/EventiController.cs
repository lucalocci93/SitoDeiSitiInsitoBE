using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.Controllers
{
    public class EventiController : BaseController
    {
        public EventiController(UserManager UserService, AbbonamentoManager AbbonamentoService,
            DocumentoManager DocumentoManager, EventiManager EventiManager)
        : base(UserService, AbbonamentoService, DocumentoManager, EventiManager)
        {
        }

        [Authorize]
        [HttpGet("GetEvents")]
        public async Task<ActionResult> GetEvents()
        {
            var resp = await eventiManager.GetEventi().ConfigureAwait(false);

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
        [HttpGet("GetCategories")]
        public async Task<ActionResult> GetCategories()
        {
            var resp = await eventiManager.GetCategories().ConfigureAwait(false);

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
        [HttpPost("CreateEvent")]
        public async Task<ActionResult> CreateEvent([FromBody] Events evento)
        {
            var resp = await eventiManager.CreateEvent(evento).ConfigureAwait(false);

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

        [HttpGet("GetEvent")]
        public async Task<ActionResult> GetEvent([FromQuery] Guid Id)
        {
            var resp = await eventiManager.GetEvent(Id).ConfigureAwait(false);

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
        [HttpPut("UpdateEvent")]
        public async Task<ActionResult> UpdateEvent([FromBody] Events evento)
        {
            var resp = await eventiManager.UpdateEvent(evento).ConfigureAwait(false);

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

        [HttpPost("SubscribeEvent")]
        public async Task<ActionResult> SubscribeEvent([FromBody] EventSubscription eventSubscription)
        {
            if(eventSubscription != null && eventSubscription.Categories.Any())
            {
                var resp = await eventiManager.SubscribeEvent(eventSubscription).ConfigureAwait(false);

                if (resp != null)
                {
                    if (resp.success)
                    {
                        return Ok(resp.Data);
                    }
                    else
                    {
                        if (resp.Error.Code.Equals((int)ErrorCode.IscrizioneEventoGiaEffettuata))
                        {
                            return Conflict(resp.Error);
                        }
                        else
                        {
                            return BadRequest(resp.Error.Message);
                        }
                    }
                }
                else
                {
                    return Problem();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetEventSubscriptionByUser")]
        public async Task<ActionResult> GetEventSubscriptionByUser([FromQuery] Guid UserId)
        {
            var resp = await eventiManager.GetEventSubscription(UserId).ConfigureAwait(false);

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

        [HttpDelete("DeleteSubscription")]
        public async Task<ActionResult> DeleteSubscription([FromQuery] Guid EventId, Guid UserId, int Category)
        {
            var resp = await eventiManager.DeleteEventSubscription(EventId, UserId, Category).ConfigureAwait(false);

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
        [HttpGet("GetCompetitors")]
        public async Task<ActionResult> GetCompetitors([FromQuery] Guid EventId)
        {
            var resp = await eventiManager.GetCompetitors(EventId).ConfigureAwait(false);

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
        //[AllowAnonymous]
        [HttpGet("GetCompetitorsFile")]
        public async Task<ActionResult> GetCompetitorsFile([FromQuery] Guid EventId)
        {
            var resp = await eventiManager.CreateCompetitorExcel(EventId).ConfigureAwait(false);

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
