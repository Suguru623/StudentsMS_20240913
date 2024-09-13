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
    public class LeaveDetailsController : Controller
    {
        private readonly StudentsMSContext _context;

        public LeaveDetailsController(StudentsMSContext context)
        {
            _context = context;
        }

        //GET: LeaveDetails
        public async Task<IActionResult> Index(string searchString)
        {
            var studentsMSContext = _context.LeaveDetail.Include(l => l.Leave).Include(l => l.Stu);

            if (!string.IsNullOrEmpty(searchString)) 
            {
                //加if判斷式判斷沒有此學號紀錄的話顯示?
                var leaveDetail = await _context.LeaveDetail
                    .Include(l => l.Leave)
                    .Include(l => l.Stu)
                    .FirstOrDefaultAsync(l => l.StuID == searchString);
                if (leaveDetail == null)
                {
                    ViewData["Message"] = "查無此學生的請假紀錄。";
                    return View(new List<LeaveDetail>());
                }

                var NEWstudentsMSContext = await _context.LeaveDetail
                    .Include(l => l.Leave)
                    .Include(l => l.Stu)
                    .Where(l => l.StuID == leaveDetail.StuID)
                    .ToListAsync();

                return View( NEWstudentsMSContext);
            }

            return View(await studentsMSContext.ToListAsync());

           

            // 將當前的過濾字串存儲在 ViewData 中，以便在視圖中使用
            //ViewData["CurrentFilter"] = searchString;



        }



        // GET: LeaveDetails/Details/5
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

        // GET: LeaveDetails/Create
        public IActionResult Create()
        {
            
            ViewData["LeaveID"] = new SelectList(_context.Leave, "LeaveID", "LName");
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID");
            return View();
        }

        // POST: LeaveDetails/Create
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

                using (var result = command.ExecuteReader()) 
                {
                    if (result.Read()) 
                    {
                        leaveDetail.LDID = result.GetString(0);
                    }
                }
            }
            

            leaveDetail.LDDate=DateTime.Now;
            ModelState.Remove(nameof(leaveDetail.LDID));
            //ModelState 用於進行模型驗證。如果某個欄位在 ModelState 中無法通過驗證，會阻止資料儲存。
            //這裡移除了 LDID 的驗證，因為 LDID 是自動從資料庫產生的，因此不需要來自用戶端的輸入。
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

        // GET: LeaveDetails/Edit/5
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

        // POST: LeaveDetails/Edit/5
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

        // GET: LeaveDetails/Delete/5
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

        // POST: LeaveDetails/Delete/5
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
