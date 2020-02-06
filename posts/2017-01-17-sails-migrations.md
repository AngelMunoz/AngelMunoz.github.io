---
title: sails-migrations
tags:
  - sails
  - migrations
category:
  - Backend
published: "2017-01-17"
layout: post
---

[SailsJs]: https://sailsjs.com
[Waterline]: https://github.com/balderdashy/waterline
[sails-db-migrate]: https://github.com/building5/sails-db-migrate
[node-db-migrate]: https://github.com/db-migrate/node-db-migrate
[node-db-migrate Docs]: https://github.com/db-migrate/node-db-migrate
[SQL API]: https://db-migrate.readthedocs.io/en/latest/API/SQL/
[NoSQL API]: https://db-migrate.readthedocs.io/en/latest/API/NoSQL/
[twitter]: https://twitter.com/daniel_tuna
[.native()]: http://sailsjs.com/documentation/reference/waterline-orm/models/native
[.query()]: http://sailsjs.com/documentation/reference/waterline-orm/models/query

# Hello Everyone!
[SailsJs] is indeed an awesome framework and I think sometimes it doesn't receive the love it should.
However not everything is love, sails sometimes lacks some stuff when we intend to go production mode.
Sails greately gives you a powerful ORM [Waterline] which will do most of your stuff when needed and what is needed.
if for some reason waterline doesn't offer you enough, you can always rely on the [.native()] or [.query()] methods
which will accept mongo/sql statements.

One of the things waterline has is automatic migrations lifting the switch on `config/models.js` 
```js
modules.export = {
  migrate: 'alter'
}
```
**However this setting is only development recommended** because it is an experimental feature and 
sometimes if when *lifting* up the application something goes wrong it may fail and make you lose some data.
Note that you should always have **backups** of your data.
I learned this the bad way when our users table was deleted on a failed lifting (which we had backup for c: )
This ends up leading to two options
- Manual SQL migrations
- Look for a integration which does migrations

So I went looking for a solution to this and sadly thought there were no up to date hooks which could help on the case.


Then I found [sails-db-migrate] which is a hook that exposes grunt tasks to generate  and run [node-db-migrate] migrations pulling the connection data from sails itself
instead having to have a separate database.json file.

This hook does what it promises, generates the files you needs and runs the migrations and supports MySQL, PostrgeSQL and MongoDB.
using sails-mysql, sails-postgresql or sails-mongo depending on your setup.

## Installation
You need a couple of things
- sails-db-migrate (the hook)
- pg # or mysql or mongodb
- db-migrate
- sails-{mysql|mongo|postgresql}

if using mongodb you also need db-migrate-mongodb
```
npm install --save sails-db-migrate 
npm install --save mongodb 
npm install --save sails-mongo 
npm install --save db-migrate@0.9.26 
npm install --save db-migrate-mongodb
```

## Configuration
We will need a few things before doing migrations
That's right we need to register these new tasks and supply settings to the hook
```
tasks
|
|--- config
|     |
|     ... a bunch of files ...
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
|    |--- dbMigrate.js <<< add this
|
|--- pipeline.js
|--- README.md
```
and  inside `dbMigrate.js` add the following line
```js
module.exports = require('sails-db-migrate').gruntTasks
```
now inside  `config` directory we will add anotehr caled `migrations.js`
```
config
|
|--- env
|     |
|     ... files ...
|--- locales
|     |
|     ... files ...
|--- blueprints.js
|--- .
|--- .
|--- .
|--- .
|--- migrations.js <<< add this
```
and add the following lines to it
**Note** Connection name matches a field from `config/connections.js`
```js
module.exports.migrations = {
  connection: 'myMongoDbConnection',
  table: 'sails_migrations',
  migrationsDir: 'sails_migrations',
  coffeeFile: false
}
```
if you want to use a coffee file instead of a javascript one you can change that to true


