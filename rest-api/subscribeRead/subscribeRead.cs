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
                string itemName =
                    "opcda://desktop-kn9ludo/" +
                    "ICONICS.SimulatorOPCDA.2/i:Numeric.Ramp";

                string subscribeUrl =
                    "http://192.168.1.101:80/subscribe?item=" +
                    Uri.EscapeDataString(itemName) +
                    "&rate=1000";

                // Subscribe once.
                string subscribeJson = Read(subscribeUrl);
                subscribeJson = subscribeJson.Replace(@"\:", ":");

                var subscribeDeserializer =
                    new DataContractJsonSerializer(
                        typeof(SubscribeResponse));

                SubscribeResponse subscribeResponse;

                using (var stream = new MemoryStream(
                    Encoding.UTF8.GetBytes(subscribeJson)))
                {
                    subscribeResponse =
                        (SubscribeResponse)
                        subscribeDeserializer.ReadObject(stream);
                }

                string subscriptionID =
                    subscribeResponse.data.ID;

                Console.WriteLine(
                    $"Subscription ID: {subscriptionID}");

                string readUrl =
                    "http://192.168.1.101:80/read?subscription=" +
                    Uri.EscapeDataString(subscriptionID);

                var readDeserializer =
                    new DataContractJsonSerializer(
                        typeof(ReadResponse));

                // Read the subscription every second.
                while (true)
                {
                    string readJson = Read(readUrl);
                    readJson = readJson.Replace(@"\:", ":");

                    using (var stream = new MemoryStream(
                        Encoding.UTF8.GetBytes(readJson)))
                    {
                        var readResponse =
                            (ReadResponse)
                            readDeserializer.ReadObject(stream);

                        foreach (Item item in readResponse.data)
                        {
                            Console.WriteLine(
                                $"ID: {item.ID} | " +
                                $"Value: {item.Properties.Value} | " +
                                $"Quality: {item.Properties.StatusCodeDescription} | " +
                                $"Timestamp: {item.Properties.SourceTimestamp}");
                        }
                    }

                    Thread.Sleep(1000);
                }
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
        class SubscribeResponse
        {
            [DataMember]
            public SubscriptionData data { get; set; }
        }

        [DataContract]
        class SubscriptionData
        {
            [DataMember]
            public string ID { get; set; }
        }

        [DataContract]
        class ReadResponse
        {
            [DataMember]
            public List<Item> data { get; set; }
        }

        [DataContract]
        class Item
        {
            [DataMember]
            public string ID { get; set; }

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
