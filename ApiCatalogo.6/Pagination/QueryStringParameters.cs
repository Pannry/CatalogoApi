namespace ApiCatalogo._6.Pagination
{
    public class QueryStringParameters
    {
        // Nessa paginação, só vai da página 1 até a pag 50
        private const int maxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}