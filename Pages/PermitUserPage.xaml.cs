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
    /// Interaction logic for PermitUserPage.xaml
    /// </summary>
    public partial class PermitUserPage : Page
    {
        public PermitUserPage(MainViewModel _viewModel)
        {
            InitializeComponent();
            DataContext = _viewModel;
            _viewModel.FilegroupCollection = DataBaseInteraction.GetListOfAllFilegroups();
            filegroupList.ItemsSource = _viewModel.FilegroupCollection;
        }
    }
}
