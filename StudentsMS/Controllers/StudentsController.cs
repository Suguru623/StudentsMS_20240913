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
    public class StudentsController : Controller
    {
        private readonly StudentsMSContext _context;

        public StudentsController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string deptID="D01")
        {
            //var studentsMSContext = _context.Student.Include(s => s.Dept);


            VMStudent student = new VMStudent()
            {
                Student = await _context.Student.Where(d => d.DeptID == deptID).ToListAsync(),
                Department = await _context.Department.ToListAsync()
            };        
                ViewData["DepartmentName"] = _context.Department.Find(deptID).DName;
                ViewData["DepartmentID"] = deptID;
            
            return View(student);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Students/Create
        public IActionResult Create(string deptID = "D01")
        {
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName");
            ViewData["DepartmentID"] = deptID;
            return View();
        }

        // POST: Students/Create
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
                return RedirectToAction("Index", new { deptID=student.DeptID});
            }
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", student.DeptID);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id, string deptID = "D01")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = deptID;
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", student.DeptID);
            return View(student);
        }

        // POST: Students/Edit/5
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
                return RedirectToAction("Index", new { deptID = student.DeptID });
            }
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", student.DeptID);
            return View(student);
        }

        // GET: Students/Delete/5
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

        // POST: Students/Delete/5
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
            return RedirectToAction(nameof(Index), new { deptID=student.DeptID});
        }

        private bool StudentExists(string id)
        {
            return _context.Student.Any(e => e.StuID == id);
        }
    }
}
