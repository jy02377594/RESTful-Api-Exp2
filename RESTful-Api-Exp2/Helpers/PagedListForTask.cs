using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Helpers
{
    //泛型类,本身是一个集合，在集合的基础上建立翻页信息，所以要继承list类
    public class PagedListForTask<T>: List<T>
    {
        //为了避免在类外设置，set设成private
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        //前后页可以根据已有的属性算出来,expression body
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        //构造函数,new PagedListForTask给初始值
        public PagedListForTask(List<T> items, int count, int pageNumber, int pageSize)
        {
            //总的数据个数
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            //相除后可能有小数，比如5/2=2.5,就应该是第三页，所以要用到Math.Ceiling
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        //静态方法,其它类调用这个方法时不需要实例化整个类
        public static async Task<PagedListForTask<T>> CreateAsnyc(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedListForTask<T>(items, count, pageNumber, pageSize);
        }
    }
}
