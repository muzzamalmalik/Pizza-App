using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PizzaOrder.Context;
using PizzaOrder.Dtos;
using PizzaOrder.Helpers;
using PizzaOrder.IRepository;
using PizzaOrder.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace PizzaOrder.Repository
{
    public class ContactUsRepository : BaseRepository, IContactUsRepository
    {
        protected readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public ContactUsRepository(DataContext context, Microsoft.Extensions.Configuration.IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }
        public async Task<ServiceResponse<object>> AddContactPersonInfo(AddContactUsDto dtoData)
        {
            var ObjItem = new ContactUs
            {
                Name = dtoData.Name,
                ContactNo = dtoData.ContactNo,
                Subject = dtoData.Subject,
                Message = dtoData.Message,
                CompanyId = dtoData.CompanyId,
                CretedById = _LoggedIn_UserID,
                DateCreated = Convert.ToDateTime(Helpers.HelperFunctions.ToDateTime(DateTime.UtcNow)),
            };

            try
            {
                await _context.ContactUs.AddAsync(ObjItem);
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
        public async Task<ServiceResponse<object>> GetContactPersonInfoList(int CompanyId)
        {
            var list = await (from m in _context.ContactUs
                              where m.CompanyId == CompanyId
                              orderby m.Id descending
                              select new GetAllContactPersonInfoDto
                              {
                                 Id=m.Id,
                                 Name=m.Name,
                                 ContactNo=m.ContactNo,
                                 Subject=m.Subject,
                                 Message = m.Message

                              }).ToListAsync();
            
            if (list.Count > 0)
            {
                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                _serviceResponse.Message = "Record Found";
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Data = null;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }

            return _serviceResponse;
        }

    }
}
