using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoreWebApp.Data;
using CoreWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreWebApp.Controllers
{
    [Authorize(Policy = "RequireAdministratorRole")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            UsersViewModel usersViewModel = new UsersViewModel();
            usersViewModel = await FetchUsers(usersViewModel);
            return View(usersViewModel);
        }

        private async Task<UsersViewModel> FetchUsers(UsersViewModel usersViewModel)
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UsersWithRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UsersWithRolesViewModel();
                thisViewModel.UserName = user.UserName;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.UserId = user.Id;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            usersViewModel.Users = userRolesViewModel;
            usersViewModel.SelectedUser = null;
            var roles = await _roleManager.Roles.ToListAsync();
            usersViewModel.Roles = roles;
            ViewBag.Roles = roles;
            usersViewModel.DisplayMode = "WriteOnly";
            return usersViewModel;
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            UsersViewModel usersViewModel = new UsersViewModel();
            usersViewModel = await FetchUsers(usersViewModel);
            var selectedUser = await _userManager.FindByIdAsync(id);
            var thisViewModel = new UsersWithRolesViewModel();
            thisViewModel.UserId = selectedUser.Id;
            thisViewModel.UserName = selectedUser.UserName;
            thisViewModel.Email = selectedUser.Email;
            thisViewModel.FirstName = selectedUser.FirstName;
            thisViewModel.LastName = selectedUser.LastName;
            var roles = await GetUserRoles(selectedUser);
            thisViewModel.Roles = roles;
            thisViewModel.SelectedRole = (roles.Count > 0) ? roles[0] : "";
            usersViewModel.SelectedUser = thisViewModel;
            usersViewModel.DisplayMode = "ReadWrite";
            return View("Index", usersViewModel);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UsersWithRolesViewModel user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.Email,
                };
                var result = await _userManager.CreateAsync(newUser, "Default@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, user.SelectedRole);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            UsersViewModel usersViewModel = new UsersViewModel();
            usersViewModel = await FetchUsers(usersViewModel);
            return View("Index", usersViewModel);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UsersWithRolesViewModel user)
        {
            var selectedUser = await _userManager.FindByIdAsync(user.UserId);
            
            if (selectedUser == null)
            {
                return NotFound($"Unable to load user with ID '{user.UserId}'.");
            }
            if (ModelState.IsValid)
            {
                selectedUser.FirstName = user.FirstName;
                selectedUser.LastName = user.LastName;
                selectedUser.Email = user.Email;

                await _userManager.UpdateAsync(selectedUser);

                var selectedUserRole = await _userManager.GetRolesAsync(selectedUser);

                // Add new role
                if (!selectedUserRole.Contains(user.SelectedRole))
                {
                    await _userManager.AddToRoleAsync(selectedUser, user.SelectedRole);
                }

                // Remove last role
                foreach(var role in selectedUserRole)
                {
                    if (role != user.SelectedRole)
                    {
                        await _userManager.RemoveFromRoleAsync(selectedUser, role);
                    }
                }
            }

            UsersViewModel usersViewModel = new UsersViewModel();
            usersViewModel = await FetchUsers(usersViewModel);
            usersViewModel.DisplayMode = "ReadWrite";
            usersViewModel.SelectedUser = user;
            return View("Index", usersViewModel);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var selectedUser = await _userManager.FindByIdAsync(id);

            if (selectedUser == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }

            await _userManager.DeleteAsync(selectedUser);

            UsersViewModel usersViewModel = new UsersViewModel();
            usersViewModel = await FetchUsers(usersViewModel);
            return View("Index", usersViewModel);
        }
    }
}
