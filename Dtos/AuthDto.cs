using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaOrder.Dtos
{
    public class AuthDto
    {
    }
    public class UserForLoginDto
    {
        [Required]
        public string ContactNumber { get; set; }
        [Required] 
        public string Password { get; set; }
        public int UserTypeId { get; set; }

    }
    public class UserForRegisterDto
    {

        //public string Username { get; set; }

        //public string ContactNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Contact Number is not loger then 30 characters")]
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "You must specify password between 8 and 20 characters")]
        public string Password { get; set; }
        //[Required(ErrorMessage = "User Type Field Is Required")]
        public int? UserTypeId { get; set; }

        //public int Gender { get; set; }
        //public string UserDesignation { get; set; }
        public int CompanyId { get; set; }

        //public string FileName { get; set; }
        //public string FilePath { get; set; }
        //public IFormFile ImageData { get; set; }
        //public string ImageDataB { get; set; }
        //public string ImageTitle { get; set; }
        //public string FullPath { get; set; }

    }

    public class VerifyUserDto
    {
        //[Required]
        //public string FullName { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        //[Required]
        //public string Email { get; set; }
        //public int CompanyId { get; set; }
        //public int? UserTypeId { get; set; }
    }

    public class ProfileDataDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public string Email { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public int UserTypeId { get; set; }
        public int CompanyId { get; set; }
        public string Address { get; set; }
        public string FullPath { get; set; }
    }

    public class GetAllUsersDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public string Email { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public int UserTypeId { get; set; }
        public int CompanyId { get; set; }
        public string Address { get; set; }
        public string FullPath { get; set; }
    }


    public class VerifyUserReturnObjDto
    {
 
        //public string FullName { get; set; }
        public string ContactNumber { get; set; }
        //public int CompanyId { get; set; }
        //public int? UserTypeId { get; set; }
        //public string Email { get; set; }
        public string VerifyCode { get; set; }
    }


    public class UserForEditDto
    {
        [Required]
        public string Username { get; set; }

        public string FullName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "You must specify password between 8 and 20 characters")]

        [EmailAddress]
        public string Email { get; set; }

        public string Gender { get; set; }
        public string CellPhone { get; set; }
        public string UserDesignation { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IFormFile ImageData { get; set; }
        public string ImageDataB { get; set; }
        public string ImageTitle { get; set; }
        public string FullPath { get; set; }
    }
    public class UserForEditDtoAdd

    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IFormFile ImageData { get; set; }
    
    }
    public class changePWCheckDto
    {
   


        public string Value { get; set; }


    }
}
