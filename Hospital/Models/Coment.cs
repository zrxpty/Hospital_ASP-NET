using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Coment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }

        [Display(Name = "Dcoter Type")]
        public int DocterId { get; set; }
        [ForeignKey("DocterId")]
        public virtual Docter Docter { get; set; }
    }
}
