using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentsMS.Models;

public partial class StudentsMSContext : DbContext
{
    public StudentsMSContext(DbContextOptions<StudentsMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClassTime> ClassTime { get; set; }

    public virtual DbSet<Course> Course { get; set; }

    public virtual DbSet<Curriculum> Curriculum { get; set; }

    public virtual DbSet<Department> Department { get; set; }

    public virtual DbSet<Leave> Leave { get; set; }

    public virtual DbSet<LeaveDetail> LeaveDetail { get; set; }

    public virtual DbSet<RollCall> RollCall { get; set; }

    public virtual DbSet<SelectCourse> SelectCourse { get; set; }

    public virtual DbSet<Student> Student { get; set; }
    
    //4.1.4 修改GuestBookContext類別的內容，加入描述資料庫裡Login的資料表
    public virtual DbSet<Login> Login { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClassTime>(entity =>
        {
            entity.HasKey(e => e.CTID).HasName("PK__ClassTim__F4AA1BE011810E7A");

            entity.Property(e => e.CTID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CTPeriod)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CTWeek)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EndTime)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StartTime)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseID).HasName("PK__Course__C92D7187A629762E");

            entity.Property(e => e.CourseID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CName).HasMaxLength(10);
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.HasKey(e => new { e.CourseID, e.DeptID, e.CTID }).HasName("PK__Curricul__D8CD5384888EAB68");

            entity.Property(e => e.CourseID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DeptID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CTID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CTHours)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.CT).WithMany(p => p.Curriculum)
                .HasForeignKey(d => d.CTID)
                .HasConstraintName("FK__Curriculum__CTID__4222D4EF");

            entity.HasOne(d => d.Course).WithMany(p => p.Curriculum)
                .HasForeignKey(d => d.CourseID)
                .HasConstraintName("FK__Curriculu__Cours__403A8C7D");

            entity.HasOne(d => d.Dept).WithMany(p => p.Curriculum)
                .HasForeignKey(d => d.DeptID)
                .HasConstraintName("FK__Curriculu__DeptI__412EB0B6");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptID).HasName("PK__Departme__0148818EA1A605E9");

            entity.Property(e => e.DeptID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DName).HasMaxLength(20);
        });

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.HasKey(e => e.LeaveID).HasName("PK__Leave__796DB979C0B5233B");

            entity.Property(e => e.LeaveID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LName).HasMaxLength(6);
        });

        modelBuilder.Entity<LeaveDetail>(entity =>
        {
            entity.HasKey(e => e.LDID).HasName("PK__LeaveDet__61F2580E45528DC2");

            entity.Property(e => e.LDID)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.LDDate).HasColumnType("datetime");
            entity.Property(e => e.LeaveID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.StuID)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Leave).WithMany(p => p.LeaveDetail)
                .HasForeignKey(d => d.LeaveID)
                .HasConstraintName("FK__LeaveDeta__Leave__571DF1D5");

            entity.HasOne(d => d.Stu).WithMany(p => p.LeaveDetail)
                .HasForeignKey(d => d.StuID)
                .HasConstraintName("FK__LeaveDeta__StuID__5629CD9C");
        });

        modelBuilder.Entity<RollCall>(entity =>
        {
            //entity.HasKey(e => new { e.StuID, e.CourseID, e.DeptID, e.CTID }).HasName("PK__RollCall__31537E4D51ED81F9");
            entity.HasKey(e=>e.RCID).HasName("PK_RollCall");
            entity.Property(e => e.RCID)
               .HasMaxLength(12)
               .IsUnicode(false)
               .IsFixedLength();
            entity.Property(e => e.StuID)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CourseID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DeptID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CTID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.RCDate).HasColumnType("datetime");

            entity.HasOne(d => d.CT).WithMany(p => p.RollCall)
                .HasForeignKey(d => d.CTID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RollCall__CTID__4D94879B");

            entity.HasOne(d => d.Course).WithMany(p => p.RollCall)
                .HasForeignKey(d => d.CourseID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RollCall__Course__4BAC3F29");

            entity.HasOne(d => d.Dept).WithMany(p => p.RollCall)
                .HasForeignKey(d => d.DeptID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RollCall__DeptID__4CA06362");

            entity.HasOne(d => d.Stu).WithMany(p => p.RollCall)
                .HasForeignKey(d => d.StuID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RollCall__StuID__4AB81AF0");
        });

        modelBuilder.Entity<SelectCourse>(entity =>
        {
            entity.HasKey(e => new { e.StuID, e.CourseID, e.DeptID, e.CTID }).HasName("PK__SelectCo__31537E4DDC36CCF6");

            entity.Property(e => e.StuID)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CourseID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DeptID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CTID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.CT).WithMany(p => p.SelectCourse)
                .HasForeignKey(d => d.CTID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SelectCour__CTID__47DBAE45");

            entity.HasOne(d => d.Course).WithMany(p => p.SelectCourse)
                .HasForeignKey(d => d.CourseID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SelectCou__Cours__45F365D3");

            entity.HasOne(d => d.Dept).WithMany(p => p.SelectCourse)
                .HasForeignKey(d => d.DeptID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SelectCou__DeptI__46E78A0C");

            entity.HasOne(d => d.Stu).WithMany(p => p.SelectCourse)
                .HasForeignKey(d => d.StuID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SelectCou__StuID__44FF419A");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StuID).HasName("PK__Student__6CDFAB75AE043458");

            entity.Property(e => e.StuID)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Class)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DeptID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Grade)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Number)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SName).HasMaxLength(20);

            entity.HasOne(d => d.Dept).WithMany(p => p.Student)
                .HasForeignKey(d => d.DeptID)
                .HasConstraintName("FK__Student__DeptID__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
