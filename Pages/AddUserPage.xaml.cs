using LearningWPF_Permission.DataBase;
using LearningWPF_Permission.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LearningWPF_Permission.Pages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        public AddUserPage(MainViewModel _viewModel)
        {
            InitializeComponent();
            _viewModel.UserModel = new UserViewModel();
            _viewModel.UserCollection = DataBaseInteraction.GetListOfAllUsers();
            usersListView.ItemsSource = _viewModel.UserCollection;
            _viewModel.UserPermissionsText = null;
        }
    }
}
