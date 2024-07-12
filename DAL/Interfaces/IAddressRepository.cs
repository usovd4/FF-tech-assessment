using Models;

namespace DAL.Interfaces;

public interface IAddressRepository
{
    Task<Address> GetForPersonIdAsync(int personId);
    Task SaveAsync(Address address);
}