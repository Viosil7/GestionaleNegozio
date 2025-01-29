namespace GestionaleNegozio.Models
{
    public class Ordine
    {
        public int Id { get; set; }
        public int IdNegozio { get; set; }
        public int IdProdotto { get; set; }
        public string Nota { get; set; }
        public int QuantitaTotale { get; set; }
        public DateTime DataOrdine { get; set; }
    }
}
