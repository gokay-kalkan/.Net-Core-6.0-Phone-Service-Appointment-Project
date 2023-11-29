using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneService.DatabaseContext;
using PhoneService.Dtos.DealerDtos;
using PhoneService.Entities;
using PhoneService.ValidationRules.CityValidatorRules;
using PhoneService.ValidationRules.DealerValidationRules;

namespace PhoneService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DealerController : Controller
    {
        private DataContext context;

        public DealerController(DataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var dealers = context.Dealers.Where(x => x.Status == true).ToList();

            return View(dealers);
        }

        public IActionResult Create()
        {
            CityDropDown();
            return View();

        }
        [HttpPost]
        public IActionResult Create(DealerCreateDto dealerCreateDto)
        {

            CityDropDown();

            DealerCreateValidator validationRules = new DealerCreateValidator();

            ValidationResult result = validationRules.Validate(dealerCreateDto);


            if (result.IsValid)
            {
                Dealer dealer = new Dealer()
                {
                    Name = dealerCreateDto.Name,
                    CityId = dealerCreateDto.CityId
                };

                context.Dealers.Add(dealer);

                dealer.Status = true;

                context.SaveChanges();

                TempData["Success"] = "Dealer the insertion was successful";

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
            var dealer = context.Dealers.FirstOrDefault(x => x.Id == id);

            dealer.Status = false;

            context.Dealers.Update(dealer);

            context.SaveChanges();

            TempData["FullDelete"] = "The delete dealer process was successful";

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            CityDropDown();
            var dealer = context.Dealers.Where(x => x.Id == id).FirstOrDefault();
            DealerUpdateDto dealerUpdateDto = new DealerUpdateDto()
            {
                CityId = dealer.CityId,
                Name = dealer.Name,
                Id = dealer.Id
            };
            return View(dealerUpdateDto);
        }

        [HttpPost]
        public IActionResult Update(DealerUpdateDto dealerUpdateDto)
        {
            CityDropDown();

            DealerUpdateValidator validationRules = new DealerUpdateValidator();

            ValidationResult result = validationRules.Validate(dealerUpdateDto);

            if (result.IsValid)
            {
                var dealer = context.Dealers.Where(x => x.Id == dealerUpdateDto.Id).FirstOrDefault();

                dealer.Name = dealerUpdateDto.Name;

                dealer.CityId = dealerUpdateDto.CityId;

                context.Dealers.Update(dealer);

                context.SaveChanges();

                TempData["Update"] = "the dealer update process was successful";

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


        public void CityDropDown()
        {
            List<SelectListItem> value = (from i in context.Cities.Where(x => x.Status == true).ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Name,
                                              Value = i.Id.ToString()
                                          }).ToList();

            ViewBag.cities = value;

        }
    }
}
