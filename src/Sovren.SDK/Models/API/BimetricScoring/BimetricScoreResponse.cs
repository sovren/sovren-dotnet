// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using Sovren.Models.API.Matching;

namespace Sovren.Models.API.BimetricScoring
{
    /// <inheritdoc/>
    public class BimetricScoreResponse : ApiResponse<BimetricScoreResponseValue> { }

    /// <summary>
    /// The <see cref="ApiResponse{T}.Value"/> from a 'BimetricScore' response
    /// </summary>
    public class BimetricScoreResponseValue : BaseScoredResponseValue<BimetricScoreResult> { }
}