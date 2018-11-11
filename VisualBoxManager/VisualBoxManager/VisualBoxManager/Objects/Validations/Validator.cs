using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using VisualBoxManager.Objects.Validations;

namespace VisualBoxManager.Objects.Validations
{
    static class Validator
    {
        public const int minNumberOfCharName = 2;
        public const int minNumberOfCharPass = 5;
        public static ValidationResult ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address == email)
                {
                    return new ValidationResult(false);
                }
                else
                {
                    return new ValidationResult(true, "Email not valid.");
                }
            }
            catch
            {
                return new ValidationResult(true, "Email not valid.");
            }
        }
        

        public static ValidationResult ValidateName(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                return new ValidationResult(true, "Input is required");
            }
            else if (name.Length < minNumberOfCharName)
            {
                return new ValidationResult(true, "Name must be more the " + (minNumberOfCharName - 1) + " characters");             
            }
            return new ValidationResult(false);
        }

        public static ValidationResult ValidatePass(string pass, string passRe)
        {
            if (string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(passRe))
            {
                return new ValidationResult(true,"Password is required");
            }
            else if (!pass.Equals(passRe))
            {
                return new ValidationResult(true,"Paswwords don't match");
            }
            else if (pass.Length < minNumberOfCharPass)
            {
                return new ValidationResult(true,"Minimum " + minNumberOfCharPass + " characters");
            }

            return new ValidationResult(false);
        }
    }
}
