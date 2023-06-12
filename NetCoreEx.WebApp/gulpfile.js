var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var download = require('gulp-download');
var cssnano = require('gulp-cssnano');
var cleanCss = require('gulp-clean-css');
var rename = require('gulp-rename');
var stripCssComments = require('gulp-strip-css-comments');
var clean = require('gulp-clean');
var minify = require('gulp-minify');
var cssmin = require('gulp-cssmin');
var version = "1.0";

var paths = {
    styles: {
        //Thứ tự ở dưới sẽ được ưu tiên
        src: [
            'wwwroot/assets/plugins/global/plugins.bundle.css',
            'wwwroot/assets/css/style.bundle.css',
            'wwwroot/assets/plugins/custom/fullcalendar/fullcalendar.bundle.css',
            "wwwroot/vendor/css/*.css",
            'wwwroot/css/*.css'
        ],
        dest: 'wwwroot/assets-result/' + version + '/css/'
    },
    scripts: {
        src: [
            "wwwroot/assets/plugins/global/plugins.bundle.js",
            "wwwroot/assets/js/scripts.bundle.js",
            'wwwroot/assets/plugins/custom/fullcalendar/fullcalendar.bundle.js',
            'wwwroot/assets/plugins/custom/inputmask/jquery.inputmask.js',
            "wwwroot/vendor/js/*.js",
            "wwwroot/js/*.js"
        ],
        dest: 'wwwroot/assets-result/' + version + '/js'
    },
    //Tải những trang trên mạng
    vendor: {
        styles: [
        ],
        scripts: [
            "https://tygia.vn/Assets/TyGia/datepicker/bootstrap-datepicker.js"
        ]
    }
};
gulp.task('clean-css', function () {
    return del(['wwwroot/assets-result/' + version + '/css/*']);
});
gulp.task('clean-js', function () {
    return del(['wwwroot/assets-result/' + version + '/js/*']);
});
gulp.task('vendor-js', () => {
    return download(paths.vendor.scripts)
        .pipe(gulp.dest('wwwroot/vendor/js'));
});
gulp.task('vendor-css', () => {
    return download(paths.vendor.styles)
        .pipe(gulp.dest('wwwroot/vendor/css'));
});
gulp.task('js', function () {
    return gulp.src(paths.scripts.src, { sourcemaps: true })
        .pipe(uglify())
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest(paths.scripts.dest));
});

gulp.task('css', function () {
    return gulp.src(paths.styles.src)
        .pipe(cleanCss())
        .pipe(stripCssComments())
        .pipe(cssmin())
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest(paths.styles.dest));
});