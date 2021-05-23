using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBloki.Domain.Entities;
using System.Data.Entity;

namespace SuperBloki.Domain.Concrete
{
    class EFDbContext : DbContext
    {
        public DbSet<Constructor> Constructors { get; set; }
    }
}
