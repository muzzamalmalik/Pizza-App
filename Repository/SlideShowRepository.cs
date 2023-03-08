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
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace PizzaOrder.Repository
{
    public class SlideShowRepository : BaseRepository, ISlideShowRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public SlideShowRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddSlideShow(SlideShowAddDto dtoData)
        {
            if (dtoData != null)
            {

                var ObjSlideShow = new SlideShow
                {
                    //Id = dtoData.Id,
                    ImageDescription = dtoData.ImageDescription,
                    CompanyId = dtoData.CompanyId,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                };

                await _context.SlideShow.AddAsync(ObjSlideShow);
                await _context.SaveChangesAsync();
                _serviceResponse.Data = ObjSlideShow.Id;



                if (dtoData.ImageData != null)
                {
                    foreach (var item in dtoData.ImageData)
                    {
                        var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "SliderImages");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        var fullPath = Path.Combine(pathToSave);
                        dtoData.FilePath = "SliderImages";
                        dtoData.FileName = fileName;
                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }
                        var filePath = Path.Combine(_HostEnvironment.WebRootPath, "SliderImages", fileName);
                        //string pathString = filePath.LastIndexOf("/") + 1;
                        try
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                await item.CopyToAsync(stream);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }



                        var ObjSlideShowImages = new SlideShowImages
                        {
                            FilePath = "SliderImages",
                            FileName = dtoData.FileName,
                            SlideShowId = ObjSlideShow.Id,
                            CretedById = _LoggedIn_UserID,
                            DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                        };

                        await _context.SlideShowImages.AddAsync(ObjSlideShowImages);
                        await _context.SaveChangesAsync();


                    }
                }

            }
            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditSlideShow(int id, int imageid, SlideShowEditDto dtoData)
        {
            var ObjSlideShow = await _context.SlideShow.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (ObjSlideShow != null)
            {
                //ObjSlideShow.Id = dtoData.Id;
                ObjSlideShow.ImageDescription = dtoData.ImageDescription;
                ObjSlideShow.CompanyId = dtoData.CompanyId;
                ObjSlideShow.UpdateById = _LoggedIn_UserID;
                ObjSlideShow.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));

                _context.SlideShow.Update(ObjSlideShow);
                await _context.SaveChangesAsync();
                _serviceResponse.Data = ObjSlideShow.Id;
            }

            var ObjSlideShowImages = await _context.SlideShowImages.FirstOrDefaultAsync(x => x.Id.Equals(imageid));
            if (ObjSlideShowImages != null)
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

                    ObjSlideShowImages.FilePath = dtoData.FilePath;
                    ObjSlideShowImages.FileName = dtoData.FileName;
                    ObjSlideShowImages.SlideShowId = dtoData.SlideShowId;
                    ObjSlideShowImages.UpdateById = _LoggedIn_UserID;
                    ObjSlideShowImages.DateModified = DateTime.UtcNow;

                    _context.SlideShowImages.Update(ObjSlideShowImages);
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

        public async Task<ServiceResponse<object>> GetAllSlideShows(int CompanyId)
        {
            var list = await (from m in _context.SlideShow
                              join p in _context.SlideShowImages
                              on m.Id equals p.SlideShowId
                              where m.CompanyId == CompanyId


                              select new GetAllSlideShowDto
                              {
                                  //Id = m.Id,
                                  ImageDescription = m.ImageDescription,
                                  CompanyId = m.CompanyId,
                                  FileName = p.FileName,
                                  FilePath = p.FilePath,
                                  FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + p.FilePath + '/' + p.FileName,

                                  //objGetAllSlideShowDataDto = _context.SlideShowImages.Where(o => o.SlideShowId == m.Id).Select(p => new GetAllSlideShowDataDto
                                  //{
                                  //    //IdImages = p.Id,
                                  //    //HeaderSliderId = m.Id,
                                  //    FileName = p.FileName,
                                  //    FilePath = p.FilePath,
                                  //    FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + p.FilePath + '/' + p.FileName,

                                  //}).ToList()

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
        public async Task<ServiceResponse<object>> AddFeaturedAds(AddFeaturedAdsDto dtoData)
        {


            if (dtoData != null)
            {
                var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "FeaturedImage");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                var fullPath = Path.Combine(pathToSave);
                dtoData.FilePath = "FeaturedImage";
                dtoData.FileName = fileName;
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var filePath = Path.Combine(_HostEnvironment.WebRootPath, "FeaturedImage", fileName);
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

                var featuredAd = new FeaturedAds
                {
                    CompanyId = dtoData.CompanyId,
                    StartDate = dtoData.StartDate,
                    EndDate = dtoData.EndDate,
                    CretedById = _LoggedIn_UserID,
                    DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
                    IsActivated = dtoData.IsActivated,
                    FilePath = "FeaturedImage",
                    FileName = dtoData.FileName,
                };

                await _context.FeaturedAds.AddAsync(featuredAd);
                await _context.SaveChangesAsync();
                _serviceResponse.Message = CustomMessage.Added;
                _serviceResponse.Success = true;

            }

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditFeaturedAds(int id, EditFeaturedAdsDto dtoData)
        {
            if (dtoData != null)
            {
                var featuredAd = await _context.FeaturedAds.FirstOrDefaultAsync(x => x.Id == id);
                if (featuredAd != null)
                {
                    if (dtoData.ImageData != null)
                    {
                        var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "FeaturedImage");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                        var fullPath = Path.Combine(pathToSave);
                        dtoData.FilePath = "FeaturedImage";
                        dtoData.FileName = fileName;
                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }
                        var filePath = Path.Combine(_HostEnvironment.WebRootPath, "FeaturedImage", fileName);
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
                        featuredAd.FilePath = "FeaturedImage";
                        featuredAd.FileName = dtoData.FileName;
                    }
                    //featuredAd.CompanyId = dtoData.CompanyId;
                    featuredAd.StartDate = dtoData.StartDate;
                    featuredAd.EndDate = dtoData.EndDate;
                    //featuredAd.ModifiedById = _LoggedIn_UserID;
                    featuredAd.DateModified = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow));
                    featuredAd.IsActivated = dtoData.IsActivated;

                    _context.FeaturedAds.Update(featuredAd);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Message = CustomMessage.Updated;
                    _serviceResponse.Success = true;
                }
                else
                {
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                    _serviceResponse.Success = false;
                }
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
        public async Task<ServiceResponse<object>> GetAllFeaturedAds(double Lat, double Long, int Range)
        {
            DateTime SDate;
            DateTime EDate;
            SDate = DateTime.Now;
            EDate = DateTime.Now;
            List<GetAllFeaturedAds> Caldistance = new List<GetAllFeaturedAds>();
            var query = (from c in _context.Company
                         select c).ToList();
            foreach (var place in query)
            {
                double distance = Distance(Range, Lat, Long, place.Latitude, place.Longitude);
                if (distance < Range)
                {
                    var list = await (from m in _context.FeaturedAds
                                      where
                                            m.IsActivated == true
                                            && m.CompanyId == place.Id
                                            && (m.StartDate.Date <= SDate.Date
                                            && m.EndDate.Date >= SDate.Date)

                                      select new GetAllFeaturedAds
                                      {
                                          CompanyId = m.CompanyId,
                                          FullPath = _configuration.GetSection("AppSettings:SiteUrl").Value + m.FilePath + '/' + m.FileName,
                                          StartDate= m.StartDate,
                                          EndDate= m.EndDate,

                                      }).ToListAsync();
                    Caldistance.AddRange(list);
                }
            }
            if (Caldistance.Count > 0)
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
}