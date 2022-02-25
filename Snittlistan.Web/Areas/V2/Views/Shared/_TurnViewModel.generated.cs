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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/Shared/_TurnViewModel.cshtml")]
    public partial class _Areas_V2_Views_Shared__TurnViewModel_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<Snittlistan.Web.Areas.V2.Controllers.RosterController.InitialDataViewModel.TurnViewModel>
    {
        public _Areas_V2_Views_Shared__TurnViewModel_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"span6\">\r\n        <h4 class=\"pull-left\">\r\n   " +
"         Omgång ");


            
            #line 6 "..\..\Areas\V2\Views\Shared\_TurnViewModel.cshtml"
              Write(Model.Turn);

            
            #line default
            #line hidden
WriteLiteral("\r\n            <em>(");


            
            #line 7 "..\..\Areas\V2\Views\Shared\_TurnViewModel.cshtml"
            Write(Html.FormatDateSpan(Model.StartDate, Model.EndDate));

            
            #line default
            #line hidden
WriteLiteral(")</em>\r\n        </h4>\r\n        <a class=\"pull-right rosters\" href=\"");


            
            #line 9 "..\..\Areas\V2\Views\Shared\_TurnViewModel.cshtml"
                                       Write(Url.Action("View", "Roster", new { season = Model.SeasonStart, turn = Model.Turn }));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n            <i class=\"icon-user\"></i>\r\n            Uttagningar\r\n        </a>\r" +
"\n    </div>\r\n</div>\r\n");


            
            #line 15 "..\..\Areas\V2\Views\Shared\_TurnViewModel.cshtml"
  
    var start = 0;
    while (true)
    {
        var rosters = Model.Rosters.Skip(start).Take(3).ToArray();
        if (rosters.Length == 0)
        {
            break;
        }

            
            #line default
            #line hidden
WriteLiteral("        <div class=\"row\">\r\n");


            
            #line 25 "..\..\Areas\V2\Views\Shared\_TurnViewModel.cshtml"
             foreach (var roster in rosters)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"span4\">\r\n                    ");


            
            #line 28 "..\..\Areas\V2\Views\Shared\_TurnViewModel.cshtml"
               Write(Html.DisplayFor(model => roster));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");


            
            #line 30 "..\..\Areas\V2\Views\Shared\_TurnViewModel.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n");


            
            #line 32 "..\..\Areas\V2\Views\Shared\_TurnViewModel.cshtml"
        start += 3;
    }


            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591