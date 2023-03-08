using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace PizzaOrder.Dtos
{
    public class UserDto
    {
    }
    public class UserForAddDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot be longer then 50 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Email cannot be longer then 50 characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "User Type Field Is Required")]
        public int? UserTypeId { get; set; }
       // [Required(ErrorMessage = "Restaurant Field Is Required")]
   
        public int updatebyId { get; set; }
        public string UpdatedDateTime { get; set; }

        public int? AreaId { get; set; }

        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public string ContactNumber { get; set; }
        public int CompanyId { get; set; }


    }

    
    public class LoginUserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime LastActive { get; set; }
        public int CompanyId { get; set; }
        public bool IsMainBranch { get; set; }

        public int VerificationCode { get; set; }


    }
    public class UserForUpdateDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot be longer then 50 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Email cannot be longer then 50 characters")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }
  
        
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public string ContactNumber { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public bool IsPrimaryPhoto { get; set; } = true;
        [Required(ErrorMessage = "User Type Field Is Required")]
        public int? UserTypeId { get; set; }
        [Required(ErrorMessage = "Restaurant Field Is Required")]
        public int? RestaurantId { get; set; }
        public IFormFileCollection Files { get; set; }
        public bool Active { get; set; } = true;
        public int updatebyId { get; set; }
        public string UpdatedDateTime { get; set; }
    }


    public class UserUpdateForAppDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot be longer then 50 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Email cannot be longer then 50 characters")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }

        public string Address2 { get; set; }
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public string ContactNumber { get; set; }
        public int? MarriageStatusId { get; set; }
        public string NumberOfChildren { get; set; }
        public int? IndustryId { get; set; }
        public bool JobIntrest { get; set; }
        public string EducationalBackground { get; set; }
        public int? EmploymentSectorId { get; set; }
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FilePath{ get; set; }
        public int updatebyId { get; set; }
        public string UpdatedDateTime { get; set; }
    }
    
    public class UserForUpdatemobileverifecationDto
    {
       // public bool Active { get; set; } = true;
        public int Code { get; set; }

    }

    public class UserForDetailsDto : BaseDto
    {
        public int Id { get; set; }
        public string FullPath { get; set; }
        public string RestaurantlogoFullPath { get; set; }
        public string Username { get; set; }
        public string ContactNumber { get; set; }
        public string NumberOfChildren { get; set; }                 
        public int MarriageStatusId { get; set; }
        public int EmploymentSectorId { get; set; }
        public int IndustryId { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public int UserTypeId { get; set; }
        public int updatebyId { get; set; }
        public string UpdatedDateTime { get; set; }
        public string UserInitial { get; set; }
        public string DateofBirth { get; set; }
        public bool JobIntrest { get; set; }

        public string EducationalBackground { get; set; }
        //public int RestaurantId { get; set; }
    }
    public class UserForListDto : BaseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }

        public string DateofBirth { get; set; }
        public string ContactNumber { get; set; }
        public string Role { get; set; }
        public int UserTypeId { get; set; }
        public int updatebyId { get; set; }
        public string UpdatedDateTime { get; set; }
        public int CompanyId { get; set; }
        public int CheckinCounts { get; set; }
        public int LeaveCounts { get; set; }
        public string UserDesignation { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public string CompanyName { get; set; }
        // public int RestaurantId { get; set; }
    }


   public class AwardAddDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoTitle { get; set; }
        public IFormFile File { get; set; }
        public  IFormFile LogoData { get; set; }
        
        public string LogoDataB { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public bool Active { get; set; }
    }

    public class RestaurantuserListDto
    {
        public int RestaurantUserId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

    }
    public class UserAddCheckDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
     

    }
    public class ChangePW
    { public string Password { get; set; } }

    public class Rlogo
    { public string RestaurantlogoFullPath { get; set; } }



}

