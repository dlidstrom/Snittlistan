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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/Shared/Error.cshtml")]
    public partial class _Areas_V2_Views_Shared_Error_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<HandleErrorInfo>
    {
        public _Areas_V2_Views_Shared_Error_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n");


            
            #line 3 "..\..\Areas\V2\Views\Shared\Error.cshtml"
  
    ViewBag.Title = "Syntax Error";


            
            #line default
            #line hidden
WriteLiteral(@"
<h2>Oförutsett fel</h2>

<p>Ett oförutsett fel inträffade, vi beklagar det. Felet har loggats
och vi kommer titta på det.</p>

<p>Under tiden kan Du kan prova att gå tillbaka och prova igen, annars
kan du kontakta <a href=""mailto:dlidstrom@gmail.com"">Daniel Lidström</a>.</p>
");


        }
    }
}
#pragma warning restore 1591
