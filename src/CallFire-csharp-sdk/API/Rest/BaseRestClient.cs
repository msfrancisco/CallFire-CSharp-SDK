﻿using System.IO;
using System.Xml.Serialization;
using CallFire_csharp_sdk.Common;

namespace CallFire_csharp_sdk.API.Rest
{
    public abstract class BaseRestClient<T>
    {
        private const string RestApiUrl = "https://www.callfire.com/api/1.1/rest/";
        internal readonly IHttpClient XmlClient;

        internal BaseRestClient(string username, string password)
            : this(CreateXmlServiceClient(username, password))
        {
        }

        private static IHttpClient CreateXmlServiceClient(string username, string password)
        {
            return new HttpClient(RestApiUrl, username, password);
        }

        internal BaseRestClient(IHttpClient xmlClient)
        {
            XmlClient = xmlClient;
        }
        
        internal TU BaseRequest<TU>(HttpMethod method, object request, CallfireRestRoute<T> route)
        {
            var response = XmlClient.Send(route.ToString(), method, request);

            if (string.IsNullOrEmpty(response))
            {
                return default(TU);
            }

            var deserializer = new XmlSerializer(typeof(TU));
            TextReader reader = new StringReader(response);
            return (TU)deserializer.Deserialize(reader);
        }
    }
}
