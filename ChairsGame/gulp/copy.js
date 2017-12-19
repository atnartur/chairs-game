const gulp = require('gulp');

gulp.task('copy',  () => gulp.src([
    './node_modules/bootstrap/dist/css/bootstrap.min.css',
    './node_modules/bootstrap/dist/css/bootstrap-grid.min.css',
    // './node_modules/jquery/dist/jquery.min.js',
    // './node_modules/bootstrap/dist/js/bootstrap.min.js'
]).pipe(gulp.dest('./wwwroot/dist/')));