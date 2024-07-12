using DAL.Interfaces;
using Models;

namespace FullStackTechTest.Models.Admin;

public class SpecialiesDetailsViewModel
{
    public Speciality Speciality { get; set; }
    public bool IsEditing { get; set; }

    public static async Task<SpecialiesDetailsViewModel> CreateAsync(int specialityId, bool isEditing, ISpecialityRepository specialityRepository)
    {
        var model = new SpecialiesDetailsViewModel
        {
            Speciality = await specialityRepository.GetByIdAsync(specialityId),
            IsEditing = isEditing
        };
        return model;
    }
}
