using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s
{
    public class ResponseDTO<T>
    {
        public T Entity { get; set; }
        public bool IsSuccessfull { get; set; }
        public string ErrorMessage { get; set; }
    }

}
