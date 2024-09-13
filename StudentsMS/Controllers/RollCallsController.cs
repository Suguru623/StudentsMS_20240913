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
    public class RollCallsController : Controller
    {
        private readonly StudentsMSContext _context;

        public RollCallsController(StudentsMSContext context)
        {
            _context = context;
        }

        // GET: RollCalls
        public async Task<IActionResult> Index(string stuID = "113D01101", string deptID = "D01", string courseID = "C01", string ctID = "T11", DateTime? selectedDate = null)
        {
            //var studentsMSContext = _context.RollCall.Include(r => r.CT).Include(r => r.Course).Include(r => r.Dept).Include(r => r.Stu);
            //return View(await studentsMSContext.ToListAsync());

            var rc = new VMRollCall()
            {
                RollCall = await _context.RollCall.Where(r => r.DeptID == deptID && r.CourseID == courseID)
                            .Where(r => !selectedDate.HasValue || r.RCDate.Date == selectedDate.Value.Date)
                            .ToListAsync(),
                //RCID = await _context.RollCall.ToListAsync(),
                Student = await _context.Student.ToListAsync(),
                Department = await _context.Department.ToListAsync(),
                Course = await _context.Course.ToListAsync(),
                ClassTime = await _context.ClassTime.ToListAsync()
            };

            // 將科系ID和課程ID儲存在 ViewData 中，這樣在表單提交後可以保持這些值
            ViewData["DepartmentName"] = (await _context.Department.FindAsync(deptID))?.DName ?? "選擇科系";
            ViewData["CourseName"] = (await _context.Course.FindAsync(courseID))?.CName ?? "選擇課程";
            //ViewData["DepartmentName"] = _context.Department.Find(deptID).DName;
            //ViewData["CourseName"] = _context.Course.Find(courseID).CName;
            ViewData["DepartmentID"] = deptID;
            ViewData["CourseID"] = courseID;

            // 如果選擇了日期，將日期格式化為 "yyyy-MM-dd" 字串並存入 ViewData，否則設為空字串
            ViewData["SelectedDate"] = selectedDate?.ToString("yyyy-MM-dd") ?? string.Empty;
            ViewData["StuID"] = stuID;
            ViewData["CtID"] = ctID;

            return View(rc);
        }

        // GET: RollCalls/Details/5
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
                .FirstOrDefaultAsync(m => m.RCID == id);
            if (rollCall == null)
            {
                return NotFound();
            }

            return View(rollCall);
        }

        // GET: RollCalls/Create
        public IActionResult Create()
        {
            //ViewData["RCID"] = new SelectList(_context.RollCall, "RCID", "RCID");
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID");
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName");
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName");
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID");
            return View();
        }

        // POST: RollCalls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RCID,StuID,CourseID,DeptID,CTID,RCDate,ArrivalTime")] RollCall rollCall)
        {
            var studentName = string.Empty;

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
            

            //??????
            //rollCall.RCID = _context.RollCall.FromSqlRaw("select dbo.getRCID()").SingleOrDefault().ToString();
            rollCall.ArrivalTime = DateTime.Now; //時間戳記
            rollCall.RCDate = DateTime.Now; //時間戳記
            ModelState.Remove(nameof(rollCall.RCID));
            //ModelState 用於進行模型驗證。如果某個欄位在 ModelState 中無法通過驗證，會阻止資料儲存。
            //這裡移除了 RCID 的驗證，因為 RCID 是自動從資料庫產生的，因此不需要來自用戶端的輸入。
            if (ModelState.IsValid)
            {
                _context.Add(rollCall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            //ViewData["RCID"] = new SelectList(_context.RollCall, "RCID", "RCID");
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", rollCall.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", rollCall.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", rollCall.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", rollCall.StuID);
            return View(rollCall);
        }

        // GET: RollCalls/Edit/5
        public async Task<IActionResult> Edit(string id)
        {//string RCID,string StuID, string CourseID, string DeptID, string CTID
            if (id == null)
            {
                return NotFound();
            }
            
            var rollCall = await _context.RollCall.FindAsync(id);
            if (rollCall == null)
            {
                return NotFound();
            }
            //ViewData["RCID"] = new SelectList(_context.RollCall, "RCID", "RCID");
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", rollCall.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", rollCall.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", rollCall.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", rollCall.StuID);
            return View(rollCall);
        }

        // POST: RollCalls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RCID,StuID,CourseID,DeptID,CTID,RCDate,ArrivalTime")] RollCall rollCall)
        { 
            if (id != rollCall.RCID)
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
                    if (!RollCallExists(rollCall.RCID))
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
            //ViewData["RCID"] = new SelectList(_context.RollCall, "RCID", "RCID");
            ViewData["CTID"] = new SelectList(_context.ClassTime, "CTID", "CTID", rollCall.CTID);
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CName", rollCall.CourseID);
            ViewData["DeptID"] = new SelectList(_context.Department, "DeptID", "DName", rollCall.DeptID);
            ViewData["StuID"] = new SelectList(_context.Student, "StuID", "StuID", rollCall.StuID);
            return View(rollCall);
        }

        // GET: RollCalls/Delete/5
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
                .FirstOrDefaultAsync(r => r.RCID == id);
            if (rollCall == null)
            {
                return NotFound();
            }

            return View(rollCall);
        }

        // POST: RollCalls/Delete/5
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

        private bool RollCallExists(string RCID)
        {
            return _context.RollCall.Any(r => r.RCID == RCID);
        }
    }
}
