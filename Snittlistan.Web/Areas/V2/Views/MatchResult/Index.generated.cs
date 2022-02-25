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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/MatchResult/Index.cshtml")]
    public partial class _Areas_V2_Views_MatchResult_Index_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<MatchResultViewModel>
    {
        public _Areas_V2_Views_MatchResult_Index_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
 if (User.IsInRole2(WebsiteRoles.Uk.UkTasks))
{

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"row\">\r\n        <div class=\"span6 admin\">\r\n            <a href=\"");


            
            #line 7 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                Write(Url.Action("Register", "MatchResultAdmin"));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                <i class=\"sprite-glyphicons_030_pencil\"></i>\r\n               " +
" Manuellt\r\n            </a>\r\n\r\n            <div class=\"clearfix\"></div>\r\n       " +
" </div>\r\n    </div>\r\n");


            
            #line 15 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 17 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
 foreach (var turn in Model.Turns.Keys.OrderByDescending(x => x))
{
    var minDate = Model.Turns[turn].Select(x => x.Date)
        .Min();
    var maxDate = Model.Turns[turn].Select(x => x.Date)
        .Max();
    var matchResults = Model.Turns[turn];

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"row\">\r\n        <h4>\r\n            Omgång ");


            
            #line 26 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
              Write(turn);

            
            #line default
            #line hidden
WriteLiteral("\r\n            <em>(");


            
            #line 27 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
            Write(Html.FormatDateSpan(minDate.Date, maxDate.Date));

            
            #line default
            #line hidden
WriteLiteral(")</em>\r\n        </h4>\r\n");


            
            #line 29 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
         foreach (var result in matchResults.SortResults())
        {

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"span4\">\r\n                <div class=\"well-small\">\r\n      " +
"              <dl>\r\n                        <dt>\r\n                            La" +
"g\r\n                        </dt>\r\n                        <dt>\r\n                " +
"            ");


            
            #line 38 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                       Write(Content.TeamLabel(result.TeamLevel, result.Team));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </dt>\r\n                        <dt>\r\n                  " +
"          Motståndare\r\n                        </dt>\r\n                        <d" +
"d>\r\n                            ");


            
            #line 44 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                       Write(result.Opponent);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </dd>\r\n                        <dt>\r\n                  " +
"          Detaljer\r\n                        </dt>\r\n                        <dd>\r" +
"\n                            <a href=\"");


            
            #line 50 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                                Write(Url.Action("Details", new
                                     {
                                         id = result.BitsMatchId,
                                         result.RosterId
                                     }));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                                ");


            
            #line 55 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                           Write(result.FormattedResult);

            
            #line default
            #line hidden
WriteLiteral("\r\n                            </a>\r\n                        </dd>\r\n              " +
"      </dl>\r\n                    <p>\r\n                        <strong>\r\n        " +
"                    ");


            
            #line 61 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                       Write(result.Location);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </strong>\r\n                        <div>");


            
            #line 63 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                        Write(result.MatchCommentaryHtml);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    </p>\r\n");


            
            #line 65 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                     foreach (var part in result.BodyText)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <p>\r\n                            ");


            
            #line 68 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                       Write(part);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </p>\r\n");


            
            #line 70 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n            </div>\r\n");


            
            #line 73 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");


            
            #line 75 "..\..\Areas\V2\Views\MatchResult\Index.cshtml"
}

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591