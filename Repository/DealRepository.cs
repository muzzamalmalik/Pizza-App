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
    public class DealRepository : BaseRepository, IDealRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public DealRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddDeal(AddDealDto dtoData)
        {
            var objUser = await (from u in _context.Deals
                                 where u.Title == dtoData.Title.Trim() && u.CompanyId == dtoData.CompanyId

                                 select new
                                 {
                                     Title = u.Title,
                                     CompanyId = u.CompanyId,
                                     Description = u.Description,
                                 }).FirstOrDefaultAsync();

            if (objUser != null)
            {
                if (objUser.Title.Length > 0 && dtoData.Title.Trim() == objUser.Title)
                {
                    _serviceResponse.Message = "Deal with this name ALready Exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = objUser.Title;
            }
            else
            if (dtoData.ImageData != null && dtoData.ImageData.Length > 0)
            {
                var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "DealImages");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                var fullPath = Path.Combine(pathToSave);
                dtoData.FilePath = "DealImages";
                dtoData.FileName = fileName;
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var filePath = Path.Combine(_HostEnvironment.WebRootPath, "DealImages", fileName);
                //string pathString = filePath.LastIndexOf("/") + 1;
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await dtoData.ImageData.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }


                var ObjDeal = new Deal
                {
                    Title = dtoData.Title,
                    Description = dtoData.Description,
                    Price= dtoData.Price,
                    Percentage= dtoData.Percentage,
                    DiscountAmount = dtoData.DiscountAmount,
                    FilePath = "DealImages",
                    FileName = dtoData.FileName,
                    CompanyId = dtoData.CompanyId,
                    ActiveQueue = dtoData.ActiveQueue,
                };

                try
                {
                    await _context.Deals.AddAsync(ObjDeal);
                    await _context.SaveChangesAsync();
                    //_serviceResponse.Data = ObjDeal;
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Added;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditDeal(int id, EditDealDto dtoData)
        {
            var objdeal = await _context.Deals.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objdeal != null)
            {
                if (dtoData.ImageData != null && dtoData.ImageData.Length > 0)
                {
                    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "DealImages");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                    var fullPath = Path.Combine(pathToSave);
                    dtoData.FilePath = "DealImages";
                    dtoData.FileName = fileName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "DealImages", fileName);
                    //string pathString = filePath.LastIndexOf("/") + 1;
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            await dtoData.ImageData.CopyToAsync(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    objdeal.Title = dtoData.Title;
                    objdeal.Description = dtoData.Description;
                    objdeal.Price= dtoData.Price;
                    objdeal.Percentage= dtoData.Percentage;
                    objdeal.DiscountAmount= dtoData.DiscountAmount;
                    objdeal.FilePath = dtoData.FilePath;
                    objdeal.FileName = dtoData.FileName;
                    objdeal.ActiveQueue = dtoData.ActiveQueue;
                    objdeal.CompanyId = dtoData.CompanyId;

                    _context.Deals.Update(objdeal);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Updated;
                }
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllDeal(int CompanyId)
        {
            var list = await (from m in _context.Deals
                              where m.CompanyId == CompanyId

                              select new GetAllDealDto
                              {
                                  Id = m.Id,
                                  CompanyId = m.CompanyId,
                                  Title = m.Title,
                                  Description = m.Description,
                                  Price= m.Price,
                                  Percentage = m.Percentage,
                                  DiscountAmount= m.DiscountAmount,
                                  ActiveQueue = m.ActiveQueue,
                                  FileName = m.FileName,
                                  FilePath = m.FilePath,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,

                                  ObjGetAllDealSection = (from n in _context.DealSection where n.DealId == m.Id
                                                          
                                                          select new GetAllDealSectionDto
                                                          {
                                                              Id = n.Id,
                                                              DealId = n.DealId,
                                                              ChooseQuantity = n.ChooseQuantity,
                                                              CompanyId = n.CompanyId,
                                                              CategoryId = n.CategoryId,

                                                          }).ToList(),

                                  ObjGetAllDealSectionDetail = (from o in _context.DealSectionDetail
                                                                where o.DealId == m.Id

                                                                select new GetAllDealSectionDetailDto
                                                                {
                                                                    Id = o.Id,
                                                                    DealId = o.DealId,
                                                                    DealSectionId = o.DealSectionId,
                                                                    ItemId = o.ItemId,
                                                                    CompanyId = o.CompanyId,

                                                                }).ToList(),

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

        public async Task<ServiceResponse<object>> GetDealDetailsById(int id)
        {
            var objdeal = (from m in _context.Deals
                                 join n in _context.DealSection on m.Id equals n.DealId
                                 where m.Id == id

                                 select new GetDealDetailsByIdDto
                                 {
                                     Id = m.Id,
                                     Title = m.Title,
                                     Description = m.Description,
                                     Price = m.Price,
                                     Percentage = m.Percentage,
                                     DiscountAmount = m.DiscountAmount,
                                     FileName = m.FileName,
                                     FilePath = m.FilePath,
                                     FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
                                     ActiveQueue = m.ActiveQueue,
                                     CompanyId = m.CompanyId,
                                     DealId = n.DealId,
                                     ChooseQuantity = n.ChooseQuantity,

                                     ObjGetAllFlavours = (from q in _context.Items
                                                          where q.CategoryId == n.CategoryId

                                                          select new GetAllFlavoursDto
                                                          {
                                                              Id = q.Id,
                                                              FlavourName = q.Name,
                                                              CategoryId = q.CategoryId,

                                                          }).ToList(),


                                     //ObjGetAllDealSectionDetail = (from o in _context.DealSectionDetail
                                     //                              where o.DealId == objdeal.Id

                                     //                              select new GetAllDealSectionDetailDto
                                     //                              {
                                     //                                  Id = o.Id,
                                     //                                  DealId = o.DealId,
                                     //                                  DealSectionId = o.DealSectionId,
                                     //                                  ItemId = o.ItemId,
                                     //                                  CompanyId = o.CompanyId,
                                     //                                  ItemName = _context.Items.FirstOrDefault(x=>x.Id == o.ItemId).Name,

                                     //                              }).ToList(),
                                 });


            if (objdeal != null)
            {

                _serviceResponse.Data = objdeal;
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
