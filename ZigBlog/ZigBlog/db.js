﻿db.createCollection('Parameters');

db.createCollection('Posts');

db.createCollection('Users');
db.Users.createIndex({ UserNameLower: 1 }, { unique: true });
db.Users.createIndex({ EmailAddress: 1 }, { unique: true, sparse: true });

db.createCollection('Roles');
db.Roles.insert([{ "Name": "Administrator" }, { "Name": "Blogger" }, { "Name": "Commenter" }]);