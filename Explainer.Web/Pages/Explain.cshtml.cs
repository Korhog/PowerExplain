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

        public void OnGet(object data)
        {
            var transfer = DataTransfer.GetInstance();
            Rows = TreeBuilder.Build(transfer.GetData());
        }
    }
}