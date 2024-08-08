using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TaskManangerSystem.Services.Tool
{
    public class EntityRule
    {
        [StringSyntax(StringSyntaxAttribute.Regex)]
        public const string Alias = @"^[a-z\d]{5,16}$";

        [StringSyntax(StringSyntaxAttribute.Regex)]
        public const string PassWord = @"^[\dA-Za-z]{8,128}$";


          
    }
}
