using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentBazaar.Web.Models;
using System.Collections.Generic;

namespace StudentBazaar.Web.ViewModels
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; } = new Product();

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public List<IFormFile>? Files { get; set; }
    }
}
