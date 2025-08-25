using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace CakePrizeDB.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService(SqlConnection sqlConnection)
        {
            _repository = new UserRepository(sqlConnection);
        }

        /// <summary>
        /// Authenticates a user with username and password
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>The authenticated user if valid, null otherwise</returns>
        public UserModel? AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = _repository.GetByUsername(username);
            if (user == null)
                return null;

            // Check if user is active
            if (!user.IsActive)
            {
                // Log failed login attempt for disabled user
                // You might want to add logging here
                return null;
            }

            // For now, using simple password comparison
            // In production, you should use proper password hashing
            if (VerifyPassword(password, user.Password))
            {
                _repository.UpdateLastLogin(user.Id);
                return user;
            }

            return null;
        }

        /// <summary>
        /// Creates a new user with hashed password
        /// </summary>
        public UserModel CreateUser(string username, string password, string firstName, string lastName, 
            string email, UserRole role, string createdUser)
        {
            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = HashPassword(password), // Hash the password
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Role = role,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
                ModifiedDate = DateTime.Now,
                ModifiedUser = createdUser
            };

            _repository.Insert(user);
            return user;
        }

        /// <summary>
        /// Gets all users (for admin purposes)
        /// </summary>
        public List<UserModel> GetAllUsers()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a user by username (without password verification)
        /// </summary>
        public UserModel? GetUserByUsername(string username)
        {
            return _repository.GetByUsername(username);
        }

        /// <summary>
        /// Gets only active users
        /// </summary>
        public List<UserModel> GetActiveUsers()
        {
            return _repository.GetAll().Where(u => u.IsActive).ToList();
        }

        /// <summary>
        /// Disables a user (sets IsActive to false)
        /// </summary>
        public bool DisableUser(Guid userId, string modifiedBy)
        {
            return _repository.UpdateUserStatus(userId, false, modifiedBy);
        }

        /// <summary>
        /// Enables a user (sets IsActive to true)
        /// </summary>
        public bool EnableUser(Guid userId, string modifiedBy)
        {
            return _repository.UpdateUserStatus(userId, true, modifiedBy);
        }

        /// <summary>
        /// Updates user information
        /// </summary>
        public bool UpdateUser(UserModel user, string modifiedBy)
        {
            user.ModifiedDate = DateTime.Now;
            user.ModifiedUser = modifiedBy;
            return _repository.Update(user);
        }

        /// <summary>
        /// Checks if a user has permission to access a specific feature
        /// </summary>
        public bool HasPermission(UserModel user, UserRole requiredRole)
        {
            return user.Role >= requiredRole;
        }

        /// <summary>
        /// Checks if a user is an admin
        /// </summary>
        public bool IsAdmin(UserModel user)
        {
            return user.Role == UserRole.Admin;
        }

        /// <summary>
        /// Checks if a user can manage ingredients (PastryChef or Admin)
        /// </summary>
        public bool CanManageIngredients(UserModel user)
        {
            return user.Role >= UserRole.PastryChef;
        }

        /// <summary>
        /// Checks if a user can manage brands (PastryChef or Admin)
        /// </summary>
        public bool CanManageBrands(UserModel user)
        {
            return user.Role >= UserRole.PastryChef;
        }

        /// <summary>
        /// Checks if a user can access reports (Sales, PastryChef, or Admin)
        /// </summary>
        public bool CanAccessReports(UserModel user)
        {
            return user.Role >= UserRole.Sales;
        }

        /// <summary>
        /// Checks if a user can manage products (PastryChef or Admin)
        /// </summary>
        public bool CanManageProducts(UserModel user)
        {
            return user.Role >= UserRole.PastryChef;
        }

        /// <summary>
        /// Simple password hashing (use a more secure method in production)
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// Verifies a password against a hash
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }
}
