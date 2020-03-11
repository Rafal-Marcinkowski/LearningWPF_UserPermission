using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningWPF_Permission.ViewModels
{
    public class FilegroupViewModel : BaseViewModel
    {
        string _fileGroupName { get; set; }
        public string FileGroupName
        {
            get => _fileGroupName;
            set
            {
                _fileGroupName = value;
                OnPropertyChanged(nameof(FileGroupName));
            }
        }
    }
}
