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
    public class NormalStudentsController : Controller
    {
        private readonly StudentsMSContext _context;

        public NormalStudentsController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: NormalStudents
        public async Task<IActionResult> Index()
        {

            string aa = HttpContext.Session.GetString("Account");

            var student = await _context.Student
              .Include(s => s.Dept)
              .FirstOrDefaultAsync(m => m.StuID == aa);

            var studentsMSContext = _context.Student.Include(s => s.Dept).Where(s=>s.DeptID == student.DeptID);
            return View(await studentsMSContext.ToListAsync());
           

        }

        // GET: NormalStudents/Details/5
        public async Task<IActionResult> Details(string id)
        {
            
            //if (id == null)
            //{
            //    return NotFound();
            //}
            //ViewData["Account"] = HttpContext.Session.GetString("Account");
            string aa = HttpContext.Session.GetString("Account");

            var student = await _context.Student
                .Include(s => s.Dept)
                .FirstOrDefaultAsync(m => m.StuID == aa);
            
            //ViewData["Deptid"] = studentaa.DeptID;
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: NormalStudents/Create
        public IActionResult Create()
        {
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DeptID");
            return View();
        }

        // POST: NormalStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StuID,SName,Grade,Class,Number,DeptID")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DeptID", student.DeptID);
            return View(student);
        }

        // GET: NormalStudents/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DeptID", student.DeptID);
            return View(student);
        }

        // POST: NormalStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StuID,SName,Grade,Class,Number,DeptID")] Student student)
        {
            if (id != student.StuID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StuID))
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
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DeptID", student.DeptID);
            return View(student);
        }

        // GET: NormalStudents/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Dept)
                .FirstOrDefaultAsync(m => m.StuID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: NormalStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
            return _context.Student.Any(e => e.StuID == id);
        }
    }
}
