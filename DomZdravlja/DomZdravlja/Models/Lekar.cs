using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravlja.Models
{
    public class Lekar : Korisnik
    {

        public List<Termin>ListaZakazanihISlobodnihTermina { get; set; } = new List<Termin>();
        public override string ToString()
        {
             return $"{base.ToString()},Termini: {string.Join(",", ListaZakazanihISlobodnihTermina)}";
        }
    }
}