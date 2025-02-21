using Microsoft.AspNetCore.Identity;

namespace Domain.Common.BaseEntities
{
    public class BaseUser : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; } = null;
    }
}
