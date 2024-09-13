using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentsMS.Models;

namespace StudentsMS.Controllers
{
    public class NormalSelectCoursesController : Controller
    {
        private readonly StudentsMSContext _context;

        public NormalSelectCoursesController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: NormalSelectCourses
        //public async Task<IActionResult> Index()
        //{
        //    var studentsMSContext = _context.SelectCourse.Include(s => s.CT).Include(s => s.Course).Include(s => s.Dept).Include(s => s.Stu);
        //    return View(await studentsMSContext.ToListAsync());
        //}

        // GET: NormalSelectCourses
        public async Task<IActionResult> Index()
        {
            string aa = HttpContext.Session.GetString("Account");
            //var studentsMSContext = _context.SelectCourse.Include(s => s.CT).Include(s => s.Course).Include(s => s.Dept).Include(s => s.Stu).FirstOrDefaultAsync(m => m.StuID == aa);
            //return View(await studentsMSContext.ToListAsync());

            var selectCourses = await _context.SelectCourse
                .Include(s => s.CT)
                .Include(s => s.Course)
                .Include(s => s.Dept)
                .Include(s => s.Stu)
                .Where(m => m.StuID == aa)
                .ToListAsync();

            return View(selectCourses); // 传递一个集合

        }

        // GET: NormalSelectCourses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            ViewData["Account"] = HttpContext.Session.GetString("Account");
            string aa = HttpContext.Session.GetString("Account");
            var selectCourse = await _context.SelectCourse
                .Include(s => s.CT)
                .Include(s => s.Course)
                .Include(s => s.Dept)
                .Include(s => s.Stu)
                .FirstOrDefaultAsync(m => m.StuID == aa);
            if (selectCourse == null)
            {
                return NotFound();
            }

            return View(selectCourse);
        }

        // GET: NormalSelectCourses/Create
        public IActionResult Create()
        {
            string aa = HttpContext.Session.GetString("Account");



            var stu = _context.Student.Find(aa);

            var selectCourse = _context.Curriculum.Where(m=>m.DeptID==stu.DeptID).Include(m => m.Course);


            //ViewData["StuID"] = aa;
            //ViewData["Result"] = result;
            //var selectCourse = _context.SelectCourse
            //   .Include(s => s.CT)
            //   .Include(s => s.Course)
            //   .Include(s => s.Dept)
            //   .Include(s => s.Stu)
            //   .FirstOrDefault(m => m.StuID == aa);

            //ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID");
            //ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName");
            //ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName");
            //ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID");
            ViewData["StuID"] = aa;
            ViewData["selectCourse"] = selectCourse;


            return View();

        }

        // POST: NormalSelectCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StuID,CourseID,DeptID,CTID")] string StuID,string CourseID )//SelectCourse selectCourse
        {
            string aa = HttpContext.Session.GetString("Account");
            
            var stu = _context.Student.Find(aa);

            
            var StuSelect = await _context.Curriculum.Where(m=> m.CourseID == CourseID).FirstOrDefaultAsync();


            var selectCourse = new SelectCourse
            {
                
                StuID = stu.StuID,
                DeptID = stu.DeptID,
                CourseID = StuSelect.CourseID,
                CTID = StuSelect.CTID,
                
            };

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

        // GET: NormalSelectCourses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectCourse = await _context.SelectCourse.FindAsync(id);
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

        // POST: NormalSelectCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StuID,CourseID,DeptID,CTID")] SelectCourse selectCourse)
        {
            if (id != selectCourse.StuID)
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
                    if (!SelectCourseExists(selectCourse.StuID))
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

        // GET: NormalSelectCourses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectCourse = await _context.SelectCourse
                .Include(s => s.CT)
                .Include(s => s.Course)
                .Include(s => s.Dept)
                .Include(s => s.Stu)
                .FirstOrDefaultAsync(m => m.StuID == id);
            if (selectCourse == null)
            {
                return NotFound();
            }

            return View(selectCourse);
        }

        // POST: NormalSelectCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var selectCourse = await _context.SelectCourse.FindAsync(id);
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
