using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class BubbleSort<Type> : BaseSort<Type>
    {
        public BubbleSort(Comparator comp) : base(comp) { }
        public override Type[] Sort(Type[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                    if (comp(array[j], array[j + 1]) > 0)
                        (array[j + 1], array[j]) = (array[j], array[j + 1]);
            return array;
        }
    }
}
