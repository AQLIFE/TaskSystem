using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using TaskManangerSystem.IServices;

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
    public class EntityValidatorAttribute(string? errorMessage =null,string alias = "ProertyAlias", [StringSyntax(StringSyntaxAttribute.Regex)] string? pattern = null, bool required = false, int min = 0, int max = int.MaxValue) : ValidationAttribute
    {
        public string PropertyAlias { set; get; } = alias;

        public string? Pattern { get; set; } = pattern;

        public bool Required { get; set; } = required;

        public Range? Range { get; set; } = new(min, max);


        public new string ErrorMessage { get; set; } = errorMessage??"验证失败";

        public override bool IsValid(object? value)
        {
            if (value is string e)
            {
                return IsMatch(e) && InLen(e);
            }
            else if (value is int q)
            {
                return InRange(q);
            }
            else return IsRequired(value);


        }

        private bool InRange(int value)
        {
            if (Range is not null && Range.Min <= value && value <= Range.Max)
                throw new EntityException(EntityExceptionType.RangeException,ErrorMessage,PropertyAlias,value.ToString());
            else return true;
        }
        private bool IsMatch(string value)
        {
            Required = true;
            if (IsRequired(value) && Pattern is not null && !new Regex(Pattern).IsMatch(value))
                throw new EntityException(EntityExceptionType.RegexException, ErrorMessage, PropertyAlias, value);
            else return true;
        }


        private bool InLen(string value)
        {
            Required = true;
            if (IsRequired(value) && Range is not null && Range.Min <= value.Length && value.Length <= Range.Max)
                throw new EntityException(EntityExceptionType.StrLenException, ErrorMessage, PropertyAlias, value);
            else return true;
        }

        private bool IsRequired(object? value)
        {
            if (Required && value is null) throw new EntityException(EntityExceptionType.RequiredException, ErrorMessage, PropertyAlias, string.Empty);
            else return true;
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
        public override string Message => base.Message;

        public EntityExceptionType EType { set; get; } = exceptionType;

        private string ErrorKey { set; get; } = key;
        private string ErrorValue { set; get; } = value;

        public string ShowException = $"[ {exceptionType.ToString()} ] {key}.{value} {message}";
    }

}
