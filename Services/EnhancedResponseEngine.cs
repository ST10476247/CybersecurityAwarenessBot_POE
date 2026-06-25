using System;
using System.Collections.Generic;
using System.Linq;
using CyberBot.Data;
using CyberBot.Models;

namespace CyberBot.Services
{
    public enum BotIntent
    {
        Unknown,
        Greeting,
        AskAboutCybersecurity,
        AskForQuiz,
        AskForTasks,
        AddTask,
        ShowActivityLog,
        Help,
        Exit
    }

    public class EnhancedResponseEngine
    {
        private readonly UserProfile _user;
        private readonly DatabaseContext _db;
        private readonly Random _random;
        private bool _awaitingTaskTitle = false;
        private bool _awaitingTaskReminder = false;
        private TaskItem _pendingTask = null;

        private static readonly Dictionary<string, string[]> _intentPatterns = new Dictionary<string, string[]>
        {
            ["Greeting"] = new[] { "hello", "hi", "hey", "good morning", "good afternoon", "good evening" },
            ["Quiz"] = new[] { "quiz", "play", "game", "test my knowledge", "question", "start quiz" },
            ["Tasks"] = new[] { "task", "tasks", "show my tasks", "list tasks", "view tasks", "my tasks" },
            ["AddTask"] = new[] { "add task", "create task", "new task", "set reminder", "remind me", "need to do", "i need to" },
            ["ActivityLog"] = new[] { "show log", "activity log", "what have you done", "history", "show activity" },
            ["Help"] = new[] { "help", "what can you do", "features", "commands" },
            ["Exit"] = new[] { "exit", "quit", "bye", "goodbye" },
            ["Password"] = new[] { "password", "passcode", "strong password", "password safety" },
            ["Phishing"] = new[] { "phishing", "fake email", "scam email" },
            ["Scam"] = new[] { "scam", "fraud", "scammed" },
            ["Privacy"] = new[] { "privacy", "personal information", "data privacy" },
            ["Malware"] = new[] { "malware", "virus", "trojan", "ransomware" },
            ["VPN"] = new[] { "vpn", "virtual private network", "public wifi" },
            ["2FA"] = new[] { "2fa", "two-factor", "two factor", "mfa" }
        };

        private static readonly Dictionary<string, string[]> _topicResponses = new Dictionary<string, string[]>
        {
            ["Password"] = new[] {
                "Use 12+ characters with a mix of symbols, numbers, and letters.",
                "A password manager is your best friend! Try Bitwarden or 1Password.",
                "Never reuse passwords across accounts."
            },
            ["Phishing"] = new[] {
                "Always hover over links to check their real destination.",
                "Legitimate companies never ask for passwords via email.",
                "Watch out for urgent or threatening language designed to make you act fast."
            },
            ["Scam"] = new[] {
                "If it sounds too good to be true, it's definitely a scam!",
                "Never share OTPs or PINs with anyone.",
                "Verify the sender's identity before trusting them."
            },
            ["Privacy"] = new[] {
                "Review your social media privacy settings regularly.",
                "Be careful about what apps you give permissions to.",
                "Consider using a privacy-focused browser like Firefox or Brave."
            },
            ["Malware"] = new[] {
                "Keep your software updated!",
                "Only download files from trusted sources.",
                "Install reputable antivirus software and run regular scans."
            },
            ["VPN"] = new[] {
                "Always use a VPN on public Wi-Fi.",
                "Avoid free VPNs as they often sell your data.",
                "A VPN encrypts your connection, hiding your activity from ISPs."
            },
            ["2FA"] = new[] {
                "Enable 2FA on every account that supports it!",
                "Use an authenticator app instead of SMS for better security.",
                "2FA adds a crucial second layer of protection."
            }
        };

        public EnhancedResponseEngine(UserProfile user, DatabaseContext db)
        {
            _user = user;
            _db = db;
            _random = new Random();
        }

