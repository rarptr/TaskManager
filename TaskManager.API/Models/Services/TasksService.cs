using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Models.Data;
using TaskManager.Common.Models;

namespace TaskManager.API.Models.Services
{
    public class TasksService : AbstractionService
    {
        private readonly ApplicationContext _db;

        public TasksService(ApplicationContext db)
        {
            _db = db;
        }

        public TaskModel Get(int id)
        {
            Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            return task?.ToDto();
        }

        public IQueryable<CommonModel> GetTasksForUser(int userId)
        {
            return _db.Tasks.Where(t => t.CreatorId == userId || t.ExecutorId == userId)
                            .Select(t => t.ToShortDto());
        }

        public IQueryable<CommonModel> GetAll(int deskId)
        {
            return _db.Tasks.Where(t => t.DeskId == deskId).Select(t => t.ToShortDto());
        }

        public bool Create(TaskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Task newTask = new Task(model);
                _db.Tasks.Add(newTask);
                _db.SaveChanges();
            });
            return result;
        }

        public bool Update(int id, TaskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                task.Name = model.Name;
                task.Description = model.Description;
                task.Photo = model.Photo;
                task.StartDate = model.CreationDate;
                task.EndDate = model.EndDate;
                task.File = model.File;
                task.DeskId = model.DeskId;
                task.Column = model.Column;
                task.CreatorId = model.CreatorId;
                task.ExecutorId = model.ExecutorId;

                _db.Tasks.Update(task);
                _db.SaveChanges();
            });
            return result;
        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                _db.Tasks.Remove(task);
                _db.SaveChanges();
            });
            return result;
        }
    }
}
