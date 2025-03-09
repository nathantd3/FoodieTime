﻿using FoodieTime.Data.Services;
using FoodieTime.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;

namespace FoodieTime.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IFilesService _filesService;
        public SettingsController(IUsersService usersService, IFilesService filesService)
        {
            _usersService = usersService;
            _filesService = filesService;
        }
        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 1;
            var userDb = await _usersService.GetUser(loggedInUserId);
            return View(userDb);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(UpdateProfilePictureVM profilPictureVM)
        {
            var loggedInUser = 1;
            var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilPictureVM.ProfilePictureImage, Data.Helpers.Enums.ImageFileType.ProfilePicture);

            await _usersService.UpdateUserProfilePicture(loggedInUser, uploadedProfilePictureUrl);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileVM profileVM)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM updatePasswordVM)
        {
            return RedirectToAction("Index");
        }
    }
}
