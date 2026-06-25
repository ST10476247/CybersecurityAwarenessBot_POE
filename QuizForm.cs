using System;
using System.Drawing;
using System.Windows.Forms;
using CyberBot.Data;
using CyberBot.Services;
using CyberBot.Models;

namespace CyberBot
{
    public partial class QuizForm : Form
    {
        private DatabaseContext _db;
        private QuizEngine _quizEngine;
        private Label lblQuestionNumber;
        private Label lblQuestion;
        private FlowLayoutPanel pnlAnswers;
        private Label lblFeedback;
        private Button btnNext;
        private Button btnStart;
        private Label lblScore;

        public QuizForm(DatabaseContext db)
        {
            _db = db;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblQuestionNumber = new System.Windows.Forms.Label();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.pnlAnswers = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFeedback = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblScore = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblQuestionNumber
            // 
            this.lblQuestionNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuestionNumber.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblQuestionNumber.ForeColor = System.Drawing.Color.Cyan;
            this.lblQuestionNumber.Location = new System.Drawing.Point(10, 10);
            this.lblQuestionNumber.Name = "lblQuestionNumber";
            this.lblQuestionNumber.Size = new System.Drawing.Size(740, 25);
            this.lblQuestionNumber.TabIndex = 0;
            this.lblQuestionNumber.Text = "Click 'Start Quiz' to begin!";
            this.lblQuestionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblQuestion
            // 
            this.lblQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuestion.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblQuestion.ForeColor = System.Drawing.Color.White;
            this.lblQuestion.Location = new System.Drawing.Point(10, 45);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(740, 90);
            this.lblQuestion.TabIndex = 1;
            this.lblQuestion.Text = "Welcome to the Cybersecurity Quiz!";
            this.lblQuestion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlAnswers
            // 
            this.pnlAnswers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAnswers.Location = new System.Drawing.Point(10, 145);
            this.pnlAnswers.Name = "pnlAnswers";
            this.pnlAnswers.Size = new System.Drawing.Size(740, 250);
            this.pnlAnswers.TabIndex = 2;
            // 
            // lblFeedback
            // 
            this.lblFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFeedback.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblFeedback.ForeColor = System.Drawing.Color.Yellow;
            this.lblFeedback.Location = new System.Drawing.Point(10, 405);
            this.lblFeedback.Name = "lblFeedback";
            this.lblFeedback.Size = new System.Drawing.Size(740, 45);
            this.lblFeedback.TabIndex = 3;
            this.lblFeedback.Text = "";
            this.lblFeedback.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnNext.Enabled = false;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(630, 455);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(120, 35);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "Next Question";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(100)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(480, 455);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(140, 35);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "▶ Start Quiz";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblScore
            // 
            this.lblScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblScore.ForeColor = System.Drawing.Color.White;
            this.lblScore.Location = new System.Drawing.Point(10, 465);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(68, 19);
            this.lblScore.TabIndex = 6;
            this.lblScore.Text = "Score: 0/0";
            // 
            // QuizForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(760, 500);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblFeedback);
            this.Controls.Add(this.pnlAnswers);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.lblQuestionNumber);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "QuizForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cybersecurity Quiz - CyberBot";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _quizEngine = new QuizEngine();
            _quizEngine.StartQuiz();
            _db.AddLogEntry("Quiz Started", "User launched a new quiz session");
            lblScore.Text = "Score: 0/0";
            lblFeedback.Text = "";
            btnStart.Enabled = false;
            LoadCurrentQuestion();
        }

        private void LoadCurrentQuestion()
        {
            pnlAnswers.Controls.Clear();
            if (_quizEngine.IsQuizComplete())
            {
                var (score, total, message) = _quizEngine.GetFinalScore();
                lblQuestionNumber.Text = "Quiz Complete!";
                lblQuestion.Text = message;
                lblScore.Text = $"Score: {score}/{total}";
                btnNext.Text = "Play Again";
                btnNext.Enabled = true;
                _db.AddLogEntry("Quiz Finished", $"Final score: {score}/{total}");
                return;
            }

            var q = _quizEngine.GetCurrentQuestion();
            lblQuestionNumber.Text = $"Question {_quizEngine.CurrentQuestionIndex + 1} (of {_quizEngine.TotalQuestions})";
            lblQuestion.Text = q.QuestionText;
            btnNext.Enabled = false;

            int index = 0;
            foreach (var answer in q.Options)
            {
                var btn = new Button();
                btn.Text = $"{(char)('A' + index)}. {answer}";
                btn.Tag = index;
                btn.BackColor = Color.FromArgb(45, 45, 48);
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.Gray;
                btn.Font = new Font("Segoe UI", 10F);
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(10, 0, 0, 0);
                btn.Width = pnlAnswers.Width - 25;
                btn.Height = 40;
                btn.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                btn.Click += AnswerButton_Click;
                pnlAnswers.Controls.Add(btn);
                index++;
            }
        }

        private void AnswerButton_Click(object sender, EventArgs e)
        {
            var clickedBtn = (Button)sender;
            int selectedIdx = (int)clickedBtn.Tag;
            bool correct = _quizEngine.SubmitAnswer(selectedIdx, out string feedback);
            
            foreach (Control ctrl in pnlAnswers.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.Enabled = false;
                    int idx = (int)btn.Tag;
                    if (idx == _quizEngine.GetCurrentQuestion().CorrectAnswerIndex)
                    {
                        btn.BackColor = Color.Green;
                    }
                    else if (idx == selectedIdx && !correct)
                    {
                        btn.BackColor = Color.FromArgb(190, 0, 0);
                    }
                }
            }

            lblFeedback.Text = feedback;
            btnNext.Enabled = true;
            var (score, total, _) = _quizEngine.GetFinalScore();
            lblScore.Text = $"Score: {score}/{total}";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_quizEngine.IsQuizComplete())
            {
                btnNext.Text = "Next Question";
                btnStart_Click(null, EventArgs.Empty);
            }
            else LoadCurrentQuestion();
        }
    }
}
