using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers.Dto
{
    public class QueryPageableParams
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }
        public override string ToString()
        {
            return $"{maxPageSize}, {PageNumber}, {PageSize}";
        }
    }
}