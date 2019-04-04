using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SurveysApp.Models;
using System.Security.Principal;
using System.Web.Security;

namespace SurveysApp.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public User User { get; set; }

        public IIdentity Identity { get; private set; }

        public CustomPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            // return Roles.IsUserInRole(User.Username, role);
            return true;
        }
    }
}