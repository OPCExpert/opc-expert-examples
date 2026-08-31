using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace OPC_Expert_Rest_API
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string url =
                    "http://192.168.1.101:80/subscribe?item=" +
                    "opcda://desktop-kn9ludo/" +
                    "ICONICS.SimulatorOPCDA.2/i:Numeric.Ramp&rate=1000";

                var deserializer =
                    new DataContractJsonSerializer(typeof(JsonObject));

                do
                {
                    string json = Read(url);

                    // The server returns invalid escapes such as "\:".
                    json = json.Replace(@"\:", ":");

                    using (var stream = new MemoryStream(
                        Encoding.UTF8.GetBytes(json)))
                    {
                        var response =
                            (JsonObject)deserializer.ReadObject(stream);

                        Console.WriteLine(
                            $"Subscription ID: {response.data.ID} | " +
                            $"Update rate: {response.data.UpdateRate} ms");

                        foreach (Item item in
                            response.data.SubscribedItems)
                        {
                            Console.WriteLine(
                                $"Item: {item.UserFriendlyName} | " +
                                $"Value: {item.Properties.Value} | " +
                                $"Quality: {item.Properties.StatusCodeDescription} | " +
                                $"Timestamp: {item.Properties.SourceTimestamp}");
                        }
                    }

                    Thread.Sleep(1000);
                }
                while (true);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            Console.Read();
        }

        static string Read(string url)
        {
            var request = WebRequest.Create(url);

            request.Method = "GET";
            request.ContentType = "application/json";

            using (WebResponse response = request.GetResponse())
            using (var reader =
                new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        [DataContract]
        class JsonObject
        {
            [DataMember]
            public SubscriptionData data { get; set; }
        }

        [DataContract]
        class SubscriptionData
        {
            [DataMember]
            public int UpdateRate { get; set; }

            [DataMember]
            public List<Item> SubscribedItems { get; set; }

            [DataMember]
            public string ID { get; set; }

            [DataMember]
            public int ItemCount { get; set; }

            [DataMember]
            public string LastReadTimestamp { get; set; }
        }

        [DataContract]
        class Item
        {
            [DataMember]
            public string UserFriendlyName { get; set; }

            [DataMember]
            public string BrowsePath { get; set; }

            [DataMember]
            public Properties Properties { get; set; }
        }

        [DataContract]
        class Properties
        {
            [DataMember]
            public double Value { get; set; }

            [DataMember]
            public string StatusCodeDescription { get; set; }

            [DataMember]
            public string SourceTimestamp { get; set; }
        }
    }
}
