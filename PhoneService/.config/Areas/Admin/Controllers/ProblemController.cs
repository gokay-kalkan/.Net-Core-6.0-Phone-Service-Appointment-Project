using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneService.DatabaseContext;
using PhoneService.Entities;
using System.Data;
using System.Drawing.Drawing2D;

namespace PhoneService.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProblemController:Controller
    {
       

        private DataContext context;

        public ProblemController(DataContext context)
        {
            this.context = context;
        }


       public IActionResult Index()
        {
            var problems = context.Problems.Where(x => x.Status == true).ToList();
            return View(problems);
        }
        
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]

        public IActionResult Create(Problem data)
        {

            context.Problems.Add(data);
            data.Status = true;
            context.SaveChanges();

            TempData["Success"] = "Problem the insertion was successful";

            return RedirectToAction("Index");
           

        }

        public IActionResult Update(int id)
        {
            var problem = context.Problems.FirstOrDefault(x => x.Id == id);
            return View(problem);

        }
        [HttpPost]

        public IActionResult Update(Problem data)
        {
            var problem = context.Problems.FirstOrDefault(x => x.Id == data.Id);
            problem.Description = data.Description;
            context.Problems.Update(problem);
        
            context.SaveChanges();

            TempData["Success"] = "the brand update process was successful";

            return RedirectToAction("Index");


        }


        public IActionResult Delete(int id)
        {
            var problems = context.Problems.FirstOrDefault(x => x.Id == id);

            problems.Status = false;

            context.Problems.Update(problems);

            context.SaveChanges();

            TempData["FullDelete"] = "The delete problem process was successful";

            return RedirectToAction("Index");
        }

    }
}
