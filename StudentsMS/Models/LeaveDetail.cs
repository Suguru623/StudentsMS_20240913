using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsMS.Models;

public partial class LeaveDetail
{
    [Key]   
    [Display(Name = "請假單代碼")]
    public string LDID { get; set; } = null!;

    
    [Required(ErrorMessage = "必填")]
    [Display(Name = "學號")]
    public string StuID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [Display(Name = "假別")]
    public string LeaveID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [Display(Name = "請假申請日期")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")] //設定時間格式
    public DateTime LDDate { get; set; }

    [Required(ErrorMessage = "必填")]
    [Display(Name = "開始時間")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")] //設定時間格式
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "必填")]
    [Display(Name = "結束時間")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")] //設定時間格式
    public DateTime EndTime { get; set; }

    [Display(Name = "假別")]
    public virtual Leave? Leave { get; set; }

    [Display(Name = "學號")]
    public virtual Student? Stu { get; set; }
}
