using Quiz.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quiz.Commands
{
    internal class RelayCommand : ICommand
    {
        //private readonly CreatorViewModel _viewModel;

        //public RelayCommand(CreatorViewModel viewModel)
        //{
        //    _viewModel = viewModel;
        //}
        //public event EventHandler CanExecuteChanged;

        //public bool CanExecute(object parameter)
        //{
        //    //throw new NotImplementedException();
        //    return false;
        //}

        //public void Execute(object parameter)
        //{
        //    _viewModel.AddQuestion();
        //}


        private Action<object> execute;
        private Func<object, bool> canExecute;


        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
