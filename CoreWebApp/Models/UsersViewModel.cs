using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApp.Models
{
    public class UsersViewModel
    {
        public List<UsersWithRolesViewModel> Users { get; set; }
        public UsersWithRolesViewModel SelectedUser { get; set; }
        public string DisplayMode { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}
