using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Week.Areas.Admin.Attributes
{
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (String.IsNullOrEmpty(context.HttpContext.Session.GetString("user_email")))
            {
                context.HttpContext.Response.Redirect("/Admin/Account/Login");
            }

        }
        
    }
}
