using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PizzaOrder.Models;

namespace PizzaOrder.Models
{
    public class User : BaseEntity
    {    
        [StringLength(50, ErrorMessage = "Username cannot be longer then 50 characters")]
        public string UserName { get; set; }
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }
        [StringLength(200, ErrorMessage = "Address cannot be longer then 200 characters")]
        public string Address { get; set; }
        [StringLength(30, ErrorMessage = "CellPhone cannot be longer then 30 characters")]
        public string CellPhone { get; set; }
        public bool Active { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }       
        public int  Gender { get; set; }
        public DateTime? DateofBirth { get; set; }
        public int UserTypeId { get; set; }

        [StringLength(30, ErrorMessage = "Contact Number is not loger then 30 characters")]
        public string ContactNumber { get; set; }
 
        [Required]
        [StringLength(200, ErrorMessage = "File Path cannot be longer then 200 characters")]
        public string FilePath { get; set; }
        [StringLength(50, ErrorMessage = "File Name cannot be longer then 50 characters")]
        public string FileName { get; set; }
        public int? VerifyCode { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company  ObjCompany { get; set; }

        [ForeignKey("UserTypeId")]
        public virtual UserType ObjUserType { get; set; }


    }
}
