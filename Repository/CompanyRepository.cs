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
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public CompanyRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddCompany(AddCompanyDto dtoData)
        {
            var objUser = await (from u in _context.Company
                                 where u.Name == dtoData.Name.Trim()

                                 select new
                                 {
                                     Name = u.Name,
                                 }).FirstOrDefaultAsync();

            if (objUser != null)
            {
                if (objUser.Name.Length > 0 && dtoData.Name.Trim() == objUser.Name)
                {
                    _serviceResponse.Message = "Company with this Name ALready Exists";
                }
                _serviceResponse.Success = false;
                _serviceResponse.Data = objUser.Name;
            }
            else
            if (dtoData.ImageData != null && dtoData.ImageData.Length > 0)
            {
                var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "CompanyLogoImages");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                var fullPath = Path.Combine(pathToSave);
                dtoData.FilePath = "CompanyLogoImages";
                dtoData.FileName = fileName;
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var filePath = Path.Combine(_HostEnvironment.WebRootPath, "CompanyLogoImages", fileName);
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


                var ObjItem = new Company
                {
                    Name = dtoData.Name,
                    Address= dtoData.Address,
                    ContactPerson = dtoData.ContactPerson,
                    PhoneNumber= dtoData.PhoneNumber,
                    CellNumber= dtoData.CellNumber,
                    SecondaryCellNumber= dtoData.SecondaryCellNumber,
                    SecondaryContactPerson= dtoData.SecondaryContactPerson,
                    FilePath = "CompanyLogoImages",
                    FileName = dtoData.FileName,
                    Longitude= dtoData.Longitude,
                    Latitude= dtoData.Latitude,
                };

                try
                {
                    await _context.Company.AddAsync(ObjItem);
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

        public async Task<ServiceResponse<object>> EditCompany(int id, EditCompanyDto dtoData)
        {
            var objitem = await _context.Company.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objitem != null)
            {
                if (dtoData.ImageData != null && dtoData.ImageData.Length > 0)
                {
                    var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "CompanyLogoImages");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                    var fullPath = Path.Combine(pathToSave);
                    dtoData.FilePath = "CompanyLogoImages";
                    dtoData.FileName = fileName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var filePath = Path.Combine(_HostEnvironment.WebRootPath, "CompanyLogoImages", fileName);
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
                    objitem.Address = dtoData.Address;
                    objitem.ContactPerson = dtoData.ContactPerson;
                    objitem.PhoneNumber = dtoData.PhoneNumber;
                    objitem.CellNumber = dtoData.CellNumber;
                    objitem.SecondaryContactPerson = dtoData.SecondaryContactPerson;
                    objitem.SecondaryCellNumber = dtoData.SecondaryCellNumber;
                    objitem.FilePath = dtoData.FilePath;
                    objitem.FileName = dtoData.FileName;
                    objitem.Longitude = dtoData.Longitude;
                    objitem.Latitude = dtoData.Latitude;

                    _context.Company.Update(objitem);
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

        public async Task<ServiceResponse<object>> GetAllCompany()
        {
            try
            {
                var list = await (from m in _context.Company

                                  select new GetAllCompanyDto
                                  {
                                      Id = m.Id,
                                      Name = m.Name,
                                      Address = m.Address,
                                      ContactPerson = m.ContactPerson,
                                      PhoneNumber= m.PhoneNumber,
                                      CellNumber= m.CellNumber,
                                      SecondaryContactPerson= m.SecondaryContactPerson,
                                      SecondaryCellNumber= m.SecondaryCellNumber,
                                      FileName = m.FileName,
                                      FilePath = m.FilePath,
                                      FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
                                      Longitude= m.Longitude,
                                      Latitude= m.Latitude,
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

        public async Task<ServiceResponse<object>> GetCompanyById(int id)
        {
            var ObjItemDetail = await _context.Company.FirstOrDefaultAsync(x => x.Id == id);
            if (ObjItemDetail != null)
            {
                var data = new GetCompanyByIdDto
                {
                    Id = ObjItemDetail.Id,
                    Name = ObjItemDetail.Name,
                    Address = ObjItemDetail.Address,
                    ContactPerson= ObjItemDetail.ContactPerson,
                    PhoneNumber= ObjItemDetail.PhoneNumber,
                    CellNumber= ObjItemDetail.CellNumber,
                    SecondaryContactPerson= ObjItemDetail.SecondaryContactPerson,
                    SecondaryCellNumber= ObjItemDetail.SecondaryCellNumber,
                    FileName= ObjItemDetail.FileName,
                    FilePath= ObjItemDetail.FilePath,
                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + ObjItemDetail.FilePath + '/' + ObjItemDetail.FileName,
                    Longitude= ObjItemDetail.Longitude,
                    Latitude= ObjItemDetail.Latitude,
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
