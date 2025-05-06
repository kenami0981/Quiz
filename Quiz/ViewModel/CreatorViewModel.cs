using Microsoft.Win32;
using Quiz.Commands;
using Quiz.Model;
using Quiz.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Quiz.ViewModel
{
    internal class CreatorViewModel : BaseViewModel
    {
       
        public ObservableCollection<Question> Questions { get; } = new ObservableCollection<Question>();

        private Visibility _firstElementVisibility = Visibility.Visible;
        public Visibility FirstElementVisibility
        {
            get => _firstElementVisibility;
            set
            {
                _firstElementVisibility = value;
                OnPropertyChanged(nameof(FirstElementVisibility));
            }
        }

        private Visibility _secondElementVisibility = Visibility.Collapsed;
        public Visibility SecondElementVisibility
        {
            get => _secondElementVisibility;
            set
            {
                _secondElementVisibility = value;
                OnPropertyChanged(nameof(SecondElementVisibility));
            }
        }
        private string _name;
        public string Name {
            get
            { 
                return _name;
            }
            set 
            { 
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
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
            }
        }
        public static string FormatQuiz(QuizName quiz)
        {
            var sb = new StringBuilder();
            sb.AppendLine(quiz.Name);

            foreach (var question in quiz.Questions)
            {
                sb.AppendLine(question.QuestionText);

                foreach (var answer in question.Answers)
                {
                    sb.AppendLine($"{answer.Text} {(answer.IsCorrect ? "true" : "false")}");
                }

                sb.AppendLine(); // Pusta linia między pytaniami
            }
            
            return sb.ToString().Trim();
        }


        private QuizName _quizTitle;
        private ICommand _addQuizTitleCommand;
        public ICommand AddQuizTitleCommand {
            get {
                if (_addQuizTitleCommand == null) {
                    _addQuizTitleCommand = new RelayCommand(
                    (object o) =>
                    {
                        _quizTitle = new QuizName(_name);
                        Console.WriteLine(_quizTitle.Name);
                        FirstElementVisibility = Visibility.Collapsed;

                        SecondElementVisibility = Visibility.Visible;
                    },
                    (object o) =>
                    {
                        return true;
                    });
                }
                return _addQuizTitleCommand;

            }
        }
        private ICommand _decryptCommand;
        public ICommand DecryptCommand
        {
            get
            {
                if (_decryptCommand == null)
                {
                    _decryptCommand = new RelayCommand(
                    (object o) =>
                    {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*",
                        Title = "Zapis studentów"
                    };

                        if (openFileDialog.ShowDialog() == true)
                        {
                            var encryptionService = new EncryptionService("tajne_haslo");
                            string encryptedPath = openFileDialog.FileName;
                            string decryptedPath = "C:\\Users\\HP\\Downloads\\quiz_decrypted.txt";
                            encryptionService.Decrypt(encryptedPath, decryptedPath);
                        }
                    },
                    (object o) =>
                    {
                        return true;
                    });
                }
                return _decryptCommand;

            }
        }
        private ICommand _saveToFileCommand;
        public ICommand SaveToFileCommand
        {
            get
            {
                if (_saveToFileCommand == null)
                {
                    _saveToFileCommand = new RelayCommand(
                    (object o) =>
                    {
                        //var encryptionService = new EncryptionService("tajny_klucz");
                        //string quizText = FormatQuiz(_quizTitle); // Używamy FormatQuiz, aby sformatować quiz
                        //encryptionService.EncryptAndSaveToFile(quizText, "C:\\Users\\HP\\Downloads\\quiz_encrypted123.txt");
                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*",
                            Title = "Zapisz plik"
                        };

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            // Pobranie kolekcji pracowników
                            var encryptionService = new EncryptionService("tajne_haslo");

                            string quizText = $"{_quizTitle.Name}\n";
                            foreach (var question in _quizTitle.Questions)
                            {
                                quizText += $"{question.QuestionTitle}\n";
                                foreach (var answer in question.Answers)
                                {
                                    quizText += $"{answer.Text} {(answer.IsCorrect ? "true" : "false")}\n";
                                }
                                quizText += "\n";
                            }


                            string encryptedPath = saveFileDialog.FileName;
                            encryptionService.EncryptAndSaveToFile(quizText, encryptedPath);


    

                            MessageBox.Show("Plik zapisany pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                        }
                        
                    },
                    (object o) =>
                    {
                        return true;
                    });
                }
                return _saveToFileCommand;
            }
        }

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
                        Answer answer1 = new Answer(_answer1, _isAnswer1Correct);
                        Answer answer2 = new Answer(_answer2, _isAnswer2Correct);
                        Answer answer3 = new Answer(_answer3, _isAnswer3Correct);
                        Answer answer4 = new Answer(_answer4, _isAnswer4Correct);
                        var answers = new List<Answer> { answer1, answer2, answer3, answer4 };
                        Question question = new Question(_questionTitle, _questionText, answers);
                        Console.WriteLine(question.ToString());
                        _quizTitle.AddQuestion(question);
                        Questions.Add(question);
                       


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

