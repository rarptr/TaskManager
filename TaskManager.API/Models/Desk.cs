using System.Collections.Generic;

namespace TaskManager.API.Models
{
    public class Desk: CommonObject
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Columns { get; set; }
        // ID админа (внешний ключ)
        public int AdminId { get; set; }
        public User Admin { get; set; }
        // ID проекта (внешний ключ)
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
