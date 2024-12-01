using Elastic.Clients.Elasticsearch;
using System.Diagnostics;
using VakantieProject.Models;
using static System.Net.Mime.MediaTypeNames;

namespace VakantieProject.Data
{
    public class dashboardService
    {
        private readonly ElasticsearchClientSettings settings;
        private readonly ElasticsearchClient client;

        public dashboardService()
        {
            settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"));
            client = new ElasticsearchClient(settings);
        }

        public async Task CreateAppleIndexWithMappingAsync()
        {
            await client.Indices.CreateAsync<Apple>(index => index
                     .Index("indexapple22")
                     .Mappings(mappings => mappings
                         .Properties(properties => properties
                             .FloatNumber(x => x.Price)
                             .Date(x => x.CreatedAt)
                             .Text(x => x.Name)
                             .Text(x => x.Description)
                             .Text(x => x.Id)
                             .Object(o => o.fruitvalue, objConfig => objConfig
                                .Properties(p => p
                                    .FloatNumber(t => t.fruitvalue.NewValue)
                                    .FloatNumber(t => t.fruitvalue.OldValue)
                                    .FloatNumber(t => t.fruitvalue.CalculatedDifference)

                         )
                     )
                 )));

        }

        public async Task CreateApples(Apple apple)
        {


          var response = await client.IndexAsync(apple, idx => idx
                .Index("indexapple22")
            );

        }
    }
}
