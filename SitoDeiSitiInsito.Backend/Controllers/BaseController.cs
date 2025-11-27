using Microsoft.AspNetCore.Mvc;
using SitoDeiSiti.Backend.Services;
using SitoDeiSiti.External.SumUp;

namespace Identity.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager userManager;
        protected readonly AbbonamentoManager abbonamentoManager;
        protected readonly DocumentoManager documentoManager;
        protected readonly EventiManager eventiManager;

        public BaseController(UserManager UserService, AbbonamentoManager AbbonamentoService,
            DocumentoManager DocumentoManager, EventiManager EventiManager)
        {
            userManager = UserService;
            abbonamentoManager = AbbonamentoService;
            documentoManager = DocumentoManager;
            eventiManager = EventiManager;
        }
    }
}
