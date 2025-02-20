using Domain.Common.BaseEntities;

namespace Domain.Entities
{
    public class User : BaseUser
    {
        public string Address { get; set; }
        public bool Active { get; set; } = true;
    }
}
