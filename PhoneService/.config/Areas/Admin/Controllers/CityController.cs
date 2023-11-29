using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PhoneService.DatabaseContext;
using PhoneService.Dtos.CityDtos;
using PhoneService.Entities;
using PhoneService.ValidationRules.BrandValidatorRules;
using PhoneService.ValidationRules.CityValidatorRules;

using System.Drawing.Drawing2D;

namespace PhoneService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CityController : Controller
    {

        private DataContext context;

        public CityController(DataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {


            var cityList = context.Cities.Where(x => x.Status == true).ToList();
          

            return View(cityList);
        }
        public IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult Create(CityCreateDto createCityDto)
        {
            CityCreateValidator validationRules = new CityCreateValidator();

            ValidationResult result = validationRules.Validate(createCityDto);

           

            if (result.IsValid)
            {
                City city = new City
                {
                    Name = createCityDto.Name,
                  
                };

                context.Cities.Add(city);
                city.Status = true;
                context.SaveChanges();   

                TempData["Success"] = "Şehir ekleme işlemi başarıyla gerçekleşti";

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
            var cities = context.Cities.FirstOrDefault(x => x.Id == id);
            cities.Status = false;
            context.Cities.Update(cities);

            context.SaveChanges();

            TempData["FullDelete"] = "Şehir silme işlemi başarıyla gerçekleşti";

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var cities = context.Cities.Where(x => x.Id == id).FirstOrDefault();
            CityUpdateDto cityUpdateDto = new CityUpdateDto()
            {
                Id = cities.Id,
                Name = cities.Name
            };
            return View(cityUpdateDto);
        }

        [HttpPost]
        public IActionResult Update(CityUpdateDto cityUpdateDto)
        {
            CityUpdateValidator validationRules = new CityUpdateValidator();

            ValidationResult result = validationRules.Validate(cityUpdateDto);

            if (result.IsValid)
            {
                var cities = context.Cities.FirstOrDefault(x => x.Id == cityUpdateDto.Id);

                cities.Id = cityUpdateDto.Id;

                cities.Name = cityUpdateDto.Name;

                context.Cities.Update(cities);

                context.SaveChanges();

                TempData["Update"] = "Şehir güncelleme işlemi başarıyla gerçekleşti";

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
    }
}
