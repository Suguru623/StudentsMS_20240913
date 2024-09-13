using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsMS.Models;

public partial class Leave
{
    [Key]
    [Required(ErrorMessage = "必填")]
    [Display(Name = "假別代碼")]
    public string LeaveID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    [Display(Name = "假別名稱")]
    [StringLength(6, ErrorMessage = "假別名稱最多6個字")]
    public string LName { get; set; } = null!;

    public virtual ICollection<LeaveDetail> LeaveDetail { get; set; } = new List<LeaveDetail>();
}
