using DatabaseLayer;
using EntityLayer;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Lookup : ILookUp
    {
      
        private static readonly string connectionString = "Server=DESKTOP-UJ0JPTI\\SQLEXPRESS;Database=NeosSoft_Sushant;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";
        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            try
            {
                var countries = new List<Country>();
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetCountries", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                countries.Add(new Country
                                {
                                    Row_Id = reader.GetInt32(reader.GetOrdinal("RowId")),
                                    CountryName = reader.GetString(reader.GetOrdinal("CountryName"))
                                });
                            }
                        }
                    }
                }
                return countries;
            }
            catch (SqlException ex)
            {
                // Log detailed SQL exception information
                Console.WriteLine($"SQL Exception: {ex.Message}");
                Console.WriteLine($"Error Number: {ex.Number}");
                Console.WriteLine($"Error State: {ex.State}");
                Console.WriteLine($"Error Line Number: {ex.LineNumber}");
                throw;
            }
            catch (Exception ex)
            {
                // Log general exception information
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<State>> GetStatesByCountryIdAsync(int countryId)
        {
            var states = new List<State>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetStatesByCountryId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CountryId", countryId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            states.Add(new State
                            {
                                Row_Id = reader.GetInt32(reader.GetOrdinal("RowId")),
                                StateName = reader.GetString(reader.GetOrdinal("StateName"))
                            });
                        }
                    }
                }
            }
            return states;
        }
        public async Task<IEnumerable<City>> GetCitiesByStateIdAsync(int stateId)
        {
            var cities = new List<City>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetCitiesByStateId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StateId", stateId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cities.Add(new City
                            {
                                Row_Id = reader.GetInt32(reader.GetOrdinal("RowId")),
                                CityName = reader.GetString(reader.GetOrdinal("CityName"))
                            });
                        }
                    }
                }
            }
            return cities;
        }
    }
}
