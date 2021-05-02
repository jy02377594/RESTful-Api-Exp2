using AutoMapper;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Profiles
{
    public class EmployeeTaskProfile : Profile
    {
        public EmployeeTaskProfile(){
            CreateMap<EmployeeTask, EmployeeTaskDto>()
                .ForMember(destinationMember: dest => dest.taskDays,
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.StartTime.Day - src.Deadline.Day));

            CreateMap<EmployeeTaskAddDto, EmployeeTask>();
        }
    }
}
