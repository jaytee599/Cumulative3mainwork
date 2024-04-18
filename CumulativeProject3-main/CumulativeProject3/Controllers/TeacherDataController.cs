using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using CumulativeProject3.Models;

namespace CumulativeProject3.Controllers;

public class TeacherDataController : ApiController
{
    private readonly SchoolDbContext _schoolDbContext = new();

    /// <summary>
    /// Returns a list of teachers in the system
    /// </summary>
    /// <example>GET api/TeacherData/ListTeachers</example>
    /// <returns>
    /// A list of Teacher (first names and last names)
    /// </returns>
    [HttpGet]
    public IEnumerable<Teacher> ListTeachers()
    {
        //Create an instance of a connection
        var conn = _schoolDbContext.Connection();

        //Open the connection between the web server and database
        conn.Open();

        //Establish a new command (query) for our database
        var cmd = conn.CreateCommand();

        //SQL QUERY
        cmd.CommandText = "select * from teachers";

        //Gather Result Set of Query into a variable
        var resultSet = cmd.ExecuteReader();

        //Create an empty list of Teacher Names
        var teachers = new List<Teacher> { };

        //Loop Through Each Row the Result Set
        while (resultSet.Read())
        {
            //Access Column information by the DB column name as an index
            var teacherId = Convert.ToInt32(resultSet["teacherid"]);
            var teacherFname = resultSet["teacherfname"].ToString();
            var teacherLname = resultSet["teacherlname"].ToString();
            var employeenumber = resultSet["employeenumber"].ToString();
            var hiredate = resultSet["hiredate"].ToString();
            var salary = (decimal)resultSet["salary"];


            var newteacher = new Teacher();
            newteacher.teacherId = teacherId;
            newteacher.teacherFname = teacherFname;
            newteacher.teacherLname = teacherLname;
            newteacher.employeenumber = employeenumber;
            newteacher.hiredate = hiredate;
            newteacher.salary = salary;

            //Add the Teacher Name to the List
            teachers.Add(newteacher);
        }

        //Close the connection between the MySQL Database and the WebServer
        conn.Close();

        //Return the final list of teacher names
        return teachers;
    }


    /// <summary>
    /// Finds a teacher in the system given an ID
    /// </summary>
    /// <param name="id">The teacher primary key</param>
    /// <returns>An teacher object</returns>
    [HttpGet]
    [Route("api/TeacherData/FindTeacher/{id}")]
    public Teacher FindTeacher(int id)
    {
        var newTeacher = new Teacher();

        //Create an instance of a connection
        var conn = _schoolDbContext.Connection();

        //Open the connection between the web server and database
        conn.Open();

        //Establish a new command (query) for our database
        var cmd = conn.CreateCommand();

        //SQL QUERY
        cmd.CommandText = "select * from teachers where teacherid = " + id;

        //Gather Result Set of Query into a variable
        var resultSet = cmd.ExecuteReader();

        while (resultSet.Read())
        {
            //Access Column information by the DB column name as an index
            var teacherId = Convert.ToInt32(resultSet["teacherid"]);
            var teacherFname = resultSet["teacherfname"].ToString();
            var teacherLname = resultSet["teacherlname"].ToString();
            var employeenumber = resultSet["employeenumber"].ToString();
            var hiredate = resultSet["hiredate"].ToString();
            var salary = (decimal)resultSet["salary"];


            newTeacher.teacherId = teacherId;
            newTeacher.teacherFname = teacherFname;
            newTeacher.teacherLname = teacherLname;
            newTeacher.employeenumber = employeenumber;
            newTeacher.hiredate = hiredate;
            newTeacher.salary = salary;
        }


        return newTeacher;
    }
    
