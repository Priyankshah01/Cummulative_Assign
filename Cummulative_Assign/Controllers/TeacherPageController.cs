using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cummulative_Assign.Models;
using Cummulative1_schooldb.Models;
using MySql.Data.MySqlClient;

namespace Cummulative_Assign.Controllers
{
    /// <summary>
    /// Controller for handling web pages related to teachers.
    /// This controller interacts with the TeacherAPIController to retrieve teacher data.
    /// </summary>
    public class TeacherPageController : Controller
    {
        private readonly SchoolDbContext cummulative_assign1 = new SchoolDbContext();

        /// <summary>
        /// Displays a list of teachers based on an optional search key.
        /// Retrieves data from the TeacherAPIController and passes it to the List view.
        /// </summary>
        /// <param name="SearchKey">Optional search parameter to filter teachers.</param>
        /// <returns>A view displaying the list of teachers.</returns>
        public ActionResult List(string SearchKey)
        {
            TeacherAPIController controller = new TeacherAPIController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }

        /// <summary>
        /// Displays detailed information about a specific teacher.
        /// Retrieves teacher details from the TeacherAPIController.
        /// </summary>
        /// <param name="id">The unique identifier of the teacher.</param>
        /// <returns>A view displaying the details of the selected teacher.</returns>
        public ActionResult Show(int id)
        {
            TeacherAPIController controller = new TeacherAPIController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }
    }
}
