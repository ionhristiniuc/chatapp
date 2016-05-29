using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Containers;

namespace Chat.DataService.Core
{
    public class ObjectMapper
    {
        public static Items<T> ToItems<T>(IEnumerable<T> data, int page, int perPage, int count)
        {
            return new Items<T>
            {
                Data = data,
                Page = page,
                TotalPages = count % perPage == 0 ? count / perPage : count / perPage + 1,
                TotalElements = count
            };
        }
    }
}
