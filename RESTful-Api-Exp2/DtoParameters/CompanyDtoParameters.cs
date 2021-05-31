using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.DtoParameters
{
    public class CompanyDtoParameters
    {
        private const int MaxPageSize = 20;
        //public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string SearchTerm { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="The Page Number must greater than 1")]
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 5;

        [Range(1, 50, ErrorMessage = "The Page Size must greater than 1")]
        //拆分pagesize属性
        public int PageSize
        {
            //get { return _pageSize; }
            //set { _pageSize = value; }
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize ? MaxPageSize : value);
        }

        public string OrderBy { get; set; } = "Name";
        public string Fields { get; set; }
    }
}
