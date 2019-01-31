using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainForumCommon
{
    public class NoRainPage
    {
        public int PageIndex { get; set; }

        public long DataCount { get; set; }

        public int ShowPageNum { get; set; } = 10;

        public int PageDataNum { get; set; } = 10;

        public string Url { get; set; }
        

        public string GetPaging()
        {
            if (DataCount <= PageDataNum)
            {
                return string.Empty;
            }
            int pageCount = (int)Math.Ceiling(DataCount * 1.0d / PageDataNum);

            int startPageNum = Math.Max(1, (PageIndex - (ShowPageNum / 2)));
            int endPageNum = Math.Min(pageCount, PageIndex + (ShowPageNum / 2));
            StringBuilder sb = new StringBuilder();

            if (PageIndex > 5)
            {
                sb.Append("<a href='").Append(Url.Replace("@parms", (1).ToString())).Append("'>首页")
                    .Append("</a>");

                sb.Append("<a class='layui-laypage-prev' href='").Append(Url.Replace("@parms", (PageIndex - 1).ToString())).Append("'>上页")
                    .Append("</a>");
            }

            for (int i = startPageNum; i <= endPageNum; i++)
            {
                if (i == PageIndex)
                {
                    sb.Append("<a  style='background-color:rgb(255,87,34);'><em class='layui-laypage-em'></em><em>")
                        .Append(i)
                        .Append("</em></a>");
                }
                else
                {
                    sb.Append("<a href='").Append(Url.Replace("@parms", (i).ToString())).Append("'>")
                        .Append(i)
                        .Append("</a>");
                }
            }


            if (PageIndex < pageCount - 4)
            {
                sb.Append("<a class='layui-laypage-next' href='").Append(Url.Replace("@parms", (PageIndex + 1).ToString())).Append("'>下页")
                    .Append("</a>");

                sb.Append("<a class='layui-laypage-last' href='").Append(Url.Replace("@parms", (pageCount).ToString())).Append("'>尾页")
                    .Append("</a>");
            }
            return sb.ToString();
        }
    }
}
