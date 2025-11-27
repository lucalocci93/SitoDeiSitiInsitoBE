using Identity.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.External.SumUp;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SitoDeiSiti.Controllers
{
    public class EchoController : ControllerBase
    {
        private readonly SitoDeiSitiInsitoContext db;
        private readonly SumUpManager SumUp;

        public EchoController(SitoDeiSitiInsitoContext context, SumUpManager sumUpManager)
        {
            db = context;
            SumUp = sumUpManager;
        }

        [AllowAnonymous]
        [HttpGet("echo")]
        public IActionResult Echo()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("CompleteEcho")]
        public async Task<IActionResult> CompleteEcho()
        {
            Echo echo = new();

            try
            {
                echo.CanConnectDatabase = db.Database.CanConnect();
            }
            catch(Exception ex)
            {
                echo.CanConnectDatabase = false;
            }

            try
            {
                db.Database.OpenConnection();
                db.Database.CloseConnection();

                echo.CanExecuteQuery = true;
            }
            catch(Exception ex)
            {
                echo.CanExecuteQuery = false;
            }

            try
            {
                var SumUpCheckoutList = await SumUp.GetSumUpCheckoutList().ConfigureAwait(false);
                echo.CanReachSumUp = SumUpCheckoutList != null &&
                    !SumUpCheckoutList.Any(s => s.error_code.Equals("-404"));
            }
            catch (Exception ex)
            {
                echo.CanReachSumUp = false;
            }

            return Ok(echo);
        }

    }
}
