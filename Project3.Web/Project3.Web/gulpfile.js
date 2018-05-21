/// <binding BeforeBuild='clean, babel' ProjectOpened='watch' />
const gulp = require("gulp");
const babel = require("gulp-babel");
const plumber = require("gulp-plumber");
const merge = require('gulp-merge');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const rename = require('gulp-rename');
const del = require('del');
const cache = require('gulp-cached');
const imagemin = require('gulp-imagemin');
const es6Path = "src/content/js/**/*.js";
// const ignorePaths = ["!Src/Vendor/", "!Content/js/plugins/", "!Content/js/vendor/"];
const compilePath = "wwwroot/dist/js";
const fontsPath = "wwwroot/dist/fonts";
const imagesPath = "wwwroot/dist/images";
const cssPath = "wwwroot/dist/css";

// gulp.task('cleanSrc', () => {
//     return del(["Src/content/js/**"]);
// });

// gulp.task("copyFromContentToSrc", ["cleanSrc"], () => {
//     gulp.src(["Content/js/**/*.js", "!Content/js/**/*.min.js", "!Content/js/components", "!Content/js/components/**", "!Content/js/plugins", "!Content/js/plugins/**", "!Content/js/sections", "!Content/js/sections/**", "!Content/js/vendor", "!Content/js/vendor/**"])
//         .pipe(gulp.dest("Src/js"));
// });

gulp.task('clean', () => {
    return del.sync(["wwwroot/dist"]);
});

gulp.task("minify", () => {
    gulp.src([paths.js, "!src/content/js/**/*.min.js"])
        .pipe(cache('minifying'))
        .pipe(plumber())
        .pipe(babel())
        .pipe(uglify())
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest(compilePath));
});

gulp.task("bundle.babel", () => {
    gulp.src([es6Path])
        .pipe(cache('transpiling'))
        .pipe(plumber())
        .pipe(babel())
        .pipe(concat('site.js'))
        .pipe(gulp.dest(compilePath));
});

gulp.task("babel:watch", () => {
    gulp.watch(es6Path, ["bundle.babel"]);
});

gulp.task("watch", () => {
    gulp.watch(es6Path, ["bundle.babel"]);
});

//gulp.task("copyFonts", () => {
//    gulp.src([
//        // "Src/sass/vendor/Metronic/demo/fonts/**",
//        "Src/fonts/**"
//    ])
//        .pipe(gulp.dest(fontsPath));
//});

gulp.task('image', () =>
    gulp.src('src/images/*')
        .pipe(imagemin())
        .pipe(gulp.dest(imagesPath))
);

gulp.task('style', () =>
    gulp.src('src/content/css/*.css')
    .pipe(concat('style.css'))
    .pipe(gulp.dest(cssPath))
);


gulp.task("vendor:js", () => {
    gulp.src([
        "node_modules/bootstrap/dist/js/bootstrap.js",
        "node_modules/jquery-validation/dist/jquery.validate.js",
        "node_modules/@fortawesome/fontawesome/index.js"
    ])
        .pipe(concat('vendor.js'))
        .pipe(gulp.dest(compilePath));
});

gulp.task("default", ["clean", "image", "vendor:js", "style", "bundle.babel", "babel:watch"]);