using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
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

            if (dtoData != null)
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


                    var ObjDeal = new Deal
                    {
                        Title = dtoData.Title,
                        Description = dtoData.Description,
                        Price = dtoData.Price,
                        Percentage = dtoData.Percentage,
                        DiscountAmount = (dtoData.Price * dtoData.Percentage)/100,
                        FilePath = "DealImages",
                        FileName = dtoData.FileName,
                        //CompanyId = dtoData.CompanyId,
                        CompanyId = _LoggedIn_CompanyId,
                        ActiveQueue = dtoData.ActiveQueue,
                        IsActive=dtoData.IsActive,
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
                    objdeal.IsActive = dtoData.IsActive;
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
            if(page==0)
            {
                var list = await (from m in _context.Deals
                                  where CompanyId == m.CompanyId
                                  orderby m.Id descending
                                  select new GetAllDealDto
                                  {
                                      Id = m.Id,
                                      Title = m.Title,
                                      Description = m.Description ?? "",
                                      Price = m.Price-m.DiscountAmount??0,
                                      Percentage = m.Percentage,
                                      DiscountAmount = m.DiscountAmount,
                                      CompanyId = m.CompanyId,
                                      ActiveQueue = m.ActiveQueue,
                                      FileName = m.FileName,
                                      FilePath = m.FilePath,
                                      IsActive = m.IsActive,
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
                                                                  IsActive=n.IsActive
                                                              }).ToList(),

                                      ObjGetAllDealSectionDetail = (from o in _context.DealSectionDetail
                                                                    where Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value) == m.Id

                                                                    select new GetAllDealSectionDetailDto
                                                                    {
                                                                        Id = o.Id,
                                                                        //DealId = _configuration.GetSection("AppSettings:CompanyId").Value,
                                                                        DealSectionId = o.DealSectionId,
                                                                        ItemId = o.ItemId,
                                                                        IsActive=o.IsActive
                                                                    }).ToList(),
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
            }
            else
            {
                var list = await (from m in _context.Deals
                                  where CompanyId == m.CompanyId

                                  orderby m.Id descending
                                  select new GetAllDealDto
                                  {
                                      Id = m.Id,
                                      Title = m.Title,
                                      Description = m.Description ?? "",
                                      Price = m.Price,
                                      Percentage = m.Percentage,
                                      DiscountAmount = m.DiscountAmount,
                                      ActiveQueue = m.ActiveQueue,
                                      FileName = m.FileName,
                                      FilePath = m.FilePath,
                                      FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
                                      IsActive = m.IsActive,
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
                                                                  IsActive=n.IsActive

                                                              }).ToList(),

                                      ObjGetAllDealSectionDetail = (from o in _context.DealSectionDetail
                                                                    where Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value) == m.Id

                                                                    select new GetAllDealSectionDetailDto
                                                                    {
                                                                        Id = o.Id,
                                                                        //DealId = _configuration.GetSection("AppSettings:CompanyId").Value,
                                                                        DealSectionId = o.DealSectionId,
                                                                        ItemId = o.ItemId,
                                                                        IsActive=o.IsActive
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
                    CompanyId = objdeal.CompanyId,
                    Title = objdeal.Title,
                    Description = objdeal.Description,
                    Price = objdeal.Price,
                    Percentage = objdeal.Percentage,
                    DiscountAmount = objdeal.DiscountAmount,
                    ActiveQueue = objdeal.ActiveQueue,
                    FileName = objdeal.FileName,
                    FilePath = objdeal.FilePath,
                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + objdeal.FilePath + '/' + objdeal.FileName,
                    IsActive=objdeal.IsActive,
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
                                             IsActive=m.IsActive,

                                             ObjGetAllFlavours = (from q in _context.DealSectionDetail
                                                                  where q.DealSectionId == m.Id

                                                                  select new GetAllFlavoursDto
                                                                  {
                                                                      Id = q.Id,
                                                                      FlavourName = q.ObjItem.Name,
                                                                      ItemId = q.ItemId,
                                                                      Quantity = 0,
                                                                      IsActive= q.IsActive,

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
        public async Task<ServiceResponse<object>> AddDealData(AddDealDataDto dtoData)
        {
            IDbContextTransaction objTrans = null;
            objTrans = _context.Database.BeginTransaction();

            if (dtoData != null)
            {
                // Add Deal
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
                   
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await dtoData.ImageData.CopyToAsync(stream);
                    }                                       
                }
                else
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = "Image cannot be Empty....";

                    return _serviceResponse;
                }
                var ObjDeal = new Deal
                {

                    Title = dtoData.Title,
                    Description = dtoData.Description,
                    Price = dtoData.Price,
                    Percentage = dtoData.Percentage,
                    DiscountAmount = dtoData.Price-((dtoData.Price * dtoData.Percentage) / 100),
                    FileName = dtoData.FileName,
                    FilePath = dtoData.FilePath,
                    ActiveQueue = dtoData.ActiveQueue,
                    IsActive = dtoData.IsActive,
                    CompanyId = _LoggedIn_CompanyId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),

                };
                await _context.Deals.AddAsync(ObjDeal);
                await _context.SaveChangesAsync();

                // Add Deal Sections
                if(dtoData.DealSectionsStr != null && dtoData.DealSectionsStr.Length > 0)
                {
                    string[] dealsectionstring= dtoData.DealSectionsStr.Split(";");
                    for(int j=0;j< dealsectionstring.Length;j++)
                    {
                        AddDealSectionDto obj = new AddDealSectionDto();
                        
                        string[] dealsectionlist = dealsectionstring[j].Split(new char[] { '|' }, 2);
                        if (dealsectionlist.Length > 0)
                        {
                            string[] objSection = dealsectionlist[0].Split("=");
                            string[] objDetail = dealsectionlist[1].Split("=");
                            if (objSection.Length > 0)
                            {
                                string[] splitObjDetail = objSection[1].Split(",");
                                string[] splitObjSectionDetail = objDetail[1].Split("|");
                                if (splitObjDetail.Length > 0)
                                {
                                    for (int i = 0; i < splitObjDetail.Length; i++)
                                    {
                                        string[] splitdealsection = splitObjDetail[i].Split(":");
                                        switch (splitdealsection[0])
                                        {
                                            case "CategoryId":
                                                obj.CategoryId = Convert.ToInt32(splitdealsection[1]);
                                                break;
                                            case "ChooseQuantity":
                                                obj.ChooseQuantity = Convert.ToInt32(splitdealsection[1]);
                                                break;
                                            case "Title":
                                                obj.Title = Convert.ToString(splitdealsection[1]);
                                                break;
                                        }

                                    }
                                    var list = (from it in _context.Items where it.CategoryId == obj.CategoryId select it).ToList();
                                    if (obj.ChooseQuantity == 0)
                                    {
                                        _serviceResponse.Success = false;
                                        _serviceResponse.Message = "Quantity  Cannot be Zero... ";

                                        objTrans.Rollback();
                                        return _serviceResponse;
                                    }
                                    else
                                    {
                                        if (splitObjSectionDetail.Length < obj.ChooseQuantity)
                                        {
                                            _serviceResponse.Success = false;
                                            _serviceResponse.Message = "Items are less then the Quantity Choosen";

                                            objTrans.Rollback();
                                            return _serviceResponse;
                                        }
                                        else
                                        {
                                            var DealSectionToCreate = new DealSection
                                            {
                                                DealId = ObjDeal.Id,
                                                CategoryId = obj.CategoryId,
                                                ChooseQuantity = obj.ChooseQuantity,
                                                Title = obj.Title,
                                                IsActive = true,
                                                CretedById = _LoggedIn_UserID,
                                                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow))
                                            };
                                            await _context.DealSection.AddAsync(DealSectionToCreate);
                                            await _context.SaveChangesAsync();

                                            //Add Deal Section Detail
                                            if(splitObjSectionDetail.Length > 0)
                                            {
                                                for (int k = 0; k < splitObjSectionDetail.Length; k++)
                                                {

                                                    AddDealSectionDetailDto objDealSectionDetail = new AddDealSectionDetailDto();

                                                    string[] spliteddealsectionDetailstring = splitObjSectionDetail[k].Split(",");
                                                    if (spliteddealsectionDetailstring.Length > 0)
                                                    {
                                                        for (int i = 0; i < spliteddealsectionDetailstring.Length; i++)
                                                        {
                                                            string[] objdealsectionDetailstring = spliteddealsectionDetailstring[i].Split(":");
                                                            switch (objdealsectionDetailstring[0])
                                                            {
                                                                case "ItemId":
                                                                    objDealSectionDetail.ItemId = Convert.ToInt32(objdealsectionDetailstring[1]);
                                                                    break;
                                                                case "ItemSizeId":
                                                                    objDealSectionDetail.ItemSizeId = Convert.ToInt32(objdealsectionDetailstring[1]);
                                                                    break;
                                                            }
                                                        }
                                                        var DealSectionDetailToCreate = new DealSectionDetail
                                                        {
                                                            DealSectionId = DealSectionToCreate.Id,
                                                            ItemId = objDealSectionDetail.ItemId,
                                                            ItemSizeid = objDealSectionDetail.ItemSizeId,
                                                            IsActive = true,
                                                            CretedById = _LoggedIn_UserID,
                                                            DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow))

                                                        };

                                                        await _context.DealSectionDetail.AddAsync(DealSectionDetailToCreate);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                _serviceResponse.Success = false;
                                                _serviceResponse.Message = "Please Save Section Detail";

                                                objTrans.Rollback();
                                                return _serviceResponse;
                                            }
                                        }
                                    }
                                        
                                }


                            }
                        }

                    }                   
                }
                objTrans.Commit();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Dto data or deal or deal sections is null or empty.";
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> EditDealData(int id, EditDealDataDto dtoData)
        {
            IDbContextTransaction objTrans = null;
            objTrans = _context.Database.BeginTransaction();
            var objdealData = await _context.Deals.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (objdealData != null)
            {
               // Add Deal
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
                   
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await dtoData.ImageData.CopyToAsync(stream);
                    }
                   
                    objdealData.FileName = dtoData.FileName;
                    objdealData.FilePath = dtoData.FilePath;
                }
                else
                {
                    objdealData.FileName = objdealData.FileName;
                    objdealData.FilePath = objdealData.FilePath;
                }
                objdealData.Title = dtoData.Title;
                objdealData.Description = dtoData.Description;
                objdealData.Price = dtoData.Price;
                objdealData.Percentage = dtoData.Percentage;
                objdealData.DiscountAmount = dtoData.DiscountAmount;               
                objdealData.ActiveQueue = dtoData.ActiveQueue;
                objdealData.IsActive = dtoData.IsActive;
                objdealData.CompanyId = dtoData.CompanyId;
                objdealData.UpdateById = _LoggedIn_UserID;
                objdealData.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                _context.Deals.Update(objdealData);
                await _context.SaveChangesAsync();

                //Add Deal Sections
                if (dtoData.DealSectionsStr != null && dtoData.DealSectionsStr.Length > 0)
                {
                    string[] dealsectionstring = dtoData.DealSectionsStr.Split(";");
                    for (int j = 0; j < dealsectionstring.Length; j++)
                    {
                        EditDealSectionDto obj = new EditDealSectionDto();
                        
                        string[] dealsectionlist = dealsectionstring[j].Split(new char[] { '|' }, 2);
                        if (dealsectionlist.Length > 0)
                        {
                            string[] objSection = dealsectionlist[0].Split("=");
                            string[] objDetail = dealsectionlist[1].Split("=");
                            if (objSection.Length > 0)
                            {
                                string[] splitObjSection = objSection[1].Split(",");
                                string[] splitObjSectionDetail = objDetail[1].Split("|");

                                if (splitObjSection.Length > 0)
                                {
                                    for (int i = 0; i < splitObjSection.Length; i++)
                                    {
                                        string[] splitdealsection = splitObjSection[i].Split(":");
                                        switch (splitdealsection[0])
                                        {
                                            case "Id":
                                                obj.Id = Convert.ToInt32(splitdealsection[1]);
                                                break;
                                            case "CategoryId":
                                                obj.CategoryId = Convert.ToInt32(splitdealsection[1]);
                                                break;
                                            case "ChooseQuantity":
                                                obj.ChooseQuantity = Convert.ToInt32(splitdealsection[1]);
                                                break;
                                            case "Title":
                                                obj.Title = Convert.ToString(splitdealsection[1]);
                                                break;
                                            case "Mode":
                                                obj.Mode = Convert.ToInt32(splitdealsection[1]);
                                                break;
                                        }
                                    }
                                    if (obj.ChooseQuantity == 0)
                                    {
                                        _serviceResponse.Success = false;
                                        _serviceResponse.Message = "Quantity  Cannot be Zero... ";

                                        objTrans.Rollback();
                                        return _serviceResponse;
                                    }
                                    else
                                    {
                                        if (obj.Id > 0)
                                        {
                                            if (splitObjSectionDetail.Length < obj.ChooseQuantity)
                                            {
                                                _serviceResponse.Success = false;
                                                _serviceResponse.Message = "Items are less then the Quantity Choosen";

                                                objTrans.Rollback();
                                                return _serviceResponse;
                                            }
                                            else
                                            {
                                                var objdealSectionData = await _context.DealSection.Where(x => x.DealId == objdealData.Id && x.Id == obj.Id).FirstOrDefaultAsync();
                                                if (obj.Mode == 2)
                                                {
                                                    if (objdealSectionData != null)
                                                    {
                                                        objdealSectionData.DealId = objdealData.Id;
                                                        objdealSectionData.CategoryId = obj.CategoryId;
                                                        objdealSectionData.ChooseQuantity = obj.ChooseQuantity;
                                                        objdealSectionData.Title = obj.Title;
                                                        objdealSectionData.UpdateById = _LoggedIn_UserID;
                                                        objdealSectionData.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                                                        _context.DealSection.Update(objdealSectionData);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                    else
                                                    {
                                                        _serviceResponse.Success = false;
                                                        _serviceResponse.Message = CustomMessage.RecordNotFound;
                                                    }
                                                }
                                                else if (obj.Mode == 3)
                                                {
                                                    if (objdealSectionData != null)
                                                    {
                                                        //objdealSectionData.IsActive = false;
                                                        var objdealSectionDetailData = await _context.DealSectionDetail.Where(x => x.DealSectionId == objdealSectionData.Id).ToListAsync();
                                                        foreach(var itm in objdealSectionDetailData)
                                                        {

                                                            _context.DealSectionDetail.Remove(itm);
                                                            await _context.SaveChangesAsync();
                                                        }
                                                        _context.DealSection.Remove(objdealSectionData);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                    else
                                                    {
                                                        _serviceResponse.Success = false;
                                                        _serviceResponse.Message = CustomMessage.RecordNotFound;
                                                    }
                                                }
                                                int counter = 0;

                                                //Add Deal Section Detail
                                                if (splitObjSectionDetail.Length > 0 && obj.Mode != 3)
                                                {
                                                    for (int k = 0; k < splitObjSectionDetail.Length; k++)
                                                    {
                                                        EditDealSectionDetailDto objDealSectionDetail = new EditDealSectionDetailDto();

                                                        string[] spliteddealsectionDetailstring = splitObjSectionDetail[k].Split(",");
                                                        for (int i = 0; i < spliteddealsectionDetailstring.Length; i++)
                                                        {
                                                            string[] objdealsectionDetailstring = spliteddealsectionDetailstring[i].Split(":");
                                                            switch (objdealsectionDetailstring[0])
                                                            {
                                                                case "Id":
                                                                    objDealSectionDetail.Id = Convert.ToInt32(objdealsectionDetailstring[1]);
                                                                    break;
                                                                case "ItemId":
                                                                    objDealSectionDetail.ItemId = Convert.ToInt32(objdealsectionDetailstring[1]);
                                                                    break;
                                                                case "ItemSizeId":
                                                                    objDealSectionDetail.ItemSizeId = Convert.ToInt32(objdealsectionDetailstring[1]);
                                                                    break;
                                                                case "Mode":
                                                                    objDealSectionDetail.Mode = Convert.ToInt32(objdealsectionDetailstring[1]);
                                                                    break;
                                                            }
                                                        }
                                                        if (objDealSectionDetail.Mode == 3)
                                                        {
                                                            counter++;
                                                        }
                                                        int length = 0;
                                                        if (counter > splitObjSectionDetail.Length)
                                                        {
                                                            length =  counter- splitObjSectionDetail.Length;
                                                        }
                                                        else
                                                        {
                                                            length = splitObjSectionDetail.Length - counter;
                                                        }
                                                        if (length < obj.ChooseQuantity && obj.Mode != 3)
                                                        {
                                                            _serviceResponse.Success = false;
                                                            _serviceResponse.Message = "Items are less then the Quantity Choosen";

                                                            objTrans.Rollback();
                                                            return _serviceResponse;
                                                        }
                                                        if (objDealSectionDetail.Id > 0)
                                                        {
                                                            var objdealSectionDetailData = await _context.DealSectionDetail.Where(x => x.DealSectionId == objdealSectionData.Id && x.Id == objDealSectionDetail.Id).FirstOrDefaultAsync();
                                                            if (objDealSectionDetail.Mode == 2)
                                                            {
                                                                if (objdealSectionDetailData != null)
                                                                {
                                                                    objdealSectionDetailData.DealSectionId = obj.Id;
                                                                    objdealSectionDetailData.ItemId = objDealSectionDetail.ItemId;
                                                                    objdealSectionDetailData.ItemSizeid = objDealSectionDetail.ItemSizeId;
                                                                    objdealSectionDetailData.UpdateById = _LoggedIn_UserID;
                                                                    objdealSectionDetailData.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                                                                    _context.DealSectionDetail.Update(objdealSectionDetailData);
                                                                    await _context.SaveChangesAsync();
                                                                }
                                                                else
                                                                {
                                                                    _serviceResponse.Success = false;
                                                                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                                                                }
                                                            }
                                                            else if (objDealSectionDetail.Mode == 3)
                                                            {
                                                                if (objdealSectionDetailData != null)
                                                                {
                                                                    //objdealSectionDetailData.IsActive = false;

                                                                    _context.DealSectionDetail.Remove(objdealSectionDetailData);
                                                                    await _context.SaveChangesAsync();
                                                                }
                                                                else
                                                                {
                                                                    _serviceResponse.Success = false;
                                                                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            var DealSectionDetailToCreate = new DealSectionDetail
                                                            {
                                                                DealSectionId = obj.Id,
                                                                ItemId = objDealSectionDetail.ItemId,
                                                                ItemSizeid = objDealSectionDetail.ItemSizeId,
                                                                IsActive = true,
                                                                CretedById = _LoggedIn_UserID,
                                                                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow))

                                                            };
                                                            await _context.DealSectionDetail.AddAsync(DealSectionDetailToCreate);
                                                            await _context.SaveChangesAsync();

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    _serviceResponse.Success = false;
                                                    _serviceResponse.Message = "Please Save Section Detail";

                                                    objTrans.Rollback();
                                                    return _serviceResponse;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (splitObjSectionDetail.Length < obj.ChooseQuantity)
                                            {
                                                _serviceResponse.Success = false;
                                                _serviceResponse.Message = "Items are less then the Quantity Choosen";

                                                objTrans.Rollback();
                                                return _serviceResponse;
                                            }
                                            else
                                            {
                                                var DealSectionToCreate = new DealSection
                                                {
                                                    DealId = objdealData.Id,
                                                    CategoryId = obj.CategoryId,
                                                    ChooseQuantity = obj.ChooseQuantity,
                                                    Title = obj.Title,
                                                    IsActive = true,
                                                    CretedById = _LoggedIn_UserID,
                                                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow))

                                                };
                                                await _context.DealSection.AddAsync(DealSectionToCreate);
                                                await _context.SaveChangesAsync();

                                                //Add Deal Section Detail
                                                if (splitObjSectionDetail.Length > 0)
                                                {
                                                    for (int k = 0; k < splitObjSectionDetail.Length; k++)
                                                    {
                                                        EditDealSectionDetailDto objDealSectionDetail = new EditDealSectionDetailDto();
                                                        string[] spliteddealsectionDetailstring = splitObjSectionDetail[k].Split(",");
                                                        if (spliteddealsectionDetailstring.Length > 0)
                                                        {
                                                            for (int i = 0; i < spliteddealsectionDetailstring.Length; i++)
                                                            {
                                                                string[] objdealsectionDetailstring = spliteddealsectionDetailstring[i].Split(":");
                                                                switch (objdealsectionDetailstring[0])
                                                                {
                                                                    case "ItemId":
                                                                        objDealSectionDetail.ItemId = Convert.ToInt32(objdealsectionDetailstring[1]);
                                                                        break;
                                                                    case "ItemSizeId":
                                                                        objDealSectionDetail.ItemSizeId = Convert.ToInt32(objdealsectionDetailstring[1]);
                                                                        break;
                                                                }
                                                            }
                                                            var DealSectionDetailToCreate = new DealSectionDetail
                                                            {
                                                                DealSectionId = DealSectionToCreate.Id,
                                                                ItemId = objDealSectionDetail.ItemId,
                                                                ItemSizeid = objDealSectionDetail.ItemSizeId,
                                                                IsActive = true,
                                                                CretedById = _LoggedIn_UserID,
                                                                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow))

                                                            };
                                                            await _context.DealSectionDetail.AddAsync(DealSectionDetailToCreate);
                                                            await _context.SaveChangesAsync();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    _serviceResponse.Success = false;
                                                    _serviceResponse.Message = "Please Save Section Detail";

                                                    objTrans.Rollback();
                                                    return _serviceResponse;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                objTrans.Commit();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Dto data or deal or deal sections is null or empty.";
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> DeleteDealById(int id)
        {
            var objdeal= await _context.Deals.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (objdeal != null)
            {
                objdeal.IsActive = false;
                _context.Deals.Update(objdeal);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Deleted;
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }


            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetNewDealDetailsById(int id)
        {
            var objdeal = await _context.Deals.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (objdeal != null)
            {
                var data = new GetDealDetailsByIdDto
                {
                    Id = objdeal.Id,
                    CompanyId = objdeal.CompanyId,
                    Title = objdeal.Title,
                    Description = objdeal.Description,
                    Price = objdeal.Price,
                    Percentage = objdeal.Percentage,
                    DiscountAmount = objdeal.DiscountAmount,
                    ActiveQueue = objdeal.ActiveQueue,
                    FileName = objdeal.FileName,
                    FilePath = objdeal.FilePath,
                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + objdeal.FilePath + '/' + objdeal.FileName,
                    IsActive=objdeal.IsActive,
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
                                             IsActive=m.IsActive,
                                             ObjDealSections = (from q in _context.DealSectionDetail
                                                                  where q.DealSectionId == m.Id

                                                                  select new GetAllFlavoursDto
                                                                  {
                                                                      Id = q.Id,
                                                                      ItemName = q.ObjItem.Name,
                                                                      ItemId = q.ItemId,
                                                                      Quantity = 0,
                                                                      IsActive=q.IsActive
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