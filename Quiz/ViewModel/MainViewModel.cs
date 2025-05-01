using Quiz.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quiz.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel = new CreatorViewModel();
        public BaseViewModel SelectedViewModel { 
            get { return _selectedViewModel; }
            set { 
                _selectedViewModel = value; 
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel() {
            UpdateViewCommand = new UpdateViewCommand(this);
        }
    }
}
