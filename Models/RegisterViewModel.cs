using System.ComponentModel.DataAnnotations;

namespace BankAccount.Models
{
    public class RegisterViewModel 
    {        
        [Required]
        [Display(Name = "First Name")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string first_name{get;set;}
        
        [Required]
        [Display(Name = "Last Name")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string last_name{get;set;}

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string email{get;set;}

        [Required]
        [Display(Name = "Password")]
        [MinLength(8,ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string password{get;set;}

        [Required]
        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirmation{get;set;} // PwConfirm
    }

    public class LoginViewModel : BaseEntity
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string LogEmail{get;set;}

        [Required]
        [Display(Name = "Password")]
        [MinLength(8,ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string LogPassword{get;set;}
    }
}