using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cummulative_Assign.Models;
using Cummulative1_schooldb.Models;
using MySql.Data.MySqlClient;

namespace Cummulative_Assign.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly SchoolDbContext cummulative_assign1 = new SchoolDbContext();

        // GET: TeacherPage/List
        public ActionResult List(string SearchKey)
        {
            TeacherAPIController controller = new TeacherAPIController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }

        // GET: TeacherPage/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherAPIController controller = new TeacherAPIController();
            Teacher NewTeacher = controller.FindTeacher(id);

            return View(NewTeacher);
        }
    }
}
