using AutoMapper;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Profiles
{
    public class CompanyProfile : Profile
    {
        //属性和类型一样会直接映射给目标
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>();
        }
    }
}
