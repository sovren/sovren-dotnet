// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using Sovren.Models.API.BimetricScoring;
using Sovren.Models.API.Matching;
using Sovren.Models.API.Matching.Request;
using Sovren.Models.API.Matching.UI;
using Sovren.Models.Job;
using Sovren.Models.Resume;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sovren
{
    /// <summary/>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class SovrenClientWithUI
    {
        internal MatchUISettings UISessionOptions { get; set; }
        internal SovrenClient InternalClient { get; set; }
        internal SovrenClientWithUI(SovrenClient client, MatchUISettings uiSessionOptions)
        {
            InternalClient = client;
            UISessionOptions = uiSessionOptions;
        }
    }

    /// <summary/>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class AIMatchingServiceExtensions
    {
        /// <summary>
        /// Access methods for generating Sovren Matching UI sessions. For example: <code>sovClient.UI(options).Search(...)</code>
        /// </summary>
        /// <param name="client">the internal SovrenClient to make the API calls</param>
        /// <param name="uiSessionOptions">
        /// Options/settings for the Matching UI.
        /// <br/>NOTE: if you do not provide a <see cref="UIOptions.Username"/> (in <see cref="MatchUISettings.UIOptions"/>),
        /// the user will be prompted to login as soon as the Matching UI session is loaded
        /// </param>
        public static SovrenClientWithUI UI(this SovrenClient client, MatchUISettings uiSessionOptions = null)
        {
            return new SovrenClientWithUI(client, uiSessionOptions);
        }

        /// <summary>
        /// Create a Matching UI session to find matches for a resume or job that is already indexed
        /// </summary>
        /// <param name="sovClient">The SovrenClient</param>
        /// <param name="indexId">The index containing the document you want to match</param>
        /// <param name="documentId">The ID of the document to match</param>
        /// <param name="indexesToQuery">The indexes to find results in. These must all be of the same type (resumes or jobs)</param>
        /// <param name="preferredWeights">
        /// The preferred category weights for scoring the results. If none are provided,
        /// Sovren will determine the best values based on the source resume/job
        /// </param>
        /// <param name="filters">Any filters to apply prior to the match (a result must satisfy all the filters)</param>
        /// <param name="settings">Settings for this match</param>
        /// <param name="numResults">The number of results to show. If not specified, the default will be used.</param>
        /// <exception cref="SovrenException">Thrown when an API error occurs</exception>
        public static async Task<GenerateUIResponse> MatchIndexedDocument(
            this SovrenClientWithUI sovClient,
            string indexId,
            string documentId,
            IEnumerable<string> indexesToQuery,
            CategoryWeights preferredWeights = null,
            FilterCriteria filters = null,
            SearchMatchSettings settings = null,
            int numResults = 0)
        {
            MatchByDocumentIdOptions options = sovClient.InternalClient.CreateRequest(indexesToQuery, preferredWeights, filters, settings, numResults);
            UIMatchByDocumentIdOptions uiOptions = new UIMatchByDocumentIdOptions(options, sovClient.UISessionOptions);
            return await sovClient.InternalClient.UIMatch(indexId, documentId, uiOptions);
        }

        /// <summary>
        /// Create a Matching UI session to search for resumes or jobs that meet specific criteria
        /// </summary>
        /// <param name="sovClient">The SovrenClient</param>
        /// <param name="indexesToQuery">The indexes to find results in. These must all be of the same type (resumes or jobs)</param>
        /// <param name="query">The search query. A result must satisfy all of these criteria</param>
        /// <param name="settings">The settings for this search request</param>
        /// <param name="pagination">Pagination settings. If not specified the default will be used</param>
        /// <exception cref="SovrenException">Thrown when an API error occurs</exception>
        public static async Task<GenerateUIResponse> Search(
            this SovrenClientWithUI sovClient,
            IEnumerable<string> indexesToQuery,
            FilterCriteria query,
            SearchMatchSettings settings = null,
            PaginationSettings pagination = null)
        {
            SearchRequest request = sovClient.InternalClient.CreateRequest(indexesToQuery, query, settings, pagination);
            UISearchRequest uiRequest = new UISearchRequest(request, sovClient.UISessionOptions);
            return await sovClient.InternalClient.UISearch(uiRequest);
        }

        /// <summary>
        /// Create a Matching UI session to find matches for a non-indexed resume. For matching against indexed resumes,
        /// see <see cref="MatchIndexedDocument"/>
        /// </summary>
        /// <param name="sovClient">The SovrenClient</param>
        /// <param name="resume">The resume (generated by the Sovren Resume Parser) to use as the source for a match query</param>
        /// <param name="indexesToQuery">The indexes to find results in. These must all be of the same type (resumes or jobs)</param>
        /// <param name="preferredWeights">
        /// The preferred category weights for scoring the results. If none are provided,
        /// Sovren will determine the best values based on the source resume
        /// </param>
        /// <param name="filters">Any filters to apply prior to the match (a result must satisfy all the filters)</param>
        /// <param name="settings">Settings for this match</param>
        /// <param name="numResults">The number of results to show. If not specified, the default will be used.</param>
        /// <exception cref="SovrenException">Thrown when an API error occurs</exception>
        public static async Task<GenerateUIResponse> MatchResume(
            this SovrenClientWithUI sovClient,
            ParsedResume resume,
            IEnumerable<string> indexesToQuery,
            CategoryWeights preferredWeights = null,
            FilterCriteria filters = null,
            SearchMatchSettings settings = null,
            int numResults = 0)
        {
            MatchResumeRequest request = sovClient.InternalClient.CreateRequest(resume, indexesToQuery, preferredWeights, filters, settings, numResults);
            UIMatchResumeRequest uiRequest = new UIMatchResumeRequest(request, sovClient.UISessionOptions);
            return await sovClient.InternalClient.UIMatch(uiRequest);
        }

        /// <summary>
        /// Create a Matching UI session to find matches for a non-indexed job
        /// </summary>
        /// <param name="sovClient">The SovrenClient</param>
        /// <param name="job">The job (generated by the Sovren Job Parser) to use as the source for a match query</param>
        /// <param name="indexesToQuery">The indexes to find results in. These must all be of the same type (resumes or jobs)</param>
        /// <param name="preferredWeights">
        /// The preferred category weights for scoring the results. If none are provided,
        /// Sovren will determine the best values based on the source job
        /// </param>
        /// <param name="filters">Any filters to apply prior to the match (a result must satisfy all the filters)</param>
        /// <param name="settings">Settings for this match</param>
        /// <param name="numResults">The number of results to show. If not specified, the default will be used.</param>
        /// <exception cref="SovrenException">Thrown when an API error occurs</exception>
        public static async Task<GenerateUIResponse> MatchJob(
            this SovrenClientWithUI sovClient,
            ParsedJob job,
            IEnumerable<string> indexesToQuery,
            CategoryWeights preferredWeights = null,
            FilterCriteria filters = null,
            SearchMatchSettings settings = null,
            int numResults = 0)
        {
            MatchJobRequest request = sovClient.InternalClient.CreateRequest(job, indexesToQuery, preferredWeights, filters, settings, numResults);
            UIMatchJobRequest uiRequest = new UIMatchJobRequest(request, sovClient.UISessionOptions);
            return await sovClient.InternalClient.UIMatch(uiRequest);
        }

        /// <summary>
        /// Create a Matching UI session to score one or more target documents against a source resume
        /// </summary>
        /// <param name="sovClient">The SovrenClient</param>
        /// <param name="sourceResume">The source resume</param>
        /// <param name="targetDocuments">The target resumes/jobs</param>
        /// <param name="preferredWeights">
        /// The preferred category weights for scoring the results. If none are provided,
        /// Sovren will determine the best values based on the source resume
        /// </param>
        /// <param name="settings">Settings to be used for this scoring request</param>
        /// <typeparam name="TTarget">Either <see cref="ParsedResumeWithId"/> or <see cref="ParsedJobWithId"/></typeparam>
        /// <exception cref="SovrenException">Thrown when an API error occurs</exception>
        public static async Task<GenerateUIResponse> BimetricScore<TTarget>(
            this SovrenClientWithUI sovClient,
            ParsedResumeWithId sourceResume,
            List<TTarget> targetDocuments,
            CategoryWeights preferredWeights = null,
            SearchMatchSettings settings = null) where TTarget : IParsedDocWithId
        {
            BimetricScoreResumeRequest request = sovClient.InternalClient.CreateRequest(sourceResume, targetDocuments, preferredWeights, settings);
            UIBimetricScoreResumeRequest uiRequest = new UIBimetricScoreResumeRequest(request, sovClient.UISessionOptions);
            return await sovClient.InternalClient.UIBimetricScore(uiRequest);
        }

        /// <summary>
        /// Create a Matching UI session to score one or more target documents against a source job
        /// </summary>
        /// <param name="sovClient">The SovrenClient</param>
        /// <param name="sourceJob">The source job</param>
        /// <param name="targetDocuments">The target resumes/jobs</param>
        /// <param name="preferredWeights">
        /// The preferred category weights for scoring the results. If none are provided,
        /// Sovren will determine the best values based on the source job
        /// </param>
        /// <param name="settings">Settings to be used for this scoring request</param>
        /// <typeparam name="TTarget">Either <see cref="ParsedResumeWithId"/> or <see cref="ParsedJobWithId"/></typeparam>
        /// <exception cref="SovrenException">Thrown when an API error occurs</exception>
        public static async Task<GenerateUIResponse> BimetricScore<TTarget>(
            this SovrenClientWithUI sovClient,
            ParsedJobWithId sourceJob,
            List<TTarget> targetDocuments,
            CategoryWeights preferredWeights = null,
            SearchMatchSettings settings = null) where TTarget : IParsedDocWithId
        {
            BimetricScoreJobRequest request = sovClient.InternalClient.CreateRequest(sourceJob, targetDocuments, preferredWeights, settings);
            UIBimetricScoreJobRequest uiRequest = new UIBimetricScoreJobRequest(request, sovClient.UISessionOptions);
            return await sovClient.InternalClient.UIBimetricScore(uiRequest);
        }
    }
}