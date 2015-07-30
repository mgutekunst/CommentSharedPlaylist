using Microsoft.Practices.ServiceLocation;
using SharedPlaylist.Core.ViewModels;

namespace SharedPlaylist.Core.Utils
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}
