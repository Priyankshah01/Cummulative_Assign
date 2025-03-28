using System;
using System.Collections.Generic;
using System.Web.Http;
using Cummulative_Assign.Models;
using Cummulative1_schooldb.Models;
using MySql.Data.MySqlClient;

namespace Cummulative_Assign.Controllers
{
    /// <summary>
    /// API Controller for accessing teacher data.
    /// This controller provides endpoints to retrieve a list of teachers
    /// and find a specific teacher by their ID.
    /// </summary>
    public class TeacherAPIController : ApiController
    {
        private readonly SchoolDbContext cummulative_assign1 = new SchoolDbContext();

        /// <summary>
        /// Retrieves a list of teachers from the database.
        /// Allows optional searching by teacher first name, last name, hire date, or salary.
        /// </summary>
        /// <param name="SearchKey">An optional search key to filter teachers by name, hire date, or salary.</param>
        /// <returns>A list of teacher objects.</returns>
 [HttpGet]
 [Route("api/TeacherData/ListTeachers")]
 public IEnumerable<Teacher> ListTeachers(string searchKey)
 {
     // Create a connection to the database
     MySqlConnection Conn = cummulative_assign1.AccessDatabase();
     Conn.Open();

     // Prepare SQL query to retrieve all teachers
     MySqlCommand cmd = Conn.CreateCommand();
     cmd.CommandText = "SELECT * FROM Teachers";
     cmd.Prepare();

            // Execute the query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // Create a list to hold teacher objects
            List<Teacher> Teachers = new List<Teacher>();

            // Loop through each row in the result set
            while (ResultSet.Read())
            {
                // Retrieve column information
                int TeacherId = Convert.ToInt32(ResultSet["teacherId"]);
                string TeacherFname = ResultSet["teacherFname"].ToString();
                string TeacherLname = ResultSet["teacherLname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                // Create a new Teacher object and populate its properties
                Teacher NewTeacher = new Teacher
                {
                    TeacherId = TeacherId,
                    TeacherFname = TeacherFname,
                    TeacherLname = TeacherLname,
                    EmployeeNumber = EmployeeNumber,
                    HireDate = HireDate,
                    Salary = Salary
                };

                // Add the teacher to the list
                Teachers.Add(NewTeacher);
            }

            // Close the database connection
            Conn.Close();

            // Return the list of teachers
            return Teachers;
        }

        /// <summary>
        /// Retrieves a specific teacher from the database based on the given ID.
        /// </summary>
        /// <param name="id">The unique identifier of the teacher.</param>
        /// <returns>The teacher object with details, or null if no teacher is found.</returns>
                [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            // Create a new Teacher object
            Teacher NewTeacher = new Teacher();

            // Create a connection to the database
            MySqlConnection Conn = cummulative_assign1.AccessDatabase();
            Conn.Open();

            // Prepare SQL query to retrieve teacher information
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Teachers WHERE teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            // Execute the query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // Populate the teacher object with information from the result set
            while (ResultSet.Read())
            {
                NewTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherId"]);
                NewTeacher.TeacherFname = ResultSet["teacherFname"].ToString();
                NewTeacher.TeacherLname = ResultSet["teacherLname"].ToString();
                NewTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                NewTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                NewTeacher.Salary = Convert.ToDecimal(ResultSet["salary"]);
            }
            ResultSet.Close(); // Close the result set

            // Close the database connection
            Conn.Close();

            // Return the teacher object
            return NewTeacher;
        }
    }
}
