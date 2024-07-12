using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using DAL.Data;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace DAL;

public class PersonRepository : IPersonRepository
{
    //public async Task<List<Person>> ListAllAsync()
    //{
    //    var peopleList = new List<Person>();
        
    //    var sql = new StringBuilder();
    //    sql.AppendLine("SELECT * FROM people");

    //    await using (var connection = new MySqlConnection(Config.DbConnectionString))
    //    {
    //        await connection.OpenAsync();
            
    //        var command = new MySqlCommand(sql.ToString(), connection);
            
    //        var reader = await command.ExecuteReaderAsync();
    //        while (await reader.ReadAsync())
    //        {
    //            peopleList.Add(PopulatePerson(reader));
    //        }
    //    }

    //    return peopleList;
    //}

    public async Task<List<Person>> ListAllAsync()
    {
        var peopleList = new List<Person>();
        try
        {
            await using (AppDB context = new AppDB())
            {
                List<Person> dbVal = context.People.ToList();

                if (dbVal != null)
                {
                    peopleList = dbVal;
                }
            }
        }
        catch (Exception ex) 
        {
            throw new Exception("Failed to retrieve all people from DB", ex);
        }

        return peopleList;
    }

    public async Task<bool> UpdatePeopleFromJSON(List<JsonWrapper> peopleList)
    {
        bool retval = false;
        try
        {
            await using (AppDB context = new AppDB())
            {
                List<Person> dbPeople = context.People.ToList();
                List<Address> dbAddress = context.Addresses.ToList();
                //List<int> GMCids = dbPeople.Select(x => x.GMC).ToList();

                foreach(JsonWrapper person in peopleList)
                {
                    Person personToInsert = dbPeople.Where(x => x.GMC.Equals(person.GMC)).FirstOrDefault();
                    if (personToInsert == null)
                    {
                        //if the person does not already exists create a new instance of a person
                        personToInsert = new Person();
                        personToInsert.GMC = person.GMC;
                        personToInsert.FirstName = person.firstName;
                        personToInsert.LastName = person.lastName;
                        context.People.Add(personToInsert);
                    }
                    else
                    {
                        //update First and Last names regardless
                        personToInsert.FirstName = person.firstName;
                        personToInsert.LastName = person.lastName;
                    }

                    context.SaveChanges();

                    //updating person's address
                    //JSON file provides an array of addresses but current front end views and some of the methods in address repository do not support a single person having multiple addresses
                    //this should be fixed in the future, but for now due to time constraints I shall simply accept the first element in the address array as 'correct'
                    AddressWrapper jsonAddress = person.address == null ? null : person.address.FirstOrDefault();
                    if (jsonAddress == null) {
                        jsonAddress = new AddressWrapper();
                    }

                    //if address for the person is found - update the values
                    Address addressToInsert = dbAddress.Where(x => x.PersonId == personToInsert.Id).FirstOrDefault();
                    if (addressToInsert == null)
                    {
                        //or create a new instance of address to add to the db
                        addressToInsert = new Address();
                        addressToInsert.PersonId = personToInsert.Id;
                        addressToInsert.Line1 = jsonAddress.line1;
                        addressToInsert.City = jsonAddress.city;
                        addressToInsert.Postcode = jsonAddress.postcode;
                        context.Addresses.Add(addressToInsert);
                    }
                    else
                    {
                        addressToInsert.Line1 = jsonAddress.line1;
                        addressToInsert.City = jsonAddress.city;
                        addressToInsert.Postcode = jsonAddress.postcode;
                    }
                    
                    context.SaveChanges();
                }
            }
            retval = true;
            
        }
        catch (Exception ex)
        {
            throw new Exception("People table update failed", ex);
        }
        return retval;
    }

    public async Task<Person> GetByIdAsync(int personId)
    {
        var person = new Person();
        
        var sql = new StringBuilder();
        sql.AppendLine("SELECT * FROM people");
        sql.AppendLine("WHERE Id = @personId");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("personId", personId);

            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                person = PopulatePerson(reader);
            }
        }

        return person;
    }

    public async Task SaveAsync(Person person)
    {
        var sql = new StringBuilder();
        sql.AppendLine("UPDATE people SET");
        sql.AppendLine("FirstName = @firstName,");
        sql.AppendLine("LastName = @lastName,");
        sql.AppendLine("GMC = @gmc");
        sql.AppendLine("WHERE Id = @personId");
        
        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("firstName", person.FirstName);
            command.Parameters.AddWithValue("lastName", person.LastName);
            command.Parameters.AddWithValue("gmc", person.GMC);
            command.Parameters.AddWithValue("personId", person.Id);

            await command.ExecuteNonQueryAsync();
        }
    }

    private Person PopulatePerson(IDataRecord data)
    {
        var person = new Person
        {
            Id = int.Parse(data["Id"].ToString()),
            FirstName = data["FirstName"].ToString(),
            LastName = data["LastName"].ToString(),
            GMC = int.Parse(data["GMC"].ToString())
        };
        return person;
    }
}