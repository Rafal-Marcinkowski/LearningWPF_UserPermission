using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningWPF_Permission.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        string _name { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        string _surname { get; set; }
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        string _area { get; set; }
        public string Area
        {
            get => _area;
            set
            {
                _area = value;
                OnPropertyChanged(nameof(Area));
            }
        }
        string _position { get; set; }
        public string Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
    }
}
