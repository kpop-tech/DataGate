﻿// Model class for binding shareclasses

// Created: 10/2019
// Author:  Philip Shishov

// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
namespace Pharus.App.Models.BindingModels.ShareClasses
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateShareClassBindingModel : BaseCreateEntityBindingModel
    {
        [Required(ErrorMessage = "You must enter a value for the ShareClass Name!")]
        [StringLength(200, ErrorMessage = "The ShareClass Name must be no longer than 200 characters")]
        [RegularExpression(@"^[A-Z-0-9]+(\s[A-Z-0-9]+)*$", ErrorMessage = "Not in correct format!")]
        [Display(Name = "Official ShareClass Name")]
        public string ShareClassName { get; set; }

        [Display(Name = "Investor Type")]
        public string InvestorType { get; set; }

        [Display(Name = "Share Type")]
        public string ShareType { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public string CurrencyCode { get; set; }

        [Display(Name = "Country Issue")]
        public string CountryIssue { get; set; }

        [Display(Name = "Country Risk")]
        public string CountryRisk { get; set; }

        [Display(Name = "Emission Date")]
        public DateTime? EmissionDate { get; set; }

        [Display(Name = "Inception Date")]
        public DateTime? InceptionDate { get; set; }

        [Display(Name = "Last Nav Date")]
        public DateTime? LastNavDate { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Initial Price")]
        public double InitialPrice { get; set; }

        [Display(Name = "Accounting Code")]
        public string AccountingCode { get; set; }

        [Display(Name = "Hedged")]
        public string IsHedged { get; set; }

        [Display(Name = "Listed")]
        public string IsListed { get; set; }

        [Display(Name = "Bloomberg Market")]
        public string BloombergMarket { get; set; }

        [Display(Name = "Bloomberg Code")]
        public string BloombergCode { get; set; }

        [Display(Name = "Bloomberg Id")]
        public string BloombergId { get; set; }

        [Display(Name = "ISIN Code")]
        public string ISINCode { get; set; }

        [Display(Name = "Valor Code")]
        public string ValorCode { get; set; }

        [Display(Name = "WKN")]
        public string WKN { get; set; }

        [Display(Name = "Date Business Year")]
        public DateTime? DateBusinessYear { get; set; }

        [Display(Name = "Prospectus Code")]
        public string ProspectusCode { get; set; }

        [Required(ErrorMessage = "Please choose a subfund container!")]
        [Display(Name = "Subfund Container")]
        public string SubFundContainer { get; set; }
    }
}