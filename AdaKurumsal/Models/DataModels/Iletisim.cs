using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdaKurumsal.Models.DataModels
{
    [Table("Iletisim")]
    public class Iletisim : BaseModel
    {
        [MaxLength(4, ErrorMessage = "Dil alanı en fazla 4 karakter olabilir.")]
        public string Dil { get; set; }



        [MaxLength(20, ErrorMessage = "1. Telefon alanı en fazla 20 karakter olabilir.")]
        public string? Telefon1 { get; set; }



        [MaxLength(20, ErrorMessage = "2. Telefon alanı en fazla 20 karakter olabilir.")]
        public string? Telefon2 { get; set; }



        [MaxLength(25, ErrorMessage = "1. Email alanı en fazla 25 karakter olabilir.")]
        public string? Email1 { get; set; }



        [MaxLength(25, ErrorMessage = "2. Email alanı en fazla 25 karakter olabilir.")]
        public string? Email2 { get; set; }




        [MaxLength(75, ErrorMessage = "Footer Adres satırı 1 alanı en fazla 75 karakter olabilir.")]
        public string? FooterAdresSatir { get; set; }




        [MaxLength(75, ErrorMessage = "Footer Adres satırı 2 alanı en fazla 75 karakter olabilir.")]
        public string? FooterAdresSatir2 { get; set; }




        [MaxLength(75, ErrorMessage = "Adres satırı 1 alanı en fazla 75 karakter olabilir.")]
        public string? AdresSatir1 { get; set; }




        [MaxLength(75, ErrorMessage = "Adres satırı 2 alanı en fazla 75 karakter olabilir.")]
        public string? AdresSatir2 { get; set; }
    }
}
