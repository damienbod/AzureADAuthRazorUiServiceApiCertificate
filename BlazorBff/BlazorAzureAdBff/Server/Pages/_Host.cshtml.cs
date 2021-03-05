using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorAzureADWithApis.Server.Pages
{
    public class _HostModel : PageModel
    {
        public string SiteName { get; set; } = "damienbod";
        public string PageDescription { get; set; } = "damienbod init description";
        public void OnGet()
        {
            (SiteName, PageDescription) = GetMetaData();
        }

        private (string, string) GetMetaData()
        {
            var metadata = Request.Path.Value switch
            {
                "/directapi" => ("damienbod/directapi", "This is the meta data for the directapi"),
                "/graphapi" => ("damienbod/graphapicall", "This is the meta data for the graphapicall"),
                "/home" => ("damienbod/home", "This is the meta data for the home"),
                _ => ("damienbod", "general description")
            };
            return metadata;
        }
    }
}
