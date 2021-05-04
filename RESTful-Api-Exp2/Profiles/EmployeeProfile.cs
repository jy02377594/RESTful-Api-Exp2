using AutoMapper;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Profiles
{
    public class EmployeeProfile : Profile
    {
        //public EmployeeProfile()
        //{
        //    //属性和类型一样会直接映射给目标
        //    CreateMap<Employee, EmployeeDto>();
        //}

        //如果属性或类型和目标类不一样，可以手动映射
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(destinationMember: dest => dest.EmployeeName,
                //这样直接拼接字符串也行，但是不规范
                //memberOptions: opt => opt.MapFrom(mapExpression: src => src.FirstName + " " + src.LastName));
                memberOptions: opt => opt.MapFrom(mapExpression: src => $"{src.FirstName} {src.LastName}"))
                .ForMember(destinationMember: dest => dest.GenderDisplay,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Gender.ToString()))
                .ForMember(destinationMember: dest => dest.WorkAge,
                memberOptions: opt => opt.MapFrom(mapExpression: src => DateTime.Now.Year - src.HiredDate.Year)
                );

            CreateMap<EmployeeAddDto, Employee>();
            CreateMap<EmployeePutDto, Employee>();
        }
    }
}
