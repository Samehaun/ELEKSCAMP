using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ELEKSUNI;

namespace WebUI.Pages
{
    public class QuestModel : PageModel
    {
        Quest quest;
        public void OnGet(string name)
        {
            quest = new Quest(name);
        }
    }
}
