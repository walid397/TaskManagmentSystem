using Application.Contract;
using Application.DTO_s;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class Services : IServices
    {
        ITaskReposaitory _taskReposaitory;
        IMapper _mapper;
        public Services(ITaskReposaitory taskReposaitory, IMapper mapper ) 
        {
            _taskReposaitory = taskReposaitory;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<TaskItemDto>> CreateAsync(TaskItemDto entity)
        {
            var tasksList = await _taskReposaitory.GetAllAsync();
            var tasks = tasksList.ToList();
            var existTask = tasks.Any(t => t.Name == entity.Name);
            if (existTask)
            {
                ResponseDTO<TaskItemDto> FailedResult = new ResponseDTO<TaskItemDto>
                {
                    Entity = null,
                    IsSuccessfull = false,
                    ErrorMessage = "Task Already Exist"
                };
                return FailedResult;
            }
            else
            {

            var taskItem = _mapper.Map<TaskItem>(entity);
                var result = await _taskReposaitory.CreateAsync(taskItem);
                var taskmapDtp = _mapper.Map<TaskItemDto>(result);
                ResponseDTO<TaskItemDto> SuccessedResult = new ResponseDTO<TaskItemDto>
                {
                    Entity = taskmapDtp,
                    IsSuccessfull = true,
                    ErrorMessage = "Successed"
                };
                return SuccessedResult;
            }

        }

        public async Task<ResponseDTO<TaskItemDto>> DeleteAsync(int id)
        {
            var tasksList = await _taskReposaitory.GetAllAsync();
            var tasks = tasksList.ToList(); 
            var existTask = tasks.Any(t => t.Id == id); 
            if (!existTask)
            {
                ResponseDTO<TaskItemDto> FailedResult = new ResponseDTO<TaskItemDto>
                {
                    Entity = null,
                    IsSuccessfull = false,
                    ErrorMessage = "Task Not Found"
                };
                return FailedResult;
            }
            else
            {

                var Task = await _taskReposaitory.DeleteAsync(id);
                var DeletedTaskMapped = _mapper.Map<TaskItemDto>(Task);

                ResponseDTO<TaskItemDto> SuccessResult = new ResponseDTO<TaskItemDto>
                {
                    Entity = DeletedTaskMapped,
                    IsSuccessfull = true,
                    ErrorMessage = "Task Deleted Successfully"
                };
                return SuccessResult;

            }
        }

        public async Task<ResponseDTO<List<TaskItemDto>>> GetAllAsync()
        {
            var listOfTasks = await _taskReposaitory.GetAllAsync();
            var listOfTasksList = listOfTasks.ToList();
            var listOfTasksMapping = _mapper.Map<List<TaskItemDto>>(listOfTasksList);

            return new ResponseDTO<List<TaskItemDto>>
            {
                Entity = listOfTasksMapping,
                IsSuccessfull = true
            };
        }


        public async Task<ResponseDTO<TaskItemDto>> GetByIdAsync(int id)
        {
            var task = await _taskReposaitory.GetByIdAsync(id); 

            if (task == null)
            {
                return new ResponseDTO<TaskItemDto>
                {
                    Entity = null,
                    IsSuccessfull = false,
                    ErrorMessage = "Task Not Found"
                };
            }

            var taskMapped = _mapper.Map<TaskItemDto>(task);

            return new ResponseDTO<TaskItemDto>
            {
                Entity = taskMapped,
                IsSuccessfull = true,
                ErrorMessage = null
            };
        }



        public async Task<ResponseDTO<List<TaskItemDto>>> FilterByPriority(Priority level)
        {
            var filteredTasks = await _taskReposaitory.FilterByPriority(level);

            if (!filteredTasks.Any()) 
            {
                return new ResponseDTO<List<TaskItemDto>>
                {
                    Entity = null,
                    IsSuccessfull = false,
                    ErrorMessage = "No tasks found for the given priority"
                };
            }

            var mappedTasks = _mapper.Map<List<TaskItemDto>>(filteredTasks.ToList());

            return new ResponseDTO<List<TaskItemDto>>
            {
                Entity = mappedTasks,
                IsSuccessfull = true,
                ErrorMessage = null
            };
        }


        public async Task<ResponseDTO<TaskItemDto>> UpdateAsync(TaskItemDto taskItem)
        {
            var existingTask = await _taskReposaitory.GetByIdAsync(taskItem.Id);

            if (existingTask == null)
            {
                return new ResponseDTO<TaskItemDto>
                {
                    Entity = null,
                    IsSuccessfull = false,
                    ErrorMessage = "Task Not Found"
                };
            }

            existingTask.Id = taskItem.Id;
            existingTask.Priority = taskItem.Priority;
            existingTask.AssignedUser = taskItem.AssignedUser;
            existingTask.DueDate = taskItem.DueDate;
            existingTask.Name = taskItem.Name;
            existingTask.Description = taskItem.Description;

            var updatedTask = await _taskReposaitory.UpdateAsync(existingTask);

            var mappedUpdatedTask = _mapper.Map<TaskItemDto>(updatedTask);

            return new ResponseDTO<TaskItemDto>
            {
                Entity = mappedUpdatedTask,
                IsSuccessfull = true,
                ErrorMessage = null
            };
        }

    }
}
