using AutoMapper;
using AutoMapper.Configuration;
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
        protected readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public CompanyRepository(DataContext context, Microsoft.Extensions.Configuration.IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddCompany(AddCompanyDto dtoData)
        {
            if (dtoData.ImageData != null)
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
            }

            var ObjItem = new Company
            {
                Name = dtoData.Name,
                Address = dtoData.Address,
                ContactPerson = dtoData.ContactPerson,
                CellNumber = dtoData.CellNumber,
                SecondaryCellNumber = dtoData.SecondaryCellNumber,
                SecondaryContactPerson = dtoData.SecondaryContactPerson,
                UserTypeId = dtoData.UserTypeId,
                FilePath = "CompanyLogoImages",
                FileName = dtoData.FileName,
                Longitude = dtoData.Longitude,
                Latitude = dtoData.Latitude,
                CretedById = _LoggedIn_UserID,
                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
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

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditCompany(int id, EditCompanyDto dtoData)
        {
            var objitem = await _context.Company.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objitem != null)
            {
                if (dtoData.ImageData != null)
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
                }
                try
                {
                    objitem.Name = dtoData.Name;
                    objitem.Address = dtoData.Address.Trim();
                    objitem.ContactPerson = dtoData.ContactPerson;
                    objitem.CellNumber = dtoData.CellNumber;
                    objitem.SecondaryContactPerson = dtoData.SecondaryContactPerson;
                    objitem.SecondaryCellNumber = dtoData.SecondaryCellNumber;
                    objitem.UserTypeId = dtoData.UserTypeId;
                    objitem.FilePath = dtoData.FilePath;
                    objitem.FileName = dtoData.FileName;
                    objitem.Longitude = dtoData.Longitude;
                    objitem.Latitude = dtoData.Latitude;
                    objitem.UpdateById = _LoggedIn_UserID;
                    objitem.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                    _context.Company.Update(objitem);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Updated;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
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
                                      CellNumber = m.CellNumber,
                                      SecondaryContactPerson = m.SecondaryContactPerson,
                                      SecondaryCellNumber = m.SecondaryCellNumber,
                                      UserTypeId = m.UserTypeId,
                                      FileName = m.FileName,
                                      FilePath = m.FilePath,
                                      FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
                                      Longitude = m.Longitude,
                                      Latitude = m.Latitude,
                                      CreatedById = m.CretedById,
                                      DateCreated = m.DateCreated,
                                      UpdatedById = m.UpdateById,
                                      DateModified = m.DateModified,
                                  }).ToListAsync();

                if (list.Count() > 0)
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
                    ContactPerson = ObjItemDetail.ContactPerson,
                    CellNumber = ObjItemDetail.CellNumber,
                    SecondaryContactPerson = ObjItemDetail.SecondaryContactPerson,
                    SecondaryCellNumber = ObjItemDetail.SecondaryCellNumber,
                    //UserId=ObjItemDetail.UserId,
                    UserTypeId = ObjItemDetail.UserTypeId,
                    FileName = ObjItemDetail.FileName,
                    FilePath = ObjItemDetail.FilePath,
                    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + ObjItemDetail.FilePath + '/' + ObjItemDetail.FileName,
                    Longitude = ObjItemDetail.Longitude,
                    Latitude = ObjItemDetail.Latitude,
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


   

        public async Task<ServiceResponse<object>> GetAllCompanyByLatLong(int Range, double Lat, double Long)
        {
            {


                List<GetAllCompanyDto> Caldistance = new List<GetAllCompanyDto>();
                var query = (from c in _context.Company
                             select c).ToList();
                foreach (var place in query)
                {
                    double distance = Distance(Range, Lat, Long, place.Latitude, place.Longitude);
                    if (distance < Range)
                    {
                        var data = new GetAllCompanyDto
                                 {
                                       Id = place.Id,
                                      Name = place.Name,
                                      Address = place.Address,
                                      //ContactPerson = m.ContactPerson,
                                      //CellNumber = m.CellNumber,
                                      //SecondaryContactPerson = m.SecondaryContactPerson,
                                      //SecondaryCellNumber = m.SecondaryCellNumber,
                                      //UserTypeId = m.UserTypeId,
                                      FileName = place.FileName,
                                      FilePath = place.FilePath,
                                      FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + place.FilePath + '/' + place.FileName,
                                      Longitude = place.Longitude,
                                      Latitude = place.Latitude,
                                      //CreatedById = m.CretedById,
                                      //DateCreated = m.DateCreated,
                                      //UpdatedById = m.UpdateById,
                                      //DateModified = m.DateModified,
                            };

                        Caldistance.Add(data);

                    }

                }

                if (Caldistance.Count() > 0)
                {
                    _serviceResponse.Data = Caldistance;
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
        public async Task<ServiceResponse<object>> SearchCompany(string SearchField)
        {
            try
            {
                var list = await (from m in _context.Company where m.Name.Contains(SearchField)

                                  select new GetAllCompanyDto
                                  {
                                      Id = m.Id,
                                      Name = m.Name,
                                      Address = m.Address,
                                      ContactPerson = m.ContactPerson,
                                      CellNumber = m.CellNumber,
                                      SecondaryContactPerson = m.SecondaryContactPerson,
                                      SecondaryCellNumber = m.SecondaryCellNumber,
                                      UserTypeId = m.UserTypeId,
                                      FileName = m.FileName,
                                      FilePath = m.FilePath,
                                      FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
                                      Longitude = m.Longitude,
                                      Latitude = m.Latitude,
                                      CreatedById = m.CretedById,
                                      DateCreated = m.DateCreated,
                                      UpdatedById = m.UpdateById,
                                      DateModified = m.DateModified,
                                  }).ToListAsync();

                if (list.Count() > 0)
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
    }
}
