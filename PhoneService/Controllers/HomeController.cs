using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneService.DatabaseContext;
using PhoneService.Entities;
using PhoneService.Models;
using System.Diagnostics;

namespace PhoneService.Controllers
{
    public class HomeController : Controller
    {

        private DataContext context;

        public HomeController(DataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            CityDropDown(); BrandDropDown();
            return View();
        }
         
        public IActionResult Index2()
        {
            CityDropDown(); BrandDropDown();
            return View();
        }


        public IActionResult DealerGet(int CityId)
        {
            List<Dealer> dealerList = context.Dealers.Where(X => X.Status == true && X.CityId == CityId).ToList();

            ViewBag.dealer = new SelectList(dealerList, "Id", "Name");
            return PartialView("DealerPartial");
        }

        public IActionResult ModelGet(int ModelId)
        {
            List<Model> modelList = context.Models.Where(X => X.Status == true && X.Id == ModelId).ToList();

            ViewBag.model = new SelectList(modelList, "Id", "Name");
            return PartialView("ModelPartial");
        }

        public PartialViewResult DealerPartial()
        {
            return PartialView();
        }

        public PartialViewResult ModelPartial()
        {
            return PartialView();
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

        public IActionResult Deneme()
        {
            return View();
        }

     

      

    }

}
