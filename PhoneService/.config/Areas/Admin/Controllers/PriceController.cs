using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneService.DatabaseContext;
using PhoneService.Dtos.PriceDtos;
using PhoneService.Entities;
using PhoneService.ValidationRules.PriceValidatorRules;

namespace PhoneService.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PriceController : Controller
    {
        private DataContext context;

        public PriceController(DataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var prices = context.Prices.Where(x => x.Status == true).ToList();

            return View(prices);
        }
        public IActionResult DealerGet(int CityId)
        {
            List<Dealer> dealerList = context.Dealers.Where(X => X.Status == true && X.CityId == CityId).ToList();

            ViewBag.dealer = new SelectList(dealerList, "Id", "Name");
            return PartialView("DealerPartial");
        }

        public PartialViewResult DealerPartial()
        {
            return PartialView();
        }
        public IActionResult Create()
        {
            CityDropDown();
            ProblemDropdown();

            return View();
        }

        [HttpPost]
        public IActionResult Create(PriceCreateDto priceCreateDto)
        {
            CityDropDown();

            ProblemDropdown();

            PriceCreateValidator validationRules = new PriceCreateValidator();

            ValidationResult result = validationRules.Validate(priceCreateDto);

            if (result.IsValid)
            {
                Price price = new Price()
                {
                    PriceValue = priceCreateDto.PriceValue,
                    CityId = priceCreateDto.CityId,
                    DealerId = priceCreateDto.DealerId,
                    ProblemId=priceCreateDto.ProblemId
                };

                context.Prices.Add(price);

                price.Status = true;

                context.SaveChanges();

                TempData["Success"] = "Fiyat ekleme işlemi başarıyla gerçekleşti";

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
            var price = context.Prices.FirstOrDefault(x => x.Id == id);

            price.Status = false;

            context.Prices.Update(price);

            context.SaveChanges();

            TempData["FullDelete"] = "Fiyat ekleme işlemi başarıyla gerçekleşti";

            return View();
        }
        public IActionResult Update(int id)
        {
            CityDropDown();
            ProblemDropdown();
            var price = context.Prices.Where(x => x.Id == id).FirstOrDefault();

            PriceUpdateDto priceUpdateDto = new PriceUpdateDto()
            {
                Id = price.Id,
                CityId = price.CityId,
                DealerId = price.DealerId,
                ProblemId = price.ProblemId,
                PriceValue = price.PriceValue,

            };
            return View(priceUpdateDto);

        }

        [HttpPost]

        public IActionResult Update(PriceUpdateDto priceUpdateDto)
        {
            CityDropDown();
            ProblemDropdown();
            PriceUpdateValidator validationRules = new PriceUpdateValidator();

            ValidationResult result = validationRules.Validate(priceUpdateDto);

            if (result.IsValid)
            {
                var price = context.Prices.Where(x => x.Id == priceUpdateDto.Id).FirstOrDefault();

                price.PriceValue = priceUpdateDto.PriceValue;

                price.CityId = priceUpdateDto.CityId;

                price.DealerId = priceUpdateDto.DealerId;
                price.ProblemId = priceUpdateDto.ProblemId;

                context.Prices.Update(price);

                context.SaveChanges();

                TempData["Update"] = "Fiyat güncelleme işlemi başarıyla gerçekleşti";
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

        public void ProblemDropdown()
        {
            List<SelectListItem> value = (from i in context.Problems.Where(x => x.Status == true).ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Description,
                                              Value = i.Id.ToString()
                                          }).ToList();

            ViewBag.problems = value;

        }


    }
}
