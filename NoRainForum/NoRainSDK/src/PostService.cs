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
        /// <summary>
        /// 获取本周热议的帖子
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListPostDTO>> GetDayPostAsync()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            SDKResult result = await client.GetAsync("GetDayPost", dict);
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

        public async Task<ListModel<ListPostDTO>> GetColligatePostAsync(int pageIndex = 1, int pageDataCount = 10, bool? isKnot = null, bool? isEssence = null)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["pageIndex"] = pageIndex.ToString();
            dict["pageDataCount"] = pageDataCount.ToString();
            if (isKnot!=null)
            {
                dict["isKnot"] = isKnot.ToString();
            }
            if (isEssence!=null)
            {
                dict["isEssence"] = isEssence.ToString();
            }
            SDKResult result = await client.GetAsync("GetPageData", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ListModel<ListPostDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ListModel<ListPostDTO>();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
        public async Task<long> AddNewAsync(AddPostModel model)
        {
            return await AddNewAsync<AddPostModel>(model);
        }
        public async Task<bool> UpdateAsync(UpdatePostModel model)
        {
            return await UpdateAsync<UpdatePostModel>(model);
        }
        public async Task<ListContentPostDTO> GetByIdAsync(long id)
        {
            return await GetByIdAsync<ListContentPostDTO>(id);
        }
        public async Task<ListModel<ListPostDTO>> GetByPostTypeIdAsync(long postTypeId, int pageIndex = 1, int pageDataCount = 10, bool? isKnot = null, bool? isEssence = null)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["pageIndex"] = pageIndex.ToString();
            dict["pageDataCount"] = pageDataCount.ToString();
            dict["postTypeId"] = postTypeId.ToString();
            if (isKnot != null)
            {
                dict["isKnot"] = isKnot.ToString();
            }
            if (isEssence != null)
            {
                dict["isEssence"] = isEssence.ToString();
            }
            SDKResult result = await client.GetAsync("GetByTypeId", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ListModel<ListPostDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ListModel<ListPostDTO>();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }

        public async Task<ListModel<ListPostDTO>> GetAdminWebPageDataAsync(int pageIndex = 1, int pageDataCount = 10)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["pageIndex"] = pageIndex.ToString();
            dict["pageDataCount"] = pageDataCount.ToString();
            
            SDKResult result = await client.GetAsync("GetAdminWebPageData", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ListModel<ListPostDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ListModel<ListPostDTO>();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }


        public async Task<List<ListPostDTO>> GetQuestionPostByUserIdAsync(long userId)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["userId"] = userId.ToString();

            SDKResult result = await client.GetAsync("GetQuestionPostByUserId", dict);
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


        public async Task<ListModel<ListPostDTO>> GetCollectionPostByUserIdAsync(long userId, int pageIndex = 1, int pageDataCount = 10)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["userId"] = userId.ToString();
            dict["pageIndex"] = pageIndex.ToString();
            dict["pageDataCount"] = pageDataCount.ToString();

            SDKResult result = await client.GetAsync("GetCollectionPostByUserId", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ListModel<ListPostDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ListModel<ListPostDTO>();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }

        public async Task<ListModel<ListPostDTO>> GetByUserIdAsync(long userId, int pageIndex = 1, int pageDataCount = 10)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["userId"] = userId.ToString();
            dict["pageIndex"] = pageIndex.ToString();
            dict["pageDataCount"] = pageDataCount.ToString();

            SDKResult result = await client.GetAsync("GetByUserId", dict);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return null;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ListModel<ListPostDTO>>(obj["data"] == null ? "" : obj["data"].ToString());
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ListModel<ListPostDTO>();
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }

        
        public async Task<bool> AdoptPostAsync(AdoptPostModel model)
        {

            SDKResult result = await client.PostAsync("AdoptPost", model);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return false;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }

        /// <summary>
        /// 用户收藏帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UserCollectionPostAsync(UserCollectionModel model)
        {

            SDKResult result = await client.PutAsync("UserCollectionPost", model);
            JObject obj = JsonConvert.DeserializeObject<JObject>(result.Result);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ErrorMsg = obj["errorMsg"].ToString();
                return false;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                throw new ApplicationException("未知的错误");
            }
        }
    }
}
