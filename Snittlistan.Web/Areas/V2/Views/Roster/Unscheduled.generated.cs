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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/Roster/Unscheduled.cshtml")]
    public partial class _Areas_V2_Views_Roster_Unscheduled_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<Snittlistan.Web.Areas.V2.Controllers.RosterController.InitialDataViewModel>
    {
        public _Areas_V2_Views_Roster_Unscheduled_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");



WriteLiteral("\r\n");


            
            #line 4 "..\..\Areas\V2\Views\Roster\Unscheduled.cshtml"
Write(Html.DisplayFor(model => model, "IndexHeader"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div class=\"row\">\r\n    <div class=\"span12\">\r\n        <h4>Inga matcher tills v" +
"idare</h4>\r\n        <p>Under tiden kan du titta på ");


            
            #line 9 "..\..\Areas\V2\Views\Roster\Unscheduled.cshtml"
                                  Write(Html.ActionLink("tidigare resultat", "Index", "MatchResult"));

            
            #line default
            #line hidden
WriteLiteral(".</p>\r\n    </div>\r\n</div>\r\n\r\n");


            
            #line 13 "..\..\Areas\V2\Views\Roster\Unscheduled.cshtml"
Write(Html.Partial("_Register"));

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
