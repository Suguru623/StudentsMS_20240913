using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsMS.Models;

public partial class Curriculum
{
    [Display(Name = "課程")]
    public string CourseID { get; set; } = null!;

    [Display(Name = "科系")]
    public string DeptID { get; set; } = null!;

    [Display(Name = "課程時間")]
    
    public string CTID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [Display(Name = "課程時數")]
   
    public string CTHours { get; set; } = null!;
    [RegularExpression("[1-8]", ErrorMessage = "請輸入正確時數")]
    //[Required(ErrorMessage = "必填")]
    [Display(Name = "課程時間")]
    public virtual ClassTime? CT { get; set; } 

    //[Required(ErrorMessage = "必填")]
    [Display(Name = "課程")]
    public virtual Course? Course { get; set; }

    //[Required(ErrorMessage = "必填")]
    [Display(Name = "科系")]
    public virtual Department? Dept { get; set; } 
}
