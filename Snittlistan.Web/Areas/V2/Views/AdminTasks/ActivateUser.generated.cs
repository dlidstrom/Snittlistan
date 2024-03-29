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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/AdminTasks/ActivateUser.cshtml")]
    public partial class _Areas_V2_Views_AdminTasks_ActivateUser_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<UserViewModel>
    {
        public _Areas_V2_Views_AdminTasks_ActivateUser_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
  
    ViewBag.Title = Model.IsActive ? "Inaktivera användare" : "Aktivera användare";
    var action = Model.IsActive ? "inaktivera" : "aktivera";


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"span8\">\r\n        <h3>");


            
            #line 10 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
       Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h3>\r\n");


            
            #line 11 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
         using (Html.BeginForm("ActivateUser", "AdminTasks", FormMethod.Post, new { @class = "form-horizontal" }))
        {

            
            #line default
            #line hidden
WriteLiteral("            <p>Är du säker på att du vill ");


            
            #line 13 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
                                     Write(action);

            
            #line default
            #line hidden
WriteLiteral(" denna användare?</p>\r\n");



WriteLiteral("            <h4>Namn: ");


            
            #line 14 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
                 Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral(", e-postadress: ");


            
            #line 14 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
                                            Write(Model.Email);

            
            #line default
            #line hidden
WriteLiteral("</h4>\r\n");


            
            #line 15 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
            if (Model.IsActive == false)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"control-group\">\r\n                    <div class=\"cont" +
"rols\">\r\n                        <label class=\"checkbox\">\r\n                      " +
"      ");


            
            #line 20 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
                       Write(Html.CheckBox("invite"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            Skicka inbjudnings-mail (kräver lösenordsbyte)\r\n   " +
"                     </label>\r\n                    </div>\r\n                </div" +
">\r\n");


            
            #line 25 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"form-actions\">\r\n                <button class=\"btn btn-pr" +
"imary btn-large\" type=\"submit\">Spara</button>\r\n                ");


            
            #line 28 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
           Write(Html.ActionLink("Avbryt", "Users", null, new { @class = "btn btn-large" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n");


            
            #line 30 "..\..\Areas\V2\Views\AdminTasks\ActivateUser.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>\r\n");


        }
    }
}
#pragma warning restore 1591
