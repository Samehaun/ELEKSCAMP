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
        public Quest quest;
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }
        public string Message { get; set; }
        public List<string> options;
        public void OnGet(string Name)
        {
            quest = new Quest(Name);
            Message = "�� ������ � ���� � ���������� �����. ���������� ��� �� ����� ���������, �� �� ������� ���� �� ���� � �������... ����";
            options = quest.GetPossibleOptions();
        }
    }
}
