using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBloki.Domain.Entities;

namespace SuperBloki.Domain.Abstract
{
    public interface IConstructorRepository
    {
        IEnumerable<Constructor> Constructors { get; }

        void SaveConstructor(Constructor constructor);
        Constructor DeleteConstructor(int ConstructorID);

    }
}
