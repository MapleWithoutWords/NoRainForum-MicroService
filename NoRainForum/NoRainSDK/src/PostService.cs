using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoRainSDK.http;
using NoRainSDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoRainSDK.src
{
    public class PostService : CommonAbstract<ListModel<ListPostDTO>>
    {
        public PostService(string appKey, string apSecret) : base(appKey, apSecret, "http://127.0.0.1:8888/PostService/api/Post/")
        {
        }
        /// <summary>
        /// 获取置顶的帖子
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListPostDTO>> GetStickAsync()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            SDKResult result = await client.GetAsync("GetStick", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<ListPostDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new List<ListPostDTO>();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
    }
}
