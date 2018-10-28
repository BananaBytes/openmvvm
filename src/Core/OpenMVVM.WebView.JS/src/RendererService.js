class RendererService {
    constructor(parserService, context) {
        this.parserService = parserService;
        this.elementCache = context.elementCache;
    }

    getNextPath (rootPath, currentPath, indexAlterFunction) {
        var affectedSubPath = currentPath;
        var posOfIndex = rootPath.length + 1;
        var tempWithoutPrefix = affectedSubPath.substring(posOfIndex);
        var indexLength = tempWithoutPrefix.indexOf("]");
        var tempBeforeIndex = affectedSubPath.substr(0, posOfIndex);
        var tempAfterIndex = tempWithoutPrefix.substr(indexLength);
        var indexString = affectedSubPath.substr(rootPath.length + 1, indexLength);
        var index = parseInt(indexString);

        var newIndex = indexAlterFunction(index);

        var nextPath = tempBeforeIndex + newIndex + tempAfterIndex;
        return {
            nextPath: nextPath,
            index: index,
            newIndex: newIndex
        };
    }

    renderCollectionItemFromTemplate (collectionPath, template, child, newIndex) {
        var clone = template.cloneNode(true);

        if (newIndex < child.children.length) {
            child.insertBefore(clone, child.children[newIndex]);

            var otherAffected = Object.keys(this.elementCache).filter((k) => {
                return k.indexOf(collectionPath + "[") === 0;
            }).reduce((newData, k) => {
                newData[k] = this.elementCache[k];
                return newData;
            },
            {});

            var temp = {};

            for (var item in otherAffected) {
                if (otherAffected.hasOwnProperty(item)) {
                    var pathInfo = this.getNextPath(collectionPath,
                        item,
                        function (indx) {
                            return indx + 1;
                        });

                    if (pathInfo.index >= newIndex) {
                        temp[pathInfo.nextPath] = this.elementCache[item];
                        delete this.elementCache[item];
                    }
                }
            }

            for (var tempItem in temp) {
                if (temp.hasOwnProperty(tempItem)) {
                    this.elementCache[tempItem] = temp[tempItem];
                }
            }
        } else {
            child.appendChild(clone);
        }

        this.parserService.analyzeThis(clone, collectionPath + "[" + newIndex + "]", true);
    }

    renderForeach (items, collectionPath, template, child) {
        while (child.firstChild) {
            child.removeChild(child.firstChild);
        }

        for (var i = 0; i < items.length; i++) {
            this.renderCollectionItemFromTemplate(collectionPath, template, child, i);
        }
    }
}

export default RendererService;