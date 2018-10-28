class ViewLoader {
    loadView(viewName, callback) {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", "Views/" + viewName + ".html", true);
        xhr.onreadystatechange = function () {
            if (this.readyState !== 4) return null;
            if (this.status !== 200) return null;
            var div = document.createElement("div");
            div.innerHTML = this.responseText;
            var element = div.firstChild;
            callback(element);
        };
        xhr.send();
    }
}

export const viewLoader = new ViewLoader();