//using CheckListApp.Data.Validations;
//using FluentValidation;

namespace CheckListApp.Data
{
    //[Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
