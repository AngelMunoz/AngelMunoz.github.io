---
title: Sails-Front-Typescript
published: "2017-01-12"
layout: post
tags:
 - typescript
 - sails
 - node
category:
 - Backend
---
[SailsJs]: https://sailsjs.com
[twitter]: https://twitter.com/daniel_tuna

# Hello everyone!
If you have used [Sailsjs] you may have used coffescript on your client-side javascript since any javascript/coffeescript that is put inside `assets` folder will be copied over to the `.tmp` dir used to serve static files of your sails application.
The following configurations may apply to any [compile] to javascript language as long as it has a grunt plugin (there are also gulp hooks out there).

The basic idea is to register new tasks and slightly modificate some other tasks to allow the automated grunt tasks do the job for us.

### Installation
we start on the basics
- `npm i -g typescript typings`
- `npm i grunt-ts@~5.5.1`

after all is installed we'll go to our  `tasks` folder
### configurations
Inside the `tasks` directory we will find both `config` and `register`
```
tasks
|
|--- config
|    |--- clean.js
|    |--- concat.js
|    |--- copy.js
|    |--- cssmin.js
|    |--- less.js
|    |--- sails-linker.js
|    |--- sync.js
|    |--- uglify.js
|    |--- watch.js
|    |--- typescript.js <<< add this
|
|--- register
|    |--- build.js
|    |--- buildProd.js
|    |--- compileAssets.js
|    |--- default.js
|    |--- linkAssets.js
|    |--- linkAssetsBuild.js
|    |--- linkAssetsBuildProd.js
|    |--- prod.js
|    |--- syncAssets.js
|
|--- pipeline.js
|--- README.md
```
Let's beggin looking at `typescript.js`

```js
module.exports = function(grunt){
  grunt.config.set('ts', {
    dev: {
      tsconfig: 'assets/tsconfig.json'
    }
  })
  grunt.loadNpmTasks('grunt-ts')
}
```
pretty simple right?
but we have to change a few files too
`copy.js`
```js
...
dev: {
      files: [{
        expand: true,
        cwd: './assets',
        src: ['**/*.!(less|coffee|ts|d.*|json)'],
        dest: '.tmp/public'
      }]
    },
...
```
`sync.js`
```js
...
dev: {
      files: [{
        expand: true,
        cwd: './assets',
        src: ['**/*.!(less|coffee|ts|d.*|json)'],
        dest: '.tmp/public'
      }]
    },
...
```
I've added ts, d.* and json to the regex so we don't pass our typings and typings.json files later on.


now inside `compileAssets.js` we also make a slight modification
```js
module.exports = function(grunt){
  grunt.registerTask('compileAssets', [
    'clean:dev',
    'jst:dev',
    'less:dev',
    'copy:dev',
    'coffee:dev',
    'ts'
  ]);
};
```
and now in `syncAssets.js`
```js
module.exports = function (grunt) {
  grunt.registerTask('syncAssets', [
    'jst:dev',
    'less:dev',
    'sync:dev',
    'coffee:dev'
    'ts'
  ]);
};
```

### Assets!
now inside assets you can put any `.ts` file and sails will compile it on changes made to that file.
in case you are wondering which `tsconfig.json` use:
```json
{
  "compilerOptions": {
    "target": "es5",
    "module": "system",
    "charset": "utf-8",
    "sourceMap": false,
    "outFile": "../.tmp/public/js/app.js"
  },
  "include": [
    "src/**/*"
  ],
  "exclude": [
    "node_modules",
    "bower_components",
    "typings"
  ],
  "compileOnSave": false
}
```
remember that you can completely modificate and adjust grunt tasks and typescript config to your like and needs
if you don't want to use typescript, but you want to use those shiny ESNext features you can also setup Babel
in a similar way, just add your configuration task and register it in some part of the workflow.
also if you don't need templates, less files or coffeescript files, you can always disable those tasks to improve compilation times for
other kinds of tasks that you have setup.
If everythin went well, next time you `sails lift` your app you should be able to compile typescript files for your frontend in your sails apps

This is one of the things I like about [Sailsjs], it is so configurable and allows you to addapt it to your way to work and lets you build up stable and sizable apps in short time
if you need to move your front-end code to another standalone app, you can always swap it out from your assets.

---------
That's all for today! I hope this works out for you! if you have any doubts or comments please comment below or on [twitter] :)
