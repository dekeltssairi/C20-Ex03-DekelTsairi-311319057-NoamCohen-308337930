using FacadeLayer;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace FacadeFacebook
{
    public class CheckinsSorter
    {
        public ISortStrategy SortStrategy { get; set; }

        public CheckinsSorter(ISortStrategy i_Strategy)
        {
            SortStrategy = i_Strategy;
        }

        public void Sort(List<ConfirmedSickLocation> i_ConfirmedSickLocations)
        {

            /*parts.Sort(delegate (Part x, Part y)
            {
                if (x.PartName == null && y.PartName == null) return 0;
                else if (x.PartName == null) return -1;
                else if (y.PartName == null) return 1;
                else return x.PartName.CompareTo(y.PartName);
            });*/

            for (int i = 0; i < i_ConfirmedSickLocations.Count; i++)
            {
                for (int j = 0; j < i_ConfirmedSickLocations.Count; j++)
                {
                    if (SortStrategy.ShouldSwap(i_ConfirmedSickLocations[i], i_ConfirmedSickLocations[j]))
                    {
                        swap(i_ConfirmedSickLocations, i, j);
                    }
                }
            }
        }

        private static void swap<T>(List<T> i_List, int i_IndexA, int i_IndexB)
        {
            T tmp = i_List[i_IndexA];
            i_List[i_IndexA] = i_List[i_IndexB];
            i_List[i_IndexB] = tmp;
        }
    }
}
