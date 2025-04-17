using Identity.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using System.Net.NetworkInformation;

namespace SitoDeiSiti.Controllers
{
    public class EchoController : ControllerBase
    {
        private readonly SitoDeiSitiInsitoContext db;
        private readonly IOptions<SitoDeiSiti.DTOs.SumUp> sumUp;

        public EchoController(SitoDeiSitiInsitoContext context, IOptions<SumUp> options)
        {
            db = context;
            sumUp = options;
        }

        [AllowAnonymous]
        [HttpGet("echo")]
        public IActionResult Echo()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("CompleteEcho")]
        public IActionResult CompleteEcho()
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

            Ping ping = new();

            try
            {
                PingReply reply = ping.Send(sumUp.Value.SumUpCheckoutUrl);
                if (reply.Status == IPStatus.Success)
                {
                    echo.CanReachSumUp = true;
                }
                else
                {
                    echo.CanReachSumUp = false;
                }

                //PingReply replytoken = ping.Send(sumUp.Value.SumUpAuthUrl);
                //{
                //    if (replytoken.Status == IPStatus.Success)
                //    {
                //    }
                //    else
                //    {
                //        echo.CanReachSumUp = false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                echo.CanReachSumUp = false;
            }

            return Ok(echo);
        }

    }
}
