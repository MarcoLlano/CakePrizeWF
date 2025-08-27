using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CakePrizeDB.Repositories
{
    public class UserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public UserModel? GetByUsername(string username)
        {
            using var command = new SqlCommand(
                "SELECT Id, Username, Password, FirstName, LastName, Email, Role, IsActive, " +
                "CreatedDate, CreatedUser, ModifiedDate, ModifiedUser, LastLoginDate " +
                "FROM Users WHERE Username = @Username AND IsActive = 1", _connection);
            
            command.Parameters.AddWithValue("@Username", username);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UserModel
                {
                    Id = reader.GetGuid("Id"),
                    Username = reader.GetString("Username"),
                    Password = reader.GetString("Password"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Email = reader.GetString("Email"),
                    Role = (UserRole)reader.GetInt32("Role"),
                    IsActive = reader.GetBoolean("IsActive"),
                    CreatedDate = reader.GetDateTime("CreatedDate"),
                    CreatedUser = reader.GetString("CreatedUser"),
                    ModifiedDate = reader.GetDateTime("ModifiedDate"),
                    ModifiedUser = reader.GetString("ModifiedUser"),
                    LastLoginDate = reader.IsDBNull("LastLoginDate") ? null : reader.GetDateTime("LastLoginDate")
                };
            }

            return null;
        }

        public void UpdateLastLogin(Guid userId)
        {
            using var command = new SqlCommand(
                "UPDATE Users SET LastLoginDate = @LastLoginDate WHERE Id = @Id", _connection);
            
            command.Parameters.AddWithValue("@LastLoginDate", DateTime.Now);
            command.Parameters.AddWithValue("@Id", userId);
            
            command.ExecuteNonQuery();
        }

        public List<UserModel> GetAll()
        {
            var users = new List<UserModel>();
            
            using var command = new SqlCommand(
                "SELECT Id, Username, FirstName, LastName, Email, Role, IsActive, " +
                "CreatedDate, CreatedUser, ModifiedDate, ModifiedUser, LastLoginDate " +
                "FROM Users ORDER BY Username", _connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new UserModel
                {
                    Id = reader.GetGuid("Id"),
                    Username = reader.GetString("Username"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Email = reader.GetString("Email"),
                    Role = (UserRole)reader.GetInt32("Role"),
                    IsActive = reader.GetBoolean("IsActive"),
                    CreatedDate = reader.GetDateTime("CreatedDate"),
                    CreatedUser = reader.GetString("CreatedUser"),
                    ModifiedDate = reader.GetDateTime("ModifiedDate"),
                    ModifiedUser = reader.GetString("ModifiedUser"),
                    LastLoginDate = reader.IsDBNull("LastLoginDate") ? null : reader.GetDateTime("LastLoginDate")
                });
            }

            return users;
        }

        public void Insert(UserModel user)
        {
            using var command = new SqlCommand(
                "INSERT INTO Users (Id, Username, Password, FirstName, LastName, Email, Role, IsActive, " +
                "CreatedDate, CreatedUser, ModifiedDate, ModifiedUser) " +
                "VALUES (@Id, @Username, @Password, @FirstName, @LastName, @Email, @Role, @IsActive, " +
                "@CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)", _connection);

            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Role", (int)user.Role);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", user.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", user.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", user.ModifiedUser);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates user status (active/inactive)
        /// </summary>
        public bool UpdateUserStatus(Guid userId, bool isActive, string modifiedBy)
        {
            using var command = new SqlCommand(
                "UPDATE Users SET IsActive = @IsActive, ModifiedDate = @ModifiedDate, ModifiedUser = @ModifiedUser " +
                "WHERE Id = @Id", _connection);

            command.Parameters.AddWithValue("@Id", userId);
            command.Parameters.AddWithValue("@IsActive", isActive);
            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            command.Parameters.AddWithValue("@ModifiedUser", modifiedBy);

            return command.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Updates user information
        /// </summary>
        public bool Update(UserModel user)
        {
            using var command = new SqlCommand(
                "UPDATE Users SET Username = @Username, FirstName = @FirstName, LastName = @LastName, " +
                "Email = @Email, Role = @Role, IsActive = @IsActive, ModifiedDate = @ModifiedDate, " +
                "ModifiedUser = @ModifiedUser WHERE Id = @Id", _connection);

            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Role", (int)user.Role);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@ModifiedDate", user.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", user.ModifiedUser);

            return command.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Gets user by ID
        /// </summary>
        public UserModel? GetById(Guid id)
        {
            using var command = new SqlCommand(
                "SELECT Id, Username, Password, FirstName, LastName, Email, Role, IsActive, " +
                "CreatedDate, CreatedUser, ModifiedDate, ModifiedUser, LastLoginDate " +
                "FROM Users WHERE Id = @Id", _connection);
            
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UserModel
                {
                    Id = reader.GetGuid("Id"),
                    Username = reader.GetString("Username"),
                    Password = reader.GetString("Password"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Email = reader.GetString("Email"),
                    Role = (UserRole)reader.GetInt32("Role"),
                    IsActive = reader.GetBoolean("IsActive"),
                    CreatedDate = reader.GetDateTime("CreatedDate"),
                    CreatedUser = reader.GetString("CreatedUser"),
                    ModifiedDate = reader.GetDateTime("ModifiedDate"),
                    ModifiedUser = reader.GetString("ModifiedUser"),
                    LastLoginDate = reader.IsDBNull("LastLoginDate") ? null : reader.GetDateTime("LastLoginDate")
                };
            }

            return null;
        }
    }
}
