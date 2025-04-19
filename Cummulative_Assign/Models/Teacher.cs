using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cummulative_Assign.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string TeacherFname { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string TeacherLname { get; set; }

        [Required(ErrorMessage = "Employee number is required")]
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }

        [Required(ErrorMessage = "Hire date is required")]
        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(Teacher), "ValidateHireDate")]
        public DateTime HireDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive value")]
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public static ValidationResult ValidateHireDate(DateTime hireDate, ValidationContext context)
        {
            if (hireDate > DateTime.Today)
            {
                return new ValidationResult("Hire date cannot be in the future");
            }
            return ValidationResult.Success;
        }
    }
}