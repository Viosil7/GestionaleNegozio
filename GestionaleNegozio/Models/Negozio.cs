using System.ComponentModel.DataAnnotations;

namespace GestionaleNegozio.Models
{
    public class Negozio
    {
        public int Id { get; set; }

        [Required]
        [RegionValidation]
        public string Regione { get; set; }

        [Required]
        [CityValidation]
        public string Città { get; set; }

        [Required]
        public string Indirizzo { get; set; }
    }

    public class RegionValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string region = value as string;
            string[] validRegions = { "Lazio", "Campania" };

            if (!validRegions.Contains(region))
            {
                return new ValidationResult("Seleziona una regione valida (Lazio o Campania)");
            }

            return ValidationResult.Success;
        }
    }

    public class CityValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var negozio = (Negozio)validationContext.ObjectInstance;
            string city = value as string;

            var validCities = negozio.Regione switch
            {
                "Lazio" => new[] { "Roma", "Latina", "Frosinone", "Viterbo", "Rieti" },
                "Campania" => new[] { "Napoli", "Salerno", "Caserta", "Avellino", "Benevento" },
                _ => Array.Empty<string>()
            };

            if (!validCities.Contains(city))
            {
                return new ValidationResult("Seleziona una città valida per la regione selezionata");
            }

            return ValidationResult.Success;
        }
    }
}
