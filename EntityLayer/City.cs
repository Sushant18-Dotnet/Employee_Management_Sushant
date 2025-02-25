using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int Row_Id { get; set; }

        [Required]
      
        public int StateId { get; set; }

        [Required]
        [StringLength(100)] 
        public string CityName { get; set; }

        // Navigation property
        [ForeignKey("StateId")]
        public State State { get; set; }
    }
}
