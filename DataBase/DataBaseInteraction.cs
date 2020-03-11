using LearningWPF_Permission.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearningWPF_Permission.DataBase
{
    public static class DataBaseInteraction
    {
        public static SqlCommand command = new SqlCommand();
        private static string connectionString = "Data Source = MIETOSZEW\\SQLEXPRESS; Initial Catalog = Userpermission; Integrated Security = True";
        private static SqlConnection connection = new SqlConnection(connectionString);
        public static int GetUserID(string name, string surname)
        {
            string queryString = $@"SELECT ISNULL((SELECT userid FROM users WHERE username='{name}' AND usersurname='{surname}'),0)";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            try
            {
                return (int)command.ExecuteScalar();
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }
        public static void AddUserToDB(UserViewModel user)
        {
            string queryString = $@"INSERT INTO users VALUES(@userName, @userSurname, @userArea, @userPosition)";
            command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@userName", user.Name);
            command.Parameters.AddWithValue("@userSurname", user.Surname);
            command.Parameters.AddWithValue("@userArea", user.Area);
            command.Parameters.AddWithValue("@userPosition", user.Position);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public static ObservableCollection<UserViewModel> GetListOfAllUsers()
        {
            ObservableCollection<UserViewModel> allUsers = new ObservableCollection<UserViewModel>();
            string queryString = "SELECT * FROM users";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                UserViewModel user = new UserViewModel();
                user.Name = dataReader.GetString(1);
                user.Surname = dataReader.GetString(2);
                user.Area = dataReader.GetString(3);
                user.Position = dataReader.GetString(4);
                allUsers.Add(user);
            }
            connection.Close();
            return allUsers;
        }
        public static string GetListOfUserPermissions(int userID)
        {
            string userPermissionsText = null;
            int number = 0;
            string queryString = $"SELECT filegroupid FROM userpermissions WHERE userid='{userID}'";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                number++;
            }
            if (number == 0)
            {
                connection.Close();
                return null;
            }
            int[] filegroupIDtab = new int[number];
            connection.Close();
            connection.Open();
            dataReader = command.ExecuteReader();
            int tabCounter = 0;
            while (dataReader.Read())
            {
                filegroupIDtab[tabCounter] = dataReader.GetInt32(0);
                tabCounter++;
            }
            connection.Close();
            queryString = $@"SELECT filegroupname FROM filegroups WHERE filegroupid in ({String.Join(",", filegroupIDtab)})";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            dataReader = command.ExecuteReader();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    userPermissionsText += dataReader.GetString(0) + "| ";
                }
            }
            connection.Close();
            return userPermissionsText;
        }
        public static ObservableCollection<UserViewModel> GetListOfAllowedUsers(int filegroupID)
        {
            ObservableCollection<UserViewModel> allowedUsersCollection = new ObservableCollection<UserViewModel>();
            string queryString = $"SELECT userid FROM userpermissions WHERE filegroupid='{filegroupID}'";
            int number = 0;
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                number++;
            }
            if (number == 0)
            {
                connection.Close();
                return allowedUsersCollection;
            }
            connection.Close();
            connection.Open();
            int[] userIDTab = new int[number];
            dataReader = command.ExecuteReader();
            int tabCounter = 0;
            while (dataReader.Read())
            {
                userIDTab[tabCounter] = dataReader.GetInt32(0);
                tabCounter++;
            }
            connection.Close();
            connection.Open();
            queryString = $@"SELECT * FROM users WHERE userid in ({String.Join(",", userIDTab)})";
            command = new SqlCommand(queryString, connection);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                UserViewModel userModel = new UserViewModel();
                userModel.Name = dataReader.GetString(1);
                userModel.Surname = dataReader.GetString(2);
                userModel.Area = dataReader.GetString(3);
                userModel.Position = dataReader.GetString(4);
                allowedUsersCollection.Add(userModel);
            }
            connection.Close();
            return allowedUsersCollection;
        }
        public static bool IsThisPossitionAlreadyInDB(string possitionName)
        {
            string queryString = $@"SELECT ISNULL((SELECT possitionid FROM possitions WHERE possitionname=@possitionName),0)";
            command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@possitionName", possitionName);
            connection.Open();
            try
            {
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static int GetPossitionID(string possitionName)
        {
            string queryString = $@"SELECT ISNULL((SELECT possitionid FROM possitions WHERE possitionname='{possitionName}'),0)";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            try
            {
                return (int)command.ExecuteScalar();
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool IsThisPossitionAlreadyPermitted(int possitionID, int fileID)
        {
            string queryString = $@"SELECT ISNULL((SELECT permissionid FROM possitionpermissions WHERE possitionid='{possitionID}' AND filegroupid='{fileID}'),0)";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            try
            {
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static void AddPossitionPermissionToDB(int possitionID, int fileID)
        {
            string queryString = $@"INSERT INTO possitionpermissions VALUES('{possitionID}','{fileID}')";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            command.Connection = connection;
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    MessageBox.Show("Something went wrong while permiting user.");
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public static void AddPossitionToDB(string possitionName)
        {
            string queryString = $@"INSERT INTO possitions VALUES(@possitionName)";
            command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("possitionName", possitionName);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            command.Connection = connection;
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    MessageBox.Show("Something went wrong while adding new possition to DB");
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public static List<string> GetListOfAllPossitions()
        {
            List<string> positions = new List<string>();
            string queryString = "SELECT possitionname FROM possitions";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                string position = dataReader.GetString(0);
                positions.Add(position);
            }
            connection.Close();
            return positions;
        }
        public static ObservableCollection<FilegroupViewModel> GetListOfAllFilegroups()
        {
            ObservableCollection<FilegroupViewModel> files = new ObservableCollection<FilegroupViewModel>();
            string queryString = "SELECT * FROM filegroups";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                FilegroupViewModel file = new FilegroupViewModel();
                file.FileGroupName = dataReader.GetString(1);
                files.Add(file);
            }
            connection.Close();
            return files;
        }
        public static void AddFilegroupToDB(string fileName)
        {
            string queryString = $@"INSERT INTO filegroups VALUES(@fileName)";
            command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@fileName", fileName);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool IsFilegroupAlreadyInDB(string fileName)
        {
            string queryString = $@"SELECT ISNULL((SELECT filegroupid FROM filegroups WHERE filegroupname=@fileName),0)";
            command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@fileName", fileName);
            connection.Open();
            try
            {
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static int GetFileIDFromDB(string fileName)
        {
            string queryString = $@"SELECT ISNULL((SELECT filegroupid FROM filegroups WHERE filegroupname=@fileName),0)";
            command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@fileName", fileName);
            connection.Open();
            try
            {
                return (int)command.ExecuteScalar();
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool IsUserAlreadyPermittedToFilegroup(int userID, int fileID)
        {
            string queryString = $@"SELECT ISNULL((SELECT permissionid FROM userpermissions WHERE userid='{userID}' AND filegroupid='{fileID}'),0)";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            try
            {
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static void AddPermissionToDB(int userID, int fileID)
        {
            string queryString = $"INSERT INTO userpermissions VALUES('{userID}','{fileID}')";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            command.Connection = connection;
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    MessageBox.Show("Something went wrong while permiting user.");
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public static void DeleteUserPermission(int userID, int fileID)
        {
            string queryString = $"DELETE FROM userpermissions WHERE userid='{userID}' AND filegroupid='{fileID}'";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            command.Connection = connection;
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    MessageBox.Show("Something went wrong while deleting user permission.");
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public static void DeletePossitionPermission(int possitionID, int fileID)
        {
            string queryString = $"DELETE FROM possitionpermissions WHERE possitionid='{possitionID}' AND filegroupid='{fileID}'";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            command.Connection = connection;
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    MessageBox.Show("Something went wrong while deleting all permissions from selected possition.");
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public static ObservableCollection<UserViewModel> GetListOfAllUsersWithSelectedPossition(string possitionName)
        {
            ObservableCollection<UserViewModel> usersCollection = new ObservableCollection<UserViewModel>();
            string queryString = $"SELECT * FROM users WHERE userposition='{possitionName}'";
            command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                UserViewModel user = new UserViewModel();
                user.Name = dataReader.GetString(1);
                user.Surname = dataReader.GetString(2);
                usersCollection.Add(user);
            }
            connection.Close();
            return usersCollection;
        }
    }
}
