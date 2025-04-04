using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Cummulative_Assign.Models;
using Cummulative1_schooldb.Models;

namespace Cummulative_Assign.Controllers
{
    /// <summary>
    /// Controller for handling web pages related to teachers.
    /// This controller interacts with the TeacherAPIController to retrieve teacher data.
    /// </summary>
    public class TeacherPageController : Controller
    {
        private readonly SchoolDbContext Cummulative_Assign = new SchoolDbContext();

        /// <summary>
        /// Displays a list of teachers based on an optional search key (Hire Date).
        /// Retrieves data from the TeacherAPIController and passes it to the List view.
        /// </summary>
        /// <param name="SearchKey">Optional search parameter to filter teachers by Hire Date.</param>
        /// <returns>A view displaying the list of teachers.</returns>
        public ActionResult List(string SearchKey)
        {
            TeacherAPIController controller = new TeacherAPIController();

            // Convert SearchKey to DateTime (if provided)
            DateTime? searchDate = null;
            if (!string.IsNullOrEmpty(SearchKey))
            {
                DateTime parsedDate;
                if (DateTime.TryParse(SearchKey, out parsedDate))
                {
                    searchDate = parsedDate;
                }
            }

            // Fetch teachers and filter by Hire Date if applicable
            IEnumerable<Teacher> Teachers = controller.ListTeachers(null); // Get all teachers
            if (searchDate.HasValue)
            {
                Teachers = Teachers.Where(t => t.HireDate.Date == searchDate.Value.Date);
            }

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

        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Displays a form to add a new teacher.
        /// </summary>
        /// <returns>The view displaying a form to add a new teacher using AJAX request.</returns>
        /// <example>
        /// GET : /TeacherPage/Ajax_New
        /// </example>
        public ActionResult Ajax_New()
        {
            return View();
        }

        /// <summary>
        /// Creates a new teacher with the provided information.
        /// </summary>
        /// <param name="TeacherFname">The first name of the teacher.</param>
        /// <param name="TeacherLname">The last name of the teacher.</param>
        /// <param name="EmployeeNumber">The employee number of the teacher.</param>
        /// <param name="HireDate">The hire date of the teacher.</param>
        /// <param name="Salary">The salary of the teacher.</param>
        /// <returns>
        /// A response indicating the success or failure of the operation.
        /// Returns a 200 OK response if the teacher is added successfully.
        /// Returns a 400 Bad Request response if the provided information is missing or incorrect.
        /// </returns>
        /// <example>
        /// Example of POST request body
        /// POST /Teacher/Create
        /// {
        ///     "TeacherFname": "Priyank",
        ///     "TeacherLname": "Shah",
        ///     "EmployeeNumber": "T1234",
        ///     "HireDate": "2025-04-01",
        ///     "Salary": 65
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, decimal? Salary)
        {
            // Check for missing information
            if (string.IsNullOrEmpty(TeacherFname) || string.IsNullOrEmpty(TeacherLname) ||
                string.IsNullOrEmpty(EmployeeNumber) || HireDate == null || HireDate > DateTime.Now || Salary == null || Salary < 0)
            {
                // Return the view with an error message
                ViewBag.Message = "Missing or incorrect information when adding a teacher";
                return View("New");
            }
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary ?? 0;

            TeacherAPIController controller = new TeacherAPIController();
            controller.AddTeacher(NewTeacher);

            // Return the view to list page
            return RedirectToAction("List");
        }

        /// <summary>
        /// Displays a confirmation page to delete a specific teacher.
        /// </summary>
        /// <param name="id">The ID of the teacher to delete.</param>
        /// <returns>The view displaying a confirmation page to delete the teacher.</returns>
        /// <example>
        /// GET : /Teacher/DeleteConfirm/{id}
        /// </example>
        public ActionResult DeleteConfirm(int id)
        {
            TeacherAPIController controller = new TeacherAPIController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        /// <summary>
        /// Displays a confirmation page to delete a specific teacher using AJAX.
        /// </summary>
        /// <param name="id">The ID of the teacher to delete.</param>
        /// <returns>The view displaying a confirmation page to delete the teacher using AJAX request.</returns>
        /// <example>
        /// GET : /Teacher/Ajax_DeleteConfirm/{id}
        /// </example>
        public ActionResult Ajax_DeleteConfirm(int id)
        {
            TeacherAPIController controller = new TeacherAPIController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        /// <summary>
        /// Deletes a specific teacher from the system.
        /// </summary>
        /// <param name="id">The ID of the teacher to delete.</param>
        /// <returns>A response indicating the success or failure of the operation.</returns>

        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherAPIController controller = new TeacherAPIController();
            controller.DeleteTeacher(id);

           
            // Return the view to list page
            return RedirectToAction("List");
        }
    }
}
