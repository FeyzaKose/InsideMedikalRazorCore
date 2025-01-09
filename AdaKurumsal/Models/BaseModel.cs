using System.ComponentModel.DataAnnotations;

namespace AdaKurumsal.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        [MaxLength(4, ErrorMessage = "Dil alanı en fazla 4 karakter olabilir.")]
        public string Language { get; set; }
    }
}
