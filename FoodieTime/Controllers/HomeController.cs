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
            int loggedInUserId = 1;

            var allPosts = await _context.Posts
                .Where(n => !n.IsPrivate || n.UserId == loggedInUserId)
                .Include(n => n.User)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User)
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

            if(post.Image != null && post.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if(post.Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/uploaded");
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await post.Image.CopyToAsync(stream);

                    newPost.ImageUrl = "/images/uploaded/" + fileName;
                }
            }

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            int loggedInUserId = 1;

            var like = await _context.Likes
                .Where(l => l.PostId == postLikeVM.PostId && l.UserId == loggedInUserId)
                .FirstOrDefaultAsync();

            if(like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newLike = new Like()
                {
                    PostId = postLikeVM.PostId,
                    UserId = loggedInUserId
                };
                await _context.Likes.AddAsync(newLike);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {
            int loggedInUserId = 1;

            var favorite = await _context.Favorites
                .Where(l => l.PostId == postFavoriteVM.PostId && l.UserId == loggedInUserId)
                .FirstOrDefaultAsync();

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newFavorite = new Favorite()
                {
                    PostId = postFavoriteVM.PostId,
                    UserId = loggedInUserId
                };
                await _context.Favorites.AddAsync(newFavorite);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            int loggedInUserId = 1;

            var post = await _context.Posts
                .FirstOrDefaultAsync(l => l.Id == postVisibilityVM.PostId && l.UserId == loggedInUserId);

            if(post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
        {
            int loggedInUserId = 1;

            var newComment = new Comment()
            {
                UserId = loggedInUserId,
                PostId = postCommentVM.PostId,
                Content = postCommentVM.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            var commentDb = await _context.Comments.FirstOrDefaultAsync(c => c.Id == removeCommentVM.CommentId);

            if(commentDb != null)
            {
                _context.Comments.Remove(commentDb);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


    }
}
