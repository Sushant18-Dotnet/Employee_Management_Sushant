using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
   
        [Table("Country")]
        public class Country
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


            // row id is country id here
            public int Row_Id { get; set; }

            [Required]
            [StringLength(100)] 



            public string CountryName { get; set; }
        }
    }

