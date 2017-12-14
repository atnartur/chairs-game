const gulp = require('gulp');
const livereload = require('gulp-livereload');
const gutil = require("gulp-util");
const webpack = require('./webpack');

gulp.task('watch', function() {
    livereload.listen();
    const cb = () => livereload.reload();

    webpack.watch({}, (err, stats) => {
        if (err) throw new gutil.PluginError("webpack", err);

    gutil.log("[webpack]", stats.toString({
        colors: true,
        minimal: true,
        chunks: false
    }));

    cb();
});
});