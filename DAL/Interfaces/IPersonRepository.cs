using Models;

namespace DAL.Interfaces;

public interface IPersonRepository
{
    Task<List<Person>> ListAllAsync();
    Task<Person> GetByIdAsync(int personId);
    Task SaveAsync(Person person);
    Task<bool> UpdatePeopleFromJSON(List<JsonWrapper> peopleList);
}