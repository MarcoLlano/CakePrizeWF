using CakePrizeDB.Models;
using CakePrizeDB.Services;
using CakePrizeView.Utils;
using Microsoft.Data.SqlClient;

namespace CakePrizeView.Forms.Menu
{
    public partial class UserManagementForm : Form
    {
        private Form previousForm;
        private SqlConnection sqlConnection;
        private UserService userService;
        private List<UserModel> users;

        public UserManagementForm(Form previousForm, SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            this.sqlConnection = sqlConnection;
            this.userService = new UserService(sqlConnection);

            // Check if user has admin permissions
            if (!UserSession.IsAdmin())
            {
                MessageBox.Show("Solo los administradores pueden acceder a la gestión de usuarios.", 
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            LoadUsers();
            SetupEventHandlers();
        }

        private void LoadUsers()
        {
            try
            {
                if (chkShowInactive.Checked)
                {
                    users = userService.GetAllUsers();
                }
                else
                {
                    users = userService.GetActiveUsers();
                }
                RefreshUserList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshUserList()
        {
            listViewUsers.Items.Clear();
            foreach (var user in users)
            {
                var item = new ListViewItem(user.Username);
                item.SubItems.Add($"{user.FirstName} {user.LastName}");
                item.SubItems.Add(user.Email);
                item.SubItems.Add(UserSession.GetRoleDisplayName(user.Role));
                item.SubItems.Add(user.IsActive ? "Activo" : "Inactivo");
                item.SubItems.Add(user.LastLoginDate?.ToString("dd/MM/yyyy HH:mm") ?? "Nunca");
                item.Tag = user;

                // Set different colors for active/inactive users
                if (!user.IsActive)
                {
                    item.BackColor = System.Drawing.Color.LightGray;
                    item.ForeColor = System.Drawing.Color.DarkGray;
                }

                listViewUsers.Items.Add(item);
            }
        }

        private void SetupEventHandlers()
        {
            btnAddUser.Click += BtnAddUser_Click;
            btnEditUser.Click += BtnEditUser_Click;
            btnDeleteUser.Click += BtnDisableUser_Click;
            btnBack.Click += BtnBack_Click;
            chkShowInactive.CheckedChanged += ChkShowInactive_CheckedChanged;
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función de agregar usuario no implementada aún.", 
                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // TODO: Implement AddUserForm
            // var addUserForm = new AddUserForm(sqlConnection);
            // if (addUserForm.ShowDialog() == DialogResult.OK)
            // {
            //     LoadUsers(); // Refresh the list
            // }
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            if (listViewUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un usuario para editar.", 
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show("Función de editar usuario no implementada aún.", 
                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // TODO: Implement EditUserForm
            // var selectedUser = (UserModel)listViewUsers.SelectedItems[0].Tag;
            // var editUserForm = new EditUserForm(selectedUser, sqlConnection);
            // if (editUserForm.ShowDialog() == DialogResult.OK)
            // {
            //     LoadUsers(); // Refresh the list
            // }
        }

        private void BtnDisableUser_Click(object sender, EventArgs e)
        {
            if (listViewUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un usuario para desactivar/activar.", 
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedUser = (UserModel)listViewUsers.SelectedItems[0].Tag;
            
            if (selectedUser.Username == UserSession.GetCurrentUsername())
            {
                MessageBox.Show("No puede desactivar su propio usuario.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string action = selectedUser.IsActive ? "desactivar" : "activar";
            string actionPast = selectedUser.IsActive ? "desactivado" : "activado";
            
            var result = MessageBox.Show(
                $"¿Está seguro de que desea {action} al usuario '{selectedUser.Username}'?", 
                $"Confirmar {action.ToUpper()}", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success;
                    if (selectedUser.IsActive)
                    {
                        success = userService.DisableUser(selectedUser.Id, UserSession.GetCurrentUsername());
                    }
                    else
                    {
                        success = userService.EnableUser(selectedUser.Id, UserSession.GetCurrentUsername());
                    }

                    if (success)
                    {
                        LoadUsers(); // Refresh the list
                        MessageBox.Show($"Usuario {actionPast} correctamente.", 
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el estado del usuario.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al {action} usuario: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Hide();
            previousForm.Show();
        }

        private void ChkShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            LoadUsers(); // Reload users when filter changes
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (previousForm != null && !previousForm.IsDisposed)
            {
                previousForm.Show();
            }
        }
    }
}
