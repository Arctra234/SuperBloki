using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperBloki.Domain.Entities;
using SuperBloki.Domain.Abstract;

namespace SuperBloki.Domain.Concrete
{
    public class EFConstructorRepository : IConstructorRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Constructor> Constructors
        {
            get { return context.Constructors; }
        }

        public void SaveConstructor(Constructor constructor)
        {
            if (constructor.ConstructorID == 0)
                context.Constructors.Add(constructor);
            else
            {
                Constructor dbEntry = context.Constructors.Find(constructor.ConstructorID);
                if (dbEntry != null)
                {
                    dbEntry.ConstructorNumber = constructor.ConstructorNumber;
                    dbEntry.Name = constructor.Name;
                    dbEntry.Series = constructor.Series;
                    dbEntry.Producer = constructor.Producer;
                    dbEntry.ElementsAmount = constructor.ElementsAmount;
                    dbEntry.Priсe = constructor.Priсe;
                    dbEntry.ImageData = constructor.ImageData;
                    dbEntry.ImageMimeType = constructor.ImageMimeType;

                }
            }
            context.SaveChanges();
        }

        public Constructor DeleteConstructor(int constructorID)
        {
            Constructor dbEntry = context.Constructors.Find(constructorID);
            if (dbEntry != null)
            {
                context.Constructors.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }

}
