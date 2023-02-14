using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class CombSort<Type> : BaseSort<Type>
    {
        readonly int coef = 127;
        public CombSort(Comparator comp) : base(comp) { }

        public override Type[] Sort(Type[] array)
        {
            int gap = array.Length;
            bool swaps = true;

            while (gap > 1 || swaps)
            {
                gap = gap * 100 / coef;

                if (gap < 1)
                    gap = 1;

                swaps = false;

                for (int i = 0; i + gap < array.Length; i++)
                {
                    int igap = i + gap;

                    if (comp(array[i], array[igap]) > 0)
                    {
                        (array[igap], array[i]) = (array[i], array[igap]);
                        swaps = true;
                    }
                }
            }
            return array;
        }
    }
}
