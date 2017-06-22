## Usage

In the html files where you want to use JsBridge, include a copy of the mt.js file.

```html
<script src="js/mt.js"></script>
```

Alernatively you can call InjectMtJavascript on your UIWebView but you will have to call it everytime a new page is loaded and since you usually have to wait until the page is loaded to do so, it is recommended to include mt.js instead to insure it's available when you need it.

### Browser Side

From a UIWebView you can do the following:

#### Log to Native side 

```javascript
Mt.API.info( 'This message will print on the native side using Console.WriteLine' );
```

#### Fire Events on the Native side 

```javascript
Mt.App.fireEvent('promptUser', { 
    msg: 'Hi, this msg is from the browser.',
    extra: 'You can send more then one property back',
    question: 'Did you get this message?',
    answer: 42
});
```

#### Subscribe to Events triggered from the Native side 

```javascript
Mt.App.addEventListener('handleNativeEvent', function(data) {

    if (data && data.ArbitraryProperty) {
	    console.log( data.ArbitraryProperty );
    }

});
```

### Native Side

From your MonoTouch application you can interact with your UIWebView as follows:

#### Fire Events on the Browser side 

```c#
viewController.WebView.FireEvent( "handleNativeEvent", new {
	Message = "The Native code says hi back. ;)",
    ArbitraryProperty = "more properties",
    Success = true
});
```

#### Subscribe to Events triggered from theBrowser side 

```c#
viewController.WebView.AddEventListener( "promptUser", delegate(FireEventData arg) {

    // show a native action sheet
    BeginInvokeOnMainThread (delegate { 
        var sheet = new UIActionSheet ( arg.Data["question"].ToString() );
        sheet.AddButton ( "Yes" );
        sheet.AddButton ( "No" );
        sheet.CancelButtonIndex = 1;
        sheet.ShowInView ( viewController.View );
    });

});
```

## Other Resources

* [Source Code Repository](https://github.com/crdeutsch/MonoTouch-JsBridge)
