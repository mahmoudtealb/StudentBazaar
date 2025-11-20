
namespace StudentBazaar.Web.ViewModels
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; } = new Product();

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> StudyYears { get; set; } = new List<SelectListItem>();

        // الصور المرفوعة من الفورم
        public List<IFormFile>? Files { get; set; }
    }
}
