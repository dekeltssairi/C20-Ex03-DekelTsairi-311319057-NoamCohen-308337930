using System;
using FacadeLayer;

namespace FacadeFacebook
{
    public class ByDateAndTimeSorter : ISortStrategy
    {
        public bool ShouldSwap(ConfirmedSickLocation i_FirsCheckin, ConfirmedSickLocation i_SecondCheckin)
        {
            return DateTime.Compare(i_FirsCheckin.DateOfSickConfirmation, i_SecondCheckin.DateOfSickConfirmation) < 0;
        }
    }
}
