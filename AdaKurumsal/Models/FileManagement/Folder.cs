using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdaKurumsal.Models.FileManagement
{
    public class Folder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Path { get; set; }

        public Guid? ParentFolderId { get; set; }

        [ForeignKey("ParentFolderId")]
        public virtual Folder ParentFolder { get; set; }

        public virtual ICollection<Folder> SubFolders { get; set; }
        public virtual ICollection<Image> Images { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
