using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Hospital.Models.ViewModels
{
    public class DoctorVM
    {
        public Docter Docter { get; set; }
       
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
