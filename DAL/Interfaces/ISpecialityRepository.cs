using Models;

namespace DAL.Interfaces;

public interface ISpecialityRepository
{
    Task<List<Speciality>> ListAllAsync();
    Task<Speciality> GetByIdAsync(int specialityId);
    Task SaveAsync(Speciality speciality);
    Task DeleteByIdAsync(int specialityId);
    Task<List<string>> GetForPersonIdAsync(int personId);
    Task<List<Checkbox>> GetCheckboxesForPersonIdAsync(int personId);
    Task SaveSpecialityAsync(int personId, List<int> specialityCheckboxValues);
}