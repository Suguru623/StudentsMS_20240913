using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using StudentsMS.Models;
using StudentsMS.ViewModels;

namespace StudentsMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly StudentsMSContext _context;

        public LoginController(StudentsMSContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            string accountId = HttpContext.Session.GetString("Account");

            
                id = accountId; 
            

            var login = await _context.Login.FirstOrDefaultAsync(l => l.Account == id);
            if (login == null)
            {
                return NotFound();
            }
            return View(login);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Login login)
        {
            string accountId = HttpContext.Session.GetString("Account");
            if (login.Account != accountId)
            {
                return NotFound();
            }

            var existingLogin = await _context.Login.FirstOrDefaultAsync(l => l.Account == accountId);
            //if (existingLogin == null)
            //{
            //    return NotFound();
            //}

            // 更新密码
            existingLogin.Password = login.Password;            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existingLogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoginExists(existingLogin.Account))
                    {
                        return NotFound();

                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","NormalStudents");
            }
            return View(existingLogin);
        }

        private bool LoginExists(string id)
        {
            return _context.Login.Any(l => l.Account == id);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Account,Password,Type")] Login login)
        {
            if (ModelState.IsValid)
            {
                _context.Add(login);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Students");
            }
            return View(login);
        }

        // GET: Login/LossPassword
        public IActionResult LossPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LossPassword(Login login)
        {
            string accountId = HttpContext.Session.GetString("Account");
            if (login.Account != accountId)
            {
                return NotFound();
            }

            var existingLogin = await _context.Login.FirstOrDefaultAsync(l => l.Account == accountId);
            //if (existingLogin == null)
            //{
            //    return NotFound();
            //}

            // 更新密码
            existingLogin.Password = login.Password;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existingLogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoginExists(existingLogin.Account))
                    {
                        return NotFound();

                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "NormalStudents");
            }
            return View(existingLogin);
        }



        //4.2.4 建立Get與Post的Login Action
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login) //async Task<> 改成非同步，效能較好
        {
            //如果帳密正確，導入後台畫面
            //如果帳密不正確，回到登入畫面並告知帳密錯誤

            if (login == null)
            {
                return View();
            }

            //select * from Login Where account=@account and password=@password
            var result = await _context.Login.Where(m => m.Account == login.Account && m.Password == login.Password).FirstOrDefaultAsync();
            //if (result.type == 1) 登入的人是誰用type分辨，type:1為admin、type:2為一般學生
            //{
            //      改變layout
            //}
            if (result == null) //如果帳密不正確，回到登入畫面並告知帳密錯誤
            {
                //4.2.6 將ViewData["Error"]加入Login View
                ViewData["Error"] = "帳號或密碼錯誤";
                return View();
            }
            //如果帳密正確，導入後台畫面
            
                //Session["Manager"] = "登入成功";
                HttpContext.Session.SetString("Manager", JsonConvert.SerializeObject(login));
                HttpContext.Session.SetString("Account", login.Account);
                if (result.type == 1) //type: 1為admin
                {
                    return RedirectToAction("Index", "Students");
                }
               
                //if(result.type == "2") //type: 2為一般學生
                //{
                    return RedirectToAction("Index", "NormalStudents");
                //}


                ////Session["Manager"] = "登入成功";
                //HttpContext.Session.SetString("Manager", JsonConvert.SerializeObject(login));

                //return RedirectToAction("Index", "Students");
            

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Manager");

            return RedirectToAction("Index", "Home");


            //return View();
        }
    }
}
