using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneService.DatabaseContext;
using PhoneService.Dtos.ModelDtos;
using PhoneService.Entities;
using PhoneService.ValidationRules.CityValidatorRules;
using PhoneService.ValidationRules.ModelValidationRules;
using System.Runtime.Intrinsics.X86;

namespace PhoneService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModelController : Controller
    {
       private DataContext context;

        public ModelController(DataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var modellist = context.Models.ToList();

            return View(modellist);
        }

        public IActionResult Create()
        {
            BrandDropDown();

            return View();
        }

        [HttpPost]
        public IActionResult Create(ModelCreateDto modelCreateDto)
        {
            BrandDropDown();

            ModelCreateValidator validationRules = new ModelCreateValidator();

            ValidationResult result = validationRules.Validate(modelCreateDto);

            if (result.IsValid)
            {
                Model model = new Model()
                {
                    Name = modelCreateDto.Name,
                    BrandId = modelCreateDto.BrandId

                };
                context.Models.Add(model);
                model.Status = true;
                context.SaveChanges();
                TempData["Success"] = "Model ekleme işlemi başarıyla gerçekleşti";

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
            var deletemodel = context.Models.FirstOrDefault(x => x.Id == id);

            deletemodel.Status = false;

            context.Models.Update(deletemodel);

           
            context.SaveChanges();

            TempData["FullDelete"] = "Model silme işlemi başarıyla gerçekleşti";


            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            BrandDropDown();
            var model = context.Models.Where(x => x.Id == id).FirstOrDefault();
            ModelUpdateDto modelUpdateDto = new ModelUpdateDto()
            {
                BrandId = model.BrandId,
                Name = model.Name,
                Id = model.Id
            };
            return View(modelUpdateDto);
        }

        [HttpPost]
        public IActionResult Update(ModelUpdateDto modelUpdateDto)
        {
            BrandDropDown();

            ModelUpdateValdiator validationRules = new ModelUpdateValdiator();

            ValidationResult result = validationRules.Validate(modelUpdateDto);

            if (result.IsValid)
            {

                var model = context.Models.Where(x => x.Id == modelUpdateDto.Id).FirstOrDefault();

                model.Name = modelUpdateDto.Name;
                model.BrandId = modelUpdateDto.BrandId;

                context.Models.Update(model);

                context.SaveChanges();


                TempData["Update"] = "Model güncelleme işlemi başarıyla gerçekleşti";


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

        public void BrandDropDown()
        {
            List<SelectListItem> value = (from i in context.Brands.Where(x => x.Status == true).ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Name,
                                              Value = i.Id.ToString()
                                          }).ToList();

            ViewBag.brands = value;

        }

    }
}
