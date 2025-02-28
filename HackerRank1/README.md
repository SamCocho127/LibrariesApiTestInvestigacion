# Library API Service
A company is launching a quiz management service. The service will be a web API layer built using .NET, with an existing prepared infrastructure. Implement two controllers: in LibrariesController, add the DELETE method, and in StudentsController, implement the POST, and GET methods as per the guidelines below. Additionally, implement corresponding services for the controllers, and register these services in the Startup.cs file to enable Dependency Injection.

## Environment
- .NET version: 6.0

## Read-Only Files
- LibraryService.Tests/IntegrationTests.cs

## Commands
- run:  
```
dotnet clean && dotnet restore && dotnet run --project LibraryService.WebAPI
```
- install:  
```
dotnet clean && dotnet build
```
- test: 
```
dotnet restore && dotnet build && dotnet test --logger xunit --results-directory ./reports/
```

## Sample Data
Here is an example of a quiz model JSON object:

```
{
    id: 5,
    name: "Library name",
    location: "2838 Violet Ct, Columbus, IN 47201, USA"
}  
```

Here is an example of a Student model JSON object:

```
{
    id: 3,
    name: "The Norton Anthology of English Literature",
    category: "Anthology",
    activityId: 5
}  
```

## Requirements

The service should adhere to the following API format and response codes:

`POST api/Activities/{activityId}/Students`:
  - Add the Student to the given activityId. 
  - The HTTP response code should be 201 on success.
  - For the body of the request, use the JSON example of the Student model given above.
  - If a quiz with `{activityId}` does not exist, return 404.

`GET api/Activities/{activityId}/Students`:
  - Return the entire list of Students for the quiz with the given activityId.
  - The HTTP response code should be 200.
  - If a quiz with `{activityId}` does not exist, return 404.
 
 `DELETE api/Activities/{activityId}`:
  - Delete the quiz with activityId. 
  - The HTTP response code should be 204 on success.
  - If a quiz with `{activityId}` does not exist, return 404.
 
NOTE: You need to add support for Dependency Injection for internal services (LibrariesService and StudentsService) in the project Startup.cs file.

## Sample Requests & Responses
<details><summary>Expand to view details on sample requests and responses for each endpoint.</summary>

`POST api/Activities/5/Students`

Example request:

```
{
    id: 3,
    name: "The Norton Anthology of English Literature",
    category: "Anthology"
}
```
The response code will be 201, and this Student will be added to the quiz with ID 5.

`GET api/Activities/5/Students`

Example response:

The response code is 200, and when converted to JSON, the response body (assuming that the below objects are all objects in the collection) is as follows:

```
[{
    id: 3,
    name: "The Norton Anthology of English Literature",
    category: "Anthology",
    activityId: 5
} {
    id: 10,
    name: "Inception",
    category: "Thriller",
    activityId: 5
}]
```

`DELETE api/Activities/10`

Example response:

Assuming that the quiz with ID 10 exists, the response code is 204 and there are no particular requirements for the response body. This causes the quiz with ID 10 to be removed from the collection. When a quiz with ID 10 does not exist, the response code is 404 and there are no particular requirements for the response body.

</details>
