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
                "Lazio" => new[] { "Affile", "Agosta", "Albano Laziale", "Allumiere", "Anguillara Sabazia", "Anticoli Corrado", "Anzio", "Arcinazzo Romano", "Artena", "Aspra",
                    "Assisi", "Bellegra", "Boville Ernica", "Bracciano", "Campagnano di Roma", "Campoli Appennino", "Canale Monterano", "Capena", "Carpineto Romano",
                    "Castel Gandolfo", "Castel Madama", "Cerveteri", "Civitavecchia", "Colonna", "Fiano Romano", "Frascati", "Gallicano nel Lazio", "Genazzano",
                    "Genzano di Roma", "Grottaferrata", "Guidonia Montecelio", "Lanuvio", "Lido di Ostia", "Lodovico Einaudi", "Maccarese", "Marino", "Mentana",
                    "Monte Porzio Catone", "Morolo", "Nettuno", "Olevano Romano", "Palestrina", "Palombara Sabina", "Pomezia", "Ponzano Romano", "Roma", "Rocca di Papa",
                    "Rocca Priora", "San Cesareo", "San Giovanni in Laterano", "San Lorenzo", "Santa Marinella", "Sant’Angelo Romano", "Santo Stefano Roma", "Sora",
                    "Subiaco", "Tivoli", "Valmontone", "Velletri", "Villanova di Guidonia", "Vivi la città", "Acuto", "Alatri", "Arnara", "Atina", "Azzio", "Ceccano", "Ceccano", "Frosinone", "Guarcino", "Isola del Liri", "Isoletta", "Roccasecca",
                    "San Donato Val di Comino", "Sora", "Veroli", "Vico del Lazio", "Ceccano", "Viterbo", "Acquapendente", "Bagnoregio", "Barbarano Romano", "Bolsena", "Capranica", "Canino", "Celleno", "Civita", "Civitella D’Agliano", "Fabrica di Roma",
                    "Farnese", "Gradoli", "Grotte di Castro", "Montalto di Castro", "Montefiascone", "Nepi", "Orte", "Piansano", "San Lorenzo Nuovo", "San Vito Romano",
                    "Sutri", "Viterbo", "Vitorchiano", "Aprilia", "Borgo Sabotino", "Cisterna di Latina", "Fondi", "Latina", "Minturno", "Sabaudia", "Terracina", "Pontinia", "Priverno", "Sezze",
                    "Sermoneta", "Cori", "Cisterna di Latina", "Roccagorga", "San Felice Circeo", "Cisterna di Latina", "Rieti", "Amatrice", "Antrodoco", "Cittaducale", "Fara in Sabina", "Greccio", "Montopoli di Sabina", "Poggio Moiano", "Poggio Mirteto", "Rieti", "Rocca Sinibalda",
                    "Cantalice", "Fiamignano", "Leonessa", "Lunga" },
                "Campania" => new[] { "Acerra", "Anacapri", "Boscoreale", "Boscotrecase", "Brusciano", "Caivano", "Campagna", "Capri", "Casamarciano", "Caserta", "Castellammare di Stabia", "Cercola",
                    "Cicciano", "Cisterna di Latina", "Napoli", "Forio", "Giugliano in Campania", "Gragnano", "Lacco Ameno", "Massa Lubrense", "Melito di Napoli", "Meta",
                    "Nola", "Ospedaletto d’Alpinolo", "Pozzuoli", "Portici", "Sant’Anastasia", "Sant'Antimo", "Sorrento", "Torre del Greco", "Torre Annunziata", "Vico Equense", "Agropoli",
                    "Ariano Irpino", "Battipaglia", "Bellizzi", "Capaccio-Paestum", "Cava de' Tirreni", "Eboli", "Fisciano", "Giffoni Valle Piana", "Mercato San Severino", "Minori",
                    "Nocera Inferiore", "Nocera Superiore", "Pagani", "Pellezzano", "Polla", "Salerno", "Scafati", "Serre", "Siano", "Teggiano", "Avellino", "Baiano", "Bagnoli Irpino",
                    "Calabritto", "Capriglia Irpina", "Carife", "Casalbore", "Cassano Irpino", "Chiusano di San Domenico", "Contrada", "Fontanarosa", "Grottolella", "Lacedonia", "Lapio",
                    "Lauro", "Monteforte Irpino", "Montella", "Nusco", "San Mango sul Calore", "San Martino Valle Caudina", "San Michele di Serino", "Solofra", "Summonte", "Sturno",
                    "Valle di Lauro", "Vallata", "Zungoli", "Aversa", "Capua", "Castel Volturno", "Cesa", "Cervino", "Casanova", "Cellole", "Cervinara", "Frignano", "Gricignano di Aversa",
                    "Marcianise", "Maddaloni", "Mignano Monte Lungo", "Mondragone", "Piedimonte Matese", "Pignataro Maggiore", "San Nicola la Strada", "San Prisco", "Santa Maria Capua Vetere",
                    "Sessa Aurunca", "Teano", "Airola", "Amorosi", "Apice", "Benevento", "Bucciano", "Calvi", "Campolattaro", "Ceppaloni", "Colliano", "Dugenta", "Faicchio", "Guardia Sanframondi",
                    "Jelsi", "Laureana Cilento", "Luzzano", "Melizzano", "Molinara", "Montefusco", "Montesarchio", "Paolisi", "Pannarano", "San Leucio del Sannio", "San Salvatore Telesino",
                    "Sant'Agata de' Goti", "Sant'Arcangelo Trimonte", "Solopaca", "Tocco Caudio", "Valle di Maddaloni", "San Bartolomeo in Galdo"
                        },
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
