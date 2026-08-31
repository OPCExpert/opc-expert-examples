/*
 * Read one or more OPC items with the OPC Expert Web Server REST API.
 *
 * Official documentation:
 * https://opcexpert.com/opc-expert-web-server-api-documentation/
 *
 * Requirements: .NET 6 or later.
 */

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
                string url = $"http://192.168.1.101:80/read?item=opcda://desktop-kn9ludo/ICONICS.SimulatorOPCDA.2/i:Numeric.Ramp";

                //instantiate a DataContractJsonSerializer to deserialize the JSON string into an object
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(JsonObject));

                do
                {
                    //send http "GET" request to OPC Expert Rest API Server
                    string json = Read(url);

                    using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                    {
                        //deserialize the JSON string into a JSON object
                        JsonObject response = (JsonObject)deserializer.ReadObject(stream);

                        foreach (Item item in response.data)
                        {
                            Console.WriteLine($"ID: {item.ID} | Value: {item.Value} | Quality: {item.Quality} | Timestamp: {item.SourceTimestamp}");
                        }
                    }

                    //loop every 1 second
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
            //create web request object
            var request = WebRequest.Create(url);

            //set request method as "GET"
            request.Method = "GET";

            //set content response type as JSON
            request.ContentType = "application/json";

            //send request and get response from resver
            using (WebResponse response = request.GetResponse())
            {
                //read response stream from web response
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

