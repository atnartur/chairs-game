const gulp = require('gulp');

require('./gulp/webpack');
require('./gulp/copy');
require('./gulp/watch');

gulp.task('default', ['webpack', 'copy']);
