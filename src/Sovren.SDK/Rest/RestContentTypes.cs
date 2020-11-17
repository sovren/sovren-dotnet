// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using System.Text;

namespace Sovren.Rest
{
    internal static class RestContentTypes
    {
        public const string Json = "application/json";

        /// <summary>
        /// Parses something like `application/json; charset=utf-8` into its components
        /// Returns utf-8 as the encoding if not specified
        /// </summary>
        public static (string ContentType, Encoding Encoding) ParseContentTypeHeader(string headerValue)
        {
            headerValue = headerValue.ToLowerInvariant();

            string charsetFlag = "charset=";
            string charset = "utf-8";//default

            int charsetIdx = headerValue.IndexOf(charsetFlag);

            if (charsetIdx > -1)
            {
                int charsetEnds = headerValue.IndexOf(";", charsetIdx);
                int charsetStarts = charsetIdx + charsetFlag.Length;

                if (charsetEnds == -1)
                {
                    charset = headerValue.Substring(charsetStarts);
                }
                else
                {
                    charset = headerValue.Substring(charsetStarts, charsetEnds - charsetStarts);
                }
            }

            Encoding encoding = Encoding.GetEncoding(charset);

            string contentType = headerValue.Split(';')[0].Trim();

            return (contentType, encoding);
        }

        /// <summary>
        /// Outputs something like `application/json; charset=utf-8`
        /// </summary>
        public static string GetContentTypeHeader(string contentType, Encoding encoding)
        {
            string header = $"{contentType}";

            if (encoding != null)
            {
                header += $"; {encoding.HeaderName}";
            }

            return header;
        }
    }
}
