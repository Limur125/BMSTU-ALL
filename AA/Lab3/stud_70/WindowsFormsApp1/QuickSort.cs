using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class QuickSort<Type> : BaseSort<Type>
    {
        public QuickSort(Comparator comp) : base(comp) { }

        public override Type[] Sort(Type[] array) 
        {
            return Sort(array, 0, array.Length - 1);
        }

        private Type[] Sort(Type[] array, int left, int right)
        {
            int i = left;
            int j = right;
            Type pivot = array[left];
            while (i <= j)
            {
                while (comp(array[i], pivot) < 0)
                    i++;

                while (comp(array[j], pivot) > 0)
                    j--;

                if (i <= j)
                {
                    (array[j], array[i]) = (array[i], array[j]);
                    i++;
                    j--;
                }
            }

            if (left < j)
                Sort(array, left, j);
            if (i < right)
                Sort(array, i, right);
            return array;
        }
    }
}
