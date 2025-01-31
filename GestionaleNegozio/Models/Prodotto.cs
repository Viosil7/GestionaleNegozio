using System.ComponentModel.DataAnnotations;

namespace GestionaleNegozio.Models
{
    public class Prodotto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Descrizione { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Prezzo { get; set; }
    }
}
