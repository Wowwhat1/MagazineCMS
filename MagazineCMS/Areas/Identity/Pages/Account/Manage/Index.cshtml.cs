﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MagazineCMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MagazineCMS.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly string _uploadsDirectory; // Directory to save uploaded avatars

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _uploadsDirectory = @"image\avatar\"; // Set the path to your uploads directory
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }

            // Add a property to hold the uploaded avatar file
            [Display(Name = "Avatar")]
            public IFormFile AvatarFile { get; set; }
            //public string AvatarUrl { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                //AvatarUrl = user.AvatarUrl

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            User user = (User)await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User user = (User)await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }


            // Handle avatar file upload
            if (Input.AvatarFile != null && Input.AvatarFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(Input.AvatarFile.FileName)}"; // Generate a unique filename
                string filePath = Path.Combine(_uploadsDirectory, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.AvatarFile.CopyToAsync(fileStream);
                }
                // Set the avatar URL to the path where the file is saved
                user.AvatarUrl = filePath; // You may need to store a relative path or a URL depending on your setup
            }
            /*if (true)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(Input.AvatarFile.FileName)}"; // Generate a unique filename
                string filePath = Path.Combine(_uploadsDirectory, fileName);
                System.Diagnostics.Debug.WriteLine("File Path: " + filePath);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.AvatarFile.CopyToAsync(fileStream);
                }

                *//*string basePath = "C:\\Users\\hp\\source\\repos\\MagazineCMS\\MagazineCMS";
                string relativePath = Path.Combine(basePath, Input.AvatarUrl);*/
            /*user.AvatarUrl = relativePath;*//*
            // Set the avatar URL to the path where the file is saved
            user.AvatarUrl = filePath; // You may need to store a relative path or a URL depending on your setup
        }*/





            user.Firstname = Input.Firstname;
            user.Lastname = Input.Lastname;

            

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}