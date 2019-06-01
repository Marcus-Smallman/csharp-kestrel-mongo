using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Function
{
    public class FunctionHandler
    {
        public async Task<string> Handle(object input)
        {
            // Inserts the input into the mongodb collection as a bson document.
            await this.GetCollection()
                      .InsertOneAsync(input.ToBsonDocument());

            // Cannot change status code in http response: https://github.com/openfaas/faas/issues/157
            // Suggested to add status code in body response
            var response = new ResponseModel()
            {
                response = input,
                status = 201
            };
            
            // Returns the response as Json.
            return JsonConvert.SerializeObject(response);
        }
    }
}