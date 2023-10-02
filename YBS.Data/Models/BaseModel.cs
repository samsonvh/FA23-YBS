using YBS.Data.DateTracking;

namespace YBS.Data.Models
{
    public abstract class BaseModel : IDateTracking
    {
        public DateTime CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}