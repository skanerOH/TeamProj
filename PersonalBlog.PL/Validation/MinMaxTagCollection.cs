using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Validation
{
    /// <summary>
    /// Castom validation attribute to vlaidate list of tags
    /// </summary>
    public class MinMaxTagCollection : ValidationAttribute
    {
        private readonly int _minTagLength;
        private readonly int _maxTagLength;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minTagLength">min permitted tag length</param>
        /// <param name="maxTagLength">max permitted tag length</param>
        public MinMaxTagCollection(int minTagLength, int maxTagLength)
        {
            _minTagLength = minTagLength;
            _maxTagLength = maxTagLength;
        }

        /// <summary>
        /// Main validation method
        /// </summary>
        /// <param name="value">value to validate</param>
        /// <param name="validationContext">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                IEnumerable<string> tags = value as IEnumerable<string>;
                foreach (var t in tags)
                {
                    if (t.Length > _maxTagLength)
                        return new ValidationResult($"Error tag:{t} tag length must be <={_maxTagLength}");
                    if (t.Length < _minTagLength)
                        return new ValidationResult($"Error tag:{t} tag length must be >={_minTagLength}");
                    if (!Regex.IsMatch(t, @"^(\w+)"))
                        return new ValidationResult($"Error tag:{t} tag length must consist of letters or digits");
                }
            }
            return ValidationResult.Success;
        }
    }
}
