using Quiz.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quiz.Commands
{
    internal class AddQuestionCommand : ICommand
    {
        private readonly CreatorViewModel _viewModel;

        public AddQuestionCommand(CreatorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.AddQuestion();
        }
    }
}
