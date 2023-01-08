using System.Collections.Generic;

namespace Hospital.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Docter> Docters { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
