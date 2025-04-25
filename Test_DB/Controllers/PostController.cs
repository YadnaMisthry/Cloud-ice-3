using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test_DB.Models;
using Microsoft.AspNetCore.Mvc;
using Test_DB.Models;
using System.Linq;

public class PostController : Controller
{
    private readonly TestDbContext _db;

    public PostController(TestDbContext db)
    {
        _db = db;
    }

    public ActionResult Index(string searchString, string sortOrder)
    {
        var posts = _db.GetPosts().Include(p => p.User).AsQueryable();

        // Search functionality
        if (!string.IsNullOrEmpty(searchString))
        {
            posts = posts.Where(p => p.Content.Contains(searchString) || p.User.Username.Contains(searchString));
        }

        // Sorting functionality
        switch (sortOrder)
        {
            case "newest":
                posts = posts.OrderByDescending(p => p.CreatedAt);
                break;
            case "oldest":
                posts = posts.OrderBy(p => p.CreatedAt);
                break;
            default:
                posts = posts.OrderByDescending(p => p.CreatedAt); // Default sorting by newest
                break;
        }

        return View(posts.ToList());
    }
}