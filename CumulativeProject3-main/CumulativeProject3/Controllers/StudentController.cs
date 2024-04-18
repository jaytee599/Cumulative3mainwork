using System.Web.Mvc;

namespace CumulativeProject3.Controllers;

public class StudentController : Controller
{
    // GET: Student
    public ActionResult Index()
    {
        return View();
    }
    
    //GET : /Student/List
    public ActionResult List()
    {
        var controller = new StudentDataController();
        var students = controller.ListStudents();
        return View(students);
    }

    //GET : /Student/Show/{id}
    public ActionResult Show(int id)
    {
        var controller = new StudentDataController();
        var newStudent = controller.FindStudents(id);


        return View(newStudent);
    }
}
