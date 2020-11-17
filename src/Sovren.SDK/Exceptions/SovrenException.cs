// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using Sovren.Models.API;
using Sovren.Rest;
using System;
using System.Net;
using System.Text.Json;

namespace Sovren
{
    /// <summary>
    /// The most generic exception thrown by the SDK as a result of an error response from the API
    /// </summary>
    public class SovrenException : Exception
    {
        /// <summary>
        /// The raw response from the API
        /// </summary>
        public RestResponse RestResponse { get; protected set; }

        /// <summary>
        /// The HTTP Status Code of the response. See <see href="https://docs.sovren.com/api/rest/#http-status-codes"/>
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; protected set; }

        /// <summary>
        /// The Info.Code of the response. This will indicate what type of error occurred. See <see href="https://docs.sovren.com/api/rest/#http-status-codes"/>
        /// </summary>
        public string SovrenErrorCode { get; protected set; }

        /// <summary>
        /// The Id of the transaction, use this when reporting errors to Sovren Support
        /// </summary>
        public string TransactionId { get; protected set; }

        /// <summary>
        /// The JSON request body, will only have a value if <see cref="SovrenClient.ShowFullRequestBodyInExceptions"/> is <see langword="true"/>
        /// </summary>
        public string RequestBody { get; protected set; }

        internal SovrenException(string requestBody, RestResponse response, ApiResponseInfoLite errorInfo, string transactionId)
            : base(errorInfo?.Message ?? "Invalid response object from API")
        {
            RestResponse = response;
            HttpStatusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
            SovrenErrorCode = errorInfo?.Code ?? "Unknown Error";
            TransactionId = transactionId;
            RequestBody = requestBody;
        }

        internal SovrenException(string requestBody, RestResponse response, ApiResponseInfo errorInfo)
            : this(requestBody, response, errorInfo, errorInfo.TransactionId)
        {
        }
    }
}
