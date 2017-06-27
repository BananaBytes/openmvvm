# Welcome to OpenMVVM Project!

OpenMVVM project unifies two important sub-projects:

1. OpenMVVM Framework
2. OpenMVVM.WebView Project

![OpenMVVM architecture diagram](https://raw.githubusercontent.com/BananaBytes/openmvvm/master/doc/images/OpenMVVM_diagram.PNG)

## OpenMVVM Framework

OpenMVVM Framework provides **unified API** for MVVM applications **across many platforms**.
Our goal is to cover all commonly used platforms and allow developers to use the same set of page viewmodels, classes and services everywhere.
Just use OpenMVVM base classes, contracts and appropriate implementation for desired platform and go cross-platform!

## OpenMVVM.WebView Project

OpenMVVM.WebView is open-source project based on OpenMVVM Framework that **brings HTML/CSS UI technology to C# based MVVM applications**.
Write your viewmodels in C# and let web designers create UI for them, using simple binding library.

Platforms supported so far:

* UWP
* iOS
* Android

Preparing support for
* .NET Core

### How to use

In order to get started quickly, take a look at the Samples solution directory (OpenMVVM.Samples.Basic.WebView.*). Steps may vary depending on the target platform, but here is the basic concept:

* Create empty application with WebView control
* Add references to
  * OpenMVVM.Core
  * OpenMVVM.WebView
  * OpenMVVM.WebView.UWP (or appropriate platform specific)
  * Portable.Ninject
* Add platform js and css files in www folder
* Add index.html in www folder, with content such as:

```html
<body>

<div data-mvvmstart></div>

<script src="platform/js/openmvvm.bridge.windows.js"></script>
<script src="platform/js/openmvvm.js"></script>

</body>
```

* Create ViewModels for pages, extend PageViewModel from OpenMVVM.Core
* Create ViewModelLocator (see sample)
* Create WebViewPage (or WebViewActivity or WebViewController for other platforms) by altering OnLaunched method in App.xaml.cs

```csharp
protected override void OnLaunched(LaunchActivatedEventArgs e)
{
    WebViewPage webViewPage = new WebViewPage(new ViewModelLocator());
    webViewPage.AppReady += this.CurrentContentAppReady;

    Window.Current.Content = webViewPage;
    Window.Current.Activate();
    ...
```
```csharp
    private void CurrentContentAppReady(object sender, EventArgs e)
    {
        var navigationService = ViewModelLocatorBase.InstanceFactory.GetInstance<INavigationService>();
        navigationService.NavigateTo("MainView");
    }
```

* Create MainView html template in www\Views folder
  * Example:

```html
<div data-view="MainView" data-context="MainViewModel" class="container">
    <h2 data-bind="{textContent: 'Title'}"></h2>
    <input type="text" placeholder="search..." class="style-2 clearfix focus" id="search" data-bind="{value: 'SearchInput'}">
    <br/>
    <ul id="services-list" data-repeat="Items">
        <li data-context="MainViewModel" data-actions="{click: {command: 'NavigateToItemCommand', parameter: context}}">
            <a class="image" >
                <img data-bind="{src: 'ImageUrl'}"/>
            </a>
            <div class="content">
                <h3 data-bind="{textContent: 'Title'}"></h3>
                <h4 data-bind="{textContent: 'Description'}"></h4>
            </div>
        </li>
    </ul>
</div>
```

* This should map to your MainViewModel and its property in ViewModelLocator.

*Notice: You will probably need to use Visual Studio 2017 to build the examples.*