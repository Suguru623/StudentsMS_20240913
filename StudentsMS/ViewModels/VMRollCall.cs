using StudentsMS.Models;
namespace StudentsMS.ViewModels
{
    public class VMRollCall
    {
        public List<Student>? Student { get; set; }
        public List<RollCall>? RollCall { get; set; }
        public List<Course>? Course { get; set; }
        public List<Department>? Department { get; set; }
        public List<ClassTime>? ClassTime { get; set; }
    }
}
