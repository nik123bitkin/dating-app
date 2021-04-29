namespace AppCore.HelperEntities
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        private const int MaxPageSizeValue = MaxPageSize;
        private int _pageSize = 3;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSizeValue ? MaxPageSize : value; }
        }

        public int UserId { get; set; }

        public string Gender { get; set; }

        public int MinAge { get; set; } = 18;

        public int MaxAge { get; set; } = 99;

        public string OrderBy { get; set; }

        public bool Likees { get; set; } = false;

        public bool Likers { get; set; } = false;
    }
}
