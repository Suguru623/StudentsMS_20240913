using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsMS.Models;

public partial class Department
{
    [Key]
    [Required(ErrorMessage = "必填")]
    [RegularExpression("D[0-9]{2}", ErrorMessage = "格式錯誤(ex:D01)")]
    [Display(Name = "科系代碼")]
    public string DeptID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [StringLength(20, ErrorMessage = "科系名稱最多20個字")] //設定字串長度
    [Display(Name = "科系名稱")]
    public string DName { get; set; } = null!;

    public virtual ICollection<Curriculum> Curriculum { get; set; } = new List<Curriculum>();

    public virtual ICollection<RollCall> RollCall { get; set; } = new List<RollCall>();

    public virtual ICollection<SelectCourse> SelectCourse { get; set; } = new List<SelectCourse>();

    public virtual ICollection<Student> Student { get; set; } = new List<Student>();
}
