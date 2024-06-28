using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomZdravlja.Controllers
{
    public class PacijentController : Controller
    {
        // GET: Pacijent
        public ActionResult Index()
        {
            return View();
        }
    }
}