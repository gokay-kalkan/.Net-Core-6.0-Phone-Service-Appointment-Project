using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneService.DatabaseContext;
using PhoneService.Dtos.AppointmentDtos;
using PhoneService.Entities;
using PhoneService.MailService;
using System.Runtime.ConstrainedExecution;
using System;
using Newtonsoft.Json;
using PhoneService.Extensions;
using System.Globalization;

namespace PhoneService.Controllers
{
    public class AppointmentController : Controller
    {
        private DataContext context;
        private IMapper mapper;
        public AppointmentController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //[HttpPost]
        //public IActionResult CreateAppointment(AppointmentCreateDto appointmentCreateDto)
        //{
        //    CityDropDown(); BrandDropDown();

        //    if (appointmentCreateDto != null)
        //    {
        //        Appointment data = mapper.Map<Appointment>(appointmentCreateDto);

        //        data.Dealer = context.Dealers.FirstOrDefault(d => d.Id == data.DealerId);

        //        data.Model = context.Models.FirstOrDefault(m => m.Id == data.ModelId);

        //        context.Appointments.Add(data);
        //        data.Status = true;
        //        data.AppointmentNumber = new Random().Next().ToString();

        //        DateTime appointmentDate = DateTime.Now;


        //        data.AppointmentDate = Convert.ToDateTime(appointmentDate.ToString("dd.MM.yyyy HH:mm:ss"));


        //        var appointmentpricevalue = context.Prices.Where(x => x.DealerId == data.DealerId).FirstOrDefault().PriceValue;

        //        data.Price = appointmentpricevalue;

        //        context.SaveChanges();

        //        string subject = "Telefon Servisi Randevu Oluşturuldu";

        //        string body = $"Randevu numarası: {data.AppointmentNumber}<br> Fiyat: {data.Price}<br> Tarih: {data.AppointmentDate} <br> Şehir: {data.City.Name} <br> Bayi: {data.Dealer.Name}";


        //        MailSender.SendMail(subject, body, data.CustomerEmail);

        //        return RedirectToAction("AppointmentCreated", new { id = data.Id });

        //    }
        //    return View();
        //}

        public IActionResult Step1()
        {
            CityDropDown(); BrandDropDown();

            //şehir marka model gelsin

            return View();
        }

        [HttpPost]

        public IActionResult Step1(AppointmentCreateDto appointmentCreateDto)
        {
            Appointment data = mapper.Map<Appointment>(appointmentCreateDto);
            

            HttpContext.Session.SetObject("Step1Data", appointmentCreateDto);
          
            return RedirectToAction("Step2");
        }

        [HttpPost]
        public IActionResult UpdateSelectedProblemPrice(int selectedProblemId)
        {
            var step1Data = HttpContext.Session.GetObject<AppointmentCreateDto>("Step1Data");
            // Seçilen soruna ait fiyatı veritabanından al
            var selectedProblem = context.Problems.FirstOrDefault(p => p.Id == selectedProblemId);
            var selectedProblemPrice = selectedProblem?.Prices.Where(x=>x.DealerId==step1Data.DealerId).FirstOrDefault()?.PriceValue ?? 0;

            // Fiyatı JSON olarak döndür
            return Json(selectedProblemPrice);
        }
        public IActionResult Step2()
        {
            //sorunlar gelsin

            ProblemDropDown();

            var step1Data = HttpContext.Session.GetObject<AppointmentCreateDto>("Step1Data");

            //var appointmentpricevalue = context.Prices.Where(x => x.DealerId == step1Data.DealerId && x.ProblemId==step1Data.ProblemId).FirstOrDefault().PriceValue;

            //ViewBag.pricevalue = appointmentpricevalue;
          
            return View(step1Data);
        }

        [HttpPost]
        public IActionResult Step2(AppointmentCreateDto appointmentCreateDto, List<int> selectedProblems)
        {
            appointmentCreateDto = HttpContext.Session.GetObject<AppointmentCreateDto>("Step1Data");

            ProblemDropDown();
            if (selectedProblems != null && selectedProblems.Any())
            {
              
                int selectedProblemId = selectedProblems.First();

       
                appointmentCreateDto.ProblemId = selectedProblemId;
            }
           
            HttpContext.Session.SetObject("Step2Data", appointmentCreateDto);
             
            return RedirectToAction("Step3");
        }
        //bu methodda seçilen tarihi alır veri tabanından sorgular ve o tarihe ait tüm uygun saat dilimlerini döndürür seçili saati siler eklemez saat dilimine
        public List<string> GetAvailableTimesForDate(DateTime selectedDate)
        {
           

            var appointments = context.Appointments
       .Where(appointment => appointment.AppointmentDate.Date == selectedDate.Date &&
                            appointment.AppointmentDate >= selectedDate &&
                            appointment.AppointmentDate < selectedDate.AddHours(24))
       .ToList();


            var availableTimes = new List<string>();
            var currentTime = selectedDate.Date.AddHours(0).AddMinutes(0);
            var endTime = selectedDate.Date.AddHours(23).AddMinutes(45);

            while (currentTime <= endTime)
            {
                var timeString = currentTime.ToString("HH:mm");

                
                if (!appointments.Any(appointment => appointment.AppointmentDate.ToString("HH:mm") == timeString))
                {
                    availableTimes.Add(timeString);
                }

                currentTime = currentTime.AddMinutes(15); 
            }

            return availableTimes;
        }

        public bool IsAppointmentDateTimeAvailable(DateTime selectedDateTime)
        {
            
            var availableTimes = GetAvailableTimesForDate(selectedDateTime);

           
            return availableTimes.Contains(selectedDateTime.ToString("HH:mm"));
        }

