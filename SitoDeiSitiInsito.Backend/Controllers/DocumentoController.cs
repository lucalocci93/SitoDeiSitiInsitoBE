using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SitoDeiSiti.Backend.Services;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Validators;

namespace Identity.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class DocumentoController : BaseController
    {
        private readonly GetUserDocumentValidator GetUserDocumentValidator;
        private readonly GetDocumentValidator GetDocumentValidator;

        public DocumentoController(UserManager UserService, AbbonamentoManager AbbonamentoService,
            DocumentoManager documentoManager, EventiManager eventiManager)
            : base(UserService, AbbonamentoService, documentoManager, eventiManager)
        {
            GetUserDocumentValidator = new();
            GetDocumentValidator = new();
        }

        [HttpGet("GetUserDocuments")]
        public async Task<ActionResult> GetUserDocuments(Guid User)
        {
            var validationResult = await GetUserDocumentValidator.ValidateAsync(User).ConfigureAwait(false);

            if (validationResult != null && validationResult.IsValid)
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
            else
            {
                return BadRequest(validationResult.Errors.FirstOrDefault()?.ErrorMessage);
            }
        }

        [HttpGet("GetDocument")]
        public async Task<ActionResult> Details(Guid Id)
        {
            var validationResult = await GetDocumentValidator.ValidateAsync(Id).ConfigureAwait(false);

            if (validationResult != null && validationResult.IsValid)
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
            else
            {
                return BadRequest(validationResult != null ? validationResult.Errors.FirstOrDefault()?.ErrorMessage : string.Empty);
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
