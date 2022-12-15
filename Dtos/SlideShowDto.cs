using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PizzaOrder.Dtos
{
    public class SlideShowDto
    {
    }

    public class SlideShowAddDto
    {
        public string ImageDescription { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int CompanyId { get; set; }
        public List<IFormFile> ImageData { get; set; }
    }

    public class SlideShowEditDto
    {
        public string ImageDescription { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int CompanyId { get; set; }
        public int SlideShowId { get; set; }
        public IFormFile ImageData { get; set; }

    }
    public class GetAllSlideShowDto
    {
        public string ImageDescription { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public int CompanyId { get; set; }
        //public List<GetAllSlideShowDataDto> objGetAllSlideShowDataDto { get; set; } = new();

    }

    //public class GetAllSlideShowDataDto
    //{
    //    public string FileName { get; set; }
    //    public string FilePath { get; set; }
    //    public string FullPath { get; set; }
    //}

}
