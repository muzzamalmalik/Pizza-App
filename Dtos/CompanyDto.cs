using Microsoft.AspNetCore.Http;
using System;

namespace PizzaOrder.Dtos
{
    public class CompanyDto
    {
    }

    public class AddCompanyDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string CellNumber { get; set; }
        public string SecondaryContactPerson { get; set; }
        public string SecondaryCellNumber { get; set; }
        public int UserTypeId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IFormFile ImageData { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class EditCompanyDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string CellNumber { get; set; }
        public string SecondaryContactPerson { get; set; }
        public string SecondaryCellNumber { get; set; }
        public int UserTypeId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IFormFile ImageData { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class GetAllCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string CellNumber { get; set; }
        public string SecondaryContactPerson { get; set; }
        public string SecondaryCellNumber { get; set; }
        //public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
    public class GetCompanyByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string CellNumber { get; set; }
        public string SecondaryContactPerson { get; set; }
        public string SecondaryCellNumber { get; set; }
        //public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    public int CreatedById { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    }
}
