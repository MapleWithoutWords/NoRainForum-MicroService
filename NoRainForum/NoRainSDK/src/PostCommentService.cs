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
    public class PostCommentService:
                                CommonAbstract<ListModel<ListPostCommentDTO>>
    {
        public PostCommentService(string appKey, string apSecret) : base(appKey, apSecret, "http://127.0.0.1:8888/PostService/api/PostComment/")
        {
        }
        public async Task<ListModel<ListPostCommentDTO>> GetByPostIdAsync(long postId,int pageIndex=1,int pageDataCount=10)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["pageIndex"] = pageIndex.ToString();
            dict["pageDataCount"] = pageDataCount.ToString();
            dict["postId"] = postId.ToString();
           
            SDKResult result = await client.GetAsync("GetByPostId", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ListModel<ListPostCommentDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ListModel<ListPostCommentDTO>();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
        public async Task<List<ListPostCommentDTO>> GetByCommentUserIdAsync(long commentUserId)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["commentUserId"] = commentUserId.ToString();

            SDKResult result = await client.GetAsync("GetByCommentUserId", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<ListPostCommentDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new List<ListPostCommentDTO>();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }

        public async Task<long> AddNewAsync(AddCommentModel model)
        {
            return await AddNewAsync<AddCommentModel>(model);
        }
    }
}
