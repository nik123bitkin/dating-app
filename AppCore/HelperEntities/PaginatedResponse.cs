using System.Collections.Generic;
using date_app.Helpers;

namespace AppCore.HelperEntities
{
    public class PaginatedResponse<T>
        where T : class
    {
        public PaginatedResponse()
        {
        }

        public PaginatedResponse(IEnumerable<T> data, Pagination pagination)
        {
            Data = data;
            Pagination = pagination;
        }

        public IEnumerable<T> Data { get; set; }

        public Pagination Pagination { get; set; }
    }
}
