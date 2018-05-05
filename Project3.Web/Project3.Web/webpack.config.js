const path = require("path");
const CleanWebpackPlugin = require('clean-webpack-plugin');

module.exports = {

    // This is the entry point or start of our react applicaton
    entry: "./src/index.jsx",

    // The plain compiled JavaScript will be output into this file
    output: {
        path: path.resolve(__dirname, 'wwwroot/js'),
        publicPath: '~/js/',
        filename: "bundle.js"
    },

    // This section desribes the transformations we will perform
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                use: "babel-loader",
                include: /src/,
                exclude: /node_modules/
            }
            //{
            //    // Only working with files that in in a .js or .jsx extension
            //    test: /\.jsx?$/,
            //    // Webpack will only process files in our app folder. This avoids processing
            //    // node modules and server files unnecessarily
            //    include: /src/,
            //    exclude: /node_modules/,
            //    loader: "babel",
            //    query: {
            //        // These are the specific transformations we'll be using.
            //        presets: ["react", "es2015"]
            //    }
            //}
        ]
    },
    resolve: {
        extensions: ['.js', '.jsx']
    },
    plugins: [
        new CleanWebpackPlugin(['build'], {
            verbose: true
        }),
    ],
    // This lets us debug our react code in chrome dev tools. Errors will have lines and file names
    // Without this the console says all errors are coming from just coming from bundle.js
    devtool: "eval-source-map"
};