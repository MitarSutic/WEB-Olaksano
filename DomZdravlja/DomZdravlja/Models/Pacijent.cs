using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravlja.Models
{
    public class Pacijent : Korisnik
    {

        public string JMBG { get; set; }
        public List<Termin> ListaZakazanihTermina { get; set; } = new List<Termin>();

        public override string ToString()
        {
            return $"{base.ToString()}, JMBG: {JMBG}, Termini: {string.Join(", ", ListaZakazanihTermina)}";
        }
    }
}