using SurveysApp.Models;
using SurveysApp.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SurveysApp.Security
{
    public class UserManager
    {
        public static User User
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return ((CustomPrincipal)(HttpContext.Current.User)).User;
                }
                else if (HttpContext.Current.Items.Contains("User"))
                {                   
                    return (User)HttpContext.Current.Items["User"];
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool Validate(UserLoginViewModel userLoginViewModel, HttpResponseBase response)
        {
            if (Membership.ValidateUser(userLoginViewModel.Email, userLoginViewModel.Password) == false)
            {
                return false;
            }

            var serializer = new JavaScriptSerializer();

            string userDataString = serializer.Serialize(User);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                userLoginViewModel.Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(300),
                false,
                userDataString,
                FormsAuthentication.FormsCookiePath
            );

            string encTicket = FormsAuthentication.Encrypt(ticket);

            response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            return true;
        }
    }
}