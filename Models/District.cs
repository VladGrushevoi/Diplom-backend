using System.Collections.Generic;

namespace Models
{
    public class District : BaseMosel
    {
        public string Name { get; set; }   
         public List<TypePlace> TypePlace { get; set; } 
    }
}