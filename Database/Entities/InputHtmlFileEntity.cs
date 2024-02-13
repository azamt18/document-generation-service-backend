using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;

namespace Database.Entities;

[Table("input_html_files")]
public class InputHtmlFileEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Required] 
    [Column("created_on")]
    public DateTime CreatedOn { get; set; }
    
    [Required] 
    [Column("updated_on")]
    public DateTime UpdatedOn { get; set; }

    [Required]
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_on")]
    public DateTime? DeletedOn { get; set; }

    [Required] 
    [Column("title", TypeName = "text")]
    [StringLength(100)]
    public string Title { get; set; }
    
    [Required] 
    [Column("description", TypeName = "text")]
    [StringLength(100)]
    public string Description { get; set; }
    
    [Required] 
    [Column("status")]
    public FileStatus Status { get; set; }
    
    [Required] 
    [Column("guid", TypeName = "text")]
    [StringLength(50)]
    public string FileGuid { get; set; }
}