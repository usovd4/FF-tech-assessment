using DAL;
using DAL.Interfaces;
using FullStackTechTest.Models.Admin;
using FullStackTechTest.Models.Home;
using FullStackTechTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace FullStackTechTest.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISpecialityRepository _specialityRepository;

        public AdminController(ILogger<HomeController> logger, ISpecialityRepository specialityRepository)
        {
            _logger = logger;
            _specialityRepository = specialityRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await SpecialitiesViewModel.CreateAsync(_specialityRepository);
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var model = await SpecialiesDetailsViewModel.CreateAsync(id, false, _specialityRepository);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _specialityRepository.DeleteByIdAsync(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await SpecialiesDetailsViewModel.CreateAsync(id, true, _specialityRepository);
            return View("Details", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] SpecialiesDetailsViewModel model)
        {
            await _specialityRepository.SaveAsync(model.Speciality);
            if(id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details", new { id = model.Speciality.Id });
            }

        }

    }
}
