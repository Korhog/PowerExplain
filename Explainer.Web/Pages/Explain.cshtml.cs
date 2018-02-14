using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Explainer.Web.Pages
{
    using Explainer.Web.Model;

    public class ExplainModel : PageModel
    {
        public List<TreeNode> Rows;        

        public void OnGet()
        {
            Rows = new List<TreeNode>();
        }

        public void OnGetTransfer(string key)
        {
            var transfer = DataTransfer.GetInstance();
            var data = transfer.GetData(key);
            Rows = TreeBuilder.Build(data);
        }
    }
}