using Domain.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Media : BaseAuditableEntity
    {
        public string Url { get; set; } = string.Empty;
    }
}
