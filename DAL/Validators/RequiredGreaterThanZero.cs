using System.ComponentModel.DataAnnotations;

namespace ApiService.Validators
{
    public class RequiredGreaterThanZero : ValidationAttribute
    {
      
        public override bool IsValid(object value)
        {
          
            int i;
            return value != null && int.TryParse(value.ToString(), out i) && i > 0;
        }
    }
}

