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
    
    #line 1 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
    using System.Globalization;
    
    #line default
    #line hidden
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
    
    #line 2 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
    using Snittlistan.Web.Areas.V2.ReadModels;
    
    #line default
    #line hidden
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.HtmlHelpers;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;
    using Snittlistan.Web.ViewModels;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/MatchResult/DisplayTemplates/Result4.cshtml")]
    public partial class _Areas_V2_Views_MatchResult_DisplayTemplates_Result4_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<Snittlistan.Web.Areas.V2.ReadModels.ResultSeries4ReadModel>
    {
        public _Areas_V2_Views_MatchResult_DisplayTemplates_Result4_cshtml()
        {
        }
        public override void Execute()
        {



WriteLiteral("\r\n");


            
            #line 5 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
  
    ViewBag.Title = "Result";


            
            #line default
            #line hidden
WriteLiteral(@"
<h3>Resultat</h3>
<table class=""table table-condensed table-bordered table-striped"">
    <tr>
        <td>#</td>
        <td>Namn</td>
        <td>1</td>
        <td>2</td>
        <td>3</td>
        <td>4</td>
        <td>Tot</td>
        <td>Banp</td>
    </tr>
");


            
            #line 21 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
      
        var row = 1;
    

            
            #line default
            #line hidden

            
            #line 24 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
     foreach (KeyValuePair<string, List<ResultSeries4ReadModel.PlayerGame>> player in Model.SortedPlayers())
    {

            
            #line default
            #line hidden
WriteLiteral("        <tr>\r\n            <td>");


            
            #line 27 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
            Write(row++);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n            <td>");


            
            #line 28 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
           Write(player.Key);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");


            
            #line 29 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
             for (var serie = 0; serie < 4; serie++)
            {

            
            #line default
            #line hidden
WriteLiteral("                <td>\r\n");


            
            #line 32 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
                      
                        ResultSeries4ReadModel.PlayerGame game = player.Value[serie];
                    

            
            #line default
            #line hidden
WriteLiteral("                    ");


            
            #line 35 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
                Write(game != null ? game.Pins.ToString(CultureInfo.InvariantCulture) : "");

            
            #line default
            #line hidden
WriteLiteral("\r\n                </td>\r\n");


            
            #line 37 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
            }

            
            #line default
            #line hidden

            
            #line 38 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
              
                List<ResultSeries4ReadModel.PlayerGame> series = player.Value.Where(x => x != null).ToList();
            

            
            #line default
            #line hidden
WriteLiteral("            <td>\r\n                ");


            
            #line 42 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
           Write(series.Sum(x => x.Pins));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");


            
            #line 45 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
           Write(series.Sum(x => x.Score));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");


            
            #line 48 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("    <tr>\r\n        <td colspan=\"2\"></td>\r\n        <td>");


            
            #line 51 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
       Write(Model.SerieSum(0));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        <td>");


            
            #line 52 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
       Write(Model.SerieSum(1));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        <td>");


            
            #line 53 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
       Write(Model.SerieSum(2));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        <td>");


            
            #line 54 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
       Write(Model.SerieSum(3));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        <td>");


            
            #line 55 "..\..\Areas\V2\Views\MatchResult\DisplayTemplates\Result4.cshtml"
       Write(Model.Total());

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n        <td colspan=\"3\"></td>\r\n    </tr>\r\n</table>\r\n");


        }
    }
}
#pragma warning restore 1591
