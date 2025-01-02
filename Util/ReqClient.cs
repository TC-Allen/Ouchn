
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Ouchn.Util
{
    public class ReqClient
    {
        public HttpClient client { get; set; }
        public int courseid;
        public string cookie;
        public string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";

        public ReqClient(HttpClient client, int courseid, string cookie)
        {
            this.client = client;
            this.courseid = courseid;
            this.cookie = cookie;

            client.DefaultRequestHeaders.Add("Cookie", cookie);
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
            client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
        }

        public async Task<string> get(string url)
        {
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }

#pragma warning disable CS8603 // 可能返回 null 引用。
        public async Task<JObject> getMudoles()
        {
            var content = await get($"https://lms.ouchn.cn/api/courses/{courseid}/modules");
            //func(content);
            return JObject.Parse(content);
        }

        public async Task<JArray> getActityByModules(List<int> moduleIds)
        {
            var content = await get($"https://lms.ouchn.cn/api/course/{courseid}/all-activities?module_ids=[{string.Join(",", moduleIds)}]&activity_types=learning_activities,exams,classrooms,live_records,rollcalls&no-loading-animation=true");
            return (JArray) JObject.Parse(content)["learning_activities"];
        }

        public async Task<JObject> getCompletenes()
        {
            var content = await get($"https://lms.ouchn.cn/api/course/{courseid}/my-completeness");

            return JObject.Parse(content);
        }


#pragma warning restore CS8603 // 可能返回 null 引用。
        public async Task<string> post(string url, Object data)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(serializedData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                return responseData;
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"POST请求失败，状态码：{response.StatusCode}，错误信息：{errorMessage}");
            }
            
        }

        public async Task<JObject> submitActivity(int courseid, Object data)
        {
            string apiUrl = $"https://lms.ouchn.cn/api/course/activities-read/{courseid}";

            try
            {
                return JObject.Parse(await post(apiUrl, data));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return JObject.Parse(JsonConvert.SerializeObject(ex));
            }
        }
    }
}
