using System.Web.Mvc;

namespace CumulativeProject3.Controllers;

public class TeacherController : Controller
{
    // GET: Teacher
    public ActionResult Index()
    {
        return View();
    }

    //GET : /Teacher/List
    public ActionResult List()
    {
        var controller = new TeacherDataController();
        var teacher = controller.ListTeachers();
        return View(teacher);
    }

    //GET : /Teacher/Show/{id}
    public ActionResult Show(int id)
    {
        var controller = new TeacherDataController();
        var newTeacher = controller.FindTeacher(id);


        return View(newTeacher);
    }
    
    
    //GET : /Teacher/Add
    public ActionResult Add()
    {
        return View();
    }
    
    
    //GET : /Teacher/Update
    public ActionResult Update(int id)
    {
        var controller = new TeacherDataController();
        var teacher = controller.FindTeacher(id);
        
        return View(teacher);
    }
}