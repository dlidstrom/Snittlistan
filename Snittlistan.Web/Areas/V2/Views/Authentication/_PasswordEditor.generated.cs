﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Snittlistan.Web.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Snittlistan.Web.Areas.V2;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.HtmlHelpers;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;
    using Snittlistan.Web.ViewModels;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/Authentication/_PasswordEditor.cshtml")]
    public partial class _Areas_V2_Views_Authentication__PasswordEditor_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<Snittlistan.Web.Areas.V2.Controllers.AuthenticationController.PasswordViewModel>
    {
        public _Areas_V2_Views_Authentication__PasswordEditor_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
Write(Html.DisplayFor(model => Html.ViewData.ModelState, "ValidationSummary", new { message = "Det gick inte att logga in." }));

            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"control-group\">\r\n    ");


            
            #line 5 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
Write(Html.LabelFor(x => x.Email, new { @class = "control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div class=\"controls\">\r\n        ");


            
            #line 7 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
   Write(Html.HiddenFor(x => x.Email));

            
            #line default
            #line hidden
WriteLiteral("\r\n        ");


            
            #line 8 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
   Write(Html.TextBoxFor(x => x.Email, new { disabled = "disabled" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n</div>\r\n<div class=\"control-group\">\r\n    ");


            
            #line 12 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
Write(Html.LabelFor(x => x.Password, new { @class = "control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div class=\"controls\">\r\n");


            
            #line 14 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
         if (Model.OneTimeKey != null)
        {
            
            
            #line default
            #line hidden
            
            #line 16 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
       Write(Html.TextBoxFor(
                x => x.Password,
                new
                {
                    placeholder = "Ange kod du fått i mailen",
                    pattern = @"\d*",
                    required = "required",
                    type = "text",
                    minlength = "6",
                    maxlength = "6",
                    title = "Ange kod du fått i mailen, 6 siffror"
                }));

            
            #line default
            #line hidden
            
            #line 27 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
                  
        }
        else
        {
            
            
            #line default
            #line hidden
            
            #line 31 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
       Write(Html.TextBoxFor(x => x.Password, new
            {
                type = "password",
                placeholder = "Lösenord",
                required = "required"
            }));

            
            #line default
            #line hidden
            
            #line 36 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
              
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>\r\n<div class=\"control-group\">\r\n    ");


            
            #line 41 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
Write(Html.LabelFor(x => x.RememberMe, new { @class = "control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div class=\"controls\">\r\n        ");


            
            #line 43 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
   Write(Html.CheckBoxFor(x => x.RememberMe));

            
            #line default
            #line hidden
WriteLiteral(@"
        <span class=""help-block"">
            Förbli inloggad även när sidan stängs.
        </span>
    </div>
</div>
<div class=""control-group"">
    <div class=""controls"">
        <button class=""btn btn-primary btn-large"" type=""submit"">Logga in</button>
        <a class=""btn btn-large"" href=""");


            
            #line 52 "..\..\Areas\V2\Views\Authentication\_PasswordEditor.cshtml"
                                  Write(Url.Action("Index", "Roster"));

            
            #line default
            #line hidden
WriteLiteral("\">Avbryt</a>\r\n    </div>\r\n</div>\r\n");


        }
    }
}
#pragma warning restore 1591
