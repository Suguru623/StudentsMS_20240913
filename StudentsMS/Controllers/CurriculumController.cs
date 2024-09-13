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
    public class CurriculumController : Controller
    {
        private readonly StudentsMSContext _context;

        public CurriculumController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: Curriculum
        public async Task<IActionResult> Index(string deptID = "D01", string courseID = "C01", string ctID = "T11")
        {
            //var studentsMSContext = _context.Curriculum.Include(c => c.CT).Include(c => c.Course).Include(c => c.Dept);
            //return View(await studentsMSContext.ToListAsync());

            VMCurriculum curriculum = new VMCurriculum()
            {
                Curriculum = await _context.Curriculum.Where(c => c.DeptID == deptID).ToListAsync(),
                Department = await _context.Department.ToListAsync(),
                Course= await _context.Course.ToListAsync(),
                ClassTime = await _context.ClassTime.ToListAsync(),
            };
            ViewData["DepartmentName"] = _context.Department.Find(deptID).DName;
            ViewData["DepartmentID"] = deptID;

            return View(curriculum);
        }

        // GET: Curriculum/Details/5
        public async Task<IActionResult> Details(string CourseID, string DeptID, string CTID)
        {
            if (CTID == null || CourseID == null || DeptID == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                .Include(c => c.CT)
                .Include(c => c.Course)
                .Include(c => c.Dept)
                .FirstOrDefaultAsync(m => m.CourseID ==CourseID && m.DeptID == DeptID && m.CTID  == CTID);
            if (curriculum == null)
            {
                return NotFound();
            }

            return View(curriculum);
        }

        // GET: Curriculum/Create
        public IActionResult Create()
        {
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID");
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName");
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName");
            return View();
        }

        // POST: Curriculum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,DeptID,CTID,CTHours")] Curriculum curriculum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curriculum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", curriculum.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", curriculum.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", curriculum.DeptID);
            return View(curriculum);
        }

        // GET: Curriculum/Edit/5
        public async Task<IActionResult> Edit(string CourseID , string DeptID, string CTID)
        {
            if ( CTID == null || CourseID == null || DeptID == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum.FindAsync(CourseID, DeptID, CTID);
            if (curriculum == null)
            {
                return NotFound();
            }
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", curriculum.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", curriculum.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", curriculum.DeptID);
            return View(curriculum);
        }

        // POST: Curriculum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Curriculum curriculum)
        {
            if (curriculum==null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 更新實體
                    _context.Update(curriculum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurriculumExists(curriculum.CourseID, curriculum.DeptID, curriculum.CTID))
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
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", curriculum.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", curriculum.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", curriculum.DeptID);
            return View(curriculum);
        }

        private bool CurriculumExists(string CourseID, string DeptID, string CTID)
        {
            return _context.Curriculum.Any(c => c.CourseID == CourseID && c.DeptID == DeptID && c.CTID == CTID);
        }

        // GET: Curriculum/Delete/5
        public async Task<IActionResult> Delete(string CourseID, string DeptID, string CTID)
        {
            if (CourseID == null || DeptID ==null || CTID == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                .Include(c => c.CT)
                .Include(c => c.Course)
                .Include(c => c.Dept)
                .FirstOrDefaultAsync(m => m.CourseID == CourseID && m.DeptID == DeptID && m.CTID == CTID);
            if (curriculum == null)
            {
                return NotFound();
            }

            return View(curriculum);
        }

        // POST: Curriculum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string CourseID, string DeptID, string CTID)
        {
            var curriculum = await _context.Curriculum.FirstOrDefaultAsync(m => m.DeptID == DeptID && m.CourseID == CourseID && m.CTID == CTID);
            if (curriculum != null)
            {
                _context.Curriculum.Remove(curriculum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurriculumExists(string id)
        {
            return _context.Curriculum.Any(e => e.CourseID == id);
        }
    }
}
