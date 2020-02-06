---
title: File Upload/Download in SailsJS
published: "2017-01-07"
layout: post
tags:
  - sails
categories:
  - Backend
---
[SailsJs]: https://sailsjs.org
[socket.io]: http://socket.io/


# Hello
Hello everyone today I bring you a quick post on [SailsJs] Upload/Download files [SailsJs] is a really good nodejs framework, which allows you to quickly setup a RESTful API with a controller and a model (yes that easy), yet it allows you to do classic server-side rendering for websites including custom actions, views and whatever you could expect in a classic web development,Also! did I mention it has integrated Websocket support? via [socket.io]
It is a really powerful framework built on top of [express] if you haven't checked it you should just go and try!

### Hands on!
This post assumes you know how to connect to a database (either mysql, postgre or mongo or just disk)
and that you know how to access a route from the url
Let's make a model and a controller for our Files `api/models/File.js`


```js
/**
 * File.js
 *
 * @description :: TODO: You might write a short summary of how this model works and what it represents here.
 * @docs        :: http://sailsjs.org/documentation/concepts/models-and-orm/models
 */

module.exports = {
  tableName: 'files', // entirely optional
  attributes: {
    filename: {
      type: 'string',
      required: true
    },
    path: {
      type: 'string',
      required: true
    }
}

```
now let's create our controller
`api/controllers` directory
```js
/**
 * FileController
 *
 * @description :: Server-side logic for managing Files
 * @help        :: See http://sailsjs.org/#!/documentation/concepts/Controllers
 */
module.exports = {
  
}
```
We can either do it by hand or using `sails` command tools
```
$ sails generate api File
```

we now will add upload action
```js
/**
 * FileController
 *
 * @description :: Server-side logic for managing Files
 * @help        :: See http://sailsjs.org/#!/documentation/concepts/Controllers
 */
module.exports = {
  upload: function(req, res) {
    res.json({message: 'yay!'})
  }
}
```
and if you go to the `/file/upload` url you will see the next message:
```json
  { "message": "yay!" }
```
If everything is working well up to this point, let's add some logic there
```js
module.exports = {
  upload: function(req, res) {
    req.file('file') // this is the name of the file in your multipart form
      .upload({
        // optional
        // dirname: [SOME PATH TO SAVE IN A CUSTOM DIRECTORY]
      }, function(err, uploads) {
        // try to always handle errors
        if (err) { return res.serverError(err) }
        // uploads is an array of files uploaded 
        // so remember to expect an array object
        if (uploads.length === 0) { return res.badRequest('No file was uploaded') }
        // if file was uploaded create a new registry
        // at this point the file is phisicaly available in the hard drive
        File.create({
          path: uploads[0].fd,
          filename: uploads[0].filename,
        }).exec(function(err, file) {
          if (err) { return res.serverError(err) }
          // if it was successful return the registry in the response
          return res.json(file)
        })
      })
  }
}
```
if everything goes fine, you should be able to send files now and both the files are uploaded
and the database registers where are those files

now let's do some downloads; In the same controller create a new action
```js
module.exports = {
  upload: function(req, res) {
    // the logic for uploads
  },
  download: function(req, res) {
    res.json({message: 'yay!'}) // let's first check that the route exists
  }
}
```
navigate to `/file/download` and you should see that json message
since we were saving our uploads into our database we can just download our file
searching it by id or by name, let's see
```js
module.exports = {
  upload: function(req, res) {
    // the logic for uploads
  },
  download: function(req, res) {
    var fileID = req.param('id') 
    // gets the id either in urlencode, body or url query
    File
      .findOne({ id: fileID })
        .exec((err, file) => {
          if (err) { return res.serverError(err) }
          if (!file) { return res.notFound() }
          // we use the res.download function to download the file 
          // and send a ok response
          res.download(file.path, function (err) {
            if (err) {
              return res.serverError(err)
            } else {
              return res.ok()
            }
        })
      })
  }
}
```

So that's it! you can now upload and download files in sailsjs,
sails is very flexible in the way it handles things so if you need to customize
either the uploaded file's name you can pass it in the `upload({})` method
as well as location.

### Bonus
Let's register a custom route for our upload/download actions
in your `config/routes.js` file you can add custom routes and also change urls for existing ones

```js
module.exports.routes = {
  'post /fileuploader': 'FileController.upload',
  'get /filedownloader/:id': 'FileController.download',
}
```
the string `'get /filedownloader/:id': 'FileController.download',` tells sails to only accept `GET` requests to the `/filedownloader/:id` route, which is maped to the download action inside FileController.js file `:id` ends up being the id param which we will use to find our file registry.
The same goes for `'post /fileuploader': 'FileController.upload',` which will only accept posts in the `/fileuploader` url
maping our request to the upload action in FileController

as you can see it's pretty straight forward upload and download files in [SailsJS]
I hope this helps anyone to understand file upload/download in sails; see you next time!
Have any comments!? leave them below.