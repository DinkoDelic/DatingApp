namespace DatingApp.API.Helpers
{
    public class MessageParams
    {
        public int PageNumber { get; set; } = 1;
        
        // Maxium size of one page that we allow out user to set
        private const int MaxPageSize = 50;
        private int pageSize = 6;
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
        public int UserId { get; set; }

        public string MessageContainer { get; set; } = "Unread";
    }
}