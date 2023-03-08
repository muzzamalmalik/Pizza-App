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
using System.Security.Policy;
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
            //var objUser = await (from u in _context.Deals
            //                     where u.Title == dtoData.Title.Trim() && u.CompanyId == dtoData.CompanyId

            //                     select new
            //                     {
            //                         Title = u.Title,
            //                         CompanyId = u.CompanyId,
            //                         Description = u.Description,
            //                     }).FirstOrDefaultAsync();

            if (dtoData != null)
            {
                //if (objUser.Title.Length > 0 && dtoData.Title.Trim() == objUser.Title)
                //{
                //    _serviceResponse.Message = "Deal with this name ALready Exists";
                //}
                //_serviceResponse.Success = false;
                //_serviceResponse.Data = objUser.Title;



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
                        Price = dtoData.Price,
                        Percentage = dtoData.Percentage,
                        DiscountAmount = dtoData.DiscountAmount,
                        FilePath = "DealImages",
                        FileName = dtoData.FileName,
                        CompanyId = dtoData.CompanyId,
                        ActiveQueue = dtoData.ActiveQueue,
                        CretedById = _LoggedIn_UserID,
                        DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                    };


                    await _context.Deals.AddAsync(ObjDeal);
                    await _context.SaveChangesAsync();
                    //_serviceResponse.Data = ObjDeal;
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Added;
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
                    objdeal.Price = dtoData.Price;
                    objdeal.Percentage = dtoData.Percentage;
                    objdeal.DiscountAmount = dtoData.DiscountAmount;
                    objdeal.FilePath = dtoData.FilePath;
                    objdeal.FileName = dtoData.FileName;
                    objdeal.ActiveQueue = dtoData.ActiveQueue;
                    objdeal.CompanyId = dtoData.CompanyId;
                    objdeal.UpdateById = _LoggedIn_UserID;
                    //objdeal.DateModified = DateTime.Now;
                    objdeal.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));
                    //objdeal.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.Now));
                    // objdeal.DateModified = System.DateTime.ToLocalTime("YOUR TIME ZONE (e.g. Pakistan Standard Time)");

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

        public async Task<ServiceResponse<object>> GetAllDeal(int CompanyId, int page, int pageSize)
        {
            var listCount = _context.Deals.Where(x => x.CompanyId.Equals(CompanyId)).Select(p => p.Id).Count();

            var skip = pageSize * (page - 1);
            //var canPage = skip < listCount;

            var list = await (from m in _context.Deals
                              where CompanyId == m.CompanyId

                              select new GetAllDealDto
                              {
                                  Id = m.Id,
                                  Title = m.Title,
                                  Description = m.Description,
                                  Price = m.Price,
                                  Percentage = m.Percentage,
                                  DiscountAmount = m.DiscountAmount,
                                  ActiveQueue = m.ActiveQueue,
                                  FileName = m.FileName,
                                  FilePath = m.FilePath,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,

                                  ObjGetAllDealSection = (from n in _context.DealSection
                                                          where n.DealId == m.Id

                                                          select new GetAllDealSectionDto
                                                          {
                                                              Id = n.Id,
                                                              DealId = n.DealId,
                                                              Title = n.Title,
                                                              Description = n.Description,
                                                              ChooseQuantity = n.ChooseQuantity,
                                                              CategoryId = n.CategoryId,

                                                          }).ToList(),

                                  ObjGetAllDealSectionDetail = (from o in _context.DealSectionDetail
                                                                where Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value) == m.Id

                                                                select new GetAllDealSectionDetailDto
                                                                {
                                                                    Id = o.Id,
                                                                    //DealId = _configuration.GetSection("AppSettings:CompanyId").Value,
                                                                    DealSectionId = o.DealSectionId,
                                                                    ItemId = o.ItemId,

                                                                }).ToList(),
                                  CreatedById = m.CretedById,
                                  DateCreated = m.DateCreated,
                                  UpdatedById = m.UpdateById,
                                  DateModified = m.DateModified,
                              }).Skip(skip)
                              .Take(pageSize)
                              .ToListAsync();

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
            var objdeal = await _context.Deals.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (objdeal != null)
            {
                var data = new GetDealDetailsByIdDto
                {
                    Id = objdeal.Id,
                    //CompanyId = objdeal.CompanyId,
                    Title = objdeal.Title,
                    Description = objdeal.Description,
                    Price = objdeal.Price,
                    Percentage = objdeal.Percentage,
                    DiscountAmount = objdeal.DiscountAmount,
                    ActiveQueue = objdeal.ActiveQueue,
                    FileName = objdeal.FileName,
                    FilePath = objdeal.FilePath,
                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + objdeal.FilePath + '/' + objdeal.FileName,

                    ObjGetDealSection = (from m in _context.DealSection
                                         where m.DealId == objdeal.Id

                                         select new GetDealSectionDto
                                         {
                                             Id = m.Id,
                                             DealId = m.DealId,
                                             Title = m.Title,
                                             ChooseQuantity = m.ChooseQuantity,
                                             CategoryId = m.CategoryId,
                                             Description = m.Description,

                                             ObjGetAllFlavours = (from q in _context.DealSectionDetail
                                                                  where q.DealSectionId == m.Id

                                                                  select new GetAllFlavoursDto
                                                                  {
                                                                      Id = q.Id,
                                                                      FlavourName = q.ObjItem.Name,
                                                                      ItemId = q.ItemId,
                                                                      Quantity = 0, 
                                                                      
                                                                  }).ToList(),
                                         }).ToList(),

                    CreatedById = objdeal.CretedById,
                    DateCreated = objdeal.DateCreated,
                    UpdatedById = objdeal.UpdateById,
                    DateModified = objdeal.DateModified,
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
