using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentsMS.Models;

public partial class ClassTime
{   
    public string CTID { get; set; } = null!;

    public string CTWeek { get; set; } = null!;

    public string CTPeriod { get; set; } = null!;

    public string StartTime { get; set; } = null!;

    public string EndTime { get; set; } = null!;

    public virtual ICollection<Curriculum> Curriculum { get; set; } = new List<Curriculum>();

    public virtual ICollection<RollCall> RollCall { get; set; } = new List<RollCall>();

    public virtual ICollection<SelectCourse> SelectCourse { get; set; } = new List<SelectCourse>();
}
