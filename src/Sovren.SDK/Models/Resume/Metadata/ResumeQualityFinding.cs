// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using System.Collections.Generic;

namespace Sovren.Models.Resume.Metadata
{
    /// <summary>
    /// A single resume quality issue
    /// </summary>
    public class ResumeQualityFinding
    {
        /// <summary>
        /// A unique 3-digit code to identify what type of issue was found.
        /// See all possibilities at our docs site <see href="https://docs.sovren.com/#resume-quality">here</see>.
        /// </summary>
        public int QualityCode { get; set; }

        /// <summary>
        /// Areas in the resume where this issue was found or affected by this issue. These will be
        /// section indexes, work history position identifiers, etc.
        /// </summary>
        public List<string> Identifiers { get; set; }

        /// <summary>
        /// A human-readable message explaining the issue that is being reported and possibly how to fix.
        /// </summary>
        public string Message { get; set; }
    }
}