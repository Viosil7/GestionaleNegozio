﻿namespace GestionaleNegozio.Models
{
    public class Ordine
    {
        public int Id { get; set; }
        public int IdNegozio { get; set; }
        public string Nota { get; set; }
        public DateTime DataOrdine { get; set; }
    }
}
