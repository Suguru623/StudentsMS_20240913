using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsMS.Models;

public partial class Student
{
    [Key]
    [Required(ErrorMessage = "必填")]
    [Display(Name = "學號")]
    [RegularExpression("[0-9]{3}D[0-9]{2}[1-2][0-9]{2}", ErrorMessage = "格式錯誤(ex:113D01101)")]
    public string StuID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [StringLength(20, ErrorMessage = "姓名最多20個字")] //設定字串長度
    [Display(Name = "姓名")]
    public string SName { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [Display(Name = "年級")]
    [RegularExpression("[1-4]",ErrorMessage ="請輸入正確年級")]
    public string Grade { get; set; } = null!;

    [Display(Name = "班級")]
    [Required(ErrorMessage = "必填")]
    [RegularExpression("[1-9]", ErrorMessage = "請輸入正確班級")]
    public string Class { get; set; } = null!;

    [Display(Name = "座號")]
    [Required(ErrorMessage = "必填")]
    [RegularExpression("^[1-9]$|^[1-9][0-9]$", ErrorMessage = "請輸入正確座號")]
    public string Number { get; set; } = null!;

    [Display(Name = "科系")]
    [ForeignKey("Department")]
    [RegularExpression("D[0-9]{2}",ErrorMessage ="請輸入正確科系代碼")]
    public string DeptID { get; set; } = null!;

    [Display(Name = "科系名稱")]
    public virtual Department? Dept { get; set; } = null!;

    public virtual ICollection<LeaveDetail>? LeaveDetail { get; set; } = new List<LeaveDetail>();

    public virtual ICollection<RollCall>? RollCall { get; set; } = new List<RollCall>();

    public virtual ICollection<SelectCourse>? SelectCourse { get; set; } = new List<SelectCourse>();
}
