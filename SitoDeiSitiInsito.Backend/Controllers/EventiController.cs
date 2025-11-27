using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitoDeiSiti.Backend.Services;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;
using SitoDeiSiti.Validators;
using System.Diagnostics.Eventing.Reader;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.Controllers
{
    [Route("[controller]")]
    public class EventiController : BaseController
    {
        private readonly GetEventoValidators GetEventoValidators;
        private readonly SubscribeEventValidator SubscribeEventValidator;
        private readonly GetEventSubscriptionByUserValidator GetEventSubscriptionByUserValidator;

        public EventiController(UserManager UserService, AbbonamentoManager AbbonamentoService,
            DocumentoManager DocumentoManager, EventiManager EventiManager)
        : base(UserService, AbbonamentoService, DocumentoManager, EventiManager)
        {
            GetEventoValidators = new();
            SubscribeEventValidator = new();
            GetEventSubscriptionByUserValidator = new();
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
            var res = await GetEventoValidators.ValidateAsync(Id).ConfigureAwait(false);

            if (res != null && res.IsValid)
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
            else
            {
                return BadRequest(res != null ? res.Errors : string.Empty);
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
            var res = await SubscribeEventValidator.ValidateAsync(eventSubscription).ConfigureAwait(false);

            if (res != null && res.IsValid)
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
                return BadRequest(res != null ? res.Errors : string.Empty);
            }
        }

        [HttpGet("GetEventSubscriptionByUser")]
        public async Task<ActionResult> GetEventSubscriptionByUser([FromQuery] Guid UserId)
        {
            var res = await GetEventSubscriptionByUserValidator.ValidateAsync(UserId).ConfigureAwait(false);

            if (res != null && res.IsValid)
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
            else
            {
                return BadRequest(res != null ? res.Errors : string.Empty);
            }
        }

        [Authorize]
        [HttpPost("DeleteSubscription")]
        public async Task<ActionResult> DeleteSubscription([FromBody] EventSubscription Subscription)
        {
            if (!Subscription.CompetitionId.HasValue)
            {
                return BadRequest("L'Id della gara non può essere nullo");
            }

            if (!Subscription.EventId.HasValue)
            {
                return BadRequest("la gara deve essere associata ad un evento");
            }

            if (!Subscription.EventId.HasValue)
            {
                return BadRequest("la gara deve essere associata ad un evento");
            }


            var resp = await eventiManager.DeleteEventSubscription(Subscription).ConfigureAwait(false);

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

        [Authorize(Roles = "Admin")]
        [HttpPost("AddCompetition")]
        public async Task<ActionResult> AddCompetition([FromBody] Competition competition)
        {
            var resp = await eventiManager.AddCompetition(competition).ConfigureAwait(false);
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
        [HttpDelete("DeleteCompetition")]
        public async Task<ActionResult> DeleteCompetition([FromQuery] Guid? CompetitionId, Guid? EventId)
        {
            if (!CompetitionId.HasValue)
            {
                return BadRequest("L'Id della gara non può essere nullo");
            }

            if (!EventId.HasValue)
            {
                return BadRequest("la gara deve essere associata ad un evento");
            }

            var resp = await eventiManager.DeleteCompetition(CompetitionId.Value, EventId.Value).ConfigureAwait(false);
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
        [HttpGet("GetCompetitions")]
        public async Task<ActionResult> GetCompetitions([FromQuery] Guid? EventId)
        {
            if (!EventId.HasValue)
            {
                return BadRequest("la gara deve essere associata ad un evento");
            }

            var resp = await eventiManager.GetCompetitionByEvent(EventId.Value).ConfigureAwait(false);
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
        [HttpGet("GetCompetitionsByEventAndUser")]
        public async Task<ActionResult> GetCompetitionsByEventAndUser([FromQuery] Guid? EventId, Guid? UserId)
        {
            if (!EventId.HasValue)
            {
                return BadRequest("la gara deve essere associata ad un evento");
            }

            if (!UserId.HasValue)
            {
                return BadRequest("Deve essere specificato l'utente associato alle gare");
            }


            var resp = await eventiManager.GetCompetitionsByEventAndUser(EventId.Value, UserId.Value).ConfigureAwait(false);
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
        [HttpGet("GetCompetitionSubscriptionReportByUser")]
        public async Task<ActionResult> GetCompetitionSubscriptionReportByUser([FromQuery] Guid? EventId, Guid? UserId)
        {
            if (!EventId.HasValue)
            {
                return BadRequest("la gara deve essere associata ad un evento");
            }
            if (!UserId.HasValue)
            {
                return BadRequest("Deve essere specificato l'utente associato alle gare");
            }
            var resp = await eventiManager.GetCompetitionSubscriptionReportByUser(EventId.Value, UserId.Value).ConfigureAwait(false);
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
        [HttpGet("GetCompetitionSubscriptionReportByMaster")]
        public async Task<ActionResult> GetCompetitionSubscriptionReportByMaster([FromQuery] Guid? EventId, Guid? Org)
        {
            if (!EventId.HasValue)
            {
                return BadRequest("la gara deve essere associata ad un evento");
            }
            if (!Org.HasValue)
            {
                return BadRequest("Devi far parte di una organizzazione");
            }
            var resp = await eventiManager.GetCompetitionSubscriptionReportByMaster(EventId.Value, Org.Value).ConfigureAwait(false);
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
