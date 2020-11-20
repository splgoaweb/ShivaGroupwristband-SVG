using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nop.Plugin.NopFeatures.ShipRocket.ShipRocketJsonClass;
using Nop.Plugin.NopFeatures.ShipRocket.ShipRocketJsonClass.noeway;

namespace Nop.Plugin.NopFeatures.ShipRocket
{
    public class ShipRocketApiConfiguration
    {

        #region utilitie

        /// <summary>
        /// HttpRequestMessage method
        /// </summary>
        /// <param name="token"></param>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <param name="Istoken"></param>
        /// <returns></returns>
        public HttpRequestMessage GetHttpRequestMessage(string token,
                 string url, HttpMethod method = null, HttpContent content = null, bool Istoken = false)
        {

            if (method == null)
                method = HttpMethod.Get;

            HttpRequestMessage request = new HttpRequestMessage(method, url);

            if (!Istoken)
                request.Headers.Add("Authorization", "Bearer " + token);

            request.Headers.Add("accept", "application/json");

            if (content != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;
            }

            return request;
        }

        #endregion

        #region ShipRocketMethod
        /// <summary>
        /// method to create ship rocket order
        /// </summary>
        /// <param name="token"></param>
        /// <param name="serviceurl"></param>
        /// <param name="shiprocketorder"></param>
        /// <returns></returns>
        public string createShiprocketOrder(string token, string serviceurl, ShipRocketOrderJson shiprocketorder)
        {
            using (var client = new HttpClient())
            {
                var Shiprocketorderjson = JsonConvert.SerializeObject(shiprocketorder);
                StringContent content = new StringContent(Shiprocketorderjson);
                HttpRequestMessage RequestShiprocketOrder = GetHttpRequestMessage(token, serviceurl + "/v1/external/orders/create/adhoc", HttpMethod.Post, content);
                HttpResponseMessage sResponse = client.SendAsync(RequestShiprocketOrder).Result;
                var result = sResponse.Content.ReadAsStringAsync().Result;

                return result;
            }
        }

        /// <summary>
        /// method to create shiprocket order noeway
        /// </summary>
        /// <param name="token"></param>
        /// <param name="serviceurl"></param>
        /// <param name="shiprocketorder"></param>
        /// <returns></returns>
        public string createShiprocketOrdernoeway(string token, string serviceurl, ShipRocketOrderJsonNoEway shiprocketorder)
        {
            using (var client = new HttpClient())
            {
                var Shiprocketorderjson = JsonConvert.SerializeObject(shiprocketorder);
                StringContent content = new StringContent(Shiprocketorderjson);
                HttpRequestMessage RequestShiprocketOrder = GetHttpRequestMessage(token, serviceurl + "/v1/external/orders/create/adhoc", HttpMethod.Post, content);
                HttpResponseMessage sResponse = client.SendAsync(RequestShiprocketOrder).Result;
                var result = sResponse.Content.ReadAsStringAsync().Result;

                return result;
            }
        }

        /// <summary>
        /// method to auth shiprocket user
        /// </summary>
        /// <param name="serviceurl"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string GetTocket(string serviceurl, string email, string password)
        {
            using (var client = new HttpClient())
            {
                shiprocketlogin sl = new shiprocketlogin
                {
                    email = email,
                    password = password
                };

                var Shiprocketorderjson = JsonConvert.SerializeObject(sl);
                StringContent content = new StringContent(Shiprocketorderjson);
                HttpRequestMessage RequestShiprocketOrder = GetHttpRequestMessage("", serviceurl + "/v1/external/auth/login", HttpMethod.Post, content, true);
                HttpResponseMessage sResponse = client.SendAsync(RequestShiprocketOrder).Result;
                var result = sResponse.Content.ReadAsStringAsync().Result;

                return result;
            }
        }

        /// <summary>
        /// shiprocket login class
        /// </summary>
        public class shiprocketlogin
        {
            public string email { get; set; }

            public string password { get; set; }
        }

        /// <summary>
        /// method to get shiprocket status
        /// </summary>
        /// <param name="token"></param>
        /// <param name="serviceurl"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetShiprocketOrderStatus(string token, string serviceurl, int id)
        {
            using (var client = new HttpClient())
            {
                var status = string.Empty;
                HttpRequestMessage RequestShiprocketOrder = GetHttpRequestMessage(token, serviceurl + "/v1/external/orders/show/" + id, HttpMethod.Get);
                HttpResponseMessage sResponse = client.SendAsync(RequestShiprocketOrder).Result;
                if (sResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = sResponse.Content.ReadAsStringAsync().Result;
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    status = data["data"]["status"].ToString();
                }
                return status;
            }
        }

        #endregion


    }
}
