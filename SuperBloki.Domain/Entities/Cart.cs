using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBloki.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Constructor constructor, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Constructor.ConstructorID == constructor.ConstructorID)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Constructor = constructor,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Constructor constructor)
        {
            lineCollection.RemoveAll(l => l.Constructor.ConstructorID == constructor.ConstructorID);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Constructor.Priсe * e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Constructor Constructor { get; set; }
        public int Quantity { get; set; }
    }
}
