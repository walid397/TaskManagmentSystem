using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Application.Contract
{
    public interface ITaskReposaitory
    {
        Task<TaskItem> CreateAsync(TaskItem entity);
        Task<TaskItem> DeleteAsync(int id);
        Task<IQueryable<TaskItem>> GetAllAsync();
        ValueTask<TaskItem> GetByIdAsync(int id);
        Task<TaskItem> UpdateAsync(TaskItem entity);
        Task<IQueryable<TaskItem>> FilterByPriority(Priority level);
    }

}