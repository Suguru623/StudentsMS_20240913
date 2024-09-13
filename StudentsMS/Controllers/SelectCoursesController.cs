using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentsMS.Models;
using StudentsMS.ViewModels;

namespace StudentsMS.Controllers
{
    public class SelectCoursesController : Controller
    {
        private readonly StudentsMSContext _context;

        public SelectCoursesController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: SelectCourses
        public async Task<IActionResult> Index(string stuID= "113D01101", string deptID = "D01", string courseID = "C01", string ctID = "T11")
        {
            //var studentsMSContext = _context.SelectCourse.Include(s => s.CT).Include(s => s.Course).Include(s => s.Dept).Include(s => s.Stu);
            //return View(await studentsMSContext.ToListAsync());


            var sc = new VMSelectCourse()
            {
                SelectCourse = await _context.SelectCourse.Where(s => s.DeptID == deptID &&  s.CourseID == courseID).ToListAsync(),
                Student=await _context.Student.ToListAsync(),
                Department = await _context.Department.ToListAsync(),
                Course = await _context.Course.ToListAsync(),
                ClassTime = await _context.ClassTime.ToListAsync()
            };

            ViewData["DepartmentName"] = (await _context.Department.FindAsync(deptID))?.DName ?? "選擇科系";
            ViewData["CourseName"] = (await _context.Course.FindAsync(courseID))?.CName ?? "選擇課程";
            //ViewData["DepartmentName"] = _context.Department.Find(deptID).DName;
            //ViewData["CourseName"] = _context.Course.Find(courseID).CName;
            ViewData["DepartmentID"] = deptID;
            ViewData["CourseID"]= courseID;

            return View(sc);    
            //return View(!string.IsNullOrEmpty(courseID) ? sccourse : scdept);

        }

        // GET: SelectCourses/Details/5
        public async Task<IActionResult> Details(string stuID, string deptID, string courseID, string ctID)
        {
            if (stuID == null || deptID == null || courseID == null || ctID == null)
            {
                return NotFound();
            }

            var selectCourse = await _context.SelectCourse
                .Include(s => s.CT)
                .Include(s => s.Course)
                .Include(s => s.Dept)
                .Include(s => s.Stu)
                .FirstOrDefaultAsync(s => s.StuID == stuID && s.DeptID == deptID && s.CourseID == courseID && s.CTID == ctID);
            if (selectCourse == null)
            {
                return NotFound();
            }

            return View(selectCourse);
        }

        // GET: SelectCourses/Create
        public IActionResult Create()
        {
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID");
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName");
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName");
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID");
            return View();
        }

        // POST: SelectCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StuID,CourseID,DeptID,CTID")] SelectCourse selectCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(selectCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", selectCourse.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", selectCourse.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", selectCourse.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", selectCourse.StuID);
            return View(selectCourse);
        }

        // GET: SelectCourses/Edit/5
        public async Task<IActionResult> Edit(string StuID, string CourseID, string DeptID, string CTID)
        {
            if (StuID == null || DeptID == null || CourseID == null || CTID == null)
            {
                return NotFound();
            }

            var selectCourse = await _context.SelectCourse.FindAsync(StuID, CourseID, DeptID,CTID);
            if (selectCourse == null)
            {
                return NotFound();
            }
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", selectCourse.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", selectCourse.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", selectCourse.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", selectCourse.StuID);
            return View(selectCourse);
        }

        // POST: SelectCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SelectCourse selectCourse)
        {
            if (selectCourse == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(selectCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SelectCourseExists(selectCourse.StuID,selectCourse.CourseID,selectCourse.DeptID,selectCourse.CTID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", selectCourse.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", selectCourse.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", selectCourse.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", selectCourse.StuID);
            return View(selectCourse);
        }

        private bool SelectCourseExists(string StuID, string CourseID, string DeptID, string CTID)
        {
            return _context.SelectCourse.Any(c => c.StuID == StuID && c.CourseID == CourseID && c.DeptID == DeptID && c.CTID == CTID);
        }

        // GET: SelectCourses/Delete/5
        public async Task<IActionResult> Delete(string StuID, string CourseID, string DeptID, string CTID)
        {
            if (StuID == null)
            {
                return NotFound();
            }

            var selectCourse = await _context.SelectCourse
                .Include(s => s.CT)
                .Include(s => s.Course)
                .Include(s => s.Dept)
                .Include(s => s.Stu)
                .FirstOrDefaultAsync(m => m.StuID == StuID && m.CourseID == CourseID && m.DeptID == DeptID && m.CTID == CTID );
            if (selectCourse == null)
            {
                return NotFound();
            }

            return View(selectCourse);
        }

        // POST: SelectCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string StuID, string CourseID, string DeptID, string CTID)
        {
            var selectCourse = await _context.SelectCourse.FirstOrDefaultAsync(m => m.StuID == StuID && m.DeptID == DeptID && m.CourseID == CourseID && m.CTID == CTID); 
            if (selectCourse != null)
            {
                _context.SelectCourse.Remove(selectCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SelectCourseExists(string id)
        {
            return _context.SelectCourse.Any(e => e.StuID == id);
        }
    }
}
