const gulp = require('gulp');

require('./gulp/webpack');
require('./gulp/watch');

gulp.task('default', ['webpack']);
