using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PizzaOrder.Context;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using PizzaOrder.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PizzaOrder.Repository
{
    public class ItemRepository : BaseRepository, IItemRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public ItemRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }
        public async Task<ServiceResponse<object>> AddItem(AddItemDto dtoData)
        {
            if (dtoData != null)
            {
                if (dtoData.ImageData != null && dtoData.ImageData.Length > 0)
                {
                    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "ItemImages");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                    var fullPath = Path.Combine(pathToSave);
                    dtoData.FilePath = "ItemImages";
                    dtoData.FileName = fileName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "ItemImages", fileName);
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
                }

                    var ObjItem = new Item
                    {
                        Name = dtoData.Name,
                        Description = dtoData.Description,
                        FilePath = "ItemImages",
                        FileName = dtoData.FileName,
                        Sku = dtoData.Sku,
                        CategoryId = dtoData.CategoryId,
                        CompanyId = _LoggedIn_CompanyId,
                        ActiveQueue = dtoData.ActiveQueue,
                        //ItemSizeId = dtoData.ItemSizeId,
                        Price = dtoData.Price??0,
                        IsActive= dtoData.IsActive,
                        CretedById = _LoggedIn_UserID,
                        DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                    };
                        await _context.Items.AddAsync(ObjItem);
                        await _context.SaveChangesAsync();

                        _serviceResponse.Success = true;
                        _serviceResponse.Message = CustomMessage.Added;

                    if (dtoData.ObjItemSizeStr != null)
                    {
                        List<ItemSizeDto> ObjItemSize = JsonConvert.DeserializeObject<List<ItemSizeDto>>(dtoData.ObjItemSizeStr);
                        foreach (var itm in ObjItemSize)
                        {
                          if(dtoData.Price==null|| dtoData.Price == 0)
                          {
                            var ItemSizeToCreate = new ItemSize
                            {
                                SizeDescription = itm.SizeDescription,
                                Price = itm.Price!=null? itm.Price:0,
                                ItemId = ObjItem.Id,
                                IsActive = true,
                                //CompanyId = Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value),
                                CompanyId = _LoggedIn_CompanyId,
                                CretedById = _LoggedIn_UserID,
                                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                            };

                            await _context.ItemSize.AddAsync(ItemSizeToCreate);
                            await _context.SaveChangesAsync();
                          }
                           
                        }

                    }
            }

            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> EditItem(int id, EditItemDto dtoData)
        {
            var objitem = await _context.Items.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (dtoData.ImageData != null && dtoData.ImageData.Length > 0)
            {
                // If image data is present, save it
                var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "ItemImages");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                var fullPath = Path.Combine(pathToSave);
                dtoData.FilePath = "ItemImages";
                dtoData.FileName = fileName;
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var filePath = Path.Combine(_HostEnvironment.WebRootPath, "ItemImages", fileName);
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

                // Update item fields including image
                objitem.Name = dtoData.Name;
                objitem.Description = dtoData.Description;
                objitem.FilePath = dtoData.FilePath;
                objitem.FileName = dtoData.FileName;
                objitem.Sku = dtoData.Sku;
                objitem.ActiveQueue = dtoData.ActiveQueue;
                objitem.CategoryId = dtoData.CategoryId;
                objitem.ItemSizeId = dtoData.ItemSizeId;
                objitem.Price = dtoData.Price;
                objitem.CompanyId = dtoData.CompanyId;
                objitem.IsActive=dtoData.IsActive;
                objitem.UpdateById = _LoggedIn_UserID;
                objitem.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));
            }
            else
            {
                // If image data is null, update other fields
                objitem.Name = dtoData.Name;
                objitem.Description = dtoData.Description;
                objitem.Sku = dtoData.Sku;
                objitem.ActiveQueue = dtoData.ActiveQueue;
                objitem.CategoryId = dtoData.CategoryId;
                objitem.ItemSizeId = dtoData.ItemSizeId;
                objitem.Price = dtoData.Price;
                objitem.CompanyId = dtoData.CompanyId;
                objitem.IsActive = dtoData.IsActive;
                objitem.UpdateById = _LoggedIn_UserID;
                objitem.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));
            }

            // Save changes to database
            _context.Items.Update(objitem);
            await _context.SaveChangesAsync();

            if (dtoData.ObjItemSizeStr != null)
            {
                List<EditItemSizeDto> ObjItemSize = JsonConvert.DeserializeObject<List<EditItemSizeDto>>(dtoData.ObjItemSizeStr);
                foreach (var itm in ObjItemSize)
                {
                    if (dtoData.Price == null || dtoData.Price == 0)
                    {
                        if(itm.Id>0)
                        {
                            var objitemsize = await _context.ItemSize.FirstOrDefaultAsync(x => x.Id.Equals(itm.Id));
                            if (objitemsize != null)
                            {
                                objitemsize.SizeDescription = itm.SizeDescription;
                                objitemsize.Price = itm.Price != null ? itm.Price : 0;
                                objitemsize.ItemId = itm.ItemId;
                                objitemsize.CompanyId = itm.CompanyId;
                                objitemsize.IsActive = true;
                                objitemsize.UpdateById = _LoggedIn_UserID;
                                objitemsize.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                                _context.ItemSize.Update(objitemsize);
                                await _context.SaveChangesAsync();
                                _serviceResponse.Success = true;
                                _serviceResponse.Message = CustomMessage.Updated;
                            }
                            else
                            {
                                _serviceResponse.Success = false;
                                _serviceResponse.Message = CustomMessage.RecordNotFound;
                            }
                        }
                        else
                        {
                            var ItemSizeToCreate = new ItemSize
                            {
                                SizeDescription = itm.SizeDescription,
                                Price = itm.Price != null ? itm.Price : 0,
                                ItemId = objitem.Id,
                                IsActive = true,
                                //CompanyId = Convert.ToInt32(_configuration.GetSection("AppSettings:CompanyId").Value),
                                CompanyId = _LoggedIn_CompanyId,
                                CretedById = _LoggedIn_UserID,
                                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                            };

                            await _context.ItemSize.AddAsync(ItemSizeToCreate);
                            await _context.SaveChangesAsync();
                        }
                       
                    }               
                }

            }


            // Set response message
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;

            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetAllItem(int Categoryid, int page, int pageSize)
        {
            var listCount = _context.Items.Where(x => x.CategoryId.Equals(Categoryid)).Select(p => p.Id).Count();

            //  var pageSize = 6; // set your page size, which is number of records per page

            //var page = 1; // set current page number, must be >= 1 (ideally this value will be passed to this logic/function from outside)

            var skip = pageSize * (page - 1);
            //var canPage = skip < listCount;

            var list = await (from i in _context.Items
                              let data = (from idtl in _context.ItemSize
                                          where idtl.ItemId == i.Id
                                          select idtl).FirstOrDefault()
                              where Categoryid == i.CategoryId
                              orderby i.Id descending
                              select new GetAllItemDto
                              {
                                  Id = i.Id,
                                  CompanyId = i.CompanyId,
                                  ItemName = i.Name,
                                  ItemDescription = i.Description,
                                  Price = data.Price>0 ? data.Price :i.Price ,
                                  ItemSize = data != null ? data.SizeDescription : "",
                                  //CategoryId = m.CategoryId,
                                  //Sku = m.Sku,
                                  //ActiveQueue = m.ActiveQueue,
                                  FileName = i.FileName,
                                  FilePath = i.FilePath,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + i.FilePath + '/' + i.FileName,
                                  IsActive =i.IsActive,
                                  //CreatedById = m.CretedById,
                                  //DateCreated = m.DateCreated,
                                  //UpdatedById = m.UpdateById,
                                  //DateModified = m.DateModified,
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
        public async Task<ServiceResponse<object>> GetItemDetailsById(int id)
        {
            var list =await (from itm in _context.Items
                        where itm.Id== id
            select new GetItemDetailsByIdDto
            {
                Id = itm.Id,
                Name = itm.Name,
                Description = itm.Description,
                CategoryId = itm.CategoryId,
                CategoryName = itm.ObjCategory.Name,
                IsActive=itm.IsActive,
                CompanyId=itm.CompanyId,
                Sku = itm.Sku,
                FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + itm.FilePath + '/' + itm.FileName,
                Price=itm.Price,
                objGetAllItemSize = (from o in _context.ItemSize
                                        where o.ItemId == itm.Id

                                        select new GetAllItemSizeDto
                                        {
                                            Id = o.Id,
                                            SizeDescription = o.SizeDescription,
                                            Price = o.Price,
                                            ItemId = o.ItemId,
                                            CompanyId = o.CompanyId,
                                            DateCreated = o.DateCreated,
                                        }).ToList(),

                objGetAllCrust = (from p in _context.Crusts
                                    where itm.CategoryId==40

                                    select new GetAllCrustDto
                                    {
                                        Id = p.Id,
                                        Name = p.Name,
                                        Description = p.Description,
                                        Price = p.Price,
                                        //CategoryId = p.CategoryId,
                                        //ItemId = p.ItemId,
                                        ItemSizeId = p.ItemSizeId,
                                        CompanyId = p.CompanyId,
                                        DateCreated = p.DateCreated,

                                    }).ToList(),

                objGetAllTopping = (from q in _context.Toppings
                                    //join itmsi in _context.ItemSize on q.ItemSizeId equals itmsi.Id
                                    where itm.CategoryId==40

                                    select new GetAllToppingDto
                                    {
                                        Id = q.Id,
                                        Name = q.Name,
                                        Price = q.Price,
                                        //CategoryId = q.CategoryId,
                                        ItemId = q.ItemId,
                                        ItemSizeId = q.ItemSizeId,
                                        CompanyId = q.CompanyId,
                                        ItemSizeName = ((Helpers.Enums.ItemSize)q.ItemSizeId).ToString(),
                                        DateCreated = q.DateCreated,
                                    }).ToList(),

                CreatedById = itm.CretedById,
                DateCreated = itm.DateCreated,
                UpdatedById = itm.UpdateById,
                DateModified = itm.DateModified
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
        public async Task<ServiceResponse<object>> GetAllItemByWord(GetItemsBySearchFields dtoData)
        {

            if (dtoData.MaxPrice != null && dtoData.MinPrice == null)
            {
                dtoData.MinPrice = 0;
            }

            var categoryList = dtoData.CategoryId.Split(',').Select(Int32.Parse).ToList();

            var query = from isize in _context.ItemSize
                        join it in _context.Items
                        on isize.ItemId equals it.Id into grp
                        from i in grp.DefaultIfEmpty()
                        group i by new { isize.ItemId, i.CategoryId } into g
                        where g.Count() > 1
                        select new { CategoryId = g.Key.CategoryId };

            var result = query.ToList();


            var list = await (from a in _context.Items
                              join b in _context.ItemSize on a.Id equals b.ItemId into newb
                              from b in newb.DefaultIfEmpty()
                              where ((result.Select(x => x.CategoryId).Contains(a.CategoryId) && b.SizeDescription == "Small") || !result.Select(x => x.CategoryId).Contains(a.CategoryId))
                              && (dtoData.MinPrice != null || dtoData.MaxPrice != null ? (b.Price >= dtoData.MinPrice &&
                              (dtoData.MaxPrice == null ? true : b.Price <= dtoData.MaxPrice)) : true)

                              where (a.CompanyId == dtoData.CompanyId && categoryList.Count == 1 && categoryList[0] == 0 ? true : categoryList.Contains(a.CategoryId))
                              && (dtoData.SearchField != null ? a.Name.Contains(dtoData.SearchField) : true)


                              select new GetAllItemDto
                              {
                                  Id = a.Id,
                                  CompanyId = a.CompanyId,
                                  ItemName = a.Name,
                                  ItemDescription = a.Description,
                                  Price = b.Price,
                                  CategoryId = a.CategoryId,
                                  CategoryName = a.ObjCategory.Name,
                                  ItemSize = b.SizeDescription,
                                  //Sku = a.Sku,
                                  //ActiveQueue = a.ActiveQueue,
                                  FileName = a.FileName,
                                  FilePath = a.FilePath,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + a.FilePath + '/' + a.FileName,
                                  IsActive = a.IsActive,
                                  //CreatedById = a.CretedById,
                                  //DateCreated = a.DateCreated,
                                  //UpdatedById = a.UpdateById,
                                  //DateModified = a.DateModified,
                              }).Distinct().ToListAsync();

            if (list.Count > 0)
            {
                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = list.Count + " Record Found";
            }
            else
            {
                _serviceResponse.Data = null;
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }

            return _serviceResponse;
        }

        //public async Task<ServiceResponse<object>> GetAllItemByPriceRange(int? min, int? max )
        //{

        //    var list = await (from a in _context.ItemSize.Where(a =>( a.Price >= min && a.Price <= max) && a.CompanyId == _LoggedIn_CompanyId)

        //                      select new GetAllItemDto
        //                      {
        //                          Id = a.ObjItem.Id,
        //                          //CompanyId = a.CompanyId,
        //                          ItemName = a.ObjItem.Name,
        //                          ItemDescription = a.ObjItem.Description,
        //                          Price = a.Price,
        //                          //CategoryId = a.ObjItem.CategoryId,
        //                          //Sku = a.ObjItem.Sku,
        //                          //ActiveQueue = a.ObjItem.ActiveQueue,
        //                          FileName = a.ObjItem.FileName,
        //                          FilePath = a.ObjItem.FilePath,
        //                          FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + a.ObjItem.FilePath + '/' + a.ObjItem.FileName,
        //                          //CreatedById = a.CretedById,
        //                          //DateCreated = a.DateCreated,
        //                          //UpdatedById = a.UpdateById,
        //                          //DateModified = a.DateModified,
        //                      }).ToListAsync();

        //    if (list.Count > 0)
        //    {
        //        _serviceResponse.Data = list;
        //        _serviceResponse.Success = true;
        //        _serviceResponse.Message = list.Count + " Record Found";
        //    }
        //    else
        //    {
        //        _serviceResponse.Data = null;
        //        _serviceResponse.Success = false;
        //        _serviceResponse.Message = CustomMessage.RecordNotFound;
        //    }

        //    return _serviceResponse;
        //}
        public async Task<ServiceResponse<object>> GetAllItems(int CompanyId)
        {
            var list = await (from i in _context.Items
                              join
                              ic in _context.ItemSize on
                              i.Id equals ic.ItemId

                              where CompanyId == i.CompanyId

                              select new GetAllItemDto
                              {
                                  Id = i.Id,
                                  CompanyId = i.CompanyId,
                                  ItemName = i.Name,
                                  ItemDescription = i.Description,
                                  Price = ic.Price,
                                  ItemSize = ic.SizeDescription,
                                  CategoryId = i.CategoryId,
                                  IsActive = i.IsActive,
                                  //Sku = m.Sku,
                                  //ActiveQueue = m.ActiveQueue,
                                  FileName = i.FileName,
                                  FilePath = i.FilePath,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + i.FilePath + '/' + i.FileName,
                                  //CreatedById = m.CretedById,
                                  //DateCreated = m.DateCreated,
                                  //UpdatedById = m.UpdateById,
                                  //DateModified = m.DateModified,
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
        public async Task<ServiceResponse<object>> GetItemById(int id)
        {
            var ObjItemDetail = await _context.Items.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (ObjItemDetail != null)
            {
                var data = new GetItemByIdDto
                {
                    Id = ObjItemDetail.Id,
                    ItemName = ObjItemDetail.Name,
                    ItemDescription = ObjItemDetail.Description,
                    CategoryId = ObjItemDetail.CategoryId,
                    IsActive = ObjItemDetail.IsActive,
                    CompanyId=ObjItemDetail.CompanyId,
                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + ObjItemDetail.FilePath + '/' + ObjItemDetail.FileName,



                    CreatedById = ObjItemDetail.CretedById,
                    DateCreated = ObjItemDetail.DateCreated,
                    UpdatedById = ObjItemDetail.UpdateById,
                    DateModified = ObjItemDetail.DateModified,
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
        private double Distance(int Range, double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Helpers.HelperFunctions.deg2rad(lat1)) * Math.Sin(Helpers.HelperFunctions.deg2rad(lat2)) + Math.Cos(Helpers.HelperFunctions.deg2rad(lat1)) * Math.Cos(Helpers.HelperFunctions.deg2rad(lat2)) * Math.Cos(Helpers.HelperFunctions.deg2rad(theta));
            dist = Math.Acos(dist);
            dist = Helpers.HelperFunctions.rad2deg(dist);
            dist = (dist * 60 * 1.1515) / 0.6213711922;          //miles to kms
            return (dist);
        }
        public async Task<ServiceResponse<object>> GetItemSearchbylocation(ItemSearchbylocationDto dtoData)
        {

            List<ItemSearchbylocationDto> Caldistance = new List<ItemSearchbylocationDto>();
            List<GetAllItemDto> Result = new List<GetAllItemDto>();
            var query = (from c in _context.Company
                         select c).ToList();
            foreach (var place in query)
            {

                double distance = Distance(dtoData.Range, dtoData.Lat, dtoData.Long, place.Latitude, place.Longitude);
                if (distance < dtoData.Range)
                {

                    if (dtoData.MaxPrice != null && dtoData.MinPrice == null)
                    {
                        dtoData.MinPrice = 0;
                    }

                    var categoryList = dtoData.CategoryId.Split(',').Select(Int32.Parse).ToList();

                    var Search = from isize in _context.ItemSize
                                 join it in _context.Items
                                 on isize.ItemId equals it.Id into grp
                                 from i in grp.DefaultIfEmpty()
                                 group i by new { isize.ItemId, i.CategoryId } into g
                                 where g.Count() > 1
                                 select new { CategoryId = g.Key.CategoryId };

                    var result = Search.ToList();


                    var list = (from a in _context.Items
                                join b in _context.ItemSize on a.Id equals b.ItemId into newb
                                from b in newb.DefaultIfEmpty()
                                where ((result.Select(x => x.CategoryId).Contains(a.CategoryId) && b.SizeDescription == "Small") || !result.Select(x => x.CategoryId).Contains(a.CategoryId))
                                && (dtoData.MinPrice != null || dtoData.MaxPrice != null ? (b.Price >= dtoData.MinPrice &&
                                (dtoData.MaxPrice == null ? true : b.Price <= dtoData.MaxPrice)) : true)

                                where (a.CompanyId == place.Id && categoryList.Count == 1 && categoryList[0] == 0 ? true : categoryList.Contains(a.CategoryId))
                                && (dtoData.SearchField != null ? a.Name.Contains(dtoData.SearchField) : true)


                                select new GetAllItemDto
                                {
                                    Id = a.Id,
                                    CompanyId = a.CompanyId,
                                    ItemName = a.Name,
                                    ItemDescription = a.Description,
                                    Price = b.Price,
                                    CategoryId = a.CategoryId,
                                    CategoryName = a.ObjCategory.Name,
                                    ItemSize = b.SizeDescription,
                                    CompanyName = a.ObjCompany.Name,
                                    //ActiveQueue = a.ActiveQueue,
                                    FileName = a.FileName,
                                    FilePath = a.FilePath,
                                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + a.FilePath + '/' + a.FileName,
                                    //CreatedById = a.CretedById,
                                    //DateCreated = a.DateCreated,
                                    //UpdatedById = a.UpdateById,
                                    //DateModified = a.DateModified,
                                }).Distinct().ToList();

                    if (list.Count > 0)
                    {

                        Result.AddRange(list);
                    }
                    else
                    {
                        _serviceResponse.Data = null;

                    }


                }

            }
            _serviceResponse.Data = Result;
            _serviceResponse.Success = false;
            _serviceResponse.Message = CustomMessage.RecordNotFound;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetItemsByCategoryandPrice(ItemsByCategoryandPriceDto dto)
        {
            if(dto.Id.Count>0)
            {                
                var list = await (from cat in _context.Category
                                  where dto.Id.Contains(0)?true:dto.Id.Contains(cat.Id)
                                  select new GetAllCategoryDto
                                  {
                                      Id = cat.Id,
                                      CategoryName = cat.Name,
                                      CompanyId=cat.CompanyId,
                                      objGetAllItem = (from itm in _context.Items
                                                       let its = (from its in _context.ItemSize
                                                                  where its.ItemId == itm.Id
                                                                  select its.Price).FirstOrDefault()
                                                       where dto.Id.Contains(0) ? true:itm.CategoryId==cat.Id
                                                       &&(dto.MinPrice != 0 || dto.MaxPrice != 0 ? its > 0 ? (its >= dto.MinPrice && its <= dto.MaxPrice):
                                                       (itm.Price >= dto.MinPrice && itm.Price <= dto.MaxPrice) : true)

                                                       select new GetAllItemDto
                                                       {
                                                           Id = itm.Id,
                                                           ItemName = itm.Name,
                                                           ItemDescription = itm.Description,
                                                           CategoryId = itm.CategoryId,
                                                           CategoryName = itm.ObjCategory.Name,
                                                           Price = its>0?its:itm.Price,
                                                           Sku = itm.Sku,
                                                           IsActive=itm.IsActive,
                                                           FileName = itm.FileName,
                                                           FilePath = itm.FilePath,
                                                           FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + itm.FilePath + '/' + itm.FileName,

                                                       }).ToList(),

                                  }).ToListAsync();
                //select new GetAllItemDto
                //                  {
                //                      Id=itm.Id,
                //                      ItemName=itm.Name,
                //                      ItemDescription=itm.Description,
                //                      FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + itm.FilePath + '/' + itm.FileName,
                //                      Price=its,
                //                      CategoryId=itm.CategoryId,
                //                      CategoryName=itm.ObjCategory.Name
                                      
                //                  }).ToListAsync();
                if (list != null)
                {
                    _serviceResponse.Data = list;
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.RecordFound;
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
        public async Task<ServiceResponse<object>> DeleteItemById(int id)
        {
            var objitem = await _context.Items.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (objitem != null)
            {
                objitem.IsActive = false;
                _context.Items.Update(objitem);
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
    }
}
