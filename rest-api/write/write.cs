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
                //instantiate a DataContractJsonSerializer to deserialize the JSON string into an object
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(JsonObject));
                var value = 0;

                do
                {
                    //write out new api string
                    string url = $"http://192.168.1.101:80/write?item=opcda://desktop-kn9ludo/ICONICS.SimulatorOPCDA.2/i:Numeric.Ramp&value=(value)";
                    url = url.Replace("(value)", value.ToString());

                    //send http "GET" request to OPC Expert Rest API Server
                    string json = Write(url);

                    using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                    {
                        //deserialize the JSON string into a JSON object
                        JsonObject response = (JsonObject)deserializer.ReadObject(stream);

                        foreach (Item item in response.data)
                        {
                            Console.WriteLine($"ID: {item.ID} | Value: {item.Value} | Quality: {item.Quality} | Timestamp: {item.SourceTimestamp}");
                        }
                    }

                    //toggle the value between 0 and 1
                    value = 1 - value;
                    Thread.Sleep(2000);
                }
                while (true);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            Console.Read();
        }
        static string Write(string url)
        {
            var request = WebRequest.Create(url);

            request.Method = "GET";

            request.ContentType = "application/json";

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #region Json Objects Classes

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
            public string Value { get; set; }

            [DataMember]
            public string Quality { get; set; }

            [DataMember]
            public string SourceTimestamp { get; set; }
        }

        #endregion
    }
}
