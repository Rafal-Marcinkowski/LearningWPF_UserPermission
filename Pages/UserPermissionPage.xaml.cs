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
    /// Interaction logic for SetUserPermissionPage.xaml
    /// </summary>
    public partial class UserPermissionPage : Page
    {
        public MainViewModel _viewModel = new MainViewModel();
        public UserPermissionPage(MainViewModel _viewModel)
        {
            InitializeComponent();
            this._viewModel = _viewModel;
            _viewModel.UserCollection = DataBaseInteraction.GetListOfAllUsers();
            usersList.ItemsSource = _viewModel.UserCollection;
            _viewModel.PositionsCollection = DataBaseInteraction.GetListOfAllPossitions();
            possitionsList.ItemsSource = _viewModel.PositionsCollection;
            _viewModel.FilegroupCollection = DataBaseInteraction.GetListOfAllFilegroups();
        }
        private void PermitUserButtonClick(object sender, RoutedEventArgs e)
        {
            PermissionFrame.Content = new PermitUserPage(_viewModel);
        }
        private void PermitPositionButtonClick(object sender, RoutedEventArgs e)
        {
            PermissionFrame.Content = new PermitPositionPage(_viewModel);
        }
    }
}
