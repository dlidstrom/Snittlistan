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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/MatchResult/DisplayTemplates/Header.cshtml")]
    public partial class _Areas_V2_Views_MatchResult_DisplayTemplates_Header_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<ResultHeaderViewModel>
    {
        public _Areas_V2_Views_MatchResult_DisplayTemplates_Header_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n<div class=\"well well-small\">\r\n    <dl class=\"dl-horizontal\">\r\n        <dt>lag<" +
"/dt>\r\n        <dd>");


            
            #line 6 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
       Write(Content.TeamLabel(Model.TeamLevel, Model.Team));

            
            #line default
            #line hidden
WriteLiteral("</dd>\r\n        <dt>plats</dt>\r\n        <dd>");


            
            #line 8 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
       Write(Model.Location);

            
            #line default
            #line hidden
WriteLiteral("</dd>\r\n        <dt>motstånd</dt>\r\n        <dd>");


            
            #line 10 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
       Write(Model.Opponent);

            
            #line default
            #line hidden
WriteLiteral("</dd>\r\n        <dt>när</dt>\r\n        <dd>\r\n            <em>\r\n                <tim" +
"e datetime=\"");


            
            #line 14 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
                           Write(Model.Date.ToString("s"));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                    ");


            
            #line 15 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
               Write(Model.Date.ToString("dddd d MMMM HH:mm"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </time>\r\n            </em>\r\n        </dd>\r\n        <dt>resultat" +
"</dt>\r\n        <dd>");


            
            #line 20 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
       Write(Model.FormattedResult);

            
            #line default
            #line hidden
WriteLiteral("</dd>\r\n");


            
            #line 21 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
         if (Model.BitsMatchId != 0)
        {

            
            #line default
            #line hidden
WriteLiteral("            <dt>BITS</dt>\r\n");



WriteLiteral("            <dd>\r\n                <a href=\"");


            
            #line 25 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
                    Write(Html.GenerateBitsUrl(Model.BitsMatchId));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                    Matchfakta\r\n                    <i class=\"icon-share-alt\"" +
"></i>\r\n                </a>\r\n            </dd>\r\n");


            
            #line 30 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Header.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </dl>\r\n</div>\r\n");


        }
    }
}
#pragma warning restore 1591