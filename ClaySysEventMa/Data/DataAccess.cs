using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ClaySysEventMa.Models;
using Microsoft.Extensions.Configuration;

namespace ClaySysEventMa.Data
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Add a new user
        public async Task AddUserAsync(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spAddUser", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                cmd.Parameters.AddWithValue("@Gender", user.Gender);
                cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                cmd.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                cmd.Parameters.AddWithValue("@Address", user.Address);
                cmd.Parameters.AddWithValue("@State", user.State);
                cmd.Parameters.AddWithValue("@City", user.City);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Get user by username and password
        public async Task<User> GetUserAsync(string username, string password)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spGetUser", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    user = new User
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Gender = (string)reader["Gender"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        EmailAddress = (string)reader["EmailAddress"],
                        Address = (string)reader["Address"],
                        State = (string)reader["State"],
                        City = (string)reader["City"],
                        Username = (string)reader["Username"],
                        Password = (string)reader["Password"],
                        Role = (string)reader["Role"]
                    };
                }
            }
            return user;
        }

        // Get user by username
        public async Task<User> GetUserAsync(string username)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spGetUserByUsername", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Username", username);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    user = new User
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Gender = (string)reader["Gender"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        EmailAddress = (string)reader["EmailAddress"],
                        Address = (string)reader["Address"],
                        State = (string)reader["State"],
                        City = (string)reader["City"],
                        Username = (string)reader["Username"],
                        Password = (string)reader["Password"],
                        Role = (string)reader["Role"]
                    };
                }
            }
            return user;
        }
        // Get user by ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spGetUserById", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    user = new User
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Gender = (string)reader["Gender"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        EmailAddress = (string)reader["EmailAddress"],
                        Address = (string)reader["Address"],
                        State = (string)reader["State"],
                        City = (string)reader["City"],
                        Username = (string)reader["Username"],
                        Password = (string)reader["Password"],
                        Role = (string)reader["Role"]
                    };
                }
            }
            return user;
        }

        // Update user
        public async Task UpdateUserAsync(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spUpdateUser", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                cmd.Parameters.AddWithValue("@Gender", user.Gender);
                cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                cmd.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                cmd.Parameters.AddWithValue("@Address", user.Address);
                cmd.Parameters.AddWithValue("@State", user.State);
                cmd.Parameters.AddWithValue("@City", user.City);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Update user role
        public async Task UpdateUserRoleAsync(int userId, string role)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spUpdateUserRole", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Role", role);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Delete user
        public async Task DeleteUserAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spDeleteUser", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Get all users
        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users = new List<User>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spGetUsers", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new User
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Gender = (string)reader["Gender"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        EmailAddress = (string)reader["EmailAddress"],
                        Address = (string)reader["Address"],
                        State = (string)reader["State"],
                        City = (string)reader["City"],
                        Username = (string)reader["Username"],
                        Password = (string)reader["Password"],
                        Role = (string)reader["Role"]
                    });
                }
            }
            return users;
        }
        // Event Operations // Add a new event
        public async Task AddEventAsync(Event @event)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spAddEvent", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Name", @event.Name);
                cmd.Parameters.AddWithValue("@Description", @event.Description);
                cmd.Parameters.AddWithValue("@Date", @event.Date);
                 cmd.Parameters.AddWithValue("@Location", @event.Location);
                
                cmd.Parameters.AddWithValue("@ImageBase64", @event.ImageBase64); // Missing in the provided method
                await cmd.ExecuteNonQueryAsync();
            }
        }
        // Get event by ID
        public async Task<Event> GetEventByIdAsync(int id)
        {
            Event @event = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spGetEventById", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    @event = new Event
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Description = (string)reader["Description"],
                        Date = (DateTime)reader["Date"],
                        Location = reader["Location"] as string, // Missing in the provided method
                        ImageBase64 = reader["ImageBase64"] as string
                    };
                }
            }
            return @event;
        }
        // Update an event
        public async Task UpdateEventAsync(Event @event)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync(); SqlCommand cmd = new SqlCommand("spUpdateEvent", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", @event.Id);
                cmd.Parameters.AddWithValue("@Name", @event.Name);
                cmd.Parameters.AddWithValue("@Description", @event.Description);
                cmd.Parameters.AddWithValue("@Date", @event.Date);
                cmd.Parameters.AddWithValue("@Location", @event.Location); // Missing in the provided method
                cmd.Parameters.AddWithValue("@ImageBase64", @event.ImageBase64);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        // Delete an event by ID
        public async Task DeleteEventAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spDeleteEvent", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        // Get list of all events
        public async Task<List<Event>> GetEventsAsync()
        {
            List<Event> events = new List<Event>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spGetEvents", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    events.Add(new Event
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Description = (string)reader["Description"],
                        Date = (DateTime)reader["Date"],
                         Location = reader["Location"] as string, // Missing in the provided method
                         ImageBase64 = reader["ImageBase64"] as string
                    });
                }
            }
            return events;
        }

        // Add a new registration
        public async Task AddRegistrationAsync(Registration registration)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spAddRegistration", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", registration.UserId);
                cmd.Parameters.AddWithValue("@EventId", registration.EventId);
                cmd.Parameters.AddWithValue("@RegistrationDate", registration.RegistrationDate);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        // Get registration by ID
        public async Task<Registration> GetRegistrationByIdAsync(int id)
        {
            Registration registration = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spGetRegistrationById", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    registration = new Registration
                    {
                        Id = (int)reader["Id"],
                        UserId = (int)reader["UserId"],
                        EventId = (int)reader["EventId"],
                        RegistrationDate = (DateTime)reader["RegistrationDate"],
                        User = new User
                        {
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"]
                        },
                        Event = new Event
                        {
                            Name = (string)reader["EventName"]
                        }
                    };
                }
            }
            return registration;
        }
        // Get list of all registrations
        public async Task<List<Registration>> GetRegistrationsAsync()
        {
            List<Registration> registrations = new List<Registration>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spGetRegistrations", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    registrations.Add(new Registration
                    {
                        Id = (int)reader["Id"],
                        UserId = (int)reader["UserId"],
                        EventId = (int)reader["EventId"],
                        RegistrationDate = (DateTime)reader["RegistrationDate"],
                        User = new User
                        {
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],

                            Username = (string)reader["Username"]
                        },
                        Event = new Event
                        {
                            Name = (string)reader["EventName"]
                        }
                    });
                }
            }
            return registrations;
        }
        // Remove a registration by ID
        public async Task RemoveRegistrationAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync(); SqlCommand cmd = new SqlCommand("spRemoveRegistration", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}