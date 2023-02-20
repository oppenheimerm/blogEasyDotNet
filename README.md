# BlogeEasey.Net Core

Blog engine build on [Asp.Net Core, Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/?WT.mc_id=dotnet-35129-website&view=aspnetcore-7.0)

# Features
* Render LaTeX mathematical expressions using [Math](https://www.mathjax.org/).
* Code Highlighting via [highlight.js](https://highlightjs.org/), using the the GitHub theme.
* Add cover photo.
* TODO - Image gallery
# Getting Started
To get your blog up and running, we need to preform and ensure the following steps have been completed

## Prerequisites

### FrontEnd

#### Dependencies

For bundling and minification you will need to install the following `npm` packages:

```sh
npm install --global gulp-cli
```
```sh
npm install --save-dev gulp-clean
```
```sh
npm install gulp-clean-css --save-dev

For `gulp-imagemin` user version: `7.0.0`
```
```sh
npm install --save-dev gulp-imagemin
```
```sh
npm install --save-dev gulp-minify
```

You can find the above files are used in the `gulpfile.js`:

```js
var gulp = require('gulp');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var cleanCss = require('gulp-clean-css');
var imagemin = require('gulp-imagemin');;

gulp.task('pack-js', function () {
    return gulp.src(['./wwwroot/js/AppView.js', 'wwwroot/js/main.js'])
        .pipe(concat('bundle.js'))
        .pipe(minify({
            ext: {
                min: '.js'
            },
            noSource: true
        }))
        .pipe(gulp.dest('./wwwroot/js/dist'));
});

gulp.task('pack-css', function () {
    return gulp.src(['./wwwroot/css/root.css', './wwwroot/css/layout.css',
        './wwwroot/css/pages.css', './wwwroot/css//buttons__forms.css',
        './wwwroot/css/responsive.css'
    ])
        .pipe(concat('main.css'))
        .pipe(cleanCss())
        .pipe(gulp.dest('./wwwroot/css/dist'));
});

gulp.task('image-min', async function () {
    return gulp.src('./wwwroot/img/assets/*.{jpg,png}')
        .pipe(imagemin())
        .pipe(gulp.dest('./wwwroot/img/dist/assets'));
});

gulp.task('default', gulp.series('pack-js', 'pack-css'));
```

- TODO - add instructions for authentication.
- TODO - add instructions for bundling and minification of front-end assets.
- Add a `posts` folder in `wwwroot\img\` if not already added.

### Backend
To build the `css` and `javascript` file *gulp* is used (Developer *PowerShell* window):
`gulp`, this ouptputs the minified version of the .css and .js files.  To 

### FrontEnd



## TODO Features