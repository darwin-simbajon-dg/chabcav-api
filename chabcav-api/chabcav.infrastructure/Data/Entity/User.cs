﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Data.Entity
{
    public class User : IdentityUser
    {
        // Add custom properties here if needed
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
