using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.ViewModel
{
    public class Cm_Menu_ViewModel
    {
        public string Path { set; get; }
        public string Name { set; get; }
        public int ParentId { set; get; }
        public int Id { set; get; }
        public string Icon { set; get; }
        public List<Cm_Menu_ViewModel> Childrens { set; get; }

    }
}
