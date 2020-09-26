using FacadeLayer;


namespace FacadeFacebook
{
    public interface ISortStrategy
    {
        bool ShouldSwap(ConfirmedSickLocation i_FirsCheckin, ConfirmedSickLocation i_SecondCheckin);
    }
}
