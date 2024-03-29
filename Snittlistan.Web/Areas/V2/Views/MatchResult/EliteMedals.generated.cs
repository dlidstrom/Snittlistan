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
    
    #line 1 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
    using Snittlistan.Web.Areas.V2.Domain;
    
    #line default
    #line hidden
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.HtmlHelpers;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;
    using Snittlistan.Web.ViewModels;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/MatchResult/EliteMedals.cshtml")]
    public partial class _Areas_V2_Views_MatchResult_EliteMedals_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<EliteMedalsViewModel>
    {
        public _Areas_V2_Views_MatchResult_EliteMedals_cshtml()
        {
        }
        public override void Execute()
        {


WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"span12\">\r\n        <p class=\"lead\">Här visas " +
"vilka elitmärken som delats ut och vilka som erövrats under säsongen ");


            
            #line 6 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                                                                                                  Write(Model.FormatSeason());

            
            #line default
            #line hidden
WriteLiteral(".</p>\r\n    </div>\r\n</div>\r\n\r\n<div class=\"row\">\r\n    <div class=\"span12\">\r\n       " +
" <h3>Elitmärke och Elitplakett (regelverk enligt Blå Boken)</h3>\r\n        <ol>\r" +
"\n            <li>\r\n                Elitmärket kan erövras vid officiell tävli" +
"ng sanktionerad av SvBF eller SDF.\r\n            </li>\r\n            <li>\r\n       " +
"         Elitmärket finns i tre valörer: Brons, Silver och Guld.\r\n            " +
"</li>\r\n            <li>\r\n                Endast ett Elitmärke kan erövras per " +
"spelår. Märke av högre valör kan inte erövras förrän märke av närmast\r\n" +
"                lägre valör erhållits.\r\n            </li>\r\n            <li>\r\n" +
"                Följande fordringar skall uppfyllas - alternativt:\r\n           " +
"     <table class=\"table\">\r\n                    <caption>\r\n                     " +
"   Medelpoäng per serie vid tävling över\r\n                    </caption>\r\n   " +
"                 <tr>\r\n                        <th>\r\n                        </t" +
"h>\r\n                        <th>\r\n                            3 ser\r\n           " +
"             </th>\r\n                        <th>\r\n                            4 " +
"ser\r\n                        </th>\r\n                        <th>\r\n              " +
"              6-8ser\r\n                        </th>\r\n                        <th" +
">\r\n                            fler än 8 ser\r\n                        </th>\r\n  " +
"                  </tr>\r\n                    <tr>\r\n                        <td>\r" +
"\n                            För Brons\r\n                        </td>\r\n        " +
"                <td>\r\n                            200\r\n                        <" +
"/td>\r\n                        <td>\r\n                            190\r\n           " +
"             </td>\r\n                        <td>\r\n                            18" +
"5\r\n                        </td>\r\n                        <td>\r\n                " +
"            180\r\n                        </td>\r\n                    </tr>\r\n     " +
"               <tr>\r\n                        <td>\r\n                            F" +
"ör Silver\r\n                        </td>\r\n                        <td>\r\n       " +
"                     210\r\n                        </td>\r\n                       " +
" <td>\r\n                            200\r\n                        </td>\r\n         " +
"               <td>\r\n                            195\r\n                        </" +
"td>\r\n                        <td>\r\n                            190\r\n            " +
"            </td>\r\n                    </tr>\r\n                    <tr>\r\n        " +
"                <td>\r\n                            För Guld\r\n                   " +
"     </td>\r\n                        <td>\r\n                            220\r\n     " +
"                   </td>\r\n                        <td>\r\n                        " +
"    210\r\n                        </td>\r\n                        <td>\r\n          " +
"                  205\r\n                        </td>\r\n                        <t" +
"d>\r\n                            200\r\n                        </td>\r\n            " +
"        </tr>\r\n                </table>\r\n            </li>\r\n            <li>\r\n  " +
"              För varje märke fordras att ha uppnått de\r\n                samm" +
"a spelår och räknas endast resultat uppnådda vid hel tävling.\r\n             " +
"   <p>\r\n                    <i>\r\n                        <strong>\r\n             " +
"               Kommentar:\r\n                        </strong>\r\n                  " +
"      I de fall tävlingen är uppdelad i s.k. \"Block\", kan resultat räknas i r" +
"espektive \"Block\" med omstart.\r\n                        (se Kap G, § 3:3)\r\n     " +
"               </i>\r\n                </p>\r\n            </li>\r\n            <li>\r\n" +
"                Elitplakett <br />\r\n                Spelare som under ytterligar" +
"e fyra år uppfyllt fordringarna för guldmärket, är berättigad erhålla\r\n   " +
"             elitplaketten i Guld och Emalj.\r\n                <p>\r\n             " +
"       <i>\r\n                        <strong>\r\n                            Kommen" +
"tar:\r\n                        </strong>\r\n                        Elitmärken kan" +
" även erövras på hallar med endast två banor, varvid högst två serier får" +
" spelas på\r\n                        samma bana - dock inte i följd.\r\n         " +
"           </i>\r\n                </p>\r\n            </li>\r\n        </ol>\r\n    </d" +
"iv>\r\n</div>\r\n\r\n<div class=\"row\">\r\n    <div class=\"span12\">\r\n        <div class=\"" +
"alert\">\r\n            <strong>\r\n                Snittlistan kan bara identifiera " +
"4 serier enligt ovan. Om du vet att du har gjort ett godkänt resultat\r\n         " +
"       med ett annat tillvägagångssätt, kontakta UK.\r\n            </strong>\r\n   " +
"     </div>\r\n    </div>\r\n</div>\r\n\r\n");


            
            #line 141 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
 if (User.IsInRole2(WebsiteRoles.EliteMedals.EditMedals))
{

            
            #line default
            #line hidden
WriteLiteral("    <div class=\"row\">\r\n        <div class=\"span12\">\r\n");


            
            #line 145 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
             using (Html.BeginForm("GeneratePdf", "EliteMedalsPrint", FormMethod.Post, new
            {
                @class = "well well-small"
            }))
            {
                ViewData["message"] = "Det gick inte att skapa ansökningarna.";
                
            
            #line default
            #line hidden
            
            #line 151 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
           Write(Html.DisplayFor(model => Html.ViewData.ModelState, "ValidationSummary"));

            
            #line default
            #line hidden
            
            #line 151 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                                                                                        

            
            #line default
            #line hidden
WriteLiteral(@"                <p>
                    Ansökan ska skickas till <a href=""mailto:kansliet@stbf.se"">Stockholms Bowlingförbund</a>.
                    Tänk på att det tar 4-5 veckor innan medaljerna är klara.
                    Ett tips är att ansöka direkt när sista matchen för säsongen är spelad!
                </p>
");



WriteLiteral(@"                <div class=""control-group"">
                    <label class=""control-label"" for=""location"">
                        Ange Din ort:
                        <input type=""text"" name=""location"" id=""location"" maxlength=""40"" required />
                    </label>
                </div>
");



WriteLiteral("                <button class=\"btn btn-secondary btn-small\" type=\"submit\">\r\n     " +
"               <i class=\"sprite-glyphicons_200_download\"></i>\r\n                 " +
"   Skapa ansökningar\r\n                </button>\r\n");


            
            #line 167 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n");


            
            #line 170 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"span12\">\r\n");


            
            #line 174 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
          
            var first = true;
            var orderedPlayers =
                Model.Players
                     .OrderBy(x => x.ExistingMedal)
                     .ThenBy(x =>
                         x.ExistingMedal == EliteMedals.EliteMedal.EliteMedalValue.Gold5
                             ? x.FormattedExistingMedal().SeasonSpan
                             : x.Name);
        

            
            #line default
            #line hidden

            
            #line 184 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
         foreach (var player in orderedPlayers)
        {

            
            #line default
            #line hidden
WriteLiteral("            <table class=\"table table-condensed table-bordered table-striped\">\r\n");


            
            #line 187 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                  
                    if (first)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <caption>Hittils erövrade elitmärken samt bästa resultat " +
"under säsongen</caption>\r\n");


            
            #line 191 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                        first = false;
                    }
                

            
            #line default
            #line hidden
WriteLiteral("                <tr>\r\n                    <th style=\"width: 25%;\">\r\n             " +
"           ");


            
            #line 196 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                    Write(player.Name + " (" + player.PersonalNumber + ")");

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </th>\r\n                    <th colspan=\"2\" style=\"width: 50" +
"%;\">\r\n");


            
            #line 199 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                          
                            var existingMedal = player.FormattedExistingMedal();
                        

            
            #line default
            #line hidden

            
            #line 202 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                         if (string.IsNullOrEmpty(existingMedal.Description) == false)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            ");

WriteLiteral("Innehar");

WriteLiteral("\r\n");



WriteLiteral("                            <span class=\"label label-inverse\">\r\n                 " +
"               ");


            
            #line 206 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                           Write(player.FormattedExistingMedal().Description);

            
            #line default
            #line hidden
WriteLiteral("\r\n                            </span>\r\n");


            
            #line 208 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        ");


            
            #line 209 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                   Write(player.FormattedExistingMedal().SeasonSpan);

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 210 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                         if (User.IsInRole2(WebsiteRoles.EliteMedals.EditMedals))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <a href=\"");


            
            #line 212 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                                Write(Url.Action("EditMedals", new
                                     {
                                         id = player.PlayerId.Substring(player.PlayerId.LastIndexOf('-') + 1)
                                     }));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                                <i class=\"sprite-glyphicons_151_edit\"></i>\r\n " +
