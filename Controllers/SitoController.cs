using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Services;
using SitoDeiSiti.Validators;
using System.Threading.Tasks;

namespace SitoDeiSiti.Controllers
{
    public class SitoController : ControllerBase
    {
        private readonly SitoManager sito;
        private readonly SitoValidators validators;

        public SitoController(SitoManager manager)
        {
            sito = manager;
            validators = new();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetPagine")]
        public async Task<IActionResult> GetPagine()
        {
            var resp = await sito.GetPagine().ConfigureAwait(false);

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
        [HttpGet("GetGraficheByPagina")]
        public async Task<IActionResult> GetGraficheByPagina([FromQuery] int Pagina)
        {
            var res = validators.Validate(Pagina);

            if (res != null && res.IsValid)
            {

                var resp = await sito.GetGraficheByPagina(Pagina).ConfigureAwait(false);

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
                return BadRequest(res.Errors);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetGrafiche")]
        public async Task<IActionResult> GetGrafiche()
        {
            var resp = await sito.GetGrafiche().ConfigureAwait(false);

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
        [HttpPost("AddGrafica")]
        public async Task<IActionResult> AddGrafica([FromBody] Graphics immagine)
        {
            var resp = await sito.AddGrafica(immagine).ConfigureAwait(false);
            
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
        [HttpDelete("RemoveGrafica")]
        public async Task<IActionResult> RemoveGrafica([FromQuery] int id)
        {
            var resp = await sito.RemoveGrafica(id).ConfigureAwait(false);

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

        [Authorize(Roles ="Admin")]
        [HttpPut("ToggleGrafica")]
        public async Task<IActionResult> ToggleGrafica([FromBody] Graphics immagine)
        {
            var resp = await sito.ToggleGrafica(immagine).ConfigureAwait(false);
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
        [HttpGet("GetRedirezioni")]
        public async Task<IActionResult> GetRedirezioni()
        {
            var resp = await sito.GetRedirezioni().ConfigureAwait(false);

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
        [HttpPost("AddRedirezione")]
        public async Task<IActionResult> AddRedirezione([FromBody] Redirections redirection)
        {
            var resp = await sito.AddRedirezione(redirection).ConfigureAwait(false);

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
        [HttpDelete("RemoveRedirezione")]
        public async Task<IActionResult> RemoveRedirezione([FromQuery] int id)
        {
            var resp = await sito.RemoveRedirezione(id).ConfigureAwait(false);

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
        [HttpPut("UpdateRedirezione")]
        public async Task<IActionResult> UpdateRedirezione([FromBody] Redirections redirection)
        {
            var resp = await sito.AddRedirezione(redirection).ConfigureAwait(false);

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
        [HttpPut("ToggleRedirezione")]
        public async Task<IActionResult> ToggleRedirezione([FromBody] Redirections redirection)
        {
            var resp = await sito.ToggleRedirezione(redirection).ConfigureAwait(false);
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
        [HttpGet("Redirect")]
        public async Task<IActionResult> Redirect([FromQuery] int Id)
        {
            var res = validators.Validate(Id);
            if (res != null && res.IsValid)
            {
                var resp = await sito.GetRedirezione(Id).ConfigureAwait(false);
                if (resp != null)
                {
                    if (resp.success)
                    {
                        return Redirect(resp.Data.url);
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
                return BadRequest(res.Errors);
            }
        }

        [Authorize]
        [HttpGet("GetVideo")]
        public async Task<IActionResult> GetVideo()
        {
            var resp = await sito.GetVideos().ConfigureAwait(false);
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
        [HttpPost("AddVideo")]
        public async Task<IActionResult> AddVideo([FromBody] Videos video)
        {
            var resp = await sito.AddVideo(video).ConfigureAwait(false);
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
        [HttpDelete("RemoveVideo")]
        public async Task<IActionResult> RemoveVideo([FromQuery] int id)
        {
            var resp = await sito.RemoveVideo(id).ConfigureAwait(false);
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
        [HttpPut("ToggleVideo")]
        public async Task<IActionResult> ToggleVideo([FromBody] Videos video)
        {
            var resp = await sito.ToggleVideo(video).ConfigureAwait(false);
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
        [HttpPatch("EmptyCache")]
        public async Task<IActionResult> EmptyCache()
        {
            var resp = await sito.EmptyCache().ConfigureAwait(false);

            if (resp)
            {
                return Ok();
            }
            else
            {
                return Problem();
            }
        }

    }
}
