using CakePrizeDB.Models;

namespace CakePrizeView.Utils
{
    /// <summary>
    /// Manages the current user session and provides access to user information and permissions
    /// </summary>
    public static class UserSession
    {
        private static UserModel? _currentUser;

        /// <summary>
        /// Sets the current user session
        /// </summary>
        public static void SetCurrentUser(UserModel user)
        {
            _currentUser = user;
        }

        /// <summary>
        /// Gets the current user
        /// </summary>
        public static UserModel? GetCurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Clears the current user session (logout)
        /// </summary>
        public static void ClearSession()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Checks if a user is currently logged in
        /// </summary>
        public static bool IsLoggedIn()
        {
            return _currentUser != null;
        }

        /// <summary>
        /// Gets the current user's role
        /// </summary>
        public static UserRole GetCurrentUserRole()
        {
            return _currentUser?.Role ?? UserRole.Employee;
        }

        /// <summary>
        /// Gets the current user's full name
        /// </summary>
        public static string GetCurrentUserFullName()
        {
            if (_currentUser == null)
                return "Usuario";
            
            return $"{_currentUser.FirstName} {_currentUser.LastName}";
        }

        /// <summary>
        /// Gets the current user's username
        /// </summary>
        public static string GetCurrentUsername()
        {
            return _currentUser?.Username ?? "";
        }

        /// <summary>
        /// Checks if the current user has admin privileges
        /// </summary>
        public static bool IsAdmin()
        {
            return _currentUser?.Role == UserRole.Admin;
        }

        /// <summary>
        /// Checks if the current user can manage ingredients
        /// </summary>
        public static bool CanManageIngredients()
        {
            return _currentUser?.Role >= UserRole.PastryChef;
        }

        /// <summary>
        /// Checks if the current user can manage brands
        /// </summary>
        public static bool CanManageBrands()
        {
            return _currentUser?.Role >= UserRole.PastryChef;
        }

        /// <summary>
        /// Checks if the current user can access reports
        /// </summary>
        public static bool CanAccessReports()
        {
            return _currentUser?.Role >= UserRole.Sales;
        }

        /// <summary>
        /// Checks if the current user can manage products
        /// </summary>
        public static bool CanManageProducts()
        {
            return _currentUser?.Role >= UserRole.PastryChef;
        }

        /// <summary>
        /// Checks if the current user has permission for a specific role
        /// </summary>
        public static bool HasPermission(UserRole requiredRole)
        {
            return _currentUser?.Role >= requiredRole;
        }

        /// <summary>
        /// Gets the role display name
        /// </summary>
        public static string GetRoleDisplayName(UserRole role)
        {
            return role switch
            {
                UserRole.Employee => "Empleado",
                UserRole.Sales => "Ventas",
                UserRole.PastryChef => "Pastelero",
                UserRole.Admin => "Administrador",
                _ => "Desconocido"
            };
        }
    }
}

