const webpack = require('webpack');
const path = require('path');

module.exports = {
    watch: true,
    entry: './wwwroot/src/app.js',
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist'),
        filename: 'bundle.js'
    },
    module: {
        rules: [
            {
                test: /\.css$/,
            },
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader', options: {
                        presets:
                            ['babel-preset-react', 'babel-preset-env']
                    }
                }
            }
        ]
    },
    resolve: {
        extensions: ['*', '.js', '.jsx']
    },


    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
        })
    ],
    
    devServer: {
        contentBase: './dist',
        hot: true
    }
    
};