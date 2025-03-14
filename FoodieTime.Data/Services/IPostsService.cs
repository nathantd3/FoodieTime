﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodieTime.Data.Models;
using Microsoft.AspNetCore.Http;

namespace FoodieTime.Data.Services
{
    public interface IPostsService
    {
        Task<List<Post>> GetAllPostsAsync(int loggedInUserId);
        Task<Post> GetPostByIdAsync(int postId);
        Task<List<Post>> GetAllFavoritedPostsAsync(int loggedInUserId);
        Task<Post> CreatePostAsync(Post post);
        Task AddPostCommentAsync(Comment comment);
        Task RemovePostCommentAsync(int commentId);
        Task TogglePostLikeAsync(int postId, int userId); 
        Task TogglePostFavoriteAsync(int postId, int userId);
        Task TogglePostVisabilityAsync(int postId, int userId);
        Task PostRemoveAsync(int postId);
    }
}
