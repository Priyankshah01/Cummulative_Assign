using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cummulative_Assign.Models
{
     /// <summary>
     /// Represents a student in the school system.
     /// </summary>
     public class Student
    {
        // Properties of the Student entity
        public int StudentId;
        public string StudentFname;
        public string StudentLname;
        public string StudentNumber;
        public DateTime EnrolDate;
    }
}