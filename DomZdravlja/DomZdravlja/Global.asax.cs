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
            List<Lekar> lekari = DataHelper.UcitajLekare("~/App_Data/lekari.txt");
            List<Pacijent> pacijenti = DataHelper.UcitajPacijente("~/App_Data/pacijenti.txt");
            List<Termin> slobodniTermini = DataHelper.UcitajSlobodneTermine("~/App_Data/slobodni termini.txt");
            List<Termin> slobodniIZakazaniTermini = DataHelper.UcitajSlobodneIZakazaneTermine("~/App_Data/zakazani i slobodni termini.txt");
            HttpContext.Current.Application["korisnici"] = korisnici;
            HttpContext.Current.Application["lekari"] = lekari;
            HttpContext.Current.Application["pacijenti"] = pacijenti;
            HttpContext.Current.Application["stermini"] = slobodniTermini;
            HttpContext.Current.Application["sIztermini"] = slobodniIZakazaniTermini;
        }
    }
}