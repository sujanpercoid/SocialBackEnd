using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class ProfileNameDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public int Followers { get; set; }
        public int Follows { get; set; }
    }
}
