using DataLayer;
using Employee_Management.Data;
using EntityLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Employee_Management.Controllers
{

        public class EmployeeController :Controller
        {
        
        // GET: Employee/Create

        private readonly ILookUp _lookupDataAccess;

        private readonly DatabaseContext _databaseContext;

        private readonly IWebHostEnvironment _environment;

        public EmployeeController(ILookUp lookupDataAccess, DatabaseContext databaseContext,IWebHostEnvironment webhost)
        {
            _lookupDataAccess = lookupDataAccess;
            _databaseContext = databaseContext;
            _environment = webhost;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<EmployeeMasters> employees;

            try
            {
                
                employees =  await EmployeeDataAccess.GetAllEmployees();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error fetching employees: {ex.Message}");

                
                employees = Enumerable.Empty<EmployeeMasters>();
            }

            return View(employees);
        }


        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            var countries = await _lookupDataAccess.GetCountriesAsync();
            ViewBag.Countries = new SelectList(countries, "Row_Id", "CountryName");
            ViewBag.States = new SelectList(new List<State>(), "Row_Id", "StateName");
            ViewBag.Cities = new SelectList(new List<City>(), "Row_Id", "CityName");

            return View();
        }

       

        [HttpGet]
        public IActionResult GetStatesByCountry(int countryId)
        {
                 var states = _databaseContext.States
                 .Where(s => s.CountryId == countryId)
                 .Select(s => new { s.CountryId,s.Row_Id, s.StateName })
                 .ToList();
                  return Json(states);
        }


        [HttpGet]
        public List<State> GetStatesByCountrys(int countryId)
        {
            List <State> state= new List<State>();

            var states = _databaseContext.States
                .Where(s => s.CountryId == countryId)
                .Select(s => new { s.CountryId, s.Row_Id, s.StateName })
                .ToList();

            foreach (var x in states)
            {
                state.Add(new State { Row_Id = x.Row_Id, StateName = x.StateName });
            }

            return state;
        }

        [HttpGet]
        public List<City> GetCitiesByStates(int stateId)
        {
            List<City> cit = new List<City>();
            var cities = _databaseContext.City
                .Where(c => c.StateId == stateId)
                .Select(c => new { c.StateId, c.Row_Id, c.CityName })
                .ToList();
            foreach(var x in cities)
            {
                cit.Add(new City { Row_Id = x.Row_Id, CityName = x.CityName });
            }
            
            return cit;
        }
        [HttpGet]
        public IActionResult GetCitiesByState(int stateId)
        {
            if (stateId <= 0)
            {
                return Json(new { error = "Invalid stateId" });
            }
            var cities = _databaseContext.City
                .Where(c => c.StateId == stateId)
                .Select(c => new { c.StateId,c.Row_Id, c.CityName })
                .ToList();
            return Json(cities);
        }



        private string ProcessUploadedFile(EmployeeViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "EmpImages/Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfileImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImageFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel model)
        {
            string uniqueFileName = ProcessUploadedFile(model);
            if (!ModelState.IsValid)
            {

                //  model.Gender = model.Gender.Equals("Male") ? "1" : "2";
                EmployeeMaster data = new EmployeeMaster()
                {

                    EmployeeCode = model.EmployeeCode,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CountryId = model.CountryId,
                    StateId = model.StateId,
                    CityId = model.RowId,
                    EmailAddress = model.EmailAddress,
                    MobileNumber = model.MobileNumber,
                    PanNumber = model.PanNumber,
                    PassportNumber = model.PassportNumber,
                    ProfileImage = model.ProfileImage,
                    Gender = model.Gender,
                    IsActive = model.IsActive,
                    DateOfBirth = model.DateOfBirth,
                    DateOfJoinee = model.DateOfJoinee,
                    IsDeleted = model.IsDeleted

                };

                EmployeeDataAccess.CreateEmployee(data);


                return RedirectToAction("Index");
            }
            return View(model);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await  _databaseContext.EmployeeMaster
                .FirstOrDefaultAsync(m => m.Row_Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            
            if (id <= 0)
            {
                return NotFound(); 
            }

            EmployeeDataAccess d = new EmployeeDataAccess();
            var employee = d.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound(); 
            }

            
            var employeeViewModel = new EmployeeViewModel
            {
                RowId = employee.Row_Id,
                EmployeeCode = employee.EmployeeCode,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                CountryId = employee.CountryId,
                StateId = employee.StateId,
                CityId = employee.CityId,
                EmailAddress = employee.EmailAddress,
                MobileNumber = employee.MobileNumber,
                PanNumber = employee.PanNumber,
                PassportNumber = employee.PassportNumber,
                ProfileImage= employee.ProfileImage,
                IsActive = employee.IsActive,
                DateOfBirth = employee.DateOfBirth,
                DateOfJoinee = employee.DateOfJoinee
            };

            
            var countries = await _lookupDataAccess.GetCountriesAsync();
           

            ViewBag.Countries = new SelectList(countries, "Row_Id", "CountryName");
            ViewBag.StateList = new SelectList(GetStatesByCountrys(employee.CountryId), "Row_Id", "StateName", new { employeeViewModel.StateId });
            ViewBag.CityList = new SelectList(GetCitiesByStates(employee.StateId), "Row_Id", "CityName", new { employeeViewModel.CityId });
           
            return View(employeeViewModel);
        }




        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel model, int id)
        {
            if (ModelState.IsValid)
            {

                return View(model);
            }


            var existingEmployee = _databaseContext.EmployeeMaster.Find(id);

            if (existingEmployee == null)
            {

                return NotFound();
            }

            // Update the employee's properties
            existingEmployee.EmployeeCode = model.EmployeeCode;
            existingEmployee.FirstName = model.FirstName;
            existingEmployee.LastName = model.LastName;
            existingEmployee.CountryId = model.CountryId;
            existingEmployee.StateId = model.StateId;
            existingEmployee.CityId = model.CityId;
            existingEmployee.EmailAddress = model.EmailAddress;
            existingEmployee.MobileNumber = model.MobileNumber;
            existingEmployee.PanNumber = model.PanNumber;
            existingEmployee.PassportNumber = model.PassportNumber;
            existingEmployee.ProfileImage = model.ProfileImage;
            existingEmployee.Gender = model.Gender;
            existingEmployee.IsActive = model.IsActive;
            existingEmployee.DateOfBirth = model.DateOfBirth;
            existingEmployee.DateOfJoinee = model.DateOfJoinee;
            existingEmployee.UpdatedDate = DateTime.Now;
            existingEmployee.IsDeleted = model.IsDeleted;
            existingEmployee.DeletedDate = model.DeletedDate;

            // Save changes to the database
            _databaseContext.EmployeeMaster.Update(existingEmployee);
            _databaseContext.SaveChanges();

            // Redirect to the index or another view after successful update
            return RedirectToAction("Index");
        }


           // POST: Employee/Delete/5
            [HttpPost]
            public IActionResult Delete(int id)
            {
                EmployeeDataAccess.DeleteEmployee(id);
            
                return RedirectToAction("Index");
            }
        }
    }


//private string ProcessUploadedFile(EmployeeViewModel model)
//{
//    string uniqueFileName = null;

//    if (model.ProfileImageFile != null)
//    {

//        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
//        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImageFile.FileName;

//        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//        // Save the file to the server
//        using (var fileStream = new FileStream(filePath, FileMode.Create))
//        {
//            model.ProfileImageFile.CopyTo(fileStream);
//        }
//    }

//    return uniqueFileName;
//}