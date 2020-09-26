using FacadeLayer;
using FacebookWrapper.ObjectModel;

namespace FacadeFacebook
{
    public class ByABCSorter: ISortStrategy
    {
        public bool ShouldSwap(ConfirmedSickLocation i_FirsCheckin, ConfirmedSickLocation i_SecondCheckin)
        {
            return i_FirsCheckin.Location.CompareTo(i_SecondCheckin.Location) < 0;
        }
    }
}
