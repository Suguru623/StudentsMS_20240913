using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsMS.Models;

public partial class RollCall
{
    [Display(Name = "點名序號")]
    public string RCID { get; set; } = null!;

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

    [Required(ErrorMessage = "必填")]
    [Display(Name = "日期")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")] //設定時間格式
    public DateTime RCDate { get; set; }

    [Required(ErrorMessage = "必填")]
    [Display(Name = "到課時間")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")] //設定時間格式
    public DateTime ArrivalTime { get; set; }

    [Display(Name = "課程時間")]
    public virtual ClassTime? CT { get; set; }

    [Display(Name = "課程")]
    public virtual Course? Course { get; set; }

    [Display(Name = "科系")]
    public virtual Department? Dept { get; set; }

    [Display(Name = "學號")]
    public virtual Student? Stu { get; set; } 
}
