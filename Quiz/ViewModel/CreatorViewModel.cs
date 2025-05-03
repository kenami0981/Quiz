using Quiz.Commands;
using Quiz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quiz.ViewModel
{
    internal class CreatorViewModel : BaseViewModel
    {

        //public ICommand AddQuestionCommand { get; }

        //public CreatorViewModel()
        //{
        //    AddQuestionCommand = new RelayCommand(this);
        //}
        private string _questionTitle;
        public string QuestionTitle
        {
            get
            {
                return _questionTitle;
            }
            set
            {
                _questionTitle = value;
                OnPropertyChanged(nameof(QuestionTitle));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private string _questionText;
        public string QuestionText
        {
            get
            {
                return _questionText;
            }
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private string _answer1;
        public string Answer1
        {
            get => _answer1;
            set
            {
                _answer1 = value;
                OnPropertyChanged(nameof(Answer1));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private string _answer2;
        public string Answer2
        {
            get => _answer2;
            set
            {
                _answer2 = value;
                OnPropertyChanged(nameof(Answer2));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private string _answer3;
        public string Answer3
        {
            get => _answer3;
            set
            {
                _answer3 = value;
                OnPropertyChanged(nameof(Answer3));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private string _answer4;
        public string Answer4
        {
            get => _answer4;
            set
            {
                _answer4 = value;
                OnPropertyChanged(nameof(Answer4));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private bool _isAnswer1Correct;
        public bool IsAnswer1Correct
        {
            get => _isAnswer1Correct;
            set
            {
                _isAnswer1Correct = value;
                OnPropertyChanged(nameof(IsAnswer1Correct));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private bool _isAnswer2Correct;
        public bool IsAnswer2Correct
        {
            get => _isAnswer2Correct;
            set
            {
                _isAnswer2Correct = value;
                OnPropertyChanged(nameof(IsAnswer2Correct));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private bool _isAnswer3Correct;
        public bool IsAnswer3Correct
        {
            get => _isAnswer3Correct;
            set
            {
                _isAnswer3Correct = value;
                OnPropertyChanged(nameof(IsAnswer3Correct));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        private bool _isAnswer4Correct;
        public bool IsAnswer4Correct
        {
            get => _isAnswer4Correct;
            set
            {
                _isAnswer4Correct = value;
                OnPropertyChanged(nameof(IsAnswer4Correct));
                (AddQuestionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        //public void AddQuestion()
        //{
        //string[] answers = { _answer1, _answer2, _answer3, _answer4 };
        //bool[] correctAnswers = { _isAnswer1Correct, _isAnswer2Correct, _isAnswer3Correct, _isAnswer4Correct};
        //    Question pyt= new Question(_questionTitle, _questionText, answers, correctAnswers);
        //    Console.WriteLine(pyt.ToString());
        //    // Logika dodawania pytania (możesz tu dodać później np. walidację i zapis)
        //    Console.WriteLine("Pytanie zostało dodane!");
        //}

        private ICommand _addQuestionCommand;
        public ICommand AddQuestionCommand
        {
            get
            {
                if (_addQuestionCommand == null)
                {
                    _addQuestionCommand = new RelayCommand(
                    (object o) =>
                    {
                        string[] answers = { _answer1, _answer2, _answer3, _answer4 };
                        bool[] correctAnswers = { _isAnswer1Correct, _isAnswer2Correct, _isAnswer3Correct, _isAnswer4Correct };
                        Question pyt = new Question(_questionTitle, _questionText, answers, correctAnswers);
                        Console.WriteLine(pyt.ToString());
                        OnPropertyChanged(nameof(_addQuestionCommand));
                    },
                    (object o) =>
                    {
                        return (_isAnswer1Correct==true || _isAnswer2Correct==true || _isAnswer3Correct == true || _isAnswer4Correct == true) &&
                        !string.IsNullOrEmpty(_answer1) &&
                        !string.IsNullOrEmpty(_answer2) &&
                        !string.IsNullOrEmpty(_answer3) &&
                        !string.IsNullOrEmpty(_answer4) &&
                        !string.IsNullOrWhiteSpace(_questionTitle) && 
                        !string.IsNullOrWhiteSpace(_questionText);
                    });
                }

                    return _addQuestionCommand;

                }

            }




        }
    }

