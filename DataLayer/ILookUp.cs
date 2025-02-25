using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface ILookUp
    {
        Task<IEnumerable<Country>> GetCountriesAsync();
        Task<IEnumerable<State>> GetStatesByCountryIdAsync(int countryId);
        Task<IEnumerable<City>> GetCitiesByStateIdAsync(int stateId);
    }
}
