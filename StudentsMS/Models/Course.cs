using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsMS.Models;

public partial class Course
{
    [Key]
    [Required(ErrorMessage = "必填")]
    [Display(Name = "課程代碼")]
    [RegularExpression("C[0-9]{2}",ErrorMessage ="格式錯誤(ex:C02)")]
    public string CourseID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [StringLength(10, ErrorMessage = "科系名稱最多10個字")] //設定字串長度
    [Display(Name = "課程名稱")]
    public string CName { get; set; } = null!;

    public virtual ICollection<Curriculum> Curriculum { get; set; } = new List<Curriculum>();

    public virtual ICollection<RollCall> RollCall { get; set; } = new List<RollCall>();

    public virtual ICollection<SelectCourse> SelectCourse { get; set; } = new List<SelectCourse>();
}
