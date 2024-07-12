using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PersonSpecialityMap
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int SpecialityId { get; set; }
    }
}