        [HttpGet] //bu method seçilen tarihe ait randevu saatlerini veritabanından sorgular select ile seçip bunu alır onun dısındakileri ekler
        //ve bunları 15 er dakikalık dilimler ile ekler getavailabletimesfordate ile aynı denilebilir ama bu geriye json döner istemciye iletir
        public IActionResult CheckAvailableTimes(DateTime selectedDate)
        {
            // Veritabanında, seçilen tarih ve saat aralığına uyan randevuları sorgula
            var startTime = selectedDate.Date; // Seçilen günün başlangıcı
            var endTime = selectedDate.Date.AddDays(1); // Seçilen günün sonu (ertesi günün başlangıcı)

            var appointments = context.Appointments
                .Where(appointment => appointment.AppointmentDate >= startTime &&
                                     appointment.AppointmentDate < endTime)
                .ToList();

            // Tüm saatleri temsil eden bir saatler listesi oluştur
            var allTimes = new List<string>();
            var currentTime = startTime;
            while (currentTime < endTime)
            {
                allTimes.Add(currentTime.ToString("HH:mm"));
                currentTime = currentTime.AddMinutes(15); // 15 dakika ekleyin
            }

            // Rezervasyon yapılan saatleri bul
            var reservedTimes = appointments.Select(appointment => appointment.AppointmentDate.ToString("HH:mm")).ToList();

            // Uygun saatleri bul (rezerve edilmemiş saatler)
            var availableTimes = allTimes.Except(reservedTimes).ToList();

            return Json(availableTimes);
        }
        public IActionResult Step3()
        {
            
            var step2Data = HttpContext.Session.GetObject<AppointmentCreateDto>("Step2Data");
            if (step2Data == null)
            {
               
                return RedirectToAction("Step1");
            }

            return View(step2Data);
        }

        [HttpPost]
        public IActionResult Step3(AppointmentCreateDto appointmentCreateDto)
        {
          var appointmentStepData = HttpContext.Session.GetObject<AppointmentCreateDto>("Step2Data");

            appointmentStepData.AppointmentDate = appointmentCreateDto.AppointmentDate;

            appointmentStepData.AppointmentTime = appointmentCreateDto.AppointmentTime;

            DateTime selectedDate = appointmentCreateDto.AppointmentDate;
            string selectedTime = appointmentCreateDto.AppointmentTime;

           
            string dateTimeString = $"{selectedDate.ToString("yyyy-MM-dd")} {selectedTime}";

            if (DateTime.TryParseExact(dateTimeString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime combinedDateTime))
            {

                appointmentStepData.AppointmentDate = combinedDateTime;

                if (IsAppointmentDateTimeAvailable(combinedDateTime))
                {
                    HttpContext.Session.SetObject("Step3Data", appointmentStepData);
                    return RedirectToAction("Step4");
                }
                else
                {
                    ViewBag.message = "Randevu Tarihi müsait değil";
                }
            }

            else
            {
                ViewBag.message = "Geçersiz tarih veya saat seçimi";
            }
            return View();
           
        }
       

        public IActionResult Step4()
        {
          
            var step3Data = HttpContext.Session.GetObject<AppointmentCreateDto>("Step3Data");


            return View(step3Data);
        }

        [HttpPost]
        public IActionResult Step4(AppointmentCreateDto appointmentCreateDto)
        {
         
            var step3Data= HttpContext.Session.GetObject<AppointmentCreateDto>("Step3Data");

            appointmentCreateDto.AppointmentDate = step3Data.AppointmentDate;

            appointmentCreateDto.BrandId = step3Data.BrandId;
            appointmentCreateDto.ModelId = step3Data.ModelId;
            appointmentCreateDto.CityId = step3Data.CityId;
            appointmentCreateDto.DealerId = step3Data.DealerId;
            appointmentCreateDto.ProblemId = step3Data.ProblemId;

            if (step3Data == null)
            {
                return RedirectToAction("Step1");
            }

            if (appointmentCreateDto!=null)
            {
                Appointment data = mapper.Map<Appointment>(appointmentCreateDto);

                data.Dealer = context.Dealers.FirstOrDefault(d => d.Id == data.DealerId);

                data.Model = context.Models.FirstOrDefault(m => m.Id == data.ModelId);
                data.Problem = context.Problems.FirstOrDefault(m => m.Id == data.ProblemId);
                data.City = context.Cities.FirstOrDefault(m => m.Id == data.CityId);
                data.Brand = context.Brands.FirstOrDefault(m => m.Id == data.BrandId);

                data.Status = true;
                data.AppointmentNumber = new Random().Next().ToString();

                var appointmentpricevalue = context.Prices.Where(x => x.DealerId == data.DealerId).FirstOrDefault().PriceValue;

                data.Price = appointmentpricevalue;

                context.Appointments.Add(data);

                context.SaveChanges();

                string subject = "Telefon Servisi Randevu Oluşturuldu";

                string body = $"Randevu numarası: {data.AppointmentNumber}<br> Fiyat: {data.Price}<br> Tarih: {data.AppointmentDate} <br> Şehir: {data.City.Name} <br> Bayi: {data.Dealer.Name}";


                MailSender.SendMail(subject, body, data.CustomerEmail);

                HttpContext.Session.Clear();

                return RedirectToAction("AppointmentCreated", new { id = data.Id });

            }
           
            return View(appointmentCreateDto);
        }

        public IActionResult AppointmentCreated(int id)
        {
            var createdappointment = context.Appointments.FirstOrDefault(x => x.Id == id);

            return View(createdappointment);
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

        public void ProblemDropDown()
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
