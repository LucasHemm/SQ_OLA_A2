﻿namespace SQL_OLA_A2
{
    public class TaskDto
    {
        public int Id { get; set; }  // Unique identifier for the task
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsFinished { get; set; }
        public string Category { get; set; }
    }
}