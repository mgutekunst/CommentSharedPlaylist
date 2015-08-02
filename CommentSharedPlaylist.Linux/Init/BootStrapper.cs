using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using SharedPlaylist.Core.ViewModels;
using SharedPlaylist.Core.Service;

namespace CommentSharedPlaylist.Linux.Init
{
    public class BootStrapper
    {

	public BootStrapper()
	{
	    ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

	    // Register ViewModels
	    SimpleIoc.Default.Register<MainViewModel>();
	    SimpleIoc.Default.Register<DataService>();
	}

    }

}