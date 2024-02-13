using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("output_pdf_files")]
    public class OutputPdfFileEntity
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

        [Column("input_html_file_id")]
        [ForeignKey(nameof(InputHtmlFile))]
        public long InputHtmlFileId { get; set; }

        public virtual InputHtmlFileEntity InputHtmlFile { get; set; }

    }
}
