using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FullStackTechTest.Models.Home;
using FullStackTechTest.Models.Shared;
using FullStackTechTest.Services;
using Models;
using DAL.Interfaces;

namespace FullStackTechTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPersonRepository _personRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly ISpecialityRepository _specialityRepository;
    private readonly ParseJsonService _parseJsonService;

    public HomeController(ILogger<HomeController> logger, IPersonRepository personRepository, IAddressRepository addressRepository, ParseJsonService parseJsonService, ISpecialityRepository specialityRepository)
    {
        _logger = logger;
        _personRepository = personRepository;
        _addressRepository = addressRepository;
        _parseJsonService = parseJsonService;
        _specialityRepository = specialityRepository;
    }


    public async Task<IActionResult> Index()
    {
        var model = await IndexViewModel.CreateAsync(_personRepository);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(IFormFile file)
    {
        try
        {
            List<JsonWrapper> jsonList = await _parseJsonService.UploadFile(file);
            bool updateSuccessfull = await _personRepository.UpdatePeopleFromJSON(jsonList);
            if (updateSuccessfull)
            {
                ViewBag.Message = "JSON uploaded successfully";
            }
            else
            {
                ViewBag.Message = "JSON upload failed";
            }
        }
        catch (Exception ex)
        {
            ViewBag.Message = "JSON upload failed" + ex.Message;
        }

        var model = await IndexViewModel.CreateAsync(_personRepository);
        return View(model);
    }

    //UpdateSpecialities
    [HttpPost]
    public async Task<IActionResult> UpdateSpecialities([FromForm] SpecialitiesEditViewModel model)
    {
        await _specialityRepository.SaveSpecialityAsync(model.PersonId, model.SpecialityCheckboxValues);
        return RedirectToAction("Details", new { id = model.PersonId });
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await DetailsViewModel.CreateAsync(id, false, _personRepository, _addressRepository, _specialityRepository);
        return View(model);
    }

    public async Task<IActionResult> SpecialityCheckboxes(int id)
    {
        var model = await SpecialitiesEditViewModel.CreateAsync(id, _specialityRepository);
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await DetailsViewModel.CreateAsync(id, true, _personRepository, _addressRepository, _specialityRepository);
        return View("Details", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [FromForm] DetailsViewModel model)
    {
        await _personRepository.SaveAsync(model.Person);
        await _addressRepository.SaveAsync(model.Address);
        return RedirectToAction("Details", new { id = model.Person.Id });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}