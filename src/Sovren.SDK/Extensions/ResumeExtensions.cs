// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using Sovren.Models.API.Parsing;
using Sovren.Models.Resume;
using Sovren.Models.Resume.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Sovren
{
    /// <summary></summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class ParseResumeResponseValueExtensions
    {
        internal ParseResumeResponseValue Value { get; set; }
        internal ParseResumeResponseValueExtensions(ParseResumeResponseValue value)
        {
            Value = value;
        }
    }

    /// <summary></summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class ResumeExtensions
    {
        /// <summary>
        /// Use this method to get easy access to most of the commonly-used data inside a parse response.
        /// <br/>For example <code>response.EasyAccess().GetCandidateName()</code>.
        /// <br/>This is just an alternative to writing your own logic to process the information provided in the response.
        /// </summary>
        public static ParseResumeResponseValueExtensions EasyAccess(this ParseResumeResponseValue response)
        {
            return new ParseResumeResponseValueExtensions(response);
        }

        /// <summary>
        /// Outputs a JSON string that can be saved to disk or any other data storage.
        /// <br/>NOTE: be sure to save with UTF-8 encoding!
        /// </summary>
        /// <param name="formatted"><see langword="true"/> for pretty-printing</param>
        /// <param name="resume">the resume</param>
        public static string ToJson(this ParsedResume resume, bool formatted = false)
        {
            JsonSerializerOptions options = SovrenJsonSerialization.DefaultOptions;
            options.WriteIndented = formatted;
            return JsonSerializer.Serialize(resume, options);
        }

        /// <summary>
        /// Gets a list of certifications found (if any) or <see langword="null"/>
        /// </summary>
        /// <param name="response"></param>
        /// <param name="onlyMatchedToList">
        /// <see langword="true"/> to only return certifications that matched to Sovren's internal list of known certifications.
        /// <br/><see langword="false"/> to return all certifications, no matter how they were found
        /// </param>
        public static IEnumerable<string> GetCertifications(this ParseResumeResponseValueExtensions response, bool onlyMatchedToList = false)
        {
            return response.Value.ResumeData?.Certifications?.Where(c => !onlyMatchedToList || c.MatchedToList).Select(c => c.Name);
        }

        /// <summary>
        /// Gets a list of licenses found (if any) or <see langword="null"/>
        /// </summary>
        /// <param name="response"></param>
        /// <param name="onlyMatchedToList">
        /// <see langword="true"/> to only return licenses that matched to Sovren's internal list of known licenses.
        /// <br/><see langword="false"/> to return all licenses, no matter how they were found
        /// </param>
        public static IEnumerable<string> GetLicenses(this ParseResumeResponseValueExtensions response, bool onlyMatchedToList = false)
        {
            return response.Value.ResumeData?.Licenses?.Where(c => !onlyMatchedToList || c.MatchedToList).Select(c => c.Name);
        }

        /// <summary>
        /// Gets a list of ISO 639-1 codes for language competencies the candidate listed (if any) or <see langword="null"/>
        /// </summary>
        public static IEnumerable<string> GetLanguageCompetencies(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.LanguageCompetencies?.Select(c => c.LanguageCode);
        }

        /// <summary>
        /// Gets the number of military experiences/posts found on a resume
        /// </summary>
        public static int GetNumberOfMilitaryExperience(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.MilitaryExperience?.Count() ?? 0;
        }

        /// <summary>
        /// Gets whether or not security clearance was found on the resume
        /// </summary>
        public static bool HasSecurityClearance(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.SecurityCredentials?.Any() ?? false;
        }

        /// <summary>
        /// Gets the severity level of the most severe resume quality finding. One of:
        /// <br/> <see cref="ResumeQualityLevel.FatalProblem"/>
        /// <br/> <see cref="ResumeQualityLevel.MajorIssue"/>
        /// <br/> <see cref="ResumeQualityLevel.DataMissing"/>
        /// <br/> <see cref="ResumeQualityLevel.SuggestedImprovement"/>
        /// <br/> null
        /// </summary>
        public static ResumeQualityLevel GetMostSevereResumeQualityFinding(this ParseResumeResponseValueExtensions response)
        {
            var fatalErrors = response.Value.ResumeData?.ResumeMetadata?.ResumeQuality?.Where(r => r.Level == ResumeQualityLevel.FatalProblem.Value);
            var majorProblems = response.Value.ResumeData?.ResumeMetadata?.ResumeQuality?.Where(r => r.Level == ResumeQualityLevel.MajorIssue.Value);
            var dataMissing = response.Value.ResumeData?.ResumeMetadata?.ResumeQuality?.Where(r => r.Level == ResumeQualityLevel.DataMissing.Value);
            var improvements = response.Value.ResumeData?.ResumeMetadata?.ResumeQuality?.Where(r => r.Level == ResumeQualityLevel.SuggestedImprovement.Value);

            if (fatalErrors != null && fatalErrors.Any()) return ResumeQualityLevel.FatalProblem;
            if (majorProblems != null && majorProblems.Any()) return ResumeQualityLevel.MajorIssue;
            if (dataMissing != null && dataMissing.Any()) return ResumeQualityLevel.DataMissing;
            if (improvements != null && improvements.Any()) return ResumeQualityLevel.SuggestedImprovement;

            return null;//no issues found (amazing)
        }

        /// <summary>
        /// Gets the last-modified date of the resume, if you provided one. Otherwise <see cref="DateTime.MinValue"/>
        /// </summary>
        public static DateTime GetDocumentLastModified(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.ResumeMetadata?.DocumentLastModified ?? DateTime.MinValue;
        }

        /// <summary>
        /// Gets the age of the resume, if it has a RevisionDate. Otherwise <see cref="TimeSpan.MaxValue"/>
        /// </summary>
        public static TimeSpan GetResumeAge(this ParseResumeResponseValueExtensions response)
        {
            DateTime revDate = response.GetDocumentLastModified();

            if (revDate == DateTime.MinValue) return TimeSpan.MaxValue;

            return DateTime.UtcNow - revDate;
        }

        /// <summary>
        /// Checks if the resume timed out during parsing. If <see langword="true"/>, the data in the resume may be incomplete
        /// </summary>
        public static bool DidTimeout(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ParsingMetadata?.TimedOut ?? false;
        }

        /// <summary>
        /// Checks if Sovren found any possible problems in the converted text of the resume (prior to parsing).
        /// <br/>For more info, see <see href="https://docs.sovren.com/#document-conversion-result-codes"/>
        /// </summary>
        public static bool HasConversionWarning(this ParseResumeResponseValueExtensions response)
        {
            string code = response.Value.ConversionMetadata?.OutputValidityCode;

            switch (code)
            {
                case "ovProbableGarbageInText":
                case "ovUnknown":
                case "ovAvgWordLengthGreaterThan20":
                case "ovAvgWordLengthLessThan4":
                case "ovTooFewLineBreaks":
                case "ovLinesSeemTooShort":
                case "ovTruncated":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Use this to get the resume as a JSON string (to save to disk or other data storage).
        /// <br/>NOTE: be sure to save with UTF-8 encoding!
        /// </summary>
        /// <param name="response"></param>
        /// <param name="piiRedacted"><see langword="true"/> for the redacted version of the resume, otherwise <see langword="false"/></param>
        public static string GetResumeAsJsonString(this ParseResumeResponseValueExtensions response, bool piiRedacted)
        {
            ParsedResume resume = piiRedacted ? response.Value.ScrubbedResumeData : response.Value.ResumeData;
            if (resume == null) return null;
            return resume.ToString();
        }

        /// <summary>
        /// Save the resume to disk using UTF-8 encoding
        /// </summary>
        /// <param name="response"></param>
        /// <param name="piiRedacted"><see langword="true"/> to save the redacted version of the resume, otherwise <see langword="false"/></param>
        /// <param name="filePath">The file to save to</param>
        public static void SaveResumeJsonToFile(this ParseResumeResponseValueExtensions response, bool piiRedacted, string filePath)
        {
            string json = response.GetResumeAsJsonString(piiRedacted);
            if (json != null)
            {
                System.IO.File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
            }
        }

        /// <summary>
        /// Gets the xx-XX language/culture value for the resume
        /// </summary>
        public static string GetCulture(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.ResumeMetadata?.DocumentCulture;
        }

        /// <summary>
        /// Gets the ISO 639-1 language code for the language the resume was written in
        /// </summary>
        public static string GetLanguage(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.ResumeMetadata?.DocumentLanguage;
        }
    }
}