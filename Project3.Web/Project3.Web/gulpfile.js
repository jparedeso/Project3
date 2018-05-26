﻿/// <binding BeforeBuild='default' ProjectOpened='watch' />
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
const es6Path = "src/content/js/*.js";
const stylePath = 'src/content/css/*.css';
const compilePath = "wwwroot/dist/js";
const fontsPath = "wwwroot/dist/fonts";
const imagesPath = "wwwroot/dist/images";
const cssPath = "wwwroot/dist/css";
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

//gulp.task("copyFonts", () => {
//    gulp.src([
//        // "Src/sass/vendor/Metronic/demo/fonts/**",
//        "Src/fonts/**"
//    ])
//        .pipe(gulp.dest(fontsPath));
//});

gulp.task('image', () =>
    gulp.src('src/content/images/*')
        .pipe(imagemin())
        .pipe(gulp.dest(imagesPath))
);

gulp.task('style', () =>
    gulp.src('src/content/css/*.css')
    .pipe(concat('site.css'))
    .pipe(gulp.dest(cssPath))
);

gulp.task("vendor:css", () => {
    gulp.src([
            "node_modules/bootstrap/dist/css/bootstrap.css",
            "node_modules/animate.css/animate.css"
        ])
        .pipe(concat('vendor.css'))
        .pipe(gulp.dest(cssPath));
});

gulp.task("vendor:js", () => {
    gulp.src([
        "node_modules/jquery/dist/jquery.js",
        "node_modules/bootstrap/dist/js/bootstrap.js",
        "node_modules/jquery-validation/dist/jquery.validate.js",
        "node_modules/@fortawesome/fontawesome/index.js"
    ])
        .pipe(concat('vendor.js'))
        .pipe(gulp.dest(compilePath));
});

gulp.task("default", ["clean", "image", "vendor:js", "vendor:css", "style", "bundle.babel"]);

gulp.task("watch", () => {
    gulp.watch(es6Path, ["bundle.babel"]);
    gulp.watch(stylePath, ["style"]);
});

gulp.task("vscode", ["clean", "image", "vendor:js", "vendor:css", "style", "bundle.babel", "watch"]);