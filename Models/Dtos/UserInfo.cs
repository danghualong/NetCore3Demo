using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Models.Dtos
{
    public class UserInfo:IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="用户名不能为空")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin
        {
            get;set;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsAdmin)
            {
                if (Password.Length < 10)
                {
                    yield return new ValidationResult("密码太简单",new[] { nameof(Password)});
                }
            }
        }
    }
}
