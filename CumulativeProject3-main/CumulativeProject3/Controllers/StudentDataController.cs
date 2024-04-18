using System;
using System.Collections.Generic;
using System.Web.Http;
using CumulativeProject3.Models;

namespace CumulativeProject3.Controllers;

public class StudentDataController : ApiController
{
    private readonly SchoolDbContext _schoolDbContext = new();

    /// <summary>
    /// Returns a list of students in the system
    /// </summary>
    /// <example>GET api/StudentData/ListStudents</example>
    /// <returns>
    /// A list of Student 
    /// </returns>
    [HttpGet]
    public IEnumerable<Students> ListStudents()
    {
        //Create an instance of a connection
        var conn = _schoolDbContext.Connection();

        //Open the connection between the web server and database
        conn.Open();

        //Establish a new command (query) for our database
        var cmd = conn.CreateCommand();

        //SQL QUERY
        cmd.CommandText = "select * from students";

        //Gather Result Set of Query into a variable
        var resultSet = cmd.ExecuteReader();

        //Create an empty list of Student Names
        var student = new List<Students> { };

        //Loop Through Each Row the Result Set
        while (resultSet.Read())
        {
            //Access Column information by the DB column name as an index
            var studentId = Convert.ToInt32(resultSet["studentid"]);
            var studentFname = resultSet["studentfname"].ToString();
            var studentLname = resultSet["studentlname"].ToString();
            var studentnumber = resultSet["studentnumber"].ToString();
            var enroldate = resultSet["enroldate"].ToString();


            var newstudent = new Students();
            newstudent.studentId = studentId;
            newstudent.studentFname = studentFname;
            newstudent.studentLname = studentLname;
            newstudent.studentnumber = studentnumber;
            newstudent.enroldate = enroldate;

            //Add the Student Name to the List
            student.Add(newstudent);
        }

        //Close the connection between the MySQL Database and the WebServer
        conn.Close();

        //Return the final list of student names
        return student;
    }


    /// <summary>
    /// Finds a student in the system given an ID
    /// </summary>
    /// <param name="id">The student primary key</param>
    /// <returns>An student object</returns>
    [HttpGet]
    [Route("api/StudentData/FindStudent/{id}")]
    public Students FindStudents(int id)
    {
        var newstudent = new Students();

        //Create an instance of a connection
        var conn = _schoolDbContext.Connection();

        //Open the connection between the web server and database
        conn.Open();

        //Establish a new command (query) for our database
        var cmd = conn.CreateCommand();

        //SQL QUERY
        cmd.CommandText = "select * from Students where studentid = " + id;

        //Gather Result Set of Query into a variable
        var resultSet = cmd.ExecuteReader();

        while (resultSet.Read())
        {
            //Access Column information by the DB column name as an index
            var studentId = Convert.ToInt32(resultSet["studentid"]);
            var studentFname = resultSet["studentfname"].ToString();
            var studentLname = resultSet["studentlname"].ToString();
            var studentnumber = resultSet["studentnumber"].ToString();
            var enroldate = resultSet["enroldate"].ToString();


            //Students Newstudent = new Students();
            newstudent.studentId = studentId;
            newstudent.studentFname = studentFname;
            newstudent.studentLname = studentLname;
            newstudent.studentnumber = studentnumber;
            newstudent.enroldate = enroldate;
        }


        return newstudent;
    }
}