using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PerfClient
{
    class User
    {
        public User(string name, int age)
        {
            this.Id = name;
            this.Name = name;
            this.Age = age;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class PopulateDB
    {
        public static async Task Populate()
        {
            DocumentClient client = BuildClient();
            for(int i = 1; i < 100; ++i)
            {
                User user = new User("User" + i, i);
                await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("UserDB", "Data"), user);
            }
        }

        private static DocumentClient BuildClient()
        {
            SecureString key = new SecureString();
            foreach (char ch in "teQnX8cQjo3UgoWY5b0G846mav7k2AAXLMMVbJZyLRuQby6KQLjPTq8tWkoateqB07FUtBZTK7UtDPGF5bJGeQ==")
            {
                key.AppendChar(ch);
            }

            key.MakeReadOnly();
            var client = new DocumentClient(new Uri("https://cosmosdb-functions.documents.azure.com:443/"), key);
            client.OpenAsync().Wait();

            return client;
        }
    }
}
