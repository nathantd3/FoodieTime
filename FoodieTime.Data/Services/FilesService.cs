﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodieTime.Data.Helpers.Enums;
using FoodieTime.Data.Models;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace FoodieTime.Data.Services
{
    public class FilesService : IFilesService
    {
        public async Task<string> UploadImageAsync(IFormFile file, ImageFileType imageFileType)
        {
            string filePathUpload = imageFileType switch
            {
                ImageFileType.PostImage => Path.Combine("images", "posts"),
                ImageFileType.StoryImage => Path.Combine("images", "stories"),
                ImageFileType.ProfilePicture => Path.Combine("images", "profilePictures"),
                ImageFileType.CoverImage => Path.Combine("images", "covers"),
                _ => throw new ArgumentException("Invalid file type")
            };

            if (file != null && file.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (file.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, filePathUpload);
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);

                    return $"{filePathUpload}\\{fileName}";
                }
            }
            return "";
        }
    }
}
