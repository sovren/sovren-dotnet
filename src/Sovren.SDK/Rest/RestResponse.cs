// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.Text.Json;
using Sovren.Rest;
using System.Threading.Tasks;
using System;

//use this namespace specifically so that we dont make the intellisense confusing for integrators
namespace Sovren
{
    /// <summary>
    /// A raw REST response from an API request
    /// </summary>
    public class RestResponse
    {
        /// <summary>
        /// The body of the HTTP response, this will only be set when <see cref="IsSuccessful"/> is false or there was a deserialization error
        /// </summary>
        public string Body { get; protected set; } 

        /// <summary>
        /// The HTTP status code returned by the server
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// A short message about the <see cref="StatusCode"/>
        /// </summary>
        public string StatusDescription { get; private set; }

        /// <summary>
        /// HTTP headers in the response
        /// </summary>
        public WebHeaderCollection Headers { get; private set; }

        internal bool IsSuccessful => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        internal Exception DeserializationException { get; set; }

        internal RestResponse(HttpStatusCode code, string description, WebHeaderCollection headers)
        {
            StatusCode = code;
            StatusDescription = description;
            Headers = headers ?? new WebHeaderCollection();
        }
    }

    internal class RestResponse<T> : RestResponse
    {
        /// <summary>
        /// Be sure to null-check this value, as it will be default(T) any time you get a non-200 response (and possibly other scenarios)
        /// </summary>
        public T Data { get; private set; }

        internal RestResponse(HttpStatusCode code, string description, WebHeaderCollection headers = null)
            : base(code, description, headers) { }

        internal static async Task<RestResponse<T>> CreateResponse(
            Stream body,
            HttpStatusCode code,
            string description,
            WebHeaderCollection headers)
        {
            RestResponse<T> response = new RestResponse<T>(code, description, headers);

            if (!string.IsNullOrEmpty(headers[HttpResponseHeader.ContentType]))
            {
                (string ContentType, Encoding Encoding) typeAndEncoding = RestContentTypes.ParseContentTypeHeader(headers[HttpResponseHeader.ContentType]);

                if (typeAndEncoding.ContentType == RestContentTypes.Json)
                {
                    string strBody = null;
                    using (StreamReader reader = new StreamReader(body, typeAndEncoding.Encoding))
                    {
                        strBody = await reader.ReadToEndAsync();
                    }

                    try
                    {
                        response.Data = JsonSerializer.Deserialize<T>(strBody, SovrenJsonSerialization.DefaultOptions);
                    }
                    catch (Exception e)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            //status code was 'ok' (request/response was normal) but we could not deserialize, save the exception to report back the error/body
                            response.DeserializationException = e;
                        }

                        //otherwise, eat the exception since a non-200 response will not have the expected response body
                    }

                    if (!response.IsSuccessful || response.DeserializationException != null)
                    {
                        response.Body = strBody;
                    }
                }
            }

            return response;
        }
    }
}
