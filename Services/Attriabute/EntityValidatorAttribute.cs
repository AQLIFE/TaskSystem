using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TaskManangerSystem.Services.Attriabute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="alias"></param>
    /// <param name="pattern"></param>
    /// <param name="required">该参数为真时，该项必定验证</param>
    /// <param name="errorMessage"></param>
    /// <param name="lenRange"></param>
    /// <param name="numRange"></param>
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityValidatorAttribute(string alias, string? errorMessage) : ValidationAttribute
    {
        public EntityValidatorAttribute([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, string alias = "ProertyAlias", string? errorMessage = null) :this(alias,errorMessage)
        {
            Required = true;
            Pattern = pattern;
        }

        public EntityValidatorAttribute(bool required, string alias = "ProertyAlias", string? errorMessage = null) : this(alias, errorMessage)
        {
            Required = required;
        }

        public EntityValidatorAttribute(int min = 0, int max = 0, string alias = "ProertyAlias", string? errorMessage = null) : this(alias, errorMessage)
        {
            if (min == max) Range = null;
            else {
                Required = true;
                Range = new Range(min, max);
            }
        }

        public string PropertyAlias { set; get; } = alias;

        public new string ErrorMessage { get; set; } = errorMessage ?? "验证失败";

        public string? Pattern { get; set; } = null;

        public bool Required { get; set; } = false;

        public Range? Range { get; set; } = null;



        public override bool IsValid(object? value)
        {
            if (value is string e)
            {
                return IsMatch(e) && InLen(e);
                // when IsMatch need 'Required' is true,IsMatch return true,ignore
            }
            else if (value is int q)
            {
                return InRange(q);
            }
            else return IsRequired(value);

        }

        private bool IsRequired(object? value)
        {
            if (!Required || (Required && value is not null)) return true;
            throw new EntityException(EntityExceptionType.RequiredException, ErrorMessage, PropertyAlias, string.Empty);
        }
        private bool IsMatch(string value)
        {
            Required = true;
            if (Pattern is null || (IsRequired(value) && Pattern is not null && new Regex(Pattern).IsMatch(value)))
                return true;
            throw new EntityException(EntityExceptionType.RegexException, ErrorMessage, PropertyAlias, value);
        }
        private bool InRange(int value)
        {
            if (Range is null || (Range is not null && Range.Min <= value && value <= Range.Max))
                return true;
            throw new EntityException(EntityExceptionType.RangeException, ErrorMessage, PropertyAlias, value.ToString());
        }
        private bool InLen(string value)
        {
            Required = true;
            if (Range is null || (IsRequired(value) && Range is not null && Range.Min <= value.Length && value.Length <= Range.Max))
                return true;
            throw new EntityException(EntityExceptionType.StrLenException, ErrorMessage, PropertyAlias, value);
        }

    }

    public class Range(int min, int max)
    {
        public int Min { get; set; } = min;
        public int Max { get; set; } = max;
    }

    public enum EntityExceptionType
    {
        RequiredException, RegexException, StrLenException, RangeException
    }

    public class EntityException(EntityExceptionType exceptionType, string message, string key, string value) : Exception(message)
    {
        public string ShowException = $"[ {exceptionType} ] {key}=>{value} {message}";
    }

}
