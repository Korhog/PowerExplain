using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Explainer.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Explainer.Web.Pages
{ 
    public class IndexModel : PageModel
    {
        public string ExplainText { get; set; }

        public IActionResult OnPost(string data)
        {
            var guid = System.Guid.NewGuid().ToString();
            DataTransfer.GetInstance().SetData(guid, data);
            return RedirectToPage("Explain", "Transfer", new { key = guid });
        }
    }
}