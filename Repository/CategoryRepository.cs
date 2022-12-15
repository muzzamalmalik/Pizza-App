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
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PizzaOrder.Repository
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public CategoryRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddCategory(AddCategoryDto dtoData)
        {
            var objUser = await (from u in _context.Category
                        where u.Name == dtoData.Name.Trim()
                        && u.CompanyId == _LoggedIn_CompanyId

                        select new
                        {
                            Name = u.Name,
                            CompanyId = _LoggedIn_CompanyId,
                            Description = u.Description,
                        }).FirstOrDefaultAsync();

            if (objUser != null)
            {
                if (objUser.Name.Length > 0 && dtoData.Name.Trim() == objUser.Name)
                {
                    _serviceResponse.Message = "This Name ALready Exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = objUser.Name;
            }
            else
            {
                var CategoryToCreate = new Category
                {
                     Name = dtoData.Name,
                    Description = dtoData.Description,
                    CompanyId = _LoggedIn_CompanyId,
                };

                await _context.Category.AddAsync(CategoryToCreate);
                await _context.SaveChangesAsync();
                //_serviceResponse.Data = CategoryToCreate;
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditCategory(int id,EditCategoryDto dtoData)
        {
            var objcategory = await _context.Category.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objcategory != null)
            {
                objcategory.Name = dtoData.Name;
                objcategory.Description = dtoData.Description;
                objcategory.CompanyId = _LoggedIn_CompanyId;

                _context.Category.Update(objcategory);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllCategories(int? companyId)
        {
            var list = await (from m in _context.Category
                              where m.CompanyId == (_LoggedIn_CompanyId > 0 ? _LoggedIn_CompanyId : companyId) 

                              select new GetAllCategoryDto
                              {
                                  Id = m.Id,
                                  CompanyId = m.CompanyId,
                                  CategoryName = m.Name,
                                  CategoryDescription = m.Description,

                                  //objGetAllItem = (from n in _context.Items where n.CategoryId == m.Id

                                  //                 select new GetAllItemDto
                                  //                 {
                                  //                     Id= n.Id,
                                  //                     ItemName = n.Name,
                                  //                     ItemDescription = n.Description,
                                  //                     CategoryId = n.CategoryId,
                                  //                     Sku = n.Sku,
                                  //                     ActiveQueue = n.ActiveQueue,
                                  //                     FileName = n.FileName,
                                  //                     FilePath = n.FilePath,
                                  //                     FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + n.FilePath + '/' + n.FileName,

                                  //                     //objGetAllItemSize = (from o in _context.ItemSize where o.ItemId == n.Id
                                                                            
                                  //                     //                     select new GetAllItemSizeDto
                                  //                     //                     {
                                  //                     //                         Id= o.Id,
                                  //                     //                         Description= o.Description,
                                  //                     //                         Price= o.Price,
                                  //                     //                         ItemId= o.ItemId,
                                  //                     //                         CompanyId= o.CompanyId,

                                  //                     //                     }).ToList(),

                                                       

                                  //                 }).ToList(),

                              }).ToListAsync();

            if (list.Count > 0)
            {
                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            else
            {
                _serviceResponse.Success= false;
                _serviceResponse.Data = null;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetCategoryById(int id)
        {
            var objcategory = await _context.Category.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (objcategory != null)
            {
                var data = new GetCategoryByIdDto
                {
                    Id = objcategory.Id,
                    Name = objcategory.Name,
                    Description = objcategory.Description,
                    CompanyId = objcategory.CompanyId,
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
