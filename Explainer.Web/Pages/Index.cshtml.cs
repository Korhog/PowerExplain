using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Explainer.Web.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public IActionResult OnPost(string data)
        {
            var transfer = Explainer.Web.Model.DataTransfer.GetInstance();
            transfer.SetData(data);
            return RedirectToPage("Explain");
        }
    }
}