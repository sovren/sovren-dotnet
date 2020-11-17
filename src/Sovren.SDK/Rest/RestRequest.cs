// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Sovren.Rest
{
    internal enum RestMethod
    {
        GET,
        POST,
        DELETE,
        PUT,
        PATCH
    }

    internal class RestRequest : IDisposable
    {
        public string Endpoint { get; private set; }
        public RestMethod Method { get; private set; }
        private bool _disposed = false;
        public Stream BodyStream { get; private set; }
        public Encoding Encoding { get; private set; } = Encoding.UTF8;
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();

        public RestRequest(string url, RestMethod method = RestMethod.GET)
        {
            Endpoint = url;
            Method = method;
        }

        public RestRequest(RestMethod method) : this("", method) { }

        public void AddHeader(string name, string value) => Headers[name] = value;

        public async Task WriteUtf8JsonBody(object o)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RestRequest));

            //do not allow this method to be called more than once
            if (BodyStream != null)
                throw new InvalidOperationException($"Cannot call {nameof(WriteUtf8JsonBody)}() more than once.");

            BodyStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(BodyStream, o, SovrenJsonSerialization.DefaultOptions);

            Headers["Content-Type"] = RestContentTypes.GetContentTypeHeader(RestContentTypes.Json, Encoding);
        }

        public async Task<string> GetBody()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RestRequest));

            if (BodyStream == null)
                return "";

            BodyStream.Seek(0, SeekOrigin.Begin);//reset to beginning just in case
            using (StreamReader reader = new StreamReader(BodyStream, Encoding))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (BodyStream != null)
                {
                    BodyStream.Flush();
                    BodyStream.Dispose();
                    BodyStream = null;
                }

                _disposed = true;
            }
        }
    }
}
