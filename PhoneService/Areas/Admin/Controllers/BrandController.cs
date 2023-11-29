using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PhoneService.DatabaseContext;
using PhoneService.Dtos.BrandDtos;
using PhoneService.Entities;
using PhoneService.ValidationRules.BrandValidatorRules;
using PhoneService.ValidationRules.CityValidatorRules;

namespace PhoneService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private DataContext context;

        public BrandController(DataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var brandlist = context.Brands.Where(x=>x.Status==true).ToList();
            return View(brandlist);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(BrandCreateDto brandCreateDto)
        {
            BrandCreateValidator validationRules = new BrandCreateValidator();

            ValidationResult result = validationRules.Validate(brandCreateDto);
            if (result.IsValid)
            {
                Brand brand = new Brand()
                {
                    Name = brandCreateDto.Name
                };

                context.Brands.Add(brand);
                brand.Status = true;
                context.SaveChanges();

                TempData["Success"] = "Brand the insertion was successful";

                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                }
            }
            return View();
        }


        public IActionResult Delete(int id)
        {
            var brands = context.Brands.FirstOrDefault(x => x.Id == id);

            brands.Status = false;

            context.Brands.Update(brands);

            context.SaveChanges();

            TempData["FullDelete"] = "The delete branding process was successful";

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var brands = context.Brands.Where(x => x.Id == id).FirstOrDefault();
            BrandUpdateDto brandUpdateDto = new BrandUpdateDto()
            {
                Id = brands.Id,
                Name = brands.Name
            };
            return View(brandUpdateDto);
        }

        [HttpPost]
        public IActionResult Update(BrandUpdateDto brandUpdateDto)
        {
            var brand = context.Brands.FirstOrDefault(x => x.Id == brandUpdateDto.Id);

            brand.Id = brandUpdateDto.Id;

            brand.Name = brandUpdateDto.Name;

            context.Brands.Update(brand);

            context.SaveChanges();

            TempData["Update"] = "the brand update process was successful";

            return RedirectToAction("Index");
        }
    }
}
