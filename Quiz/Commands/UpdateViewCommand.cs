using Quiz.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quiz.Commands
{
    internal class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel) { 
            this.viewModel = viewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("djoadj");
            if (parameter.ToString() == "create"){
                viewModel.SelectedViewModel = new CreatorViewModel();
            }
            else if (parameter.ToString() == "solve")
            {
                viewModel.SelectedViewModel = new SolveViewModel();
            }
        }
    }
}
