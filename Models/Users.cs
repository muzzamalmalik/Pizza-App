using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PizzaOrder.Models;

namespace PizzaOrder.Models
{
    public class User : BaseEntity
    {    
        public string UserName { get; set; }
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }
        [StringLength(200, ErrorMessage = "Address cannot be longer then 200 characters")]
        public string Address { get; set; }
        public bool Active { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }       
        public int  Gender { get; set; }
        public DateTime? DateofBirth { get; set; }
        public int UserTypeId { get; set; }

        [StringLength(30, ErrorMessage = "Contact Number is not loger then 30 characters")]
        public string ContactNumber { get; set; }
        public string SecoundaryContactNumber { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int? VerifyCode { get; set; }
        public int CompanyId { get; set; }


        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }


    }
}
