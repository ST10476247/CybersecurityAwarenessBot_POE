using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using CyberBot.Data;
using CyberBot.Services;

namespace CyberBot
{
    public partial class MainForm : Form
    {
        private UserProfile _user;
        private EnhancedResponseEngine _engine;
        private DatabaseContext _db;
        private bool _isAwaitingName = true;
        private TaskManagerForm _taskForm;
        private QuizForm _quizForm;
        private ActivityLogForm _logForm;

        public MainForm()
        {
            InitializeComponent();
            _db = new DatabaseContext();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblBanner.Text = AsciiArt.GetBanner();
            Task.Run(() => AudioPlayer.PlayGreeting());

            AppendBotMessage("Hello! I'm your Cybersecurity Awareness Bot. 🛡️");
            AppendBotMessage("Before we begin — what's your name?");
        }

        private void btnSend_Click(object sender, EventArgs e) => ProcessInput();
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ProcessInput();
            }
        }

        private void btnOpenTasks_Click(object sender, EventArgs e)
        {
            if (_taskForm == null || _taskForm.IsDisposed)
            {
                _taskForm = new TaskManagerForm(_db);
                _taskForm.Show();
            }
            else _taskForm.BringToFront();
        }

        private void btnOpenQuiz_Click(object sender, EventArgs e)
        {
            if (_quizForm == null || _quizForm.IsDisposed)
            {
                _quizForm = new QuizForm(_db);
                _quizForm.Show();
                _db.AddLogEntry("Quiz Launched", "User opened the quiz window");
            }
            else _quizForm.BringToFront();
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            if (_logForm == null || _logForm.IsDisposed)
            {
                _logForm = new ActivityLogForm(_db);
                _logForm.Show();
            }
            else _logForm.BringToFront();
        }

        private void ProcessInput()
        {
            string input = txtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendUserMessage(input);
            txtInput.Clear();

            if (_isAwaitingName) HandleNameInput(input);
            else HandleChatMessage(input);
        }

        private void HandleNameInput(string name)
        {
            bool hasLetter = false;
            foreach (char c in name) if (char.IsLetter(c)) { hasLetter = true; break; }

            if (!hasLetter)
            {
                AppendBotMessage("Please use a valid name with letters!");
                return;
            }

            _user = new UserProfile(name);
            _engine = new EnhancedResponseEngine(_user, _db);
            _isAwaitingName = false;

            AppendBotMessage($"Great to meet you, {_user.Name}! 🎉");
            AppendBotMessage("I'm here to help! Ask about cybersecurity, or use the buttons below to open Tasks, Quiz, or Activity Log!");

            // Enable buttons once user is known
            btnOpenTasks.Enabled = true;
            btnOpenQuiz.Enabled = true;
            btnOpenLog.Enabled = true;
        }

        private void HandleChatMessage(string input)
        {
            var response = _engine.GetResponse(input, out BotIntent intent);
            _user.QuestionCount++;
            AppendBotMessage(response);

            // Automatically open windows based on intent
            if (intent == BotIntent.AskForQuiz) btnOpenQuiz_Click(this, EventArgs.Empty);
            else if (intent == BotIntent.AskForTasks) btnOpenTasks_Click(this, EventArgs.Empty);
            else if (intent == BotIntent.ShowActivityLog) btnOpenLog_Click(this, EventArgs.Empty);

            if (_user.QuestionCount > 0 && _user.QuestionCount % 5 == 0)
            {
                AppendBotMessage($"🏆 Nice, {_user.Name}! You've asked {_user.QuestionCount} questions!");
            }
        }

        private void AppendBotMessage(string message)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionColor = Color.LightGreen;
            rtbChat.AppendText("\n🤖 CyberBot ► ");
            rtbChat.SelectionColor = Color.White;
            rtbChat.AppendText(message + "\n");
            ScrollToBottom();
        }

        private void AppendUserMessage(string message)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionColor = Color.Yellow;
            rtbChat.AppendText($"\n👤 {_user?.Name ?? "User"} ► ");
            rtbChat.SelectionColor = Color.White;
            rtbChat.AppendText(message + "\n");
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            rtbChat.SelectionStart = rtbChat.Text.Length;
            rtbChat.ScrollToCaret();
        }
    }
}
