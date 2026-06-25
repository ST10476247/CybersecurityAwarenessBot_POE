using System;
using System.Drawing;
using System.Windows.Forms;
using CyberBot.Data;
using CyberBot.Models;

namespace CyberBot
{
    public partial class TaskManagerForm : Form
    {
        private DatabaseContext _db;
        private ListView listTasks;
        private TextBox txtTaskTitle;
        private TextBox txtTaskDesc;
        private DateTimePicker dtpReminder;
        private CheckBox chkUseReminder;
        private Button btnAdd;
        private Button btnMarkComplete;
        private Button btnDelete;
        private Label lblTitle;
        private Label lblDesc;
        private Label lblReminder;

        public TaskManagerForm(DatabaseContext db)
        {
            _db = db;
            InitializeComponent();
            LoadTasks();
        }

        private void InitializeComponent()
        {
            this.listTasks = new System.Windows.Forms.ListView();
            this.txtTaskTitle = new System.Windows.Forms.TextBox();
            this.txtTaskDesc = new System.Windows.Forms.TextBox();
            this.dtpReminder = new System.Windows.Forms.DateTimePicker();
            this.chkUseReminder = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMarkComplete = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblReminder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listTasks
            // 
            this.listTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTasks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.listTasks.FullRowSelect = true;
            this.listTasks.GridLines = true;
            this.listTasks.HideSelection = false;
            this.listTasks.Location = new System.Drawing.Point(10, 10);
            this.listTasks.Name = "listTasks";
            this.listTasks.Size = new System.Drawing.Size(740, 300);
            this.listTasks.TabIndex = 0;
            this.listTasks.UseCompatibleStateImageBehavior = false;
            this.listTasks.View = System.Windows.Forms.View.Details;
            this.listTasks.Columns.Add("ID", 50);
            this.listTasks.Columns.Add("Title", 200);
            this.listTasks.Columns.Add("Description", 250);
            this.listTasks.Columns.Add("Reminder", 120);
            this.listTasks.Columns.Add("Created", 120);
            this.listTasks.Columns.Add("Completed?", 80);
            // 
            // txtTaskTitle
            // 
            this.txtTaskTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaskTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtTaskTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTaskTitle.ForeColor = System.Drawing.Color.White;
            this.txtTaskTitle.Location = new System.Drawing.Point(90, 320);
            this.txtTaskTitle.Name = "txtTaskTitle";
            this.txtTaskTitle.Size = new System.Drawing.Size(660, 22);
            this.txtTaskTitle.TabIndex = 1;
            // 
            // txtTaskDesc
            // 
            this.txtTaskDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaskDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtTaskDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTaskDesc.ForeColor = System.Drawing.Color.White;
            this.txtTaskDesc.Location = new System.Drawing.Point(90, 350);
            this.txtTaskDesc.Name = "txtTaskDesc";
            this.txtTaskDesc.Size = new System.Drawing.Size(660, 22);
            this.txtTaskDesc.TabIndex = 2;
            // 
            // dtpReminder
            // 
            this.dtpReminder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpReminder.Enabled = false;
            this.dtpReminder.Location = new System.Drawing.Point(480, 380);
            this.dtpReminder.MinDate = System.DateTime.Now.Date;
            this.dtpReminder.Name = "dtpReminder";
            this.dtpReminder.Size = new System.Drawing.Size(270, 22);
            this.dtpReminder.TabIndex = 4;
            // 
            // chkUseReminder
            // 
            this.chkUseReminder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkUseReminder.AutoSize = true;
            this.chkUseReminder.ForeColor = System.Drawing.Color.White;
            this.chkUseReminder.Location = new System.Drawing.Point(480, 410);
            this.chkUseReminder.Name = "chkUseReminder";
            this.chkUseReminder.Size = new System.Drawing.Size(118, 19);
            this.chkUseReminder.TabIndex = 5;
            this.chkUseReminder.Text = "Set Reminder?";
            this.chkUseReminder.UseVisualStyleBackColor = true;
            this.chkUseReminder.CheckedChanged += new System.EventHandler(this.chkUseReminder_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(630, 430);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 30);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "➕ Add Task";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMarkComplete
            // 
            this.btnMarkComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMarkComplete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(100)))));
            this.btnMarkComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkComplete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMarkComplete.ForeColor = System.Drawing.Color.White;
            this.btnMarkComplete.Location = new System.Drawing.Point(450, 430);
            this.btnMarkComplete.Name = "btnMarkComplete";
            this.btnMarkComplete.Size = new System.Drawing.Size(175, 30);
            this.btnMarkComplete.TabIndex = 7;
            this.btnMarkComplete.Text = "✅ Mark Completed";
            this.btnMarkComplete.UseVisualStyleBackColor = false;
            this.btnMarkComplete.Click += new System.EventHandler(this.btnMarkComplete_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(320, 430);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(125, 30);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "🗑️ Delete Task";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTitle.AutoSize = true;
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(10, 323);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(35, 15);
            this.lblTitle.TabIndex = 9;
            this.lblTitle.Text = "Title:";
            // 
            // lblDesc
            // 
            this.lblDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDesc.AutoSize = true;
            this.lblDesc.ForeColor = System.Drawing.Color.White;
            this.lblDesc.Location = new System.Drawing.Point(10, 353);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(74, 15);
            this.lblDesc.TabIndex = 10;
            this.lblDesc.Text = "Description:";
            // 
            // lblReminder
            // 
            this.lblReminder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReminder.AutoSize = true;
            this.lblReminder.ForeColor = System.Drawing.Color.White;
            this.lblReminder.Location = new System.Drawing.Point(410, 383);
            this.lblReminder.Name = "lblReminder";
            this.lblReminder.Size = new System.Drawing.Size(64, 15);
            this.lblReminder.TabIndex = 11;
            this.lblReminder.Text = "Reminder:";
            // 
            // TaskManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(760, 470);
            this.Controls.Add(this.lblReminder);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnMarkComplete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.chkUseReminder);
            this.Controls.Add(this.dtpReminder);
            this.Controls.Add(this.txtTaskDesc);
            this.Controls.Add(this.txtTaskTitle);
            this.Controls.Add(this.listTasks);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "TaskManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Task Manager - CyberBot";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void chkUseReminder_CheckedChanged(object sender, EventArgs e)
        {
            dtpReminder.Enabled = chkUseReminder.Checked;
        }

        private void LoadTasks()
        {
            listTasks.Items.Clear();
            var tasks = _db.GetAllTasks();
            foreach (var task in tasks)
            {
                var item = new ListViewItem(task.Id.ToString());
                item.SubItems.Add(task.Title);
                item.SubItems.Add(task.Description);
                item.SubItems.Add(task.ReminderDate.HasValue ? task.ReminderDate.Value.ToString("yyyy-MM-dd") : "None");
                item.SubItems.Add(task.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
                item.SubItems.Add(task.IsCompleted ? "✅ Yes" : "No");
                item.Tag = task;
                if (task.IsCompleted) item.BackColor = Color.FromArgb(40, 80, 40);
                listTasks.Items.Add(item);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskTitle.Text))
            {
                MessageBox.Show("Please enter a task title!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newTask = new TaskItem
            {
                Title = txtTaskTitle.Text.Trim(),
                Description = txtTaskDesc.Text.Trim(),
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                ReminderDate = chkUseReminder.Checked ? dtpReminder.Value : (DateTime?)null
            };

            if (_db.AddTask(newTask))
            {
                txtTaskTitle.Clear();
                txtTaskDesc.Clear();
                chkUseReminder.Checked = false;
                LoadTasks();
                MessageBox.Show("Task added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Failed to add task!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnMarkComplete_Click(object sender, EventArgs e)
        {
            if (listTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a task first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selectedTask = (TaskItem)listTasks.SelectedItems[0].Tag;
            if (_db.MarkTaskCompleted(selectedTask.Id))
            {
                LoadTasks();
                MessageBox.Show("Task marked as completed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a task first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selectedTask = (TaskItem)listTasks.SelectedItems[0].Tag;
            var confirm = MessageBox.Show($"Delete task '{selectedTask.Title}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                if (_db.DeleteTask(selectedTask.Id))
                {
                    LoadTasks();
                    MessageBox.Show("Task deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
