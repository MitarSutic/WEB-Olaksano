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
            Dictionary<string, Korisnik> korisnici = DataHelper.UcitajKorisnike("~/App_Data/korisnici.csv");
            List<Administrator> admini = DataHelper.UcitajAdmine("~/App_Data/administratori.csv");
            List<Lekar> lekari = DataHelper.UcitajLekare("~/App_Data/lekari.csv");
            List<Pacijent> pacijenti = DataHelper.UcitajPacijente("~/App_Data/pacijenti.csv");           
            List<Termin> slobodniIZakazaniTermini = DataHelper.UcitajSlobodneIZakazaneTermine("~/App_Data/slobodni i zakazani termini.csv", "~/App_Data/lekari.csv");
            HttpContext.Current.Application["admini"] = admini;
            HttpContext.Current.Application["korisnici"] = korisnici;
            HttpContext.Current.Application["lekari"] = lekari;
            HttpContext.Current.Application["pacijenti"] = pacijenti;
            HttpContext.Current.Application["sIztermini"] = slobodniIZakazaniTermini;
        }
    }
}