using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    abstract internal class BaseSort<Type>
    {

        public delegate int Comparator(Type a1, Type a2);
        protected readonly Comparator comp;
        protected BaseSort(BaseSort<Type>.Comparator comp)
        {
            this.comp = comp;
        }

        abstract public Type[] Sort(Type[] nums);
    }
}
