using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaOrder.Context;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using PizzaOrder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var objUser = await (from u in _context.Items
                                 where u.Name == dtoData.Name.Trim()
                                 && u.CategoryId == dtoData.CategoryId
                                 && u.CompanyId == dtoData.CompanyId

                                 select new
                                 {
                                     Name = u.Name,
                                     CategoryId = u.CategoryId,
                                     CompanyId = u.CompanyId,
                                     Description = u.Description,
                                 }).FirstOrDefaultAsync();

            if (objUser != null)
            {
                if (objUser.Name.Length > 0 && dtoData.Name.Trim() == objUser.Name)
                {
                    _serviceResponse.Message = "Item with this Name ALready Exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = objUser.Name;
            }
            else
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


                var ObjItem = new Item
                {
                        Name = dtoData.Name,
                        Description = dtoData.Description,
                        FilePath = "ItemImages",
                        FileName = dtoData.FileName,
                        Sku = dtoData.Sku,
                        CategoryId = dtoData.CategoryId,
                        CompanyId = dtoData.CompanyId,
                        ActiveQueue = dtoData.ActiveQueue,
                };

                try 
                { 
                    await _context.Items.AddAsync(ObjItem);
                    await _context.SaveChangesAsync();
                    //_serviceResponse.Data = ObjItem;
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

        public async Task<ServiceResponse<object>> EditItem(int id, EditItemDto dtoData)
        {
            var objitem = await _context.Items.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objitem != null)
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

                    objitem.Name = dtoData.Name;
                    objitem.Description = dtoData.Description;
                    objitem.FilePath = dtoData.FilePath;
                    objitem.FileName = dtoData.FileName;
                    objitem.Sku = dtoData.Sku;
                    objitem.ActiveQueue = dtoData.ActiveQueue;
                    objitem.CategoryId = dtoData.CategoryId;
                    objitem.CompanyId = dtoData.CompanyId;

                    _context.Items.Update(objitem);
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

        public async Task<ServiceResponse<object>> GetAllItem(int Categoryid)
        {
            try
            {
                var list = await (from m in _context.Items
                                  where m.CategoryId == Categoryid //&& m.CompanyId == _LoggedIn_CompanyId

                                  select new GetAllItemDto
                                  {
                                      Id = m.Id,
                                      CompanyId = m.CompanyId,
                                      ItemName = m.Name,
                                      ItemDescription = m.Description,
                                      Price =  _context.ItemSize.Where(x => x.ItemId == m.Id).Select(x => x.Price).FirstOrDefault(),
                                      CategoryId = m.CategoryId,
                                      Sku = m.Sku,
                                      ActiveQueue = m.ActiveQueue,
                                      FileName = m.FileName,
                                      FilePath = m.FilePath,
                                      FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetItemDetailsById(int id)
        {
            var ObjItemDetail = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (ObjItemDetail != null)
            {
                var data = new GetItemDetailsByIdDto
                {
                    Id = ObjItemDetail.Id,
                    ItemName = ObjItemDetail.Name,
                    ItemDescription = ObjItemDetail.Description,
                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + ObjItemDetail.FilePath + '/' + ObjItemDetail.FileName,

                    objGetAllItemSize = (from o in _context.ItemSize
                                         where o.ItemId == ObjItemDetail.Id

                                         select new GetAllItemSizeDto
                                         {
                                             Id = o.Id,
                                             SizeDescription = o.SizeDescription,
                                             Price = o.Price,
                                             ItemId = o.ItemId,
                                             CompanyId = o.CompanyId,
                                         }).ToList(),

                    objGetAllCrust = (from p in _context.Crusts
                                      where p.ItemId == ObjItemDetail.Id

                                      select new GetAllCrustDto
                                      {
                                          Id = p.Id,
                                          Name = p.Name,
                                          Description = p.Description,
                                          Price = p.Price,
                                          CategoryId = p.CategoryId,
                                          ItemId = p.ItemId,
                                          ItemSizeId = p.ItemSizeId,
                                          CompanyId = p.CompanyId,

                                      }).ToList(),

                    objGetAllTopping = (from q in _context.Toppings
                                        where q.ItemId == ObjItemDetail.Id

                                        select new GetAllToppingDto
                                        {
                                            Id = q.Id,
                                            Name = q.Name,
                                            Price = q.Price,
                                            CategoryId = q.CategoryId,
                                            ItemId = q.ItemId,
                                            ItemSizeId = q.ItemSizeId,
                                            CompanyId = q.CompanyId,

                                        }).ToList(),
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