        public BotIntent DetectIntent(string input)
        {
            string cleanInput = CleanInput(input);

            if (MatchesAnyPattern(cleanInput, _intentPatterns["Quiz"])) return BotIntent.AskForQuiz;
            if (MatchesAnyPattern(cleanInput, _intentPatterns["AddTask"])) return BotIntent.AddTask;
            if (MatchesAnyPattern(cleanInput, _intentPatterns["Tasks"])) return BotIntent.AskForTasks;
            if (MatchesAnyPattern(cleanInput, _intentPatterns["ActivityLog"])) return BotIntent.ShowActivityLog;
            if (MatchesAnyPattern(cleanInput, _intentPatterns["Greeting"])) return BotIntent.Greeting;
            if (MatchesAnyPattern(cleanInput, _intentPatterns["Help"])) return BotIntent.Help;
            if (MatchesAnyPattern(cleanInput, _intentPatterns["Exit"])) return BotIntent.Exit;

            // Check for topic questions
            foreach (var topic in _topicResponses.Keys)
            {
                if (MatchesAnyPattern(cleanInput, _intentPatterns[topic])) return BotIntent.AskAboutCybersecurity;
            }

            return BotIntent.Unknown;
        }

        public string GetResponse(string rawInput, out BotIntent intent)
        {
            intent = DetectIntent(rawInput);
            string cleanInput = CleanInput(rawInput);

            // Handle task flow
            if (_awaitingTaskTitle)
            {
                return HandleTaskTitleInput(rawInput);
            }
            if (_awaitingTaskReminder)
            {
                return HandleTaskReminderInput(rawInput);
            }

            switch (intent)
            {
                case BotIntent.Greeting: return GetGreetingResponse();
                case BotIntent.AskForQuiz:
                    _db.AddLogEntry("Quiz Started", "User launched the cybersecurity quiz");
                    return "Great! Let's test your knowledge! Type 'start quiz' to begin, or ask me to open the quiz window!";
                case BotIntent.AskForTasks: return GetTasksResponse();
                case BotIntent.AddTask: return StartTaskAddFlow(rawInput);
                case BotIntent.ShowActivityLog: return GetActivityLogResponse();
                case BotIntent.Help: return GetHelpResponse();
                case BotIntent.Exit: return "Goodbye! Stay safe online! 🛡️";
                case BotIntent.AskAboutCybersecurity: return GetCybersecurityTopicResponse(cleanInput);
                default: return GetFallbackResponse();
            }
        }

