using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravlja.Models
{
    public enum StatusTermina
    {
        Slobodan,
        Zakazan
    }

    public class Termin
    {
        public Lekar Lekar { get; set; }
        public string kImeLekara { get; set; }
        public StatusTermina Statustermina { get; set; }
        public DateTime DatumIVremeZakazanogTermina { get; set; }
        public string OpisTerapije {  get; set; }
        public string ImePacijenta {  get; set; }

        public override string ToString()
        {
            return $"{kImeLekara}; {Statustermina}; {DatumIVremeZakazanogTermina.ToString("dd/MM/yyyy")}; {OpisTerapije}";
        }
    }
}