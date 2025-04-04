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

        [Required]
        public string TeacherFname { get; set; }

        [Required]
        public string TeacherLname { get; set; }

        [Required]
        public string EmployeeNumber { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public decimal Salary { get; set; }

        public virtual ICollection<Course> Courses { get; set; } // Plural 'Courses' for collection

    }
}