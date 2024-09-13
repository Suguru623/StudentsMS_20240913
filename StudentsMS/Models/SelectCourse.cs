using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsMS.Models;

public partial class SelectCourse
{
    [Required(ErrorMessage = "必填")]
    [Display(Name = "學號")]
    public string StuID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [Display(Name = "課程代號")]
    public string CourseID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [Display(Name = "科系代號")]
    public string DeptID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [Display(Name = "課程時間")]
    public string CTID { get; set; } = null!;

    [Display(Name = "課程時間")]
    public virtual ClassTime? CT { get; set; } 

    [Display(Name = "課程")]
    public virtual Course? Course { get; set; } 

    [Display(Name = "科系")]
    public virtual Department? Dept { get; set; } 

    [Display(Name = "學號")]
    public virtual Student? Stu { get; set; } 
}
