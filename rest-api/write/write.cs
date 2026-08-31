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
                    "http://192.168.1.101:80/write?item=" +
                    "opcda://desktop-kn9ludo/" +
                    "ICONICS.SimulatorOPCDA.2/i:Numeric.Memory&value=1";

                var deserializer =
                    new DataContractJsonSerializer(typeof(JsonObject));

                do
                {
                    string json = Read(url);

                    using (var stream = new MemoryStream(
                        Encoding.UTF8.GetBytes(json)))
                    {
                        var response =
                            (JsonObject)deserializer.ReadObject(stream);

                        foreach (Item item in response.data)
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
