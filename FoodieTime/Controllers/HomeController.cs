using System.Diagnostics;
using FoodieTime.Data;
using FoodieTime.Data.Models;
using FoodieTime.Data.Services;
using FoodieTime.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodieTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IPostsService _postsService;
        private readonly IFilesService _filesService;
        public HomeController(ILogger<HomeController> logger, AppDbContext context, 
            IPostsService postsService, IFilesService filesService)
        {
            _logger = logger;
            _context = context;
            _postsService = postsService;
            _filesService = filesService;
        }

        public async Task<IActionResult> Index()
        {
            int loggedInUserId = 1;

            var allPosts = await _postsService.GetAllPostsAsync(loggedInUserId);
                
            return View(allPosts);
        }

        public async Task<IActionResult> Details(int postId)
        {
            var post = await _postsService.GetPostByIdAsync(postId);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            // Get the logged in user
            int loggedInUser = 1;

            var imageUploadPath = await _filesService.UploadImageAsync(post.Image, Data.Helpers.Enums.ImageFileType.PostImage);

            //Create a new post
            var newPost = new Post
            {
                Content = post.Content,
                Restaurant = post.Restaurant,
                Dish = post.Dish,
                Rating = post.Rating,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ImageUrl = imageUploadPath,
                NrOfReports = 0,
                UserId = loggedInUser
            };

            await _postsService.CreatePostAsync(newPost);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            int loggedInUserId = 1;

            await _postsService.TogglePostLikeAsync(postLikeVM.PostId, loggedInUserId);
 
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {
            int loggedInUserId = 1;

            await _postsService.TogglePostFavoriteAsync(postFavoriteVM.PostId, loggedInUserId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            int loggedInUserId = 1;

            await _postsService.TogglePostVisabilityAsync(postVisibilityVM.PostId, loggedInUserId);

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

            await _postsService.AddPostCommentAsync(newComment);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            await _postsService.RemovePostCommentAsync(removeCommentVM.CommentId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PostRemove(PostRemoveVM postRemoveVM)
        {
            await _postsService.PostRemoveAsync(postRemoveVM.PostId);

            return RedirectToAction("Index");
        }

    }
}
