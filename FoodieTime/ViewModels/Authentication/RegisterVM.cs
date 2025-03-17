﻿using System.ComponentModel.DataAnnotations;

namespace FoodieTime.ViewModels.Authentication
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First Name must be between two and 20 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must only contain letters")]
        public string FirstName {  get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last Name must be between two and 20 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name must only contain letters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }

}
