using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Models
{
    //为什么有CompnayDto的情况下还要增加不同的Dto?
    //虽然有的字段是重复的，但是在未来可能需求变更的情况下，创建，查询，更新所需要的Dto是不一样的，所以针对每项功能都创建新的Dto有助于扩展，重构
    public class CompanyAddDto
    {
        public string Name { get; set; }
        public string Introduction { get; set; }
    }
}
