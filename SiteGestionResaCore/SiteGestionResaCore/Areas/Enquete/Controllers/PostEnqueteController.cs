using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Controllers
{
    [Area("Enquete")]
    public class PostEnqueteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
