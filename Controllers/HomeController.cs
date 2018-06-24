using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccount.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace BankAccount.Controllers
{
    public class HomeController : Controller
    {
        private BankAccountContext _dbContext;
        public HomeController(BankAccountContext context)
        {
            _dbContext = context;
        }
        
        [HttpGet("/")]
        public IActionResult Index()
        {
            // RegisterViewModel viewModel = new RegisterViewModel();
            return Redirect($"/login");
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View("Index");
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("register")]
        public IActionResult RegisterUser(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(_dbContext.users.Where(u => u.email == model.email).SingleOrDefault() != null)
                {
                    ModelState.AddModelError("Email", "Email is already in use. Please login.");
                }
                else
                {
                    PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                    model.password = Hasher.HashPassword(model, model.password);
                    User user = new User
                    {
                        first_name = model.first_name,
                        last_name = model.last_name,
                        email = model.email,
                        password = model.password,
                        balance = 0,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now,
                    };
                    _dbContext.users.Add(user);
                    _dbContext.SaveChanges();
                    int UserId = _dbContext.users.Last().user_id;
                    HttpContext.Session.SetInt32("UserId",UserId);
                    return Redirect($"/account/{UserId}");
                }
            }
            return View("Register",model);
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = _dbContext.users.SingleOrDefault(u => u.email == model.LogEmail);
                if(user == null)
                {
                    ModelState.AddModelError("LogEmail", "Email does not exist.");
                }
                else
                {
                    var Hasher = new PasswordHasher<User>();
                    if(0 == Hasher.VerifyHashedPassword(user, user.password, model.LogPassword))
                    {
                        ModelState.AddModelError("LogPassword", "Password is invalid.");
                    }
                    HttpContext.Session.SetInt32("UserId", user.user_id);
                    return Redirect($"account/{user.user_id}");
                }
            }
            return View("Index");
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginUser");
        }

        [HttpGet("account/{id}")]
        public IActionResult ShowAccount(int id)
        {
            int? UserId= HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            if(id != UserId)
            {
                return Redirect($"/account/{UserId}");
            }
            User user = _dbContext.users
                        .Include(u=>u.transactions)
                        .Where(u=>u.user_id == id).SingleOrDefault();
            if(user.transactions != null)
            {
                user.transactions = user.transactions.OrderByDescending(t=>t.created_at).ToList();
            }
            return View(user);
        }

        [HttpPost("process")]
        public IActionResult Process(float amt)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            User user = _dbContext.users.Where(u=>u.user_id == UserId).SingleOrDefault();
            if(amt == 0)
            {
                TempData["Error"] = "Please specify the amount.";
            }
            else if(amt < 0 && ((amt * -1 ) > user.balance))
            {
                TempData["Error"] = "Invalid Transaction. Insufficient balance.";
            }else{
                Transactions NewTransaction = new Transactions
                {
                    amount = amt,
                    User = user,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                user.balance += amt;
                _dbContext.transactions.Add(NewTransaction); 
                _dbContext.SaveChanges();
            }
            return Redirect("account/{id}");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
