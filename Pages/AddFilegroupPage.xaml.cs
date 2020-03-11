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
    /// Interaction logic for AddFilegroupPage.xaml
    /// </summary>
    public partial class AddFilegroupPage : Page
    {
        public AddFilegroupPage(MainViewModel _viewModel)
        {
            InitializeComponent();
            _viewModel.FileID = 0;
            _viewModel.FileModel = new FilegroupViewModel();
            _viewModel.FilegroupCollection = DataBaseInteraction.GetListOfAllFilegroups();
            filegroupList.ItemsSource = _viewModel.FilegroupCollection;
            _viewModel.AllowedUsersCollection = DataBaseInteraction.GetListOfAllowedUsers(_viewModel.FileID);
            usersAllowedList.ItemsSource = _viewModel.AllowedUsersCollection;
        }
    }
}