    /// <summary>
    /// Add a new teacher in the system
    /// </summary>
    /// <param name="teacher">The teacher object</param>
    /// <returns>An teacher object</returns>
    [HttpPost]
    [Route("api/TeacherData/AddTeacher")]
    public async Task<IHttpActionResult> AddTeacher([FromBody] Teacher teacher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            //Create an instance of a connection
            var conn = _schoolDbContext.Connection();

            //Open the connection between the web server and database
            conn.Open();
            
            //Establish a new command (query) for our database
            var cmd = conn.CreateCommand();
            
            //Generate Insert Query 
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) value (@teacherFname, @teacherLname, @employeenumber, @hiredate, @salary)";
            cmd.Parameters.AddWithValue("@teacherFname", teacher.teacherFname);
            cmd.Parameters.AddWithValue("@teacherLname", teacher.teacherLname);
            cmd.Parameters.AddWithValue("@employeenumber", teacher.employeenumber);
            cmd.Parameters.AddWithValue("@hiredate", teacher.hiredate);
            cmd.Parameters.AddWithValue("@salary", teacher.salary);

            await cmd.ExecuteNonQueryAsync();

            return Ok("Teacher added successfully");
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    /// <summary>
    /// Update an existing teacher in the system
    /// </summary>
    /// <param name="id">The ID of the teacher to update</param>
    /// <param name="teacher">The updated teacher object</param>
    /// <returns>An IHttpActionResult indicating the result of the operation</returns>
    [HttpPut]
    [Route("api/TeacherData/UpdateTeacher/{id}")]
    public async Task<IHttpActionResult> UpdateTeacher(int id, [FromBody] Teacher teacher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Create an instance of a connection
            var conn = _schoolDbContext.Connection();

            // Open the connection between the web server and database
            conn.Open();

            // Establish a new command (query) for our database
            var cmd = conn.CreateCommand();

            // Generate Update Query
            cmd.CommandText = "UPDATE teachers SET teacherfname = @teacherFname, teacherlname = @teacherLname, " +
                              "employeenumber = @employeenumber, hiredate = @hiredate, salary = @salary WHERE teacherid = @teacherId";
            cmd.Parameters.AddWithValue("@teacherFname", teacher.teacherFname);
            cmd.Parameters.AddWithValue("@teacherLname", teacher.teacherLname);
            cmd.Parameters.AddWithValue("@employeenumber", teacher.employeenumber);
            cmd.Parameters.AddWithValue("@hiredate", teacher.hiredate);
            cmd.Parameters.AddWithValue("@salary", teacher.salary);
            cmd.Parameters.AddWithValue("@teacherId", id);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                return Ok("Teacher updated successfully");
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
    
    /// <summary>
    /// Deletes a teacher from the system given its ID
    /// </summary>
    /// <param name="id">The ID of the teacher to delete</param>
    /// <returns>A string message indicating the result of the operation</returns>
    [HttpDelete]
    [Route("api/TeacherData/DeleteTeacher/{id}")]
    public async Task<IHttpActionResult> DeleteTeacher(int id)
    {
        //Create an instance of a connection
        var conn = _schoolDbContext.Connection();

        //Open the connection between the web server and database
        conn.Open();
            
        // Update the classes table to set teacherid to null for classes associated with the deleted teacher
        await UpdateClassesTable(id);

        //Establish a new command (query) for our database
        var cmd = conn.CreateCommand();
            
        //Generate Delete Query
        cmd.CommandText = "delete from teachers WHERE teacherid = @teacherId";
        cmd.Parameters.AddWithValue("@teacherId", id);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        if (rowsAffected > 0)
        {
            return Ok("Teacher deleted successfully");
        }

        return NotFound();
    }
    
    private async Task UpdateClassesTable(int teacherId)
    {
        var conn = _schoolDbContext.Connection();
        conn.Open();
    
        var cmd = conn.CreateCommand();
        cmd.CommandText = "update classes set teacherid = NULL where teacherid = @teacherId";
        cmd.Parameters.AddWithValue("@teacherId", teacherId);

        await cmd.ExecuteNonQueryAsync();
    }
}