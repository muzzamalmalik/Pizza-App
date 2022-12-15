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
using System.IO;
using System.Linq;
using System.Threading.Tasks;


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
                            CompanyId = dtoData.CompanyId,
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

        public async Task<ServiceResponse<object>> EditSlideShow(int id,int imageid, SlideShowEditDto dtoData)
        {
            var ObjSlideShow = await _context.SlideShow.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (ObjSlideShow !=  null)
            {
                //ObjSlideShow.Id = dtoData.Id;
                ObjSlideShow.ImageDescription = dtoData.ImageDescription;
                ObjSlideShow.CompanyId = dtoData.CompanyId;

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
                    ObjSlideShowImages.CompanyId = dtoData.CompanyId;
                    ObjSlideShowImages.SlideShowId = dtoData.SlideShowId;

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
    }
}
