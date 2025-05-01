using Quiz.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quiz.ViewModel
{
    internal class MenuViewModel : BaseViewModel
    {
        public ICommand UpdateViewCommand { get; }

        public MenuViewModel(MainViewModel mainViewModel)
        {
            UpdateViewCommand = new UpdateViewCommand(mainViewModel);
        }

        // Konstruktor domyślny dla designera (opcjonalnie)
        public MenuViewModel() { }
    }
}
