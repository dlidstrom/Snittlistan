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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/Roster/PlayerStatus.cshtml")]
    public partial class _Areas_V2_Views_Roster_PlayerStatus_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<IEnumerable<PlayerStatusViewModel>>
    {
        public _Areas_V2_Views_Roster_PlayerStatus_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral(@"
<div class=""row"">
    <div class=""span12"">
        <p class=""lead"">
            Verifiera att inga uttagna spelare har anmält frånvaro.
        </p>
        <table class=""table table-condensed"">
            <tr>
                <th>Namn</th>
                <th></th>
                <th>Säsong</th>
                <th><span class=""badge badge-success"">5</span>&#x25BC;</th>
                <th>Form</th>
            </tr>
");


            
            #line 16 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
             foreach (var activity in Model)
            {

            
            #line default
            #line hidden
WriteLiteral("                <tr>\r\n                    <td>");


            
            #line 19 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                   Write(activity.Name);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>\r\n");


            
            #line 21 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                         foreach (var absence in activity.Absences)
                        {
                            
            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                       Write(Html.FormatDateSpan(absence.From, absence.To));

            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                                                                          
                            if (string.IsNullOrWhiteSpace(absence.Comment) == false)
                            {
                                var comment = string.Format(" ({0})", absence.Comment);
                                
            
            #line default
            #line hidden
            
            #line 27 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                           Write(comment);

            
            #line default
            #line hidden
            
            #line 27 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                                        
                            }
                        }

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 31 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                         foreach (var team in activity.Teams.SortRosters())
                        {
                            
            
            #line default
            #line hidden
            
            #line 33 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                       Write(Content.TeamLabel(team.Header.TeamLevel, team.Header.Team));

            
            #line default
            #line hidden
            
            #line 33 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                                                                                       
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </td>\r\n                    <td>\r\n                        ");


            
            #line 37 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                   Write(string.Format("{0} ({1})", activity.PlayerForm.FormattedSeasonAverage(), activity.PlayerForm.TotalSeries));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");


            
            #line 40 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                   Write(activity.PlayerForm.FormattedLast5Average());

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td class=\"");


            
            #line 42 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                          Write(activity.PlayerForm.Class());

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                        ");


            
            #line 43 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
                   Write(activity.PlayerForm.FormattedDiff());

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");


            
            #line 46 "..\..\Areas\V2\Views\Roster\PlayerStatus.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </table>\r\n    </div>\r\n</div>");


        }
    }
}
#pragma warning restore 1591
