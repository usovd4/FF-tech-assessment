using DAL.Interfaces;
using Models;

namespace FullStackTechTest.Models.Home;

public class DetailsViewModel
{
    public Person Person { get; set; }
    public Address Address { get; set; }
    public bool IsEditing { get; set; }
    public List<string> Specialities { get; set; }

    public static async Task<DetailsViewModel> CreateAsync(int personId, bool isEditing, IPersonRepository personRepository, IAddressRepository addressRepository, ISpecialityRepository specialityRepository)
    {
        var model = new DetailsViewModel
        {
            Person = await personRepository.GetByIdAsync(personId),
            Address = await addressRepository.GetForPersonIdAsync(personId),
            Specialities = await specialityRepository.GetForPersonIdAsync(personId),
            IsEditing = isEditing
        };
        return model;
    }
}