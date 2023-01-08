using System.Collections.Generic;

namespace Hospital.Models.ViewModels
{
    public class  DetailsVM
    {
        public DetailsVM()
        {
            Docter = new Docter();
        }

        public Docter Docter { get; set; }
       
      
    }
}
