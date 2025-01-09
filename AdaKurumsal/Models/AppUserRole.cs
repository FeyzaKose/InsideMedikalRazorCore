using System.ComponentModel.DataAnnotations.Schema;

namespace AdaKurumsal.Models
{
    [Table("AppUserRoles")]
    public class AppUserRole
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int UserId { get; set; }
        public bool isActive { get; set; }
    }
}
