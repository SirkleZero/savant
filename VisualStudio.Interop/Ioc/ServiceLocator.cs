using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VsServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace VisualStudio.Interop.Ioc
{
    internal static class ServiceLocator
    {
        public static void InitializePackageServiceProvider(IServiceProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            ServiceLocator.PackageServiceProvider = provider;
        }

        public static IServiceProvider PackageServiceProvider { get; private set; }

        public static TService GetInstance<TService>() where TService : class
        {
            if (typeof(TService) == typeof(IServiceProvider))
            {
                return (TService)GetServiceProvider();
            }

            return GetDTEService<TService>() ??
                   GetComponentModelService<TService>() ??
                   GetGlobalService<TService, TService>();
        }

        public static TInterface GetGlobalService<TService, TInterface>() where TInterface : class
        {
            if (ServiceLocator.PackageServiceProvider != null)
            {
                TInterface service = ServiceLocator.PackageServiceProvider.GetService(typeof(TService)) as TInterface;
                if (service != null)
                {
                    return service;
                }
            }

            return (TInterface)Package.GetGlobalService(typeof(TService));
        }

        private static TService GetDTEService<TService>() where TService : class
        {
            var dte = ServiceLocator.GetGlobalService<SDTE, DTE>();
            return (TService)QueryService(dte, typeof(TService));
        }

        private static TService GetComponentModelService<TService>() where TService : class
        {
            IComponentModel componentModel = ServiceLocator.GetGlobalService<SComponentModel, IComponentModel>();
            return componentModel.GetService<TService>();
        }

        private static IServiceProvider GetServiceProvider()
        {
            var dte = ServiceLocator.GetGlobalService<SDTE, DTE>();
            return ServiceLocator.GetServiceProvider(dte);
        }

        private static object QueryService(_DTE dte, Type serviceType)
        {
            Guid guidService = serviceType.GUID;
            Guid riid = guidService;
            var serviceProvider = dte as VsServiceProvider;

            IntPtr servicePtr;
            int hr = serviceProvider.QueryService(ref guidService, ref riid, out servicePtr);

            if (hr != VsConstants.S_OK)
            {
                return null;
            }

            object service = null;

            if (servicePtr != IntPtr.Zero)
            {
                service = Marshal.GetObjectForIUnknown(servicePtr);
                Marshal.Release(servicePtr);
            }

            return service;
        }

        private static IServiceProvider GetServiceProvider(_DTE dte)
        {
            IServiceProvider serviceProvider = new ServiceProvider(dte as VsServiceProvider);
            Debug.Assert(serviceProvider != null, "Service provider is null");
            return serviceProvider;
        }
    }
}
