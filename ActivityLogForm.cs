using System;
using System.Drawing;
using System.Windows.Forms;
using CyberBot.Data;
using CyberBot.Models;

namespace CyberBot
{
    public partial class ActivityLogForm : Form
    {
        private DatabaseContext _db;
        private ListView listLog;
        private Button btnRefresh;
        private Label lblTitle;

        public ActivityLogForm(DatabaseContext db)
        {
            _db = db;
            InitializeComponent();
            LoadLog();
        }

        private void InitializeComponent()
        {
            this.listLog = new System.Windows.Forms.ListView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listLog
            // 
            this.listLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.listLog.FullRowSelect = true;
            this.listLog.GridLines = true;
            this.listLog.HideSelection = false;
            this.listLog.Location = new System.Drawing.Point(10, 55);
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(740, 400);
            this.listLog.TabIndex = 0;
            this.listLog.UseCompatibleStateImageBehavior = false;
            this.listLog.View = System.Windows.Forms.View.Details;
            this.listLog.Columns.Add("ID", 50);
            this.listLog.Columns.Add("Action", 150);
            this.listLog.Columns.Add("Details", 400);
            this.listLog.Columns.Add("Timestamp", 140);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(630, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 35);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "🔄 Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Cyan;
            this.lblTitle.Location = new System.Drawing.Point(10, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 25);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "📋 Activity Log";
            // 
            // ActivityLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(760, 470);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.listLog);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "ActivityLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activity Log - CyberBot";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadLog()
        {
            listLog.Items.Clear();
            var logs = _db.GetRecentActivity(20); // Get last 20 actions
            foreach (var log in logs)
            {
                var item = new ListViewItem(log.Id.ToString());
                item.SubItems.Add(log.Action);
                item.SubItems.Add(log.Details);
                item.SubItems.Add(log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                listLog.Items.Add(item);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLog();
        }
    }
}
