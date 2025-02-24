using System.Diagnostics;
using FoodieTime.Data;
using FoodieTime.Data.Models;
using FoodieTime.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodieTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allPosts = await _context.Posts
                .Include(n => n.User)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();
                
            return View(allPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            // Get the logged in user
            int loggedInUser = 1;

            //Create a new post
            var newPost = new Post
            {
                Content = post.Content,
                Restaurant = post.Restaurant,
                Dish = post.Dish,
                Rating = post.Rating,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ImageUrl = "",
                NrOfReports = 0,
                UserId = loggedInUser
            };

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
