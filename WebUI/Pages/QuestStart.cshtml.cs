using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ELEKSUNI;


namespace WebUI.Pages
{
    public class QuestStartModel : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "¬ведите им€";
        }
        public void OnPost(string name)
        {
           
        }
    }
}