"                           </a>\r\n");


            
            #line 218 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </th>\r\n                    <th colspan=\"2\" style=\"width: 25%;" +
"\">\r\n");


            
            #line 221 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                          
                            var nextMedal = player.FormattedNextMedal();
                        

            
            #line default
            #line hidden

            
            #line 224 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                         if (string.IsNullOrEmpty(nextMedal.Description) == false)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            ");

WriteLiteral("Ska ha");

WriteLiteral("\r\n");



WriteLiteral("                            <span class=\"label label-inverse\">\r\n                 " +
"               ");


            
            #line 228 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                           Write(nextMedal.Description);

            
            #line default
            #line hidden
WriteLiteral("\r\n                            </span>\r\n");


            
            #line 230 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        ");


            
            #line 231 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                   Write(nextMedal.SeasonSpan);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </th>\r\n                </tr>\r\n");


            
            #line 234 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                 if (player.ExistingMedal != EliteMedals.EliteMedal.EliteMedalValue.Gold5 && player.TopThreeResults.Any())
                {

            
            #line default
            #line hidden
WriteLiteral("                    <tr>\r\n                        <th colspan=\"4\" style=\"text-ali" +
"gn: center;\">Säsongsbästa ");


            
            #line 237 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                                                                            Write(Model.FormatSeason());

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n                    </tr>\r\n");



WriteLiteral(@"                    <tr>
                        <th colspan=""2"">
                            Datum
                        </th>
                        <th>
                            Tävling
                        </th>
                        <th>
                            Poäng
                        </th>
                    </tr>
");


            
            #line 250 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                    foreach (var result in player.TopThreeResults.GroupBy(x => new
                    {
                        x.Item1.BitsMatchId,
                        x.Item1.Turn,
                        x.Item1.Date,
                        IsValidResult = x.Item2
                    }).OrderBy(x => x.Key.Turn).ThenBy(x => x.Key.Date))
                    {
                        var style = string.Empty;
                        if (result.Key.IsValidResult)
                        {
                            style = "background-color: #dff0d8;";
                        }


            
            #line default
            #line hidden
WriteLiteral("                        <tr>\r\n                            <td colspan=\"2\">");


            
            #line 265 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                                       Write(result.Key.Date.ToString("yyyy-MM-dd"));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                            <td>\r\n                                <a href=" +
"\"");


            
            #line 267 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                                    Write(Html.GenerateBitsUrl(result.Key.BitsMatchId));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                                    Omgång ");


            
            #line 268 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                                      Write(result.Key.Turn);

            
            #line default
            #line hidden
WriteLiteral("\r\n                                </a>\r\n                            </td>\r\n      " +
"                      <td style=\"");


            
            #line 271 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                                  Write(style);

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                                ");


            
            #line 272 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                           Write(result.Sum(x => x.Item1.Pins));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            </td>\r\n                        </tr>\r\n");


            
            #line 275 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
                    }
                }

            
            #line default
            #line hidden
WriteLiteral("            </table>\r\n");


            
            #line 278 "..\..\Areas\V2\Views\MatchResult\EliteMedals.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>\r\n");


        }
    }
}
#pragma warning restore 1591
