namespace YBS.Util.DateTracking
{
    public interface IDateTracking
    {
        public DateTime CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}