namespace OpenMVVM.WebView
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public class BindingInfo
    {
        private readonly WebViewApp webViewApp;

        private readonly BridgeMapper bridgeMapper;

        private object observableObject;

        private string rootPath;

        public BindingInfo(string rootPath, WebViewApp webViewApp, bool insideCollection, object observableObject, BridgeMapper bridgeMapper)
        {
            this.RootPath = rootPath;
            this.webViewApp = webViewApp;
            this.bridgeMapper = bridgeMapper;
            this.ObservableObject = observableObject;
            this.InsideCollection = insideCollection;
        }

        public string RootPath
        {
            get
            {
                return this.rootPath;
            }

            set
            {
                this.rootPath = value;
            }
        }

        public bool InsideCollection { get; }

        public object ObservableObject
        {
            get
            {
                return this.observableObject;
            }
            set
            {
                this.observableObject = value;
            }
        }

        public void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.EndsWith("[]"))
            {
                return;
            }
            var propertyInfo = sender.GetType().GetRuntimeProperties().First(p => p.Name == e.PropertyName);
            var value = propertyInfo.GetValue(sender);

            var currentPath = this.RootPath + "." + e.PropertyName;

            this.bridgeMapper.NotifyValueChanged(currentPath, value);
        }
        

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var list = sender as IList;
            if (list != null)
            {
                var collectionBindingInfo =
                    this.webViewApp.bindingInfos.Where(k => k.Key.StartsWith(this.RootPath + "[")).ToArray();

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        var index = e.NewStartingIndex;
                        var pomeraj = e.NewItems.Count;
                        for (var i = list.Count - 1; i >= index; i--)
                        {
                            var value = this.RootPath + "[" + i + "]";
                            var keyValuePairs = collectionBindingInfo.Where(k => k.Key.StartsWith(value)).ToArray();
                            foreach (var keyValuePair in keyValuePairs)
                            {
                                this.webViewApp.bindingInfos.Remove(keyValuePair.Key); // odjaviti se i sa ivenata

                                var newKey = this.RootPath + "[" + (i + pomeraj) + "]"
                                             + keyValuePair.Key.Remove(0, value.Length);
                                keyValuePair.Value.RootPath = newKey;
                                this.webViewApp.bindingInfos.Add(newKey, keyValuePair.Value);
                            }
                        }

                        this.bridgeMapper.NotifyCollectionChanged(this.RootPath, e);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        // azurirati izmedju novog i starog indeksa
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        // kao gore kod add samo kontra. napraviti dobar test case
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        // remove pa insert, da bi radili bindinzi
                        // ili rucno se pretplatiti na novi, sto nije dobro
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        var valuePairs =
                            this.webViewApp.bindingInfos.Where(
                                i => i.Key.StartsWith(this.rootPath) && i.Value.InsideCollection).ToArray();
                        foreach (var keyValuePair in valuePairs)
                        {
                            this.webViewApp.bindingInfos.Remove(keyValuePair.Key);
                        }

                        this.bridgeMapper.NotifyCollectionChanged(this.RootPath, e);
                        this.bridgeMapper.NotifyValueChanged(this.rootPath, sender);
                            // skloniti bindinge na klijentu i serveru i opaliti ponovo procesiranje
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // _webViewApp.CallJs(this._rootPath, sender);
            }
        }
    }
}