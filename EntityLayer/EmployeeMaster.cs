using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EntityLayer
{

    [Table("EmployeeMaster")]
    public class EmployeeMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Row_Id { get; set; }

        [Required]
        [StringLength(8)]
        public string EmployeeCode { get; set; } 

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [ForeignKey("Country")]

        [Display(Name = "Country Name")]
        public int CountryId { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }

        [Required ()]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(15)]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(12)]
        
        public string PanNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string PassportNumber { get; set; }

        //[StringLength(100)]
        //public IFormFile ProfileImage { get; set; }


        [StringLength(100)]
        public string ProfileImage { get; set; }


        [StringLength(50)]
        public byte? Gender { get; set; } 

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public DateTime? DateOfJoinee { get; set; }

        //[Required]
        //public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}



