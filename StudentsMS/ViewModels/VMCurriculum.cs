using StudentsMS.Models;
namespace StudentsMS.ViewModels
{
    public class VMCurriculum
    {
        public List<Curriculum>? Curriculum { get; set; }
        public List<Course>? Course { get; set; }
        public List<Department>? Department { get; set; }
        public List<ClassTime>? ClassTime { get; set; }
    }
}
