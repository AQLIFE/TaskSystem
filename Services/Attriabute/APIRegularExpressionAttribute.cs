using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TaskManangerSystem.Services.Attriabute
{
    public class APIRegularExpressionAttribute([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, string message = "不符合定义") : ValidationAttribute
    {
        public string Pattern { get; set; } = pattern;
        public new string ErrorMessage { get; set; } = message;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var regex = new Regex(Pattern);
            if (!regex.IsMatch((string)value))
            {
                throw new APIRegexException(ErrorMessage);
            }
            return ValidationResult.Success;
        }

    }


    public class APIRegexException(string message) : Exception(message) { }

}
