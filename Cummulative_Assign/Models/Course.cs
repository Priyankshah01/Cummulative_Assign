using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cummulative_Assign.Models
{
    public class Course : Controller
    {
        // GET: Course
        public int CourseId;
        public string CourseCode;
        public int TeacherId;
        public DateTime StartDate;
        public DateTime FinishDate;
        public string CourseName;
        public virtual Teacher Teacher { get; set; }

    }
}