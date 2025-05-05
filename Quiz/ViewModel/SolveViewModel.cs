using Quiz.Model;
using Quiz.Commands;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System;


namespace Quiz.ViewModel
{
    internal class SolveViewModel : BaseViewModel
    {
        private QuizName _quiz;
        private Question _currentQuestion;
        private int _currentQuestionIndex;
        private bool _isQuizFinished;
        private string _quizScore;

        public QuizName Quiz
        {
            get => _quiz;
            set
            {
                if (_quiz != value)
                {
                    _quiz = value;

                }
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
            get => CurrentQuestion != null ? "Visible" : "Collapsed";
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
                }
            }
        }

        public string ScoreVisibility
        {
            get => IsQuizFinished ? "Visible" : "Collapsed";
        }


        public string QuizScore
        {
            get => _quizScore;
            set
            {
                if (_quizScore != value)
                {
                    _quizScore = value;
                    OnPropertyChanged(nameof(QuizScore));

                }
            }
        }

        public ICommand NextQuestionCommand { get; private set; }
        public ICommand PreviousQuestionCommand { get; private set; }
        public ICommand FinishQuizCommand { get; private set; }
        public ICommand SelectAnswerCommand { get; private set; }

        public SolveViewModel()
        {
            Quiz = new QuizName("");
            LoadQuizData();
            if (Quiz.Questions.Any())
            {
                _currentQuestionIndex = 0;
                CurrentQuestion = Quiz.Questions[_currentQuestionIndex];
            }

            IsQuizFinished = false;
            QuizScore = string.Empty;

            NextQuestionCommand = new RelayCommand(NextQuestion, CanNextQuestion);
            PreviousQuestionCommand = new RelayCommand(PreviousQuestion, CanPreviousQuestion);
            FinishQuizCommand = new RelayCommand(FinishQuiz, CanFinishQuiz);
            SelectAnswerCommand = new RelayCommand(SelectAnswer, CanSelectAnswer);
        }

        private void LoadQuizData()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "quiz.txt");
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
                        string[] parts = lines[i].Trim().Split(new[] { ' ' }, 2);
                        if (parts.Length == 2)
                        {
                            string answerText = parts[0].Trim();
                            bool isCorrect = bool.Parse(parts[1].Trim());
                            currentQuestion.Answers.Add(new Answer(answerText, isCorrect));
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
            return !IsQuizFinished && _currentQuestionIndex > 0;
        }


        private void FinishQuiz(object parameter)
        {
            IsQuizFinished = true;
            CurrentQuestion = null;

            int score = 0;
            foreach (var question in Quiz.Questions)
            {
                if (question.SelectedAnswer != null && question.SelectedAnswer.IsCorrect)
                {
                    score++;
                }
            }
            QuizScore = $"Twój wynik: {score} na {Quiz.Questions.Count}";


        }

        private bool CanFinishQuiz(object parameter)
        {
            return _currentQuestionIndex >= Quiz.Questions.Count - 1 || IsQuizFinished;
        }

        private void SelectAnswer(object parameter)
        {
            if (parameter is Answer selectedAnswer && CurrentQuestion != null)
            {
                CurrentQuestion.SelectedAnswer = selectedAnswer;
            }
        }

        private bool CanSelectAnswer(object parameter)
        {
            return CurrentQuestion != null && parameter is Answer;
        }

    }
}