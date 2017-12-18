const gulp = require('gulp');
const webpack = require('webpack');
const path = require('path');
const gutil = require('gulp-util');
const parseArgs = require('minimist')(process.argv);
const env = parseArgs.e || 'development';
const moment = require('moment');
moment.locale('ru');

let plugins = [
    new webpack.DefinePlugin({
        __DEV__: env !== 'production',
        __RELEASE__: '"' + moment().format('L_LT') + '"'
    }),
    new webpack.optimize.CommonsChunkPlugin('vendor', 'vendor.min.js')
];

if (env !== 'development') {
    plugins.push(new webpack.optimize.OccurenceOrderPlugin());
    plugins.push(new webpack.NoErrorsPlugin());
    plugins.push(new webpack.optimize.DedupePlugin());
    plugins.push(new webpack.optimize.UglifyJsPlugin({
        compress: {
            warnings: false,
            drop_console: true,
            unsafe: true
        }
    }));
}

let rootDir = path.resolve(__dirname, '../src');

let jsIncludes = [
    rootDir
    // path.resolve(__dirname, '../node_modules/handlebars')
];
let resolveRoot = [rootDir];

const runner = webpack({
    context: rootDir,
    devtool: env === 'development' ? 'source-map' : null,
    entry: {
        main: './main.js',
        vendor: [
            // npm libs:
            'matreshka'
        ]
    },
    output: {
        path: path.resolve(__dirname, '../wwwroot/dist'),
        filename: '[name].min.js',
        publicPath: '/wwwroot/'
    },
    resolve: {
        root: resolveRoot
    },
    plugins: plugins,
    module: {
        loaders: [
            {
                loaders: ['babel-loader?presets[]=es2015,presets[]=stage-0,plugins[]=transform-class-properties'],
                include: jsIncludes
            },
            // {
            //     test: /\.html$/,
            //     loader: 'raw',
            //     include: htmlIncludes
            // },
            {
                test: /\.less$/,
                loader: 'style-loader!css-loader!less-loader',
                include: jsIncludes
            },
            // {
            //     test: /\.json$/,
            //     loader: 'json-loader'
            // },
            // {
            //     test: /\.handlebars/,
            //     loader: 'handlebars-loader'
            // }
        ]
    }
});

gulp.task('webpack', (callback) => runner.run((err, stats) => {
    if (err) throw new gutil.PluginError('webpack', err);

    gutil.log('[webpack]', stats.toString({
        colors: true,
        minimal: true,
        chunks: false
    }));

    callback();
}));

module.exports = runner;
