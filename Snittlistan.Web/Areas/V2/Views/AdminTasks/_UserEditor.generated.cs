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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/V2/Views/AdminTasks/_UserEditor.cshtml")]
    public partial class _Areas_V2_Views_AdminTasks__UserEditor_cshtml : Snittlistan.Web.Infrastructure.BaseViewPage<CreateUserViewModel>
    {
        public _Areas_V2_Views_AdminTasks__UserEditor_cshtml()
        {
        }
        public override void Execute()
        {

WriteLiteral(@"
<div class=""control-group"">
    <label class=""control-label"" for=""email"">E-postadress:</label>
    <div class=""controls"">
        <input type=""email""
               name=""email""
               placeholder=""""
               autocorrect = ""off""
               autocomplete = ""off""
               value=""");


            
            #line 11 "..\..\Areas\V2\Views\AdminTasks\_UserEditor.cshtml"
                 Write(Model.Email);

            
            #line default
            #line hidden
WriteLiteral(@"""
               required />
    </div>
</div>
<div class=""control-group"">
    <label class=""control-label"" for=""confirmEmail"">Bekräfta e-postadress:</label>
    <div class=""controls"">
        <input type=""email""
               name=""confirmEmail""
               placeholder=""""
               autocorrect = ""off""
               autocomplete = ""off""
               value=""");


            
            #line 23 "..\..\Areas\V2\Views\AdminTasks\_UserEditor.cshtml"
                 Write(Model.ConfirmEmail);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n               required />\r\n    </div>\r\n</div>\r\n");


        }
    }
}
#pragma warning restore 1591