        private string StartTaskAddFlow(string input)
        {
            // Try to extract title from initial command
            string[] keywords = new[] { "add task", "create task", "new task", "remind me to", "set reminder to" };
            string title = input;
            foreach (string kw in keywords)
            {
                if (input.ToLower().Contains(kw))
                {
                    int idx = input.ToLower().IndexOf(kw);
                    title = input.Substring(idx + kw.Length).Trim().Trim('.', '!', '?');
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(title) && !keywords.Any(kw => CleanInput(title) == kw))
            {
                // We have a title
                _pendingTask = new TaskItem
                {
                    Title = title,
                    Description = $"Added by {_user.Name}",
                    CreatedAt = DateTime.Now
                };
                _awaitingTaskReminder = true;
                return $"Got it! Task: '{title}'. Would you like to set a reminder? (Say 'Yes' + date, e.g., 'Yes, in 3 days' or 'No')";
            }
            else
            {
                _awaitingTaskTitle = true;
                return "Sure! What's the task you'd like to add?";
            }
        }

        private string HandleTaskTitleInput(string input)
        {
            _pendingTask = new TaskItem
            {
                Title = input.Trim(),
                Description = $"Added by {_user.Name}",
                CreatedAt = DateTime.Now
            };
            _awaitingTaskTitle = false;
            _awaitingTaskReminder = true;
            return $"Great! Task: '{_pendingTask.Title}'. Would you like to set a reminder? (Say 'Yes' + time or 'No')";
        }

        private string HandleTaskReminderInput(string input)
        {
            string clean = CleanInput(input);
            if (clean.StartsWith("no"))
            {
                _db.AddTask(_pendingTask);
                _pendingTask = null;
                _awaitingTaskReminder = false;
                return "Okay, task added without a reminder! Type 'show tasks' to see your list.";
            }

            // Try to parse reminder date
            DateTime? reminder = null;
            if (input.ToLower().Contains("tomorrow")) reminder = DateTime.Today.AddDays(1);
            else if (input.ToLower().Contains("next week")) reminder = DateTime.Today.AddDays(7);
            else if (input.ToLower().Contains("in 1 day")) reminder = DateTime.Today.AddDays(1);
            else if (input.ToLower().Contains("in 2 days")) reminder = DateTime.Today.AddDays(2);
            else if (input.ToLower().Contains("in 3 days")) reminder = DateTime.Today.AddDays(3);
            else if (input.ToLower().Contains("in 7 days")) reminder = DateTime.Today.AddDays(7);
            else if (input.ToLower().Contains("in 1 week")) reminder = DateTime.Today.AddDays(7);

            _pendingTask.ReminderDate = reminder;
            _db.AddTask(_pendingTask);
            _pendingTask = null;
            _awaitingTaskReminder = false;

            if (reminder.HasValue)
                return $"Perfect! Task added, and reminder set for {reminder.Value:MMMM dd, yyyy}! 📅";
            return "Task added! I couldn't parse that date, but no reminder is set.";
        }

        private string GetGreetingResponse()
        {
            string[] greetings = new[] {
                $"Hi {_user.Name}! How can I help you stay safe online today?",
                $"Hello {_user.Name}! Want to chat about cybersecurity or try the quiz?",
                $"Hey {_user.Name}! What's on your mind?"
            };
            return greetings[_random.Next(greetings.Length)];
        }

        private string GetTasksResponse()
        {
            var tasks = _db.GetAllTasks();
            _db.AddLogEntry("Tasks Viewed", $"User viewed {tasks.Count} tasks");

            if (tasks.Count == 0) return "You don't have any tasks yet! Type 'add task' to create one.";
            return $"You have {tasks.Count} task(s)! I'll show them in the tasks window for you!";
        }

        private string GetActivityLogResponse()
        {
            var logs = _db.GetRecentActivity();
            _db.AddLogEntry("Log Viewed", "User viewed activity log");

            if (logs.Count == 0) return "No activity recorded yet!";
            return $"Here's your recent activity! I'll display it in the log window for you!";
        }

        private string GetCybersecurityTopicResponse(string input)
        {
            foreach (var topic in _topicResponses.Keys)
            {
                if (MatchesAnyPattern(input, _intentPatterns[topic]))
                {
                    var responses = _topicResponses[topic];
                    _user.FavouriteTopic = topic;
                    string resp = responses[_random.Next(responses.Length)];
                    _db.AddLogEntry($"Topic Question", $"User asked about {topic}");
                    return resp;
                }
            }
            return GetFallbackResponse();
        }

        private string GetHelpResponse()
        {
            _db.AddLogEntry("Help Requested", "");
            return @"Here's what I can do:
• Ask about cybersecurity topics like passwords, phishing, scams, etc.
• Play the quiz! Type 'quiz' or 'game'
• Manage tasks: 'add task', 'show tasks'
• View activity log: 'show log' or 'what have you done'
• Just say 'help' at any time!";
        }

        private string GetFallbackResponse()
        {
            string[] fallbacks = new[] {
                "I'm not quite sure about that. Could you rephrase?",
                "Hmm, let me think. You can ask about cybersecurity topics, try the quiz, or manage tasks!",
                "Sorry, I don't understand that yet. Try typing 'help' to see what I can do!"
            };
            return fallbacks[_random.Next(fallbacks.Length)];
        }

        private string CleanInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return input.Trim().ToLower()
                .Replace("?", "").Replace("!", "").Replace(".", "").Replace(",", "");
        }

        private bool MatchesAnyPattern(string input, string[] patterns)
        {
            return patterns.Any(p => input.Contains(p));
        }
    }
}
