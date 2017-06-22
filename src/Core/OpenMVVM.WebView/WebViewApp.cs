namespace OpenMVVM.WebView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    using OpenMVVM.Core;

    public class WebViewApp
    {
        internal Dictionary<string, BindingInfo> bindingInfos = new Dictionary<string, BindingInfo>();

        internal Dictionary<string, BindingInfo> collectionBindingInfos = new Dictionary<string, BindingInfo>();

        private readonly ViewModelLocatorBase viewModelLocator;

        private readonly BridgeMapper bridgeMapper;

        private readonly Action onAppReady;

        //public WebViewApp(ViewModelLocatorBase viewModelLocator, Action<string, object> notifyActionArg, Action<string, object> notifyCollection, Action onAppReady)
        //{
        //    this.viewModelLocator = viewModelLocator;
        //    this.notifyAction = notifyActionArg;
        //    this.notifyCollection = notifyCollection;
        //    this.onAppReady = onAppReady;
        //}

        public WebViewApp(ViewModelLocatorBase viewModelLocator, IBridge bridge, Action onAppReady)
        {
            this.viewModelLocator = viewModelLocator;
            this.onAppReady = onAppReady;

            this.bridgeMapper = new BridgeMapper(bridge, this);
        }

        public object GetValueForPath(string bpath)
        {
            var strings = bpath.Split('.');
            string viewModelName = strings[0];

            PropertyInfo viewModelPropertyInfo = this.viewModelLocator.GetType().GetRuntimeProperty(viewModelName);

            var currentPropertyType = viewModelPropertyInfo.PropertyType;
            var currentPropertyInstance = viewModelPropertyInfo.GetValue(this.viewModelLocator);
            var currentPath = viewModelName;

            string[] bindingSegments = strings.Skip(1).ToArray();
            int? index = null;
            foreach (string bindingSegment in bindingSegments)
            {
                string propName = bindingSegment;
                if (bindingSegment.EndsWith("]"))
                {
                    var split = bindingSegment.Split('[');
                    propName = split[0];
                    index = int.Parse(split[1].TrimEnd(']'));
                }
                try
                {
                    PropertyInfo runtimePropertyInfo = currentPropertyType.GetRuntimeProperty(propName);

                    if (runtimePropertyInfo == null)
                    {
                        return null;
                    }

                    currentPropertyInstance = runtimePropertyInfo.GetValue(currentPropertyInstance);

                    if (currentPropertyInstance is IList && index != null)
                    {
                        if (((IList)currentPropertyInstance).Count > 0)
                        {
                            currentPropertyInstance = ((IList)currentPropertyInstance)[index.Value];
                            currentPropertyType = currentPropertyInstance.GetType();
                            currentPath = $"{currentPath}.{propName}[{index.Value}]";
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        currentPropertyType = runtimePropertyInfo.PropertyType;
                        currentPath = $"{currentPath}.{propName}";
                    }
                }
                catch (Exception) { }
            }

            return currentPropertyInstance;
        }

        public void RegisterBindingCollection(string bpath)
        {
            this.RegisterBinding(bpath, true);
        }

        public void RegisterBinding(string bpath, bool insideCollection = false)
        {
            var strings = bpath.Split('.');
            string viewModelName = strings[0];

            PropertyInfo viewModelPropertyInfo = this.viewModelLocator.GetType().GetRuntimeProperty(viewModelName);

            var currentPropertyType = viewModelPropertyInfo.PropertyType;
            var currentPropertyInstance = viewModelPropertyInfo.GetValue(this.viewModelLocator);
            var currentPath = viewModelName;

            this.SubscribeForUpdates(currentPropertyInstance, viewModelName, false, insideCollection);

            string[] bindingSegments = strings.Skip(1).ToArray();
            int? index = null;
            foreach (string bindingSegment in bindingSegments)
            {
                string propName = bindingSegment;
                if (bindingSegment.EndsWith("]"))
                {
                    var split = bindingSegment.Split('[');
                    propName = split[0];
                    index = int.Parse(split[1].TrimEnd(']'));
                }
                try
                {
                    PropertyInfo runtimePropertyInfo = currentPropertyType.GetRuntimeProperty(propName);

                    if (runtimePropertyInfo == null)
                    {
                        return;
                    }

                    currentPropertyInstance = runtimePropertyInfo.GetValue(currentPropertyInstance);


                    currentPropertyType = runtimePropertyInfo.PropertyType;
                    currentPath = $"{currentPath}.{propName}";
                    this.SubscribeForUpdates(
                        currentPropertyInstance,
                        currentPath,
                        bpath == currentPath,
                        insideCollection);


                    if (currentPropertyInstance is IList && index != null)
                    {
                        if (((IList)currentPropertyInstance).Count > 0)
                        {
                            currentPropertyInstance = ((IList)currentPropertyInstance)[index.Value];
                            currentPropertyType = currentPropertyInstance.GetType();
                            currentPath = $"{currentPath}[{index.Value}]";

                            this.SubscribeForUpdates(
                                currentPropertyInstance,
                                currentPath,
                                bpath == currentPath,
                                insideCollection);
                        }
                    }

                }
                catch (Exception e)
                {
                    
                }
            }
        }

        // ovu metodu treba srediti da se obrisu hendleri kad uradi rebind
        public void SubscribeForUpdates(object observableObject, string currentPath, bool get, bool insideCollection)
        {
            if (observableObject != null)
            {
                var vmType = observableObject.GetType();
                var typeInfo = vmType.GetTypeInfo();

                if (!this.bindingInfos.ContainsKey(currentPath))
                {
                    INotifyPropertyChanged notifyPropertyChanged = observableObject as INotifyPropertyChanged;
                    if (notifyPropertyChanged != null)
                    {
                        var bindingInfo = new BindingInfo(currentPath, this, insideCollection, observableObject, this.bridgeMapper);
                        this.bindingInfos.Add(currentPath, bindingInfo);

                        notifyPropertyChanged.PropertyChanged += bindingInfo.MainViewModelPropertyChanged;
                    }

                    //if (typeInfo.ImplementedInterfaces.Any(i => i == typeof(INotifyPropertyChanged)))
                    //{
                    //    var runtimeEvent = vmType.GetRuntimeEvent("PropertyChanged");
                    //    if (runtimeEvent == null)
                    //    {
                    //        runtimeEvent = vmType.GetRuntimeEvent("INotifyPropertyChanged.PropertyChanged");
                    //    }
                    //    if (runtimeEvent != null)
                    //    {
                    //        var bindingInfo = new BindingInfo(currentPath, this, insideCollection, observableObject);
                    //        this.bindingInfos.Add(currentPath, bindingInfo);

                    //        runtimeEvent.AddEventHandler(
                    //            observableObject,
                    //            new PropertyChangedEventHandler(bindingInfo.MainViewModelPropertyChanged));
                    //    }
                    //}
                }



                if (typeInfo.ImplementedInterfaces.Any(i => i == typeof(INotifyCollectionChanged)))
                {
                    if (this.collectionBindingInfos.ContainsKey(currentPath))
                    {
                        BindingInfo collectionBindingInfo = this.collectionBindingInfos[currentPath];
                        if (collectionBindingInfo.ObservableObject != observableObject)
                        {
                            ((INotifyCollectionChanged)observableObject).CollectionChanged -=
                                collectionBindingInfo.CollectionChanged;
                            this.collectionBindingInfos.Remove(currentPath);

                            var bindingInfo = new BindingInfo(currentPath, this, insideCollection, observableObject, this.bridgeMapper);
                            this.collectionBindingInfos.Add(currentPath, bindingInfo);
                            ((INotifyCollectionChanged)observableObject).CollectionChanged +=
                                bindingInfo.CollectionChanged;
                        }
                    }
                    else
                    {
                        var bindingInfo = new BindingInfo(currentPath, this, insideCollection, observableObject, this.bridgeMapper);
                        this.collectionBindingInfos.Add(currentPath, bindingInfo);
                        ((INotifyCollectionChanged)observableObject).CollectionChanged +=
                            bindingInfo.CollectionChanged;
                    }
                }

                if (get)
                {
                    this.bridgeMapper.NotifyValueChanged(currentPath, observableObject);
                }
            }
        }

        public void PropertySet(string bpath, object value)
        {
            try
            {
                var strings = bpath.Split('.');
                string vmName = strings[0];
                string propName = strings[1];

                var viewModelPropertyInfo = this.viewModelLocator.GetType().GetRuntimeProperties().First(p => p.Name == vmName);
                var propertyInfo = viewModelPropertyInfo.PropertyType.GetRuntimeProperties()
                    .First(p => p.Name == propName);
                propertyInfo.SetValue(viewModelPropertyInfo.GetValue(this.viewModelLocator), value);
            }
            catch (Exception) { }
        }

        public void FireCommand(string bpath, string param)
        {
            ICommand command = GetValueForPath(bpath) as ICommand;

            if (command == null)
            {
                return;
            }
            object paramValue = null;
            if (!string.IsNullOrEmpty(param))
            {
                paramValue = GetValueForPath(param);
            }

            command.Execute(paramValue);
        }

        private void Init()
        {
            this.onAppReady();
        }
    }
}
