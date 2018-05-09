const path = require("path");
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");
const OptimizeCssAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const bundleOutputDir = "./wwwroot/dist";

const VENDOR_LIBS = [
    "bootstrap",
    "bootstrap/dist/css/bootstrap.css",
    "event-source-polyfill",
    "react",
    "react-dom",
    "react-router-dom",
    "jquery"
];

module.exports = env => {
    const isDevBuild = !(env && env.prod);

    return [
        {
            // This is the entry point or start of our react applicaton
            entry: {
                "bundle": "./src/index.jsx",
                "vendor": VENDOR_LIBS
            },

            // The plain compiled JavaScript will be output into this file
            output: {
                path: path.join(__dirname, bundleOutputDir),
                publicPath: "dist/js/",
                filename: "js/[name].js"
            },

            // This section desribes the transformations we will perform
            module: {
                rules: [
                    {
                        test: /\.jsx?$/,
                        use: "babel-loader",
                        include: /src/,
                        exclude: /node_modules/,
                    },
                    {
                        test: /\.css$/,
                        use: ExtractTextPlugin.extract({
                            fallback: "style-loader",
                            use: "css-loader"
                        })
                        //exclude: /node_modules/
                    },
                    {
                        test: /\.(png|jpe?g|gif)$/,
                        use: [
                            {
                                loader: "url-loader",
                                options: {
                                    limit: 40000
                                },
                            }
                            //"image-webpack-loader"
                        ]
                    },
                    {
                        test: /\.(woff|woff2|eot|ttf|otf|svg)$/,
                        use: [
                            {
                                loader: "file-loader",
                                options: {
                                    name: "[sha512:hash:base64:7].[ext]",
                                    outputPath: "fonts/",
                                    publicPath: "fonts/"
                                }
                            }
                        ]
                    }
                ]
            },
            resolve: {
                extensions: [".js", ".jsx"]
            },
            plugins: [
                new webpack.ProvidePlugin({
                    $: "jquery",
                    jQuery: "jquery"
                }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
                new CleanWebpackPlugin([bundleOutputDir], {
                    verbose: true
                }),
                new webpack.optimize.CommonsChunkPlugin({
                    name: "vendor",
                    minChunks: function (module) {
                        // This prevents stylesheet resources with the .css or .scss extension
                        // from being moved from their original chunk to the vendor chunk
                        if (module.resource && (/^.*\.css$/).test(module.resource)) {
                            return false;
                        }
                        return module.context && module.context.includes("node_modules");
                    }
                }),
                new webpack.optimize.CommonsChunkPlugin({
                    name: "manifest",
                    minChunks: Infinity
                })
            ].concat(isDevBuild ? [
                new ExtractTextPlugin("css/[name].css"),
                // Plugins that apply in development builds only
                new webpack.SourceMapDevToolPlugin({
                    filename: "[file].map", // Remove this line if you prefer inline source maps
                    moduleFilenameTemplate: path.relative(bundleOutputDir, "[resourcePath]") // Point sourcemap entries to the original file locations on disk
                })
            ] : [
                new ExtractTextPlugin("css/[name].min.css"),
                new OptimizeCssAssetsPlugin({
                    assetNameRegExp: /\.css$/g,
                    cssProcessor: require("cssnano"),
                    cssProcessorOptions: { discardComments: { removeAll: true } },
                    canPrint: true
                }),
                // Plugins that apply in production builds only
                new webpack.optimize.UglifyJsPlugin(),
            ]),
            // This lets us debug our react code in chrome dev tools. Errors will have lines and file names
            // Without this the console says all errors are coming from just coming from bundle.js
            devtool: "eval-source-map"
        }
    ];
};