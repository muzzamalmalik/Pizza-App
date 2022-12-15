using Microsoft.AspNetCore.Http;
using System;

namespace PizzaOrder.Dtos
{
    //public class BillPaymentsDto
    //{
    //}

    public class AddBillPaymentsDto
    {
        public int BillId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int TransactionReference { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IFormFile ImageData { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
    }

    

    public class GetAllBillPaymentsDto
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int TransactionReference { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
    }

    public class GetBillPaymentsByIdDto
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int TransactionReference { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
    }
}
