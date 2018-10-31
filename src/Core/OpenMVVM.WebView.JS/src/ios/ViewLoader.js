class ViewLoader {
    loadView(viewName, callback) {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", "Views/" + viewName + ".html", true);
        xhr.onreadystatechange = function () {
            var div = document.createElement("div");
            div.innerHTML = this.responseText;
            var element = div.firstChild;
            callback(element);
        };
        xhr.send();
    }
}

export const viewLoader = new ViewLoader();