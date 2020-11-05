// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using Sovren.Models.API.Matching.Request;
using Sovren.Models.Job;

namespace Sovren.Models.API.Matching
{
    /// <inheritdoc/>
    public class MatchJobRequest : MatchRequest
    {
        /// <summary>
        /// The job to match. This should be generated by parsing a job with the Sovren Job Parser.
        /// </summary>
        public ParsedJob JobData { get; set; }
    }
}