## Usage
After all of this has been done we can start generating our migration files.
yo can find the [node-db-migrate Docs] really useful and will most likely cover you.
be sure to have grunt installed globally first and then type on your terminal/console
```
grunt db:migrate:create --name=add_collection_foos
```
this will output to the console
```
Running "db:migrate:create" (db:migrate) task
+ db-migrate create add_collection_foos --migrations-dir sails_migrations --table sails_migrations
DATABASE_URL=mongodb://root:****@localhost:27017/portal_pruebas
[INFO] Created migration at sails_migrations/20170114075021-add-collection-foos.js

Done.
```
if you see it was pretty sraight forward
let's check the content
```js
var dbm = global.dbm || require('db-migrate');
var type = dbm.dataType;

exports.up = function(db, callback) {
  callback();
};

exports.down = function(db, callback) {
  callback();
};
```
so far so good but let's add some collections
```js
let dbm = global.dbm || require('db-migrate')
let type = dbm.dataType
exports.up = function(db, cb){
    db.createCollection('users', function(){
      db.createCollection('roles',cb)
    }),
}

exports.down = function(db, cb){
  db.dropCollection('users', function(){
    db.dropCollection('roles', cb)
  })
}
```
this is great but to be honest my callback techniques are not so good, so I'd rather use the `async` package
so let's add up
```js
let dbm = global.dbm || require('db-migrate')
let type = dbm.dataType
const async = require('async')
exports.up = function(db, cb) {
  async.series([
    db.createCollection.bind(db, 'users'),
    db.createCollection.bind(db, 'roles'),
    db.addIndex.bind(db, 'users', 'users_index', {
      email: 1
    }, { unique: true }),
    db.insert.bind(db, 'roles', [{
      name: 'Administrator',
      val: 'admin'
    }, {
      name: 'User',
      val: 'user'
    }])
  ], cb)
}
exports.down = function(db, cb) {
  async.series([
    db.dropCollection.bind(db, 'users'),
    db.dropCollection.bind(db, 'roles')
  ], cb)
}
```
as you can see we can add collections and indexes as well as insert data in each migration, running our migration is quite simple:
```
grunt db:migrate:up
```
```
Running "db:migrate:up" (db:migrate) task
+ db-migrate up --migrations-dir sails_migrations --table sails_migrations
DATABASE_URL=mongodb://root:****@localhost:27017/portal_pruebas
[INFO] Processed migration 20170104201949-create-cUsers
[INFO] Done
```
using mongo's cli tool we can check if our collections were added or not

```
~$ show dbs
local           0.000GB
portal_pruebas  0.000GB
~$ use portal_pruebas
switched to db portal_pruebas
~$ show collections
roles
sails_migrations
users
```

and also let' check for registries
```
~$ db.roles.find({})
{ "_id" : ObjectId("5879dbf69732e272196f00fd"), "name" : "Administrator", "val" : "admin" }
{ "_id" : ObjectId("5879dbf69732e272196f00fe"), "name" : "User", "val" : "user" }
~$
```

and in the same way we can revert our migration too

```
grunt db:migrate:down
```

```
Running "db:migrate:down" (db:migrate) task
+ db-migrate down --migrations-dir sails_migrations --table sails_migrations
DATABASE_URL=mongodb://root:****@localhost:27017/portal_pruebas
[INFO] Defaulting to running 1 down migration.
INFO] Processed migration 20170104201949-create-cUsers
[INFO] Done
```

if you need to run more you can specify using
`grunt db:migrate:down --count=5`
this means that the last 5 migrations will be reverted
and if you check your data inside mongo you will fine something is quite gone

```
~$ show dbs
local           0.000GB
portal_pruebas  0.000GB
~$ use portal_pruebas
switched to db portal_pruebas
~$ show collections
sails_migrations
~$
```

that is of course because we droped collections in our down part of the migrations script

#### Hey it's all good but please I use mysql
change your database connection `connection:'theMysqlCollecion'` inside `config/migrations.js` 
to another connection name that should match a mysql connection in your `config/connections.js` file and use the SQL api.

```js
exports.up = function(db, cb) {
 async.series([
   db.createTable('pets', {
    columns: {
      id: { type: 'int', primaryKey: true, autoIncrement: true },
      name: 'string'  // shorthand notation
    },
    ifNotExists: true
  });
 ], cb)
};
```
you can find more info on the [SQL API] and [NoSQL API]
you can do what you would expect from a migration, changing table/column/collection names
insert data during migrations, drop tables/columns/collections.
You should never abandon your database wherever it is, but sometimes time constraints or convenience calls for a speedier way to migrate your models

In the use cases I've worked on, creating droping renaming and inserting has been the most I had to do.

-----
Well That's all for today I hope this works for you and takes off some worries about using sails in production
when it comes to your data.
Do you have any comments? please write them down or contact me in my [twitter] See you around.