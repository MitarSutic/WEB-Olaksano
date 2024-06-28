using DomZdravlja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DomZdravlja
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Dictionary<string, Korisnik> korisnici = DataHelper.UcitajKorisnike("~/App_Data/korisnici.txt");
            HttpContext.Current.Application["korisnici"] = korisnici;
        }
    }
}
