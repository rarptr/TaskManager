using System;

namespace TaskManager.API.Models.Services
{
    public abstract class AbstractionService
    {
        public bool DoAction(Action action)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
