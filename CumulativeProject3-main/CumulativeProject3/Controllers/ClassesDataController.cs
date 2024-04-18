using System;
using System.Collections.Generic;
using System.Web.Http;
using CumulativeProject3.Models;

namespace CumulativeProject3.Controllers;

public class ClassesDataController : ApiController
{
    private readonly SchoolDbContext _schoolDbContext = new();

    /// <summary>
    /// Returns a list of classes in the system
    /// </summary>
    /// <example>GET api/ClassesData/ListClasses</example>
    /// <returns>
    /// A list of Classes 
    /// </returns>
    [HttpGet]
    public IEnumerable<Classes> ListClasses()
    {
        //Create an instance of a connection
        var conn = _schoolDbContext.Connection();

        //Open the connection between the web server and database
        conn.Open();

        //Establish a new command (query) for our database
        var cmd = conn.CreateCommand();

        //SQL QUERY
        cmd.CommandText = "select * from classes";

        //Gather Result Set of Query into a variable
        var resultSet = cmd.ExecuteReader();

        //Create an empty list of Classes Names
        var classes = new List<Classes> { };

        //Loop Through Each Row the Result Set
        while (resultSet.Read())
        {
            //Access Column information by the DB column name as an index
            var classId = Convert.ToInt32(resultSet["classid"]);
            var classcode = resultSet["classcode"].ToString();
            var teacherId = resultSet["teacherid"] is not DBNull ? Convert.ToInt32(resultSet["teacherid"]): 0;
            var startdate = resultSet["startdate"].ToString();
            var finishdate = resultSet["finishdate"].ToString();
            var classname = resultSet["classname"].ToString();


            var newclasses = new Classes();
            newclasses.classId = classId;
            newclasses.classcode = classcode;
            newclasses.teacherId = teacherId;
            newclasses.startdate = startdate;
            newclasses.finishdate = finishdate;
            newclasses.classname = classname;

            //Add the Classes Name to the List
            classes.Add(newclasses);
        }

        //Close the connection between the MySQL Database and the WebServer
        conn.Close();

        //Return the final list of classes names
        return classes;
    }


    /// <summary>
    /// Finds the class in the system given an ID
    /// </summary>
    /// <param name="id">The class primary key</param>
    /// <returns>An class object</returns>
    [HttpGet]
    [Route("api/ClassesData/FindClasses/{id}")]
    public Classes FindClasses(int id)
    {
        var newClasses = new Classes();

        //Create an instance of a connection
        var conn = _schoolDbContext.Connection();

        //Open the connection between the web server and database
        conn.Open();

        //Establish a new command (query) for our database
        var cmd = conn.CreateCommand();

        //SQL QUERY
        cmd.CommandText = $"select * from classes where classid = {id}";

        //Gather Result Set of Query into a variable
        var resultSet = cmd.ExecuteReader();

        while (resultSet.Read())
        {
            //Access Column information by the DB column name as an index
            var classId = Convert.ToInt32(resultSet["classid"]);
            var classcode = resultSet["classcode"].ToString();
            var teacherId = resultSet["teacherid"] is not DBNull ? Convert.ToInt32(resultSet["teacherid"]): 0;
            var startdate = resultSet["startdate"].ToString();
            var finishdate = resultSet["finishdate"].ToString();
            var classname = resultSet["classname"].ToString();
            
            newClasses.classId = classId;
            newClasses.classcode = classcode;
            newClasses.teacherId = teacherId;
            newClasses.startdate = startdate;
            newClasses.finishdate = finishdate;
            newClasses.classname = classname;
        }


        return newClasses;
    }
}