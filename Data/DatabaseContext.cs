using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using CyberBot.Models;

namespace CyberBot.Data
{
    public class DatabaseContext
    {
        private const string DbFileName = "cyberbot.db";
        private string _dbPath;

        public DatabaseContext()
        {
            _dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbFileName);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();

                // Create Tasks table
                var createTasksTable = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Description TEXT,
                        IsCompleted INTEGER DEFAULT 0,
                        ReminderDate TEXT,
                        CreatedAt TEXT
                    );";

                // Create ActivityLog table
                var createLogTable = @"
                    CREATE TABLE IF NOT EXISTS ActivityLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Action TEXT NOT NULL,
                        Details TEXT,
                        Timestamp TEXT NOT NULL
                    );";

                using (var command = new SqliteCommand(createTasksTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqliteCommand(createLogTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        #region Task CRUD Operations
        public bool AddTask(TaskItem task)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    string sql = @"
                        INSERT INTO Tasks (Title, Description, IsCompleted, ReminderDate, CreatedAt)
                        VALUES (@Title, @Description, @IsCompleted, @ReminderDate, @CreatedAt);";

                    using (var command = new SqliteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Title", task.Title);
                        command.Parameters.AddWithValue("@Description", task.Description ?? string.Empty);
                        command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
                        command.Parameters.AddWithValue("@ReminderDate", task.ReminderDate?.ToString("o") ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt.ToString("o"));
                        command.ExecuteNonQuery();
                    }
                }
                AddLogEntry("Task Added", $"Task: '{task.Title}'");
                return true;
            }
            catch { return false; }
        }

        public List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                string sql = "SELECT * FROM Tasks ORDER BY CreatedAt DESC;";

                using (var command = new SqliteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new TaskItem
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            IsCompleted = reader.GetInt32(3) == 1,
                            ReminderDate = reader.IsDBNull(4) ? null : (DateTime?)DateTime.Parse(reader.GetString(4)),
                            CreatedAt = DateTime.Parse(reader.GetString(5))
                        };
                        tasks.Add(task);
                    }
                }
            }
            return tasks;
        }

        public bool MarkTaskCompleted(int taskId)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    string sql = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id;";

                    using (var command = new SqliteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", taskId);
                        command.ExecuteNonQuery();
                    }
                }
                AddLogEntry("Task Completed", $"Task ID: {taskId}");
                return true;
            }
            catch { return false; }
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    string sql = "DELETE FROM Tasks WHERE Id = @Id;";

                    using (var command = new SqliteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", taskId);
                        command.ExecuteNonQuery();
                    }
                }
                AddLogEntry("Task Deleted", $"Task ID: {taskId}");
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region Activity Log Operations
        public void AddLogEntry(string action, string details = "")
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    string sql = @"
                        INSERT INTO ActivityLog (Action, Details, Timestamp)
                        VALUES (@Action, @Details, @Timestamp);";

                    using (var command = new SqliteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Action", action);
                        command.Parameters.AddWithValue("@Details", details);
                        command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("o"));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public List<ActivityLogItem> GetRecentActivity(int count = 10)
        {
            var logs = new List<ActivityLogItem>();
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                string sql = "SELECT * FROM ActivityLog ORDER BY Timestamp DESC LIMIT @Count;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Count", count);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var log = new ActivityLogItem
                            {
                                Id = reader.GetInt32(0),
                                Action = reader.GetString(1),
                                Details = reader.GetString(2),
                                Timestamp = DateTime.Parse(reader.GetString(3))
                            };
                            logs.Add(log);
                        }
                    }
                }
            }
            return logs;
        }
        #endregion
    }
}
