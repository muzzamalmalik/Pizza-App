using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Context;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using PizzaOrder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PizzaOrder.Repository
{
    public class BillPaymentsRepository : BaseRepository, IBillPaymentsRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public BillPaymentsRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }
        public async Task<ServiceResponse<object>> AddBillPayments(AddBillPaymentsDto dtoData)
        {
            if (dtoData != null)
            {
                if (dtoData.ImageData.Length > 0)
                {
                    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "BillPaymentReceipts");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                    var fullPath = Path.Combine(pathToSave);
                    dtoData.FilePath = "BillPaymentReceipts";
                    dtoData.FileName = fileName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "BillPaymentReceipts", fileName);
                    //string pathString = filePath.LastIndexOf("/") + 1;

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await dtoData.ImageData.CopyToAsync(stream);
                    }
                }
                var objBillPayments = new BillPayments
                {
                    BillId = dtoData.BillId,
                    OrderId= dtoData.OrderId,
                    PaymentMode = dtoData.PaymentMode,
                    PaymentDate = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                    TransactionReference = dtoData.TransactionReference,
                    FileName = dtoData.FileName,
                    FilePath = dtoData.FilePath,
                    Active = true,
                    CompanyId= dtoData.CompanyId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };
                await _context.BillPayments.AddAsync(objBillPayments);
                await _context.SaveChangesAsync();

                _serviceResponse.Data = objBillPayments.Id;
                _serviceResponse.Message = CustomMessage.Added;
                _serviceResponse.Success = true;
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetAllBillPayments(int CompanyId)
        {
            var list = await (from m in _context.BillPayments
                              where m.CompanyId == CompanyId

                              select new GetAllBillPaymentsDto
                              {
                                  Id= m.Id,
                                  BillId = m.BillId,
                                  OrderId = m.OrderId,
                                  PaymentMode = m.PaymentMode,
                                  PaymentDate = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                                  TransactionReference = m.TransactionReference,
                                  FileName = m.FileName,
                                  FilePath = m.FilePath,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
                                  Active = m.Active,
                                  CompanyId = m.CompanyId,
                                  CreatedById = m.CretedById,
                                  DateCreated = m.DateCreated,
                                  UpdatedById = m.UpdateById,
                                  DateModified = m.DateModified,
                              }).ToListAsync();

            if (list.Count > 0)
            {
                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetBillPaymentsById(int id)
        {
            var objbillpayments = await _context.BillPayments.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (objbillpayments != null)
            {
                var data = new GetBillPaymentsByIdDto
                {
                    Id = objbillpayments.Id,
                    BillId = objbillpayments.Id,
                    OrderId = objbillpayments.OrderId,
                    PaymentMode = objbillpayments.PaymentMode,
                    PaymentDate = objbillpayments.PaymentDate,
                    TransactionReference = objbillpayments.TransactionReference,
                    FileName = objbillpayments.FileName,
                    FilePath = objbillpayments.FilePath,
                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + objbillpayments.FilePath + '/' + objbillpayments.FileName,
                    Active = objbillpayments.Active,
                    CompanyId = objbillpayments.CompanyId,
                    CreatedById = objbillpayments.CretedById,
                    DateCreated = objbillpayments.DateCreated,
                    UpdatedById = objbillpayments.UpdateById,
                    DateModified = objbillpayments.DateModified,
                };

                _serviceResponse.Data = data;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }

            return _serviceResponse;
        }
    }
}
