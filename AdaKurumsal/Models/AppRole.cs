using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdaKurumsal.Models
{
    [Table("AppRoles")]
    public class AppRole
    {
        public int Id { get; set; }
        [MaxLength(10, ErrorMessage = "Name alanı en fazla 30 karakter olabilir.")]
        public string RolName { get; set; }
        public bool isActive { get; set; }
    }
}
