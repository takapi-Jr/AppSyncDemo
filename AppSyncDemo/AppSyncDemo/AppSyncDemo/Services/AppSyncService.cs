using AppSyncDemo.Models;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppSyncDemo.Services
{
    public class AppSyncService
    {
        public static AppSyncService Instance { get; private set; } = new AppSyncService();
        private GraphQLHttpClient GraphQLHttpClient { get; set; }

        public AppSyncService()
        {
            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(ApiKey.AppSyncApiUrl),
            };
            this.GraphQLHttpClient = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer());
            this.GraphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("x-api-key", ApiKey.AppSyncApiKey);
        }

        public async Task<SampleModel> GetSampleAsync(string name)
        {
            var apiName = "GetSample";
            var variables = new 
            {
                name = name,
            };
            var response = await ExecQueryAsync<SampleModel>(apiName, variables);
            return response;
        }

        /// <summary>
        /// Queryを実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public async Task<T> ExecQueryAsync<T>(string apiName, object variables)
        {
            try
            {
                // GraphQL取得
                var resourceId = $"AppSyncDemo.GraphQLs.{apiName}.gql";
                var query = await GetQueryAsync(resourceId);

                // リクエスト作成
                var request = new GraphQLRequest
                {
                    Query = query,
                    OperationName = "MyQuery",
                    Variables = variables,
                };

                // Query実行
                var response = await this.GraphQLHttpClient.SendQueryAsync<JObject>(request);   // Mutation実行の場合はSendMutationAsync()を使用
                var json = response.Data[apiName]["data"].ToString();                           // AppSyncに"data"を挟んでいない場合は不要
                var ret = JsonConvert.DeserializeObject<T>(json);

                return ret;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// GraphQLを取得
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        private async Task<string> GetQueryAsync(string resourceId)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //var assembly = typeof(AppSyncService).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream(resourceId))
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
