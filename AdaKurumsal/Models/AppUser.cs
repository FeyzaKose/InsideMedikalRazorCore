using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdaKurumsal.Models
{
    [Table("AppUsers")]
    public class AppUser : BaseModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz")]
        [MaxLength(50, ErrorMessage = "Name alanı en fazla 50 karakter olabilir.")]
        public string UserName { get; set; }



        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        [MaxLength(15, ErrorMessage = "Şifre alanı en fazla 80 karakter olabilir.")]
        [Column(TypeName = "nvarchar(80)")]
        //Hash lenmiş olarak kayıt edilir ve 60 karakter uzunlupundadır.
        public string Password { get; set; }



        [Required(ErrorMessage = "Şifre Tekrarı boş bırakılamaz")]
        [NotMapped]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [MaxLength(15, ErrorMessage = "Name alanı en fazla 15 karakter olabilir.")]
        public string PasswordConfirmed { get; set; }



        [DataType(DataType.EmailAddress, ErrorMessage = "Geçerli bir email adresi giriniz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girinizz")]
        [Required(ErrorMessage = "Email boş bırakılamaz")]
        [MaxLength(30, ErrorMessage = "Name alanı en fazla 30 karakter olabilir.")]
        public string Email { get; set; }



        public bool isActive { get; set; }
        public bool isConfirm { get; set; }
    }
}
