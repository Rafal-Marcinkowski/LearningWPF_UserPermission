using LearningWPF_Permission.DataBase;
using LearningWPF_Permission.Pages;
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

namespace LearningWPF_Permission
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel _viewModel = new MainViewModel();
        public MainWindow()
        {
            DataContext = _viewModel;
            InitializeComponent();
        }
        private void UsersButtonClick(object sender, RoutedEventArgs e)
        {
            AddUserPage addUserPage = new AddUserPage(_viewModel);
            addUserPage.DataContext = _viewModel;
            MainPage.Content = addUserPage;
        }
        private void FilegroupsButtonClick(object sender, RoutedEventArgs e)
        {
            AddFilegroupPage addFilegroupPage = new AddFilegroupPage(_viewModel);
            addFilegroupPage.DataContext = _viewModel;
            MainPage.Content = addFilegroupPage;
        }
        private void UserPermissionButtonClick(object sender, RoutedEventArgs e)
        {
            UserPermissionPage userPermissionPage = new UserPermissionPage(_viewModel);
            userPermissionPage.DataContext = _viewModel;
            MainPage.Content = userPermissionPage;
        }
    }
}
