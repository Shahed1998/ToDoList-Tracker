using System.ComponentModel.DataAnnotations;

namespace Web.Models.Business_Entities
{
    // Custom attribute to specify decimal precision and scale
    public class DecimalPrecisionAttribute : ValidationAttribute
    {
        private readonly int _precision;
        private readonly int _scale;

        public DecimalPrecisionAttribute(int precision, int scale)
        {
            _precision = precision;
            _scale = scale;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true; // Null values are considered valid
            }

            decimal decimalValue;
            if (decimal.TryParse(value.ToString(), out decimalValue))
            {
                // Check precision and scale
                string[] parts = decimalValue.ToString().Split('.');
                int integerPartLength = parts[0].Length;
                int decimalPartLength = parts.Length > 1 ? parts[1].Length : 0;

                return integerPartLength <= _precision && decimalPartLength <= _scale;
            }

            return false; // Value is not a valid decimal
        }
    }
}
