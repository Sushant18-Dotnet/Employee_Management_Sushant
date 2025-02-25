using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
   

        [Table("State")]
        public class State
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


            // Row id is stateid here
            public int Row_Id { get; set; }


            [Required]
            public int CountryId { get; set; }

            [Required]
            [StringLength(100)]
            public string StateName { get; set; }

           
            [ForeignKey("CountryId")]
            public Country Country { get; set; }
        }
    }

