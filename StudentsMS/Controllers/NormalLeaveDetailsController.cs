using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using StudentsMS.Models;
using StudentsMS.ViewModels;

namespace StudentsMS.Controllers
{
    public class NormalLeaveDetailsController : Controller
    {
        private readonly StudentsMSContext _context;

        public NormalLeaveDetailsController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: NormalLeaveDetails
        public async Task<IActionResult> Index()
        {
            string aa = HttpContext.Session.GetString("Account");

            var leaveDetail = await _context.LeaveDetail
                .Include(l => l.Leave)
                .Include(l => l.Stu)
               .Where(l => l.StuID == aa).ToListAsync();
            
            return View(leaveDetail);

            

        }

        // GET: NormalLeaveDetails/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveDetail = await _context.LeaveDetail
                .Include(l => l.Leave)
                .Include(l => l.Stu)
                .FirstOrDefaultAsync(m => m.LDID == id);
            if (leaveDetail == null)
            {
                return NotFound();
            }

            return View(leaveDetail);
        }

        // GET: NormalLeaveDetails/Create
        public IActionResult Create()
        {
            

            string aa = HttpContext.Session.GetString("Account");

            
            ViewData["LeaveID"] = new SelectList(_context.Leave, "LeaveID", "LName");
            ViewData["StuID"] = new SelectList(_context.Student.Where(s => s.StuID == aa), "StuID", "StuID");
            return View();
        }

        // POST: NormalLeaveDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LDID,StuID,LeaveID,LDDate,StartTime,EndTime")] LeaveDetail leaveDetail)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand()) 
            {
                command.CommandText = "SELECT dbo.getLDID()";

                _context.Database.OpenConnection();

                using (var result=command.ExecuteReader()) 
                {
                    if (result.Read()) 
                    {
                        leaveDetail.LDID = result.GetString(0);
                    }
                }
            }
            ModelState.Remove(nameof(leaveDetail.LDID));
            leaveDetail.LDDate= DateTime.Now;
            

                if (ModelState.IsValid)
                {
                    _context.Add(leaveDetail);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            ViewData["LeaveID"] = new SelectList(_context.Leave, "LeaveID", "LName", leaveDetail.LeaveID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", leaveDetail.StuID);
            return View(leaveDetail);
        }

        // GET: NormalLeaveDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveDetail = await _context.LeaveDetail.FindAsync(id);
            if (leaveDetail == null)
            {
                return NotFound();
            }
            ViewData["LeaveID"] = new SelectList(_context.Leave, "LeaveID", "LName", leaveDetail.LeaveID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", leaveDetail.StuID);
            return View(leaveDetail);
        }

        // POST: NormalLeaveDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LDID,StuID,LeaveID,LDDate,StartTime,EndTime")] LeaveDetail leaveDetail)
        {
            if (id != leaveDetail.LDID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveDetailExists(leaveDetail.LDID))
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
            ViewData["LeaveID"] = new SelectList(_context.Leave, "LeaveID", "LName", leaveDetail.LeaveID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", leaveDetail.StuID);
            return View(leaveDetail);
        }

        // GET: NormalLeaveDetails/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveDetail = await _context.LeaveDetail
                .Include(l => l.Leave)
                .Include(l => l.Stu)
                .FirstOrDefaultAsync(m => m.LDID == id);
            if (leaveDetail == null)
            {
                return NotFound();
            }

            return View(leaveDetail);
        }

        // POST: NormalLeaveDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var leaveDetail = await _context.LeaveDetail.FindAsync(id);
            if (leaveDetail != null)
            {
                _context.LeaveDetail.Remove(leaveDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveDetailExists(string id)
        {
            return _context.LeaveDetail.Any(e => e.LDID == id);
        }
    }
}
