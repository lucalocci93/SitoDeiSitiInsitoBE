using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SitoDeiSiti.DTOs;

namespace Identity.Controllers
{
    [Authorize]
    public class DocumentoController : BaseController
    {
        public DocumentoController(UserManager UserService, AbbonamentoManager AbbonamentoService,
            DocumentoManager documentoManager, EventiManager eventiManager)
            : base(UserService, AbbonamentoService, documentoManager, eventiManager)
        {
        }

        [HttpGet("GetUserDocuments")]
        public async Task<ActionResult> GetUserDocuments(Guid User)
        {
            var resp = await documentoManager.GetAllDocumentByUser(User);

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

        [HttpGet("GetDocument")]
        public async Task<ActionResult> Details(Guid Id)
        {
            var resp = await documentoManager.GetDocumentById(Id);

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
        [HttpPost("AddDocument")]
        public async Task<ActionResult> Create([FromBody] DocumentExt document)
        {
            var resp = await documentoManager.AddDocument(document);

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

        [HttpGet("GetDocumentType")]
        public async Task<ActionResult> GetDocumentType()
        {
            var resp = await documentoManager.GetDocumentType();

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
