using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using Cummulative_Assign.Models;
using Cummulative1_schooldb.Models;
using MySql.Data.MySqlClient;

namespace Cummulative_Assign.Controllers
{
    /// <summary>
    /// API Controller for managing teacher data.
    /// </summary>
    public class TeacherAPIController : ApiController
    {
        /// <summary>
        /// Retrieves a list of teachers from the database, filtered by Hire Date if provided.
        /// </summary>
        /// <param name="searchDate">Optional search parameter to filter teachers by hire date.</param>
        /// <returns>A list of teachers matching the criteria.</returns>
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of teachers in the system filtered by an optional search key.
        /// </summary>
        /// <param name="SearchKey">Optional search key to filter teachers by first name, last name, full name, hire date, or salary.</param>
        /// <returns>
        /// A list of teacher objects.

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Teachers WHERE LOWER(teacherfname) LIKE LOWER(@Key) OR LOWER(teacherlname) LIKE LOWER(@Key) OR LOWER(CONCAT(teacherfname, ' ', teacherlname)) LIKE LOWER(@Key) or hiredate Like @Key or DATE_FORMAT(hiredate, '%d-%m-%Y') Like @Key or salary LIKE @Key ";
            cmd.Parameters.AddWithValue("@Key", "%" + SearchKey + "%");
            cmd.Prepare();
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Teacher> Teachers = new List<Teacher>();
            while (ResultSet.Read())
            {
                Teachers.Add(new Teacher()
                {
                    TeacherId = Convert.ToInt32(ResultSet["teacherId"]),
                    TeacherFname = ResultSet["teacherFname"].ToString(),
                    TeacherLname = ResultSet["teacherLname"].ToString(),
                    EmployeeNumber = ResultSet["employeenumber"].ToString(),
                    HireDate = Convert.ToDateTime(ResultSet["hiredate"]),
                    Salary = Convert.ToDecimal(ResultSet["salary"])
                });
            }
            Conn.Close();
            return Teachers;
        }

        /// <summary>
        /// Finds a teacher in the system given an ID.
        /// </summary>
        /// <param name="id">The teacher's primary key.</param>
        /// <returns>
        /// A teacher object, or null if the teacher is not found.
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = null;
            MySqlConnection Conn = School.AccessDatabase();
            try
            {
                Conn.Open();
                MySqlCommand cmd = Conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Teachers WHERE teacherid = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                if (ResultSet.Read())
                {
                    NewTeacher = new Teacher
                    {
                        TeacherId = Convert.ToInt32(ResultSet["teacherId"]),
                        TeacherFname = ResultSet["teacherFname"].ToString(),
                        TeacherLname = ResultSet["teacherLname"].ToString(),
                        EmployeeNumber = ResultSet["employeenumber"].ToString(),
                        HireDate = Convert.ToDateTime(ResultSet["hiredate"]),
                        Salary = Convert.ToDecimal(ResultSet["salary"])
                    };
                    Debug.WriteLine($"Teacher data read: ID = {NewTeacher.TeacherId}, Name = {NewTeacher.TeacherFname} {NewTeacher.TeacherLname}");
                }
                ResultSet.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in FindTeacher (Teacher Data): " + ex.Message);
                return null;
            }
            finally
            {
                Conn.Close();
            }
            return NewTeacher;

        }
        /// <summary>
        /// Adds a teacher to the MySQL Database.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teacher's table.</param>
        /// <returns>
        /// A response indicating the success or failure of the operation.
        /// Returns a 400 Bad Request response if the provided information is missing or incorrect.
        /// Returns a 200 OK response if the teacher is added successfully.
        /// </returns>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Priyank",
        ///	"TeacherLname":"Shah",
        ///	"EmployeeNumber":"T1234",
        ///	"HireDate":"04-02-2025"
        ///	"Salary": 95
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        [Route("api/TeacherData/AddTeacher")]
        public IHttpActionResult AddTeacher([FromBody] Teacher NewTeacher)
        {

            if (string.IsNullOrEmpty(NewTeacher.TeacherFname) || string.IsNullOrEmpty(NewTeacher.TeacherLname) ||
                string.IsNullOrEmpty(NewTeacher.EmployeeNumber) || NewTeacher.HireDate == null || NewTeacher.HireDate > DateTime.Now || NewTeacher.Salary < 0)
            {
                // Return a 400 Bad Request response with an error message
                return BadRequest("Invalid data provided for adding the teacher.");
            }

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname,@TeacherLname,@Employeenumber, @HireDate, @Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
            return Ok("Teacher added successfully");
        }

        /// <summary>
        /// Deletes a teacher from the connected MySQL Database if the ID of that teacher exists.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <returns>
        /// A response indicating the success of the operation..
        /// Returns a 200 OK response if the teacher is updated successfully.
        /// </returns>
        /// <example>DELETE /api/TeacherData/DeleteTeacher/3</example>
        [HttpDelete]
        [Route("api/TeacherData/DeleteTeacher/{id}")]
        public IHttpActionResult DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL QUERY
            cmd.CommandText = "DELETE FROM teachers WHERE teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                Conn.Close();

                if (rowsAffected > 0)
                {
                    return Ok("Teacher Deleted successfully");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Error deleting teacher: " + ex.Message);
                return InternalServerError();
            }
            finally
            {
                if (Conn.State == System.Data.ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }
    }

