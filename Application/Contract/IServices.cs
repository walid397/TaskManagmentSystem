using Application.DTO_s;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contract
{
    public interface IServices
    {
        public Task<ResponseDTO<List<TaskItemDto>>> GetAllAsync();
       public Task<ResponseDTO<TaskItemDto>> GetByIdAsync(int id);
       public Task<ResponseDTO<TaskItemDto>> CreateAsync(TaskItemDto entity);
       public Task<ResponseDTO<TaskItemDto>> UpdateAsync(TaskItemDto TaskItem);
       public Task<ResponseDTO<TaskItemDto>> DeleteAsync(int id);
       public Task<ResponseDTO<List<TaskItemDto>>> FilterByPriority(Priority level);

    }
}
