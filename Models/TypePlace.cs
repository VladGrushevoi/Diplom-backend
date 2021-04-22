using System.Collections.Generic;

namespace Models
{
    public class TypePlace : BaseMosel
    {
        public string Name { get; set; }

        public List<ImportantPlace> ImportantPlace { get; set; }
    }
}