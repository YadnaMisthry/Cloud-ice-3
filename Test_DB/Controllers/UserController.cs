using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Test_DB.Models;
using Test_DB.Models; // Update to your actual namespace

public class UserController : Controller
{
    private TestDbContext db = new TestDbContext();

    public ActionResult Index(string searchString, string sortOrder, string emailDomain)
    {
        var users = db.GetUsers().AsQueryable();

        if (!String.IsNullOrEmpty(searchString))
        {
            users = users.Select(u => new
            {
                User = u,
                Score = (u.Username.Contains(searchString) ? 3 : 0)
                      + (u.Email.Contains(searchString) ? 2 : 0)
                      + (u.Bio.Contains(searchString) ? 1 : 0)
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Select(x => x.User);
        }

        if (!String.IsNullOrEmpty(emailDomain))
        {
            users = users.Where(u => u.Email.EndsWith(emailDomain));
        }

        switch (sortOrder)
        {
            case "username_desc":
                users = users.OrderByDescending(u => u.Username);
                break;
            case "email":
                users = users.OrderBy(u => u.Email);
                break;
            case "email_desc":
                users = users.OrderByDescending(u => u.Email);
                break;
            default:
                users = users.OrderBy(u => u.Username);
                break;
        }

        return View(users.ToList());
    }
}
