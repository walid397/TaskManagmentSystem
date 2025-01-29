using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contract;
using Domain.Models;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;


namespace Infrastructure.Reposaitories
{
    public class TaskReposaitory : ITaskReposaitory
    {
        private readonly Context _context;  
        private readonly DbSet<TaskItem> _dbset;

        public TaskReposaitory(Context context)
        {
            _context = context; 
            _dbset = _context.Set<TaskItem>(); 
        }


        public async Task<TaskItem> CreateAsync(TaskItem entity)
        {
            var existingTask = await _dbset.FirstOrDefaultAsync(b => b.Id == entity.Id);

            if (existingTask != null)
            {
                return null;
            }

            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }


        public Task<IQueryable<TaskItem>> GetAllAsync()
        {
            var ReturnedTasks = _dbset.Select(b => b);
            return Task.FromResult(ReturnedTasks);
        }

        public async ValueTask<TaskItem> GetByIdAsync(int id)
        {
         var TaskReturned  =   await _dbset.FindAsync(id);
            return TaskReturned;
        }


        public async Task<TaskItem> UpdateAsync(TaskItem entity)
        {
            var oldTask = await _dbset.FirstOrDefaultAsync(b => b.Id == entity.Id);
            if (oldTask == null)
            {
                return null;
            }
            oldTask.Id = entity.Id;
            oldTask.Priority = entity.Priority;
            oldTask.AssignedUser = entity.AssignedUser;
            oldTask.DueDate = entity.DueDate;
            oldTask.Name = entity.Name;
            oldTask.Description = entity.Description;

            await _context.SaveChangesAsync();
            return oldTask;
        }

        public async Task<TaskItem> DeleteAsync(int id)
        {
            var existingTask = await _dbset.FirstOrDefaultAsync(b => b.Id == id);
            if (existingTask == null)
            {
                return null;
            }
            else
            {
                  _dbset.Remove(existingTask);
              await _context.SaveChangesAsync();
                return existingTask;
            }
        }

        public async Task<IQueryable<TaskItem>> FilterByPriority(Priority level)
        {
            var allTasks = await GetAllAsync();  
            var filteredTasks = allTasks.Where(b => b.Priority == level);  
            return filteredTasks;
        }

    }
}
