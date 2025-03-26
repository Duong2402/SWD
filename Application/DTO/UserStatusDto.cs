using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UserStatusDto
    {
        public Guid userId {  get; set; }
        public bool ban { get; set; }
    }
}
