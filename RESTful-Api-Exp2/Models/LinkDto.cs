namespace RESTful_Api_Exp2.Models
{
    public class LinkDto
    {
        //在下面的构造函数里set,所以这三个属性可以设为私有或者去掉
        public string Href { get; private set; }
        public string Rel { get;}
        public string Method { get;}

        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
