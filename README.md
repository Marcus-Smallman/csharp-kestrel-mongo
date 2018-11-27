# csharp-kestrel-mongo
This repository is for the csharp-kestrel-mongo-armhf OpenFaas function template

To use the template the following instructions can be followed:

    faas-cli template pull https://github.com/Marcus-Smallman/csharp-kestrel-mongo.git

    faas-cli new mongo-function --lang csharp-kestrel-mongo-armhf

 The template uses the following environment variable:
 
| Envrionment Variable  | Example                                    | Required |
| --------------------- | ------------------------------------------ | -------- |
| mongo_endpoint        | mongo_endpoint: localhost:27017            | yes      |
| mongo_database_name   | mongo_database_name: my_mongo_database     | no       |
| mongo_collection_name | mongo_collection_name: my_mongo_collection | no       |

If the **mongo_database_name** is not provided the database name will default to: `default_database`

If the **mongo_collection_name** is not provided the collection name will default to: `default_collection`

An example of a stack used to build and deploy the mongo function:

```
provider:
  name: faas
  gateway: http://127.0.0.1:8080

functions:
  mongo-function:
    lang: csharp-kestrel-mongo-armhf
    handler: ./mongo-function
    image: <docker-registry>/mongo-function
    environment:
      mongo_endpoint: localhost:27017
      mongo_database_name: mongo_database_name
      mongo_collection_name: mongo_collection_name
```

The following environment variables can also be set if the connection between the function and mongo is slow on intial start:
```
read_timeout: 20
write_timeout: 20
```
###### This sets the read and write timeout to 20 seconds.

The function can then be deployed:
```
  faas-cli build -f mongo-function.yml
  faas-cli push -f mongo-function.yml
  faas-cli deploy -f mongo-function.yml
```
Once deployed the functions can be invoked:
```
"This will be added into mongo" | faas-cli invoke -f mongo-function.yml mongo-function
```

The current MongoDB driver used for this template is: **2.3.0**