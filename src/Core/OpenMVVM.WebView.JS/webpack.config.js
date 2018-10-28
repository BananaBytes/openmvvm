"use strict";

module.exports = {
    entry: {
        android: "./src/android/openmvvm.android.js",
        windows: "./src/windows/openmvvm.windows.js",
        wpf: "./src/wpf/openmvvm.wpf.js",
        dotnetcore: "./src/dotnetcore/openmvvm.dotnetcore.js",
        ios: "./src/ios/openmvvm.ios.js"
    },
    output: {
        path: __dirname + "/dist",
        filename: "openmvvm.[name].js"
    },
    module: {
        rules: [
            {
                enforce: "pre",
                test: /\.js?$/,
                use: {
                    loader: "eslint-loader"
                },
                exclude: [/node_modules/, /lib/]
            },
            {
                test: /\.js?$/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: ["@babel/preset-env"]
                    }
                },
                exclude: /node_modules/
            }
        ]
    },
    mode: "production"
};