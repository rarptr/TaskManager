using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Common.Models;

namespace TaskManager.API.Models
{
    public class Desk : CommonObject
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

        public Desk() { }

        public Desk(DeskModel deskModel) : base(deskModel)
        {
            Id = deskModel.Id;
            AdminId = deskModel.AdminId;
            IsPrivate = deskModel.IsPrivate;
            AdminId = deskModel.AdminId;
            ProjectId = deskModel.ProjectId;

            if (deskModel.Columns.Any())
            {
                Columns = JsonConvert.SerializeObject(deskModel.Columns);
            }
        }

        public CommonModel ToShortDto()
        {
            return new CommonModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreationDate = this.CreationDate,
                Photo = this.Photo,
            };
        }

        public DeskModel ToDto()
        {
            return new DeskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreationDate = this.CreationDate,
                Photo = this.Photo,
                AdminId = this.AdminId,
                IsPrivate = this.IsPrivate,
                Columns = JsonConvert.DeserializeObject<string[]>(this.Columns),
                ProjectId = this.ProjectId
            };
        }
    }
}
