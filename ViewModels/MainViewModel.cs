using LearningWPF_Permission.DataBase;
using LearningWPF_Permission.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPF_ThirdParty.VM_Implementation;

namespace LearningWPF_Permission.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<UserViewModel> UserCollection = new ObservableCollection<UserViewModel>();
        public ObservableCollection<FilegroupViewModel> FilegroupCollection = new ObservableCollection<FilegroupViewModel>();
        public ObservableCollection<UserViewModel> AllowedUsersCollection = new ObservableCollection<UserViewModel>();
        public List<string> PositionsCollection = new List<string>();
        public string UserPermissionsText { get; set; } = null;
        public int permissionColorFlag { get; set; } = 0;
        public string Position { get; set; } = null;
        public int PositionID { get; set; } = 0;
        public UserViewModel UserModel { get; set; } = new UserViewModel();
        public FilegroupViewModel FileModel { get; set; } = new FilegroupViewModel();
        public int UserID { get; set; } = 0;
        public int FileID { get; set; } = 0;
        public bool IsFileModelSelected { get; set; } = false;
        public ICommand AddUserToDBCommand =>
            new RelayCommand(() =>
            {
                UserID = DataBaseInteraction.GetUserID(UserModel.Name, UserModel.Surname);
                if (UserID == 0)
                {
                    if (MessageBox.Show("Add " + UserModel.Name + " " + UserModel.Surname + " to database?", "Add this user to DB?",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
                    {
                        UserModel = new UserViewModel();
                        OnPropertyChanged(nameof(UserModel));
                    }
                    else
                    {
                        MessageBox.Show(UserModel.Name + " " + UserModel.Surname + " has been added to database.");
                        UserCollection.Add(UserModel);
                        OnPropertyChanged(nameof(UserCollection));
                        DataBaseInteraction.AddUserToDB(UserModel);
                        if (!DataBaseInteraction.IsThisPossitionAlreadyInDB(UserModel.Position))
                        {
                            DataBaseInteraction.AddPossitionToDB(UserModel.Position);
                            OnPropertyChanged(nameof(PositionsCollection));
                        }
                        UserID = 0;
                        UserModel = new UserViewModel();
                        OnPropertyChanged(nameof(UserModel));
                    }
                }
                else
                {
                    MessageBox.Show("User already in DB!");
                }
            }, () => (!string.IsNullOrEmpty(UserModel.Name) &&
             !string.IsNullOrEmpty(UserModel.Surname) &&
             !string.IsNullOrEmpty(UserModel.Area) &&
             !string.IsNullOrEmpty(UserModel.Position)) &&
             IsDataCorrect(UserModel.Name, UserModel.Surname, UserModel.Position));
        public ICommand SelectUserCommand =>
            new RelayCommand<UserViewModel>((user) =>
            {
                permissionColorFlag = 0;
                OnPropertyChanged(nameof(permissionColorFlag));
                UserModel = new UserViewModel();
                UserModel.Name = user.Name;
                OnPropertyChanged(nameof(UserModel.Name));
                UserModel.Surname = user.Surname;
                OnPropertyChanged(nameof(UserModel.Surname));
                UserID = DataBaseInteraction.GetUserID(UserModel.Name, UserModel.Surname);
            }, (user) => (user != null));
        public ICommand PermitUserCommand =>
            new RelayCommand<FilegroupViewModel>((filegroup) =>
            {
                FileID = DataBaseInteraction.GetFileIDFromDB(filegroup.FileGroupName);
                if (!DataBaseInteraction.IsUserAlreadyPermittedToFilegroup(UserID, FileID))
                {
                    if (MessageBox.Show("Do you want to permit " + UserModel.Name + " " + UserModel.Surname + " to download files from " + filegroup.FileGroupName + "?",
                        "Permit this user?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        MessageBox.Show(UserModel.Name + " " + UserModel.Surname + " is now permitted to download files from " + filegroup.FileGroupName + ".");
                        DataBaseInteraction.AddPermissionToDB(UserID, FileID);
                    }
                }
                else
                {
                    MessageBox.Show(UserModel.Name + " " + UserModel.Surname + " is already permitted to download files from " + filegroup.FileGroupName + ".");
                }
            }, (filegroup) => (filegroup != null));
        public ICommand CheckSelectedUserPermissionsCommand =>
            new RelayCommand<UserViewModel>((selectedUser) =>
            {
                UserID = DataBaseInteraction.GetUserID(selectedUser.Name, selectedUser.Surname);
                UserPermissionsText = $@"{selectedUser.Name} {selectedUser.Surname} is permitted to download files from: ";
                if (DataBaseInteraction.GetListOfUserPermissions(UserID) == null)
                {
                    permissionColorFlag = 2;
                    OnPropertyChanged(nameof(permissionColorFlag));
                    UserPermissionsText = $@"{selectedUser.Name} {selectedUser.Surname} is not permitted to download any files.";
                    OnPropertyChanged(nameof(UserPermissionsText));
                }
                else
                {
                    permissionColorFlag = 1;
                    OnPropertyChanged(nameof(permissionColorFlag));
                    UserPermissionsText += DataBaseInteraction.GetListOfUserPermissions(UserID);
                    OnPropertyChanged(nameof(UserPermissionsText));
                }
            }, (selectedUser) => (selectedUser != null));
        public ICommand CheckWhichUsersAreAllowedCommand =>
            new RelayCommand<FilegroupViewModel>((filegroup) =>
            {
                FileID = DataBaseInteraction.GetFileIDFromDB(filegroup.FileGroupName);
                UserCollection = DataBaseInteraction.GetListOfAllowedUsers(FileID);
                AllowedUsersCollection.Clear();
                if (UserCollection.Count == 0)
                {
                    MessageBox.Show("No users are allowed to download file from this filegroup.");
                }
                else
                {
                    foreach (UserViewModel item in UserCollection)
                    {
                        AllowedUsersCollection.Add(item);
                        OnPropertyChanged(nameof(AllowedUsersCollection));
                    }
                }
            }, (filegroup) => (filegroup != null));
        public ICommand SelectPositionCommand =>
            new RelayCommand<string>((positionName) =>
            {
                permissionColorFlag = 0;
                OnPropertyChanged(nameof(permissionColorFlag));
                Position = positionName;
                OnPropertyChanged(nameof(Position));
                PositionID = DataBaseInteraction.GetPossitionID(Position);
            }, (positionName) => (positionName != null));
        public ICommand PermitPositionCommand =>
            new RelayCommand<FilegroupViewModel>((filegroupName) =>
            {
                FileID = DataBaseInteraction.GetFileIDFromDB(filegroupName.FileGroupName);
                if (!DataBaseInteraction.IsThisPossitionAlreadyPermitted(PositionID, FileID))
                {
                    if (MessageBox.Show("Do you want to permit " + Position + " to download files from " + filegroupName.FileGroupName + "?",
                        "Permit this possition?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DataBaseInteraction.AddPossitionPermissionToDB(PositionID, FileID);
                        MessageBox.Show("All users with position: " + Position + " are now permitted to download files from "
                            + filegroupName.FileGroupName + ".");
                        for (int i = 0; i < UserCollection.Count; i++)
                        {
                            if (UserCollection[i].Position == Position)
                            {
                                UserModel.Name = UserCollection[i].Name;
                                UserModel.Surname = UserCollection[i].Surname;
                                UserID = DataBaseInteraction.GetUserID(UserModel.Name, UserModel.Surname);
                                if (!DataBaseInteraction.IsUserAlreadyPermittedToFilegroup(UserID, FileID))
                                {
                                    DataBaseInteraction.AddPermissionToDB(UserID, FileID);
                                }
                            }
                            else
                                continue;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Possition: " + Position
                    + " is already permitted to download files from filegroup: " + filegroupName.FileGroupName + ".");
                }
            }, (filegroupName) => (filegroupName != null));
        public ICommand AddFilegroupCommand =>
            new RelayCommand(() =>
            {
                if (DataBaseInteraction.IsFilegroupAlreadyInDB(FileModel.FileGroupName) == false)
                {
                    if (MessageBox.Show("Add " + FileModel.FileGroupName + " filegroup" + " to database?", "Add this filegroup to DB?",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
                    {
                        FilegroupCollection.Add(FileModel);
                        OnPropertyChanged(nameof(FilegroupCollection));
                        DataBaseInteraction.AddFilegroupToDB(FileModel.FileGroupName);
                    }
                }
                else
                {
                    MessageBox.Show("That filegroup already exists in DB.");
                }
                FileModel = new FilegroupViewModel();
                OnPropertyChanged(nameof(FileModel));
            }, () => !string.IsNullOrEmpty(FileModel.FileGroupName));
        public ICommand DeleteUserPermissionCommand =>
            new RelayCommand<FilegroupViewModel>((filegroup) =>
            {
                FileID = DataBaseInteraction.GetFileIDFromDB(filegroup.FileGroupName);
                if (!DataBaseInteraction.IsUserAlreadyPermittedToFilegroup(UserID, FileID))
                {
                    MessageBox.Show("This user is not permitted to download files from this filegroup already.");
                }
                else
                {
                    if (MessageBox.Show("Do you really want to delete permission from " + UserModel.Name + " " + UserModel.Surname + " to download files from: "
                        + filegroup.FileGroupName + "?",
                            "Delete this user permission?", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                            == MessageBoxResult.Yes)
                    {
                        DataBaseInteraction.DeleteUserPermission(UserID, FileID);
                        MessageBox.Show(UserModel.Name + " " + UserModel.Surname + " is not longer permitted to download files from: "
                            + filegroup.FileGroupName + ".");
                    }
                }
            }, (filegroup) => (filegroup != null));
        public ICommand DeleteAllPossitionPermissionsCommand =>
            new RelayCommand<FilegroupViewModel>((filegroup) =>
            {
                FileID = DataBaseInteraction.GetFileIDFromDB(filegroup.FileGroupName);
                if (!DataBaseInteraction.IsThisPossitionAlreadyPermitted(PositionID, FileID))
                {
                    MessageBox.Show("This possition is already not permitted to download files from this filegroup.");
                }
                else
                {
                    if (MessageBox.Show("Do you really want to delete all permissions from possition: " + Position + "?",
                        "Delete this possition permissions?", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                        == MessageBoxResult.Yes)
                    {
                        DataBaseInteraction.DeletePossitionPermission(PositionID, FileID);
                        AllowedUsersCollection = DataBaseInteraction.GetListOfAllUsersWithSelectedPossition(Position);
                        foreach (UserViewModel item in AllowedUsersCollection)
                        {
                            UserID = DataBaseInteraction.GetUserID(item.Name, item.Surname);
                            DataBaseInteraction.DeleteUserPermission(UserID, FileID);
                        }
                        MessageBox.Show("Possition: " + Position + " is not longer permitted to download files from: " + filegroup.FileGroupName + ".");
                    }
                }
            }, (filegroup) => (filegroup != null));
        public bool IsDataCorrect(string name, string surname, string position)
        {
            if (name.All(q => char.IsLetter(q)) && surname.All(q => char.IsLetter(q)) && position.All(q => char.IsLetter(q)))
            {
                return true;
            }
            else
                return false;
        }
    }
}
