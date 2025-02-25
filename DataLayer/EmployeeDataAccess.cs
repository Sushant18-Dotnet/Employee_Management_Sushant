using DatabaseLayer;
using EntityLayer;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;

namespace DataLayer
{
    public class EmployeeDataAccess
    {

        private static readonly string connectionString = "Server=DESKTOP-UJ0JPTI\\SQLEXPRESS;Database=NeosSoft_Sushant;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";


        public static void CreateEmployee(EmployeeMaster model)
        {

            // sp_CreateEmployee
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(Employee_Sp.CreateEmployee, connection))
                {
                    EmployeeMaster e = new EmployeeMaster();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeCode", model.EmployeeCode);
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", (object)model.LastName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CountryId", model.CountryId);
                    command.Parameters.AddWithValue("@StateId", model.StateId);
                    command.Parameters.AddWithValue("@CityId", model.CityId);
                    command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    command.Parameters.AddWithValue("@MobileNumber", model.MobileNumber);
                    command.Parameters.AddWithValue("@PanNumber", model.PanNumber);
                    command.Parameters.AddWithValue("@PassportNumber", model.PassportNumber);
                   
                    if (model.ProfileImage == null)
                    {
                        command.Parameters.Add("@ProfileImage", SqlDbType.VarBinary).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@ProfileImage", SqlDbType.VarBinary).Value = model.ProfileImage;
                    }
                    command.Parameters.AddWithValue("@Gender", model.Gender);
                    command.Parameters.AddWithValue("@IsActive", model.IsActive);
                    command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                    command.Parameters.AddWithValue("@DateOfJoinee", (object)model.DateOfJoinee ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedDate", "12/12/2025");
                    command.Parameters.AddWithValue("@UpdatedDate", (object)model.UpdatedDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
                    command.Parameters.AddWithValue("@DeletedDate", (object)model.DeletedDate ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
      
        public static string MapGender(int genderValue)
        {
            // Map integer values to string representations
            return genderValue switch
            {
                0 => "Male",
                1 => "Female",
                2 => "Other",
                _ => "Unknown"
            };
        }
        public static async Task<IEnumerable<EmployeeMasters>> GetAllEmployees()
        {


            var employees = new List<EmployeeMasters>(); 

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(Employee_Sp.GetAllEmployees, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(new EmployeeMasters
                                {
                                    Row_Id = reader.IsDBNull(reader.GetOrdinal("Row_Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Row_Id")),
                                    EmployeeCode = reader.IsDBNull(reader.GetOrdinal("EmployeeCode")) ? null : reader.GetString(reader.GetOrdinal("EmployeeCode")),
                                    FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                    DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                                    MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber")),
                                    PanNumber = reader.IsDBNull(reader.GetOrdinal("PanNumber")) ? null : reader.GetString(reader.GetOrdinal("PanNumber")),
                                    PassportNumber = reader.IsDBNull(reader.GetOrdinal("PassportNumber")) ? null : reader.GetString(reader.GetOrdinal("PassportNumber")),
                                    CountryName = reader.IsDBNull(reader.GetOrdinal("CountryName")) ? null : reader.GetString(reader.GetOrdinal("CountryName")),
                                    stateName = reader.IsDBNull(reader.GetOrdinal("statename")) ? null : reader.GetString(reader.GetOrdinal("statename")),
                                    cityName = reader.IsDBNull(reader.GetOrdinal("cityname")) ? null : reader.GetString(reader.GetOrdinal("cityname")),
                                    ProfileImage = reader.IsDBNull(reader.GetOrdinal("ProfileImage")) ? null : reader.GetString(reader.GetOrdinal("ProfileImage")),
                                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),// Handle conversion // Ensure conversion is correct
                                    IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    DateOfJoinee = reader.IsDBNull(reader.GetOrdinal("DateOfJoinee")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateOfJoinee")),
                                    //CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    //UpdatedDate = reader.IsDBNull(reader.GetOrdinal("UpdatedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                                    //IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? false : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                    //DeletedDate = reader.IsDBNull(reader.GetOrdinal("DeletedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DeletedDate")),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error fetching employees: {ex.Message}");
              
            }

            return   employees;
        }


        public EmployeeMaster GetEmployeeById(int rowId)
        {
            EmployeeMaster employee = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Employee_Sp.GetEmployeeById, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Row_id", rowId); 
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new EmployeeMaster
                            {
                                EmployeeCode = reader["EmployeeCode"].ToString(),
                                Row_Id = Convert.ToInt32(reader["Row_Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                CountryId = Convert.ToInt32(reader["CountryId"]),
                                StateId = Convert.ToInt32(reader["StateId"]),
                                CityId = Convert.ToInt32(reader["CityId"]),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                PanNumber = reader["PanNumber"].ToString(),
                                PassportNumber = reader["PassportNumber"].ToString(),
                                ProfileImage = reader["ProfileImage"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                DateOfJoinee = reader["DateOfJoinee"] as DateTime?
                            };
                        }
                    }
                }
            }
            return employee;
        }


        public static void UpdateEmployee(EmployeeViewModel model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(Employee_Sp.UpdateEmployee, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Row_Id", model.RowId);
                    command.Parameters.AddWithValue("@EmployeeCode", model.EmployeeCode);
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", (object)model.LastName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CountryId", model.CountryId);
                    command.Parameters.AddWithValue("@StateId", model.StateId);
                    command.Parameters.AddWithValue("@CityId", model.CityId);
                    command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    command.Parameters.AddWithValue("@MobileNumber", model.MobileNumber);
                    command.Parameters.AddWithValue("@PanNumber", model.PanNumber);
                    command.Parameters.AddWithValue("@PassportNumber", model.PassportNumber);
                    command.Parameters.AddWithValue("@ProfileImage", (object)model.ProfileImageFile ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Gender", (object)model.Gender ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", model.IsActive);
                    command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                    command.Parameters.AddWithValue("@DateOfJoinee", (object)model.DateOfJoinee ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedDate", (object)model.UpdatedDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
                    command.Parameters.AddWithValue("@DeletedDate", (object)model.DeletedDate ?? DBNull.Value);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public static void DeleteEmployee(int rowId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(Employee_Sp.DeleteEmployee, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Row_Id", rowId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            
                            Console.WriteLine("No rows were affected. Check if the rowId exists.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}



