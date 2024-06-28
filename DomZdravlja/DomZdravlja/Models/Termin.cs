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
        public StatusTermina StatusTermina { get; set; }
        public DateTime DatumIVremeZakazanogTermina { get; set; }
        public string OpisTerapije {  get; set; }
    }
}