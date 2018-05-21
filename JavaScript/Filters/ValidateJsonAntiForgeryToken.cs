﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace JavaScript.Filters
{


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateJsonAntiForgeryToken : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                if (request.IsAjaxRequest())
                    AntiForgery.Validate(CookieValue(request), request.Headers["__RequestVerificationToken"]);
                else
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
            }
        }

        private string CookieValue(HttpRequestBase request)
        {
            var cookie = request.Cookies[AntiForgeryConfig.CookieName];
            return cookie != null ? cookie.Value : null;
        }
    }
}