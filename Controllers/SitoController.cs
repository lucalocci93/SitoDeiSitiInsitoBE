using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Services;
using System.Threading.Tasks;

namespace SitoDeiSiti.Controllers
{
    public class SitoController : ControllerBase
    {
        private readonly SitoManager sito;
        public SitoController(SitoManager manager)
        {
            sito = manager;
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
        [HttpGet("GetImmmaginiByPagina")]
        public async Task<IActionResult> GetImmmaginiByPagina([FromQuery] int Pagina)
        {
            var resp = await sito.GetImmaginiByPagina(Pagina).ConfigureAwait(false);

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
        [HttpGet("GetImmmagini")]
        public async Task<IActionResult> GetImmmagini()
        {
            var resp = await sito.GetImmagini().ConfigureAwait(false);

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
        [HttpPost("AddImmagine")]
        public async Task<IActionResult> AddImmagine([FromBody] Images immagine)
        {
            var resp = await sito.AddImmagine(immagine).ConfigureAwait(false);
            
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
        [HttpDelete("RemoveImmagine")]
        public async Task<IActionResult> RemoveImmagine([FromQuery] int id)
        {
            var resp = await sito.RemoveImmagine(id).ConfigureAwait(false);

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
