#pragma checksum "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f57f7da9ee6f3a40440eb206ad82b0ea8729f9c5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\_ViewImports.cshtml"
using DisplayKustoDiffToUsers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml"
using DisplayKustoDiffToUsers.Controllers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml"
using DisplayKustoDiffToUsers.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f57f7da9ee6f3a40440eb206ad82b0ea8729f9c5", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7592ad54352a16fbe75fd7121a0ff7c5e0ce43af", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<KustoCommandModel>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"

<div class=""box-shadow"">
        <h3>Test Cluster:  </h3>
        <table class=""table"">
            <thead>
                <tr>
                    <th scope=""col"">Cluster</th>
                    <th scope=""col"">Object Name</th>
                    <th scope=""col"">Previous Public Version</th>
                    <th scope=""col"">New Public Version</th>
                    <th scope=""col"">Current AGC Version</th>
                    <th scope=""col""> </th>

                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 21 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml"
                  foreach (var i in Model)
                {                   

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n                            <td>Some Cluster Name</td>\r\n                            <td>");
#nullable restore
#line 25 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml"
                           Write(i.KustoObjectName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 26 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml"
                           Write(i.PreviousPublicSchema);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 27 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml"
                           Write(i.UpdatedPublicSchema);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 28 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml"
                           Write(i.CurrentAGCSchema);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>Send Command</td>\r\n                        </tr>\r\n");
#nullable restore
#line 31 "C:\Users\sheacooke\Desktop\AppService\DisplayKustoDiffToUsers\DisplayKustoDiffToUsers\Views\Home\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n\r\n        </table>\r\n    </div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<KustoCommandModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
