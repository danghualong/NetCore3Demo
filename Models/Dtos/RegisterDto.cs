using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Models.Dtos
{
    public class RegisterDto:IValidatableObject
    {
        [Required(ErrorMessage ="用户名不能为空")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="密码不能为空")]
        [MinLength(6,ErrorMessage ="密码长度不能少于6位数")]
        public string Password { get; set; }
        public string RePassword
        {
            get;set;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.Equals(Password,RePassword))
            {
                yield return new ValidationResult("密码两次输入不一致", new[] { nameof(Password) });
            }
        }
    }
}
