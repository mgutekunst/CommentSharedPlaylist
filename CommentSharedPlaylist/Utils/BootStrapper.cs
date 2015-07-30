using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using SharedPlaylist.Core.Service;
using SharedPlaylist.Core.ViewModels;

namespace CommentSharedPlaylist.Utils
{
    public class BootStrapper
    {
        private static bool _isInitialized;

        public static void Init()
        {
            if (!_isInitialized)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


                // Register ViewModels
                SimpleIoc.Default.Register<MainViewModel>();
                SimpleIoc.Default.Register<DataService>();
            }

            _isInitialized = true;
        }

        public BootStrapper()
        {
            Init();
        }

    }
}
