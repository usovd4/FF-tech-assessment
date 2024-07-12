using System.Data;
using System.Text;
using DAL.Data;
using DAL.Interfaces;
using Models;
using MySql.Data.MySqlClient;

namespace DAL;

public class SpecialityRepository : ISpecialityRepository
{
    public async Task<List<Speciality>> ListAllAsync()
    {
        var specialityList = new List<Speciality>();
        try
        {
            await using (AppDB context = new AppDB())
            {
                List<Speciality> dbVal = context.Specialities.ToList();

                if (dbVal != null)
                {
                    specialityList = dbVal;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve all specialities from DB", ex);
        }

        return specialityList;
    }

    public async Task<Speciality> GetByIdAsync(int specialityId)
    {
        var speciality = new Speciality();
        try
        {
            await using (AppDB context = new AppDB())
            {
                //diffirentiate between Edit and Add functionality
                if (specialityId != 0) 
                {
                    //check if the object already exists in db
                    Speciality dbVal = context.Specialities.Where(x => x.Id == specialityId).FirstOrDefault();
                    if (dbVal != null)
                    {
                        speciality = dbVal;
                    }
                }
              
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve speciality for id "+ specialityId +" from DB", ex);
        }

        return speciality;
    }

    public async Task SaveAsync(Speciality speciality)
    {
        try
        {
            await using (AppDB context = new AppDB())
            {
                //check if the object already exists in db
                Speciality dbVal = context.Specialities.Where(x => x.Id == speciality.Id).FirstOrDefault();
                if (dbVal != null)
                {
                    //update
                    dbVal.SpecialityName = speciality.SpecialityName;
                }
                else
                {
                    //create new
                    dbVal = new Speciality();
                    dbVal.SpecialityName = speciality.SpecialityName;
                    context.Specialities.Add(dbVal);
                }
                context.SaveChanges();
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to save speciality to the DB", ex);
        }

    }

    public async Task DeleteByIdAsync(int id)
    {
        try
        {
            await using (AppDB context = new AppDB())
            {
                //check if the object already exists in db
                Speciality dbVal = context.Specialities.Where(x => x.Id == id).FirstOrDefault();
                if (dbVal != null)
                {
                    //delete
                    context.Specialities.Remove(dbVal);
                }
                else
                {
                    //throw an exception
                    throw new Exception("Speciality with id "+ id +" does not exist!");
                }
                context.SaveChanges();
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to save speciality to the DB", ex);
        }
    }

    public async Task<List<string>> GetForPersonIdAsync(int personId)
    {
        List<string> specialityList = new List<string>();
        try
        {
            await using (AppDB context = new AppDB())
            {
                //check if the object already exists in db
                List<string> dbVal = (from t in context.People_Specialities_Map
                                      join t2 in context.Specialities on t.SpecialityId equals t2.Id
                                      where t.PersonId == personId
                                      select t2.SpecialityName).ToList();
                if (dbVal != null)
                {
                    specialityList = dbVal;
                }
                

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve speciality list for id " + personId + " from DB", ex);
        }

        return specialityList;
    }

    public async Task<List<Checkbox>> GetCheckboxesForPersonIdAsync(int personId)
    {
        List<Checkbox> checkboxes = new List<Checkbox>();
        try
        {
            await using (AppDB context = new AppDB())
            {
                //check if the object already exists in db
                List<Checkbox> dbVal = (from t in context.Specialities
                                        from t2 in context.People_Specialities_Map
                                        .Where(t2 => t2.SpecialityId == t.Id && t2.PersonId == personId).DefaultIfEmpty()
                                      select new Checkbox{
                                        Id = t.Id,
                                        Description = t.SpecialityName,
                                        isChecked = t2.Id == null ? false : true

                                      }).ToList();
                if (dbVal != null)
                {
                    checkboxes = dbVal;
                }


            }

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve speciality editing list for id " + personId + " from DB", ex);
        }

        return checkboxes;
    }

    public async Task SaveSpecialityAsync(int personId, List<int> checkboxValues)
    {
        try
        {
            await using (AppDB context = new AppDB())
            {
                List<PersonSpecialityMap> existingMappings = (from t in context.People_Specialities_Map
                                                              where t.PersonId == personId
                                                              select t).ToList();
                List<Speciality> allSpecialities = (from t in context.Specialities select t).ToList();
                List<int> mappingsToRemove = allSpecialities.Where(x=> !checkboxValues.Contains(x.Id)).Select(x=>x.Id).ToList();
                List<int> mappingsToAdd = checkboxValues;

                //remove mappings of unchecked specialities
                foreach (int specId in mappingsToRemove)
                {
                    PersonSpecialityMap map = existingMappings.Where(x=>x.SpecialityId == specId && x.PersonId == personId).FirstOrDefault();
                    if (map != null)
                    {
                        context.People_Specialities_Map.Remove(map);
                    }
                }
                context.SaveChanges();

                //add mappings of the checked specialities
                foreach (int specId in mappingsToAdd)
                {
                    PersonSpecialityMap map = existingMappings.Where(x => x.SpecialityId == specId && x.PersonId == personId).FirstOrDefault();
                    if (map == null)
                    {
                        map = new PersonSpecialityMap();
                        map.PersonId = personId;
                        map.SpecialityId = specId;
                        context.People_Specialities_Map.Add(map);
                    }
                }
                context.SaveChanges();

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update speciality list for id " + personId, ex);
        }
    }
}
