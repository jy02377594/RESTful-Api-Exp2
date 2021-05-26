using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.DtoParameters
{
    public class TaskDtoParameters
    {
        public const int MaxPageSize = 20;
        public string SearchTerm { get; set; }
        public DateTime Deadline { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The Page Number must greater than 1")]
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 5;
        [Range(1, 50, ErrorMessage = "The Page Size Range from 1 to 50")]
        public int PageSize {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize ? MaxPageSize : value);
        }

        public string OrderBy { get; set; } = "TaskName";

    }
}
