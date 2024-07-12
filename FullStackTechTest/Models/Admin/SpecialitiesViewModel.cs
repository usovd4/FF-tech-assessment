using DAL.Interfaces;
using Models;

namespace FullStackTechTest.Models.Admin;

public class SpecialitiesViewModel
{
    public List<Speciality> SpecialityList { get; set; }

    public static async Task<SpecialitiesViewModel> CreateAsync(ISpecialityRepository specialityRepository)
    {
        var model = new SpecialitiesViewModel
        {
            SpecialityList = await specialityRepository.ListAllAsync()
        };
        return model;
    }
}
