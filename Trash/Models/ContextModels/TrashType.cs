using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class TrashType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }   // Per Kilogerams

        // Navigation Properties

        public ICollection<Trash> Trashes { get; set; }
    }
}
