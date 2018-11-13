using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb.Repositories
{
    public class DatabaseRepository
    {
        private IAmazonDynamoDB _client;
        private const string TableName = "UserActivity";

        public DatabaseRepository(IAmazonDynamoDB client)
        {
            _client = client;
        }

        public async Task Initialise()
        {
            var request = new ListTablesRequest
            {
                Limit = 10
            };

            var response = await _client.ListTablesAsync(request);

            var results = response.TableNames;

            if (!results.Contains(TableName))
            {
                var createRequest = new CreateTableRequest
                {
                    TableName = TableName,
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Id",
                            AttributeType = "N"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = "HASH"  //Partition key
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 2,
                        WriteCapacityUnits = 2
                    }
                };

                await _client.CreateTableAsync(createRequest);
            }
        }
    }
}

