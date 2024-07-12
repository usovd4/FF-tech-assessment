using DAL.Interfaces;
using Models;

namespace FullStackTechTest.Models.Home
{
    public class SpecialitiesEditViewModel
    {
        public int PersonId { get; set; }
        public List<Checkbox> Checkboxes { get; set; }
        public List<int> SpecialityCheckboxValues { get; set; }

        public static async Task<SpecialitiesEditViewModel> CreateAsync(int personId, ISpecialityRepository specialityRepository)
        {
            var model = new SpecialitiesEditViewModel
            {
                PersonId = personId,
                Checkboxes = await specialityRepository.GetCheckboxesForPersonIdAsync(personId)
            };
            return model;
        }
    }
}
