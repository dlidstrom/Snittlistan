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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/AdminTasks/Adopt.cshtml")]
    public partial class _Areas_V2_Views_AdminTasks_Adopt_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<dynamic>
    {
        public _Areas_V2_Views_AdminTasks_Adopt_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div class=\"row\">\r\n    <div class=\"span12\">\r\n");


            
            #line 3 "..\..\Areas\V2\Views\AdminTasks\Adopt.cshtml"
         using (Html.BeginForm(
            "Adopt",
            "AdminTasks",
            FormMethod.Post,
            new
            {
                @class = "form-horizontal"
            }))
        {

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"control-group\">\r\n                <label for=\"playerId\" cl" +
"ass=\"control-label\"></label>\r\n                <div class=\"controls\">\r\n          " +
"          ");


            
            #line 15 "..\..\Areas\V2\Views\AdminTasks\Adopt.cshtml"
               Write(Html.DropDownList("playerId", new SelectList(ViewBag.PlayerId, "Value", "Text")));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    <span class=\"help-block\">\r\n                        Välj med" +
"lem att adoptera.\r\n                    </span>\r\n                </div>\r\n        " +
"    </div>\r\n");



WriteLiteral("            <div class=\"control-group\">\r\n                <div class=\"controls\">\r\n" +
"                    <button class=\"btn btn-primary btn-large\" type=\"submit\">Spar" +
"a</button>\r\n                    <a class=\"btn btn-large\"\r\n                      " +
" href=\"");


            
            #line 25 "..\..\Areas\V2\Views\AdminTasks\Adopt.cshtml"
                        Write(Url.Action("Index", "AdminTasks"));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                        Avbryt\r\n                    </a>\r\n                </d" +
"iv>\r\n            </div>\r\n");


            
            #line 30 "..\..\Areas\V2\Views\AdminTasks\Adopt.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>");


        }
    }
}
#pragma warning restore 1591
