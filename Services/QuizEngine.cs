using System;
using System.Collections.Generic;
using System.Linq;
using CyberBot.Models;

namespace CyberBot.Services
{
    public class QuizEngine
    {
        private List<QuizQuestion> _questions;
        private List<QuizQuestion> _shuffledQuestions;
        private int _currentQuestionIndex;
        private int _score;

        public int CurrentQuestionIndex => _currentQuestionIndex;
        public int TotalQuestions => _shuffledQuestions.Count;

        public QuizEngine()
        {
            InitializeQuestions();
            _currentQuestionIndex = 0;
            _score = 0;
        }

        private void InitializeQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    QuestionText = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Reporting phishing emails helps protect others and your organization.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: 'Password123' is a strong password.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Strong passwords are long, use a mix of characters, and avoid common words.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    QuestionText = "Which of these is a sign of a phishing email?",
                    Options = new List<string> { "Personal greeting", "Generic greeting", "Logo of a company", "Unsubscribe link" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Phishing emails often use generic greetings like 'Dear Customer' instead of your name.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    QuestionText = "What is Two-Factor Authentication (2FA)?",
                    Options = new List<string> { "Two passwords", "A second layer of security beyond a password", "An antivirus program", "A type of firewall" },
                    CorrectAnswerIndex = 1,
                    Explanation = "2FA requires a second verification step, like a code from your phone.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: It's safe to use public Wi-Fi for online banking.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Public Wi-Fi is not secure. Always use a VPN or your mobile data for sensitive activities.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    QuestionText = "What is the best way to share passwords?",
                    Options = new List<string> { "Text message", "Email", "Password manager", "Tell someone in person" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Password managers securely store and share your passwords.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: Antivirus software protects you from all threats.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Antivirus is important but doesn't protect against everything; you still need safe habits!",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    QuestionText = "What is social engineering?",
                    Options = new List<string> { "Building social networks", "Manipulating people to get sensitive information", "Social media marketing", "Computer programming" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Social engineering tricks people into giving up information, like phishing calls.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    QuestionText = "How often should you update your software?",
                    Options = new List<string> { "Never", "Once a year", "When updates are available", "Every day manually" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Software updates patch security vulnerabilities. Enable automatic updates!",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: You should use the same password for all your accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswerIndex = 1,
                    Explanation = "If one account is compromised, all your accounts would be at risk!",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    QuestionText = "What does 'HTTPS' in a URL mean?",
                    Options = new List<string> { "High Traffic", "Secure connection", "Home page", "Text only" },
                    CorrectAnswerIndex = 1,
                    Explanation = "HTTPS encrypts data between your browser and the website.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    QuestionText = "What should you do if you think you've been scammed?",
                    Options = new List<string> { "Do nothing", "Change your passwords and report it", "Reply to the scammer", "Share your experience on social media" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Act quickly to secure your accounts and prevent further damage.",
                    IsTrueFalse = false
                }
            };

            // Shuffle questions
            var rng = new Random();
            _shuffledQuestions = _questions.OrderBy(a => rng.Next()).ToList();
        }

        public bool StartQuiz()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            return true;
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentQuestionIndex < _shuffledQuestions.Count)
            {
                return _shuffledQuestions[_currentQuestionIndex];
            }
            return null;
        }

        public bool SubmitAnswer(int answerIndex, out string feedback)
        {
            var question = GetCurrentQuestion();
            if (question == null)
            {
                feedback = "Quiz is over!";
                return false;
            }

            bool isCorrect = answerIndex == question.CorrectAnswerIndex;
            if (isCorrect) _score++;

            feedback = isCorrect 
                ? $"Correct! {question.Explanation}" 
                : $"Wrong. {question.Explanation}";

            _currentQuestionIndex++;
            return isCorrect;
        }

        public bool IsQuizComplete()
        {
            return _currentQuestionIndex >= _shuffledQuestions.Count;
        }

        public (int score, int total, string message) GetFinalScore()
        {
            double percentage = (double)_score / _shuffledQuestions.Count;
            string message;

            if (percentage >= 0.9) message = "Excellent! You're a Cybersecurity Pro! 🛡️";
            else if (percentage >= 0.7) message = "Great job! Keep learning to stay safe online!";
            else if (percentage >= 0.5) message = "Good effort! Review your answers and try again!";
            else message = "Don't give up! Cybersecurity is important - keep practicing!";

            return (_score, _shuffledQuestions.Count, message);
        }
    }
}
