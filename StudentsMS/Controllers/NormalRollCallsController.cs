using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using StudentsMS.Models;
using StudentsMS.ViewModels;


namespace StudentsMS.Controllers
{
    public class NormalRollCallsController : Controller
    {
        private readonly StudentsMSContext _context;

        public NormalRollCallsController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: NormalRollCalls
        public async Task<IActionResult> Index()
        {
            //var studentsMSContext = _context.RollCall.Include(r => r.CT).Include(r => r.Course).Include(r => r.Dept).Include(r => r.Stu);
            //return View(await studentsMSContext.ToListAsync());

            string aa = HttpContext.Session.GetString("Account");

            var rollCall = await _context.RollCall
                .Include(r => r.CT)
                .Include(r => r.Course)
                .Include(r => r.Dept)
                .Include(r => r.Stu)
                .Where(r => r.StuID == aa)
                .ToListAsync();
            return View(rollCall);
        }

        // GET: NormalRollCalls/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rollCall = await _context.RollCall
                .Include(r => r.CT)
                .Include(r => r.Course)
                .Include(r => r.Dept)
                .Include(r => r.Stu)
                .FirstOrDefaultAsync(m => m.StuID == id);
            if (rollCall == null)
            {
                return NotFound();
            }

            return View(rollCall);
        }

        // GET: NormalRollCalls/Create
        public IActionResult Create()
        {
            string aa = HttpContext.Session.GetString("Account");

            var result = _context.SelectCourse.Where(m => m.StuID == aa).Include(m=>m.Course);
       
            //ViewData["RCID"] = new SelectList(_context.RollCall, "RCID", "RCID");
            //ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID");
            //ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName");
            //ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName");
           
            ViewData["StuID"] = aa;
            ViewData["Result"] = result; 

            return View();
        }

        // POST: NormalRollCalls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RCID,StuID,CourseID,DeptID,CTID,RCDate,ArrivalTime")] string StuID, string CourseID)
        {
            //string aa = HttpContext.Session.GetString("Account");
          

            var StuSelect = await _context.SelectCourse.Where(m => m.StuID == StuID && m.CourseID == CourseID).FirstOrDefaultAsync();


            var rollCall = new RollCall
            {
                RCID = "",
                StuID = StuSelect.StuID,
                DeptID=StuSelect.DeptID,
                CourseID= StuSelect.CourseID,
                CTID= StuSelect.CTID,
                RCDate= DateTime.Now,
                ArrivalTime= DateTime.Now
            };


            using (var command = _context.Database.GetDbConnection().CreateCommand()) //建立了一個資料庫連線，並創建一個用來執行 SQL 指令的物件。
            {
                command.CommandText = "SELECT dbo.getRCID()"; //設定了要執行的 SQL 指令。這裡呼叫了一個名為 dbo.getRCID() 的儲存程序


                _context.Database.OpenConnection(); //開啟資料庫連線

                using (var result = command.ExecuteReader()) //執行 SQL 指令並將結果儲存到一個 DataReader 物件中。
                {
                    if (result.Read()) //檢查是否有查詢結果
                    {
                        rollCall.RCID = result.GetString(0);// 如果有查詢結果，則將第一個欄位 (索引為 0) 的值 (也就是 RCID) 賦值給 rollCall 物件的 RCID 屬性。
                    }
                }
                //var result = _context.Set<StringResult>().FromSqlRaw("SELECT dbo.getRCID() AS Value").FirstOrDefault();
            }
            ModelState.Remove(nameof(rollCall.RCID));
            //rollCall.RCID = result.Value;

            //rollCall.RCDate = DateTime.Now;
            //rollCall.ArrivalTime = DateTime.Now;
            //ModelState.Remove(rollCall.RCID);
            

            if (ModelState.IsValid)
            {
                _context.Add(rollCall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", rollCall.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", rollCall.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", rollCall.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", rollCall.StuID);
            return View(rollCall);
        }

        // GET: NormalRollCalls/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rollCall = await _context.RollCall.FindAsync(id);
            if (rollCall == null)
            {
                return NotFound();
            }
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", rollCall.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", rollCall.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", rollCall.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", rollCall.StuID);
            return View(rollCall);
        }

        // POST: NormalRollCalls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StuID,CourseID,DeptID,CTID,RCDate,ArrivalTime")] RollCall rollCall)
        {
            if (id != rollCall.StuID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rollCall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RollCallExists(rollCall.StuID))
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
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", rollCall.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", rollCall.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", rollCall.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", rollCall.StuID);
            return View(rollCall);
        }

        // GET: NormalRollCalls/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rollCall = await _context.RollCall
                .Include(r => r.CT)
                .Include(r => r.Course)
                .Include(r => r.Dept)
                .Include(r => r.Stu)
                .FirstOrDefaultAsync(m => m.StuID == id);
            if (rollCall == null)
            {
                return NotFound();
            }

            return View(rollCall);
        }

        // POST: NormalRollCalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var rollCall = await _context.RollCall.FindAsync(id);
            if (rollCall != null)
            {
                _context.RollCall.Remove(rollCall);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RollCallExists(string id)
        {
            return _context.RollCall.Any(e => e.StuID == id);
        }
    }
}
