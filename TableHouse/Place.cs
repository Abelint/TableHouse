using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableHouse
{
    public class Place
    {
        public int Id { get; set; } = -1;
        public int Stage { get; set; } = 0;
        public int Side { get; set; } = 0;     

        public string LogoPath { get; set; } = string.Empty;
    }
}
