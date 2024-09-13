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
    public class NormalCurriculumController : Controller
    {
        private readonly StudentsMSContext _context;

        public NormalCurriculumController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: NormalCurriculum
        public async Task<IActionResult> Index()
        {
            string aa = HttpContext.Session.GetString("Account");

            var student = await _context.Student
             .Include(s => s.Dept)
             .FirstOrDefaultAsync(m => m.StuID == aa);

            var studentsMSContext =await _context.Curriculum
                .Include(c => c.CT)
                .Include(c => c.Course)
                .Include(c => c.Dept)
                .Where(c=>c.DeptID == student.DeptID)
                .ToListAsync();
            return View(studentsMSContext);
        }

        // GET: NormalCurriculum/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                .Include(c => c.CT)
                .Include(c => c.Course)
                .Include(c => c.Dept)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (curriculum == null)
            {
                return NotFound();
            }

            return View(curriculum);
        }

        // GET: NormalCurriculum/Create
        public IActionResult Create()
        {
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID");
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID");
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DeptID");
            return View();
        }

        // POST: NormalCurriculum/Create
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
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", curriculum.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DeptID", curriculum.DeptID);
            return View(curriculum);
        }

        // GET: NormalCurriculum/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum.FindAsync(id);
            if (curriculum == null)
            {
                return NotFound();
            }
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", curriculum.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", curriculum.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DeptID", curriculum.DeptID);
            return View(curriculum);
        }

        // POST: NormalCurriculum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CourseID,DeptID,CTID,CTHours")] Curriculum curriculum)
        {
            if (id != curriculum.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curriculum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurriculumExists(curriculum.CourseID))
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
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", curriculum.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DeptID", curriculum.DeptID);
            return View(curriculum);
        }

        // GET: NormalCurriculum/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                .Include(c => c.CT)
                .Include(c => c.Course)
                .Include(c => c.Dept)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (curriculum == null)
            {
                return NotFound();
            }

            return View(curriculum);
        }

        // POST: NormalCurriculum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var curriculum = await _context.Curriculum.FindAsync(id);
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
