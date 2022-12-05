﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;

namespace React_01_Podrecznik.Validation
{
    public class GoogleReCaptchaValidationAttribute : ValidationAttribute
    {
        public IConfiguration Configuration { get; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Lazy<ValidationResult> errorResult = new Lazy<ValidationResult>(() => new ValidationResult("Google reCAPTCHA validation failed", new string[] { validationContext.MemberName }));

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return errorResult.Value;
            }
            string reCaptchResponse = value.ToString();
            string reCaptchaSecret = Configuration["ReCaptcha:SecretKey"];


            HttpClient httpClient = new HttpClient();
            var httpResponse = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={reCaptchaSecret}&response={reCaptchResponse}").Result;
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return errorResult.Value;
            }

            string jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(jsonResponse);
            if (jsonData.success != true.ToString().ToLower())
            {
                return errorResult.Value;
            }

            return ValidationResult.Success;

        }
    }

    public abstract class GoogleReCaptchaModelBase
    {
        [Required]
        [GoogleReCaptchaValidation]
        [BindProperty(Name = "g-recaptcha-response")]
        public string GoogleReCaptchaResponse { get; set; }
    }
}
