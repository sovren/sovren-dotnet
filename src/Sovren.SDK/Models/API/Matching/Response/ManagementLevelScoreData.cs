// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

namespace Sovren.Models.API.Matching.Response
{
    /// <inheritdoc/>
    public class ManagementLevelScoreData : CategoryScoreData
    {
        /// <summary>
        /// Actual management level found.
        /// </summary>
        public string Actual { get; set; }

        /// <summary>
        /// Requested management level.
        /// </summary>
        public string Desired { get; set; }

        /// <summary>
        /// <see langword="true"/> when the duration of management experience matches in the source and target documents.
        /// </summary>
        public bool AmountOfExperienceMatches { get; set; }
    }
}
