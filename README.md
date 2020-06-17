Web Hack: Sticky Notes
======================
_The best-of-breed cloud-based sticky note solution. Obviously not containing any security risks._

Getting started
---------------

### Cloning, building and testing
Start by cloning this repo. Then navigate into the `sticky-notes/Notes.Api/` folder and start the application with `dotnet run`:
```shell
$ sticky-api/Notes.Api> dotnet run
```

Open your favorite browser, and navigate to [localhost:5000/swagger](http://localhost:5000/swagger). This should open [Swagger UI](https://swagger.io/tools/swagger-ui/), where you can try out the API.

![Animation showing how to use Swagger UI](Images/notes-api-swagger.gif)

_Note: Contrary to what's shown in the gif above, you should get an authentication error when trying out the API. Finding a valid username/password will be the first task of the workshop._