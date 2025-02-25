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
   
        public class EmployeeMasters
        {

        public int Row_Id { get; set; }
        public string EmployeeCode { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = "Country Name")]
        public int CountryId { get; set; }


        [Display(Name = "State Name")]
        public int StateId { get; set; }

        [Display(Name = "City Name")]
        public int CityId { get; set; }
        public string CountryName { get; set; } 
        public string stateName { get; set; }   
        public string cityName { get; set; }   
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string PanNumber { get; set; }
        public string PassportNumber { get; set; }
        public string ProfileImage { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfJoinee { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
    }


