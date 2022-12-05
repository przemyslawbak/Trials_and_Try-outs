babel --presets react,es2015 wwwroot/js/source/ -d wwwroot/js/build
browserify wwwroot/js/build/app.js -o wwwroot/bundle.js
date; echo;