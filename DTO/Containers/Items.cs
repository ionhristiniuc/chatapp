using System.Collections.Generic;

namespace DTO.Containers
{
    public class Items<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
    }
}