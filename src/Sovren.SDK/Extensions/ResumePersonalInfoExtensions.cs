// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

using Sovren.Models;
using Sovren.Models.Resume;
using System.Collections.Generic;

namespace Sovren
{
    /// <summary></summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class ResumePersonalInfoExtensions
    {
        /// <summary>
        /// Gets the candidate's birthplace (if found) or <see langword="null"/>
        /// </summary>
        public static string GetBirthplace(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.Birthplace;
        }

        /// <summary>
        /// Gets the candidate's current location (if found) or <see langword="null"/>
        /// </summary>
        public static string GetCurrentLocation(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.CurrentLocation;
        }

        /// <summary>
        /// Gets the candidate's current salary (if found) or <see langword="null"/>
        /// </summary>
        public static Salary GetCurrentSalary(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.CurrentSalary;
        }

        /// <summary>
        /// Gets the candidate's date of birth (if found) or <see langword="null"/>
        /// </summary>
        public static SovrenDate GetDateOfBirth(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.DateOfBirth;
        }

        /// <summary>
        /// Gets the candidate's driving license (if found) or <see langword="null"/>
        /// </summary>
        public static string GetDrivingLicense(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.DrivingLicense;
        }

        /// <summary>
        /// Gets the candidate's family composition (if found) or <see langword="null"/>
        /// </summary>
        public static string GetFamilyComposition(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.FamilyComposition;
        }

        /// <summary>
        /// Gets the candidate's father's name (if found) or <see langword="null"/>
        /// </summary>
        public static string GetFathersName(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.FathersName;
        }

        /// <summary>
        /// Gets the candidate's gender (if found) or <see langword="null"/>
        /// </summary>
        public static string GetGender(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.Gender;
        }

        /// <summary>
        /// Gets the candidate's marital status (if found) or <see langword="null"/>
        /// </summary>
        public static string GetMaritalStatus(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.MaritalStatus;
        }

        /// <summary>
        /// Gets the candidate's mother's maiden name (if found) or <see langword="null"/>
        /// </summary>
        public static string GetMothersMaidenName(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.MothersMaidenName;
        }

        /// <summary>
        /// Gets the candidate's mother tongue (if found) or <see langword="null"/>
        /// </summary>
        public static string GetMotherTongue(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.MotherTongue;
        }

        /// <summary>
        /// Gets the candidate's national identities (if found) or <see langword="null"/>
        /// </summary>
        public static List<NationalIdentity> GetNationalIdentities(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.NationalIdentities;
        }

        /// <summary>
        /// Gets the candidate's nationality (if found) or <see langword="null"/>
        /// </summary>
        public static string GetNationality(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.Nationality;
        }

        /// <summary>
        /// Gets the candidate's passport number (if found) or <see langword="null"/>
        /// </summary>
        public static string GetPassportNumber(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.PassportNumber;
        }

        /// <summary>
        /// Gets the candidate's preferred location (if found) or <see langword="null"/>
        /// </summary>
        public static string GetPreferredLocation(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.PreferredLocation;
        }

        /// <summary>
        /// Gets the candidate's required salary (if found) or <see langword="null"/>
        /// </summary>
        public static Salary GetRequiredSalary(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.RequiredSalary;
        }

        /// <summary>
        /// Gets the candidate's visa status (if found) or <see langword="null"/>
        /// </summary>
        public static string GetVisaStatus(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.VisaStatus;
        }

        /// <summary>
        /// Gets whether the candidate is willing to relocate (if found) or <see langword="null"/>
        /// </summary>
        public static string GetWillingToRelocate(this ParseResumeResponseValueExtensions response)
        {
            return response.Value.ResumeData?.PersonalAttributes?.WillingToRelocate;
        }
    }
}