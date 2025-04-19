using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using Cummulative_Assign.Models;
using Cummulative1_schooldb.Models;
using MySql.Data.MySqlClient;

namespace Cummulative_Assign.Controllers
{
    /// <summary>
    /// API Controller for managing teacher data.
    /// </summary>
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class TeacherAPIController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

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

        [HttpPost]
        [Route("api/TeacherData/AddTeacher")]
        public IHttpActionResult AddTeacher([FromBody] Teacher NewTeacher)
        {
            if (string.IsNullOrEmpty(NewTeacher.TeacherFname) || string.IsNullOrEmpty(NewTeacher.TeacherLname) ||
                string.IsNullOrEmpty(NewTeacher.EmployeeNumber) || NewTeacher.HireDate == null || NewTeacher.HireDate > DateTime.Now || NewTeacher.Salary < 0)
            {
                return BadRequest("Invalid data provided for adding the teacher.");
            }

            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
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

        [HttpDelete]
        [Route("api/TeacherData/DeleteTeacher/{id}")]
        public IHttpActionResult DeleteTeacher(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
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

        [HttpPut]
        [Route("api/TeacherData/UpdateTeacher/{id}")]
        public IHttpActionResult UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (TeacherInfo.HireDate > DateTime.Today) return BadRequest("Hire date cannot be in future");
            if (TeacherInfo.Salary < 0) return BadRequest("Salary cannot be negative");

            MySqlConnection Conn = School.AccessDatabase();
            try
            {
                Conn.Open();
                MySqlCommand cmd = Conn.CreateCommand();
                cmd.CommandText = @"UPDATE teachers SET 
                          teacherfname=@TeacherFname,
                          teacherlname=@TeacherLname,
                          employeenumber=@EmployeeNumber,
                          hiredate=@HireDate,
                          salary=@Salary
                          WHERE teacherid=@TeacherId";

                cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
                cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
                cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
                cmd.Parameters.AddWithValue("@HireDate", TeacherInfo.HireDate);
                cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
                cmd.Parameters.AddWithValue("@TeacherId", id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Return success message with updated teacher data
                    return Ok(new
                    {
                        Message = $"Teacher {id} updated successfully",
                        //Teacher = TeacherInfo
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                if (Conn != null && Conn.State == System.Data.ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }
    
}