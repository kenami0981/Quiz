using Quiz.Model;
using Quiz.Commands;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System;
using System.Timers;
using Microsoft.Win32;
using Quiz.Services;
using System.Windows;



namespace Quiz.ViewModel
{
    internal class SolveViewModel : BaseViewModel
    {
        private readonly System.Windows.Threading.Dispatcher _dispatcher = System.Windows.Application.Current.Dispatcher;
        private QuizName _quiz;
        private Question _currentQuestion;
        private int _currentQuestionIndex;
        private bool _isQuizFinished;
        private string _quizScore;
        private int _timeLeft;
        private bool _isQuizStarted;
        private Timer _timer;
        private Dictionary<Question, List<Answer>> _selectedAnswers;

        public QuizName Quiz
        {
            get => _quiz;
            set
            {
                if (_quiz != value)
                {
                    _quiz = value;
                    OnPropertyChanged(nameof(Quiz));

                }
            }
        }
        private Visibility _firstElementVisibility = Visibility.Collapsed;
        public Visibility FirstElementVisibility
        {
            get => _firstElementVisibility;
            set
            {
                _firstElementVisibility = value;
                OnPropertyChanged(nameof(FirstElementVisibility));
            }
        }

        private Visibility _secondElementVisibility = Visibility.Visible;
        public Visibility SecondElementVisibility
        {
            get => _secondElementVisibility;
            set
            {
                _secondElementVisibility = value;
                OnPropertyChanged(nameof(SecondElementVisibility));
            }
        }
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                if (_currentQuestion != value)
                {
                    _quizScore = null;
                    _currentQuestion = value;
                    OnPropertyChanged(nameof(CurrentQuestion));
                    OnPropertyChanged(nameof(ContentVisibility));
                    OnPropertyChanged(nameof(ScoreVisibility));
                }
            }
        }


        public string ContentVisibility
        {
            get => CurrentQuestion != null && _isQuizStarted ? "Visible" : "Collapsed";
        }

        public bool IsQuizFinished
        {
            get => _isQuizFinished;
            set
            {
                if (_isQuizFinished != value)
                {
                    _isQuizFinished = value;
                    OnPropertyChanged(nameof(IsQuizFinished));
                    OnPropertyChanged(nameof(ScoreVisibility));
                    OnPropertyChanged(nameof(ContentVisibility));
                    if (_isQuizFinished && _timer != null)
                    {
                        _timer.Stop();
                        OnPropertyChanged(nameof(TimeLeft));
                    }
                }
            }
        }

        public string ScoreVisibility
        {
            get => IsQuizFinished ? "Visible" : "Collapsed";
        }


        public string QuizScore
        {
            get => _quizScore ?? string.Empty;
            set
            {
                if (_quizScore != value)
                {
                    _quizScore = value;
                    OnPropertyChanged(nameof(QuizScore));

                }
            }
        }


        public int TimeLeft
        {
            get => _timeLeft;
            set
            {
                if (_timeLeft != value)
                {
                    _timeLeft = value;
                    OnPropertyChanged(nameof(TimeLeft));
                    if (_timeLeft <= 0 && _isQuizStarted && !IsQuizFinished)
                    {
                        FinishQuiz(null);
                    }
                }
            }
        }


        public bool IsQuizStarted
        {
            get => _isQuizStarted;
            set
            {
                if (_isQuizStarted != value)
                {
                    _isQuizStarted = value;
                    OnPropertyChanged(nameof(IsQuizStarted));
                    OnPropertyChanged(nameof(ContentVisibility));
                    OnPropertyChanged(nameof(CanStartQuiz));
                    OnPropertyChanged(nameof(CanFinishQuiz));
                }
            }
        }


        public ICommand NextQuestionCommand { get; private set; }
        public ICommand PreviousQuestionCommand { get; private set; }
        public ICommand FinishQuizCommand { get; private set; }
        public ICommand SelectAnswerCommand { get; private set; }
        public ICommand StartQuizCommand { get; private set; }


        public ICommand ShowResultsCommand { get; private set; }

        private bool _areResultsVisible;
        public bool AreResultsVisible
        {
            get => _areResultsVisible;
            set
            {
                _areResultsVisible = value;
                OnPropertyChanged(nameof(AreResultsVisible));
                OnPropertyChanged(nameof(ShowResultsVisibility));
            }
        }

        public string ShowResultsVisibility
        {
            get => AreResultsVisible ? "Visible" : "Collapsed";
        }
        private void Decrypt()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki zaszyfrowane (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*",
                Title = "Wybierz zaszyfrowany plik quizu"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var encryptionService = new EncryptionService("tajne_haslo");
                    string encryptedPath = openFileDialog.FileName;
                    string decryptedPath = Path.Combine(Path.GetTempPath(), "quiz_decrypted.txt");

                    encryptionService.Decrypt(encryptedPath, decryptedPath);

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Błąd podczas odszyfrowywania pliku: {ex.Message}");
                }
            }
        }


        private void LoadAndShowResults(object parameter)
        {
            try
            {
                
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "quiz_results.txt");
                if (!File.Exists(path))
                {
                    System.Windows.MessageBox.Show("Brak zapisanych wyników.");
                    return;
                }

                QuizResults.Clear();
                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        QuizResults.Add(line);
                    }
                }
                AreResultsVisible = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas wczytywania wyników: {ex.Message}");
            }
        }

        public SolveViewModel()
        {
            
            Quiz = new QuizName("");
            
            if (Quiz.Questions.Any())
            {
                _currentQuestionIndex = 0;
            }

            IsQuizFinished = false;
            IsQuizStarted = false;
            QuizScore = string.Empty;
            TimeLeft = 300;
            _selectedAnswers = new Dictionary<Question, List<Answer>>();
            AreResultsVisible = false;

            _timer = new Timer(1000);
            _timer.Elapsed += (s, e) =>
            {
                _dispatcher.Invoke(() => TimeLeft--);
            };
            _timer.AutoReset = true;

            StartQuizCommand = new RelayCommand(StartQuiz, CanStartQuiz);
            NextQuestionCommand = new RelayCommand(NextQuestion, CanNextQuestion);
            PreviousQuestionCommand = new RelayCommand(PreviousQuestion, CanPreviousQuestion);
            FinishQuizCommand = new RelayCommand(FinishQuiz, CanFinishQuiz);
            SelectAnswerCommand = new RelayCommand(SelectAnswer, CanSelectAnswer);
            ShowResultsCommand = new RelayCommand(LoadAndShowResults, CanShowResults);
        }


        private bool CanShowResults(object parameter)
        {
            return IsQuizFinished; // Komenda aktywna tylko po zakończeniu quizu
        }

        private void LoadQuizData()
        {
            try
            {
                //Decrypt();
                string path = Path.Combine(Path.GetTempPath(), "quiz_decrypted.txt");
                if (!File.Exists(path))
                {
                    System.Windows.MessageBox.Show($"Plik quiz.txt nie został znaleziony w ścieżce: {path}");
                    return;
                }

                string[] lines = File.ReadAllLines(path);


                if (lines.Length < 2)
                {
                    System.Windows.MessageBox.Show("Plik quiz.txt jest za krótki.");
                    return;
                }

                Quiz.Name = lines[0].Trim();


                Question currentQuestion = null;
                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i]))
                    {
                        currentQuestion = null;
                        continue;
                    }

                    if (currentQuestion == null)
                    {
                        currentQuestion = new Question();
                        currentQuestion.QuestionText = lines[i].Trim();
                        Quiz.AddQuestion(currentQuestion);
                    }
                    else
                    {
                        string line = lines[i].Trim();
                        int lastSpaceIndex = line.LastIndexOf(' ');

                        if (lastSpaceIndex > 0 && lastSpaceIndex < line.Length - 1)
                        {
                            string answerText = line.Substring(0, lastSpaceIndex);
                            string isCorrectStr = line.Substring(lastSpaceIndex + 1);

                            if (bool.TryParse(isCorrectStr, out bool isCorrect))
                            {
                                // Sprawdzenie duplikatu
                                if (!currentQuestion.Answers.Any(a => a.Text == answerText))
                                {
                                    currentQuestion.Answers.Add(new Answer(answerText, isCorrect));
                                }
                            }
                        }
                    }
                }

                if (!Quiz.Questions.Any())
                {
                    System.Windows.MessageBox.Show("Nie wczytano żadnych pytań z pliku quiz.txt");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"błąd podczas wczytywania pliku: {ex.Message}");
            }
        }


        private void StartQuiz(object parameter)

        {
            _selectedAnswers.Clear();
            QuizScore = string.Empty;
            AreResultsVisible = false;
            IsQuizFinished = false;
            _currentQuestionIndex = 0;

            FirstElementVisibility = Visibility.Visible;

            //SecondElementVisibility = Visibility.Visible;
            Decrypt();
            LoadQuizData();
            TimeLeft = 30*Quiz.Questions.Count;
            IsQuizStarted = true;
            CurrentQuestion = Quiz.Questions[_currentQuestionIndex];
            _timer.Start();
        }


        private bool CanStartQuiz(object parameter)
        {
            return !IsQuizStarted && !IsQuizFinished;
        }

        private void NextQuestion(object parameter)
        {
            if (_currentQuestionIndex < Quiz.Questions.Count - 1)
            {
                _currentQuestionIndex++;
                CurrentQuestion = Quiz.Questions[_currentQuestionIndex];
            }
            else
            {
                FinishQuiz(null);
            }
        }

        private bool CanNextQuestion(object parameter)
        {
            return !IsQuizFinished && _currentQuestionIndex < Quiz.Questions.Count - 1;
        }

        private void PreviousQuestion(object parameter)
        {
            if (_currentQuestionIndex > 0)
            {
                _currentQuestionIndex--;
                CurrentQuestion = Quiz.Questions[_currentQuestionIndex];
            }
        }

        private bool CanPreviousQuestion(object parameter)
        {
            return IsQuizStarted && !IsQuizFinished && _currentQuestionIndex > 0;
        }


        private void FinishQuiz(object parameter)

        {
            FirstElementVisibility = Visibility.Collapsed;
            IsQuizFinished = true;
            CurrentQuestion = null;
            _timer.Stop();

            int score = 0;
            int totalCorrect = 0;
            foreach (var question in Quiz.Questions)
            {
                int correctForQuestion = 0;
                int selectedCorrect = 0;
                List<Answer> selectedForQuestion;
                if (_selectedAnswers.ContainsKey(question))
                {
                    selectedForQuestion = _selectedAnswers[question];
                }
                else
                {
                    selectedForQuestion = new List<Answer>();
                }
                foreach (var answer in question.Answers)
                {
                    if (answer.IsCorrect) correctForQuestion++;
                    if (selectedForQuestion.Contains(answer) && answer.IsCorrect) selectedCorrect++;
                    if (selectedForQuestion.Contains(answer) && !answer.IsCorrect) selectedCorrect--;
                }
                if (selectedCorrect == correctForQuestion && correctForQuestion > 0) score++;



            }
            QuizScore = $"Twój wynik: {score} na {Quiz.Questions.Count}";
            SaveQuizResults();
            AreResultsVisible = false;

        }

        private bool CanFinishQuiz(object parameter)
        {
            return IsQuizStarted && (_currentQuestionIndex >= Quiz.Questions.Count - 1 || IsQuizFinished);
        }

        private void SelectAnswer(object parameter)
        {
            if (parameter is Answer selectedAnswer && CurrentQuestion != null)
            {
                if (!_selectedAnswers.ContainsKey(CurrentQuestion))
                {
                    _selectedAnswers[CurrentQuestion] = new List<Answer>();
                }

                var selectedForQuestion = _selectedAnswers[CurrentQuestion];
                if (selectedForQuestion.Contains(selectedAnswer))
                {
                    selectedForQuestion.Remove(selectedAnswer);
                }
                else
                {
                    selectedForQuestion.Add(selectedAnswer);
                }
                // Ręczne powiadomienie UI o zmianie
                OnPropertyChanged(nameof(CurrentQuestion));
            }
        }

        private bool CanSelectAnswer(object parameter)
        {
            return CurrentQuestion != null && IsQuizStarted && !IsQuizFinished;
        }

        public bool IsAnswerSelected(Answer answer)
        {
            return CurrentQuestion != null
                && _selectedAnswers.ContainsKey(CurrentQuestion)
                && _selectedAnswers[CurrentQuestion].Contains(answer);
        }


        private ObservableCollection<string> _quizResults = new ObservableCollection<string>();
        public ObservableCollection<string> QuizResults
        {
            get => _quizResults;
            set
            {
                _quizResults = value;
                OnPropertyChanged(nameof(QuizResults));
            }
        }

        private void SaveQuizResults()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "quiz_results.txt");
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine($"Wyniki quizu: {Quiz.Name} - {DateTime.Now}");
                    writer.WriteLine($"Twój wynik: {QuizScore}");
                    writer.WriteLine();

                    foreach (var question in Quiz.Questions)
                    {
                        var correctAnswers = question.Answers.Where(a => a.IsCorrect).Select(a => a.Text).ToList();
                        var selectedForQuestion = _selectedAnswers.ContainsKey(question) ? _selectedAnswers[question] : new List<Answer>();
                        var selectedAnswers = selectedForQuestion.Select(a => a.Text).ToList();

                        writer.WriteLine($"Pytanie: {question.QuestionText}");
                        writer.WriteLine($"Twoje odpowiedzi: {(selectedAnswers.Any() ? string.Join(", ", selectedAnswers) : "Brak")}");
                        writer.WriteLine($"Poprawne odpowiedzi: {string.Join(", ", correctAnswers)}");
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas zapisywania wyników: {ex.Message}");
            }
        }

    }
}