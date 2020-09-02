# Business Day Calculator

This project is an extremely useful business day calculator, that calculates the number of business days between two dates, EXCLUDING those two dates. It even takes into account public holidays! Right now you must enter all your holidays by editing a JSON file.

## Running the project

You must have .NET Core SDK 3.1.400 or higher installed. Load the solution into Visual Studio and click the "Run" button, which should present you with the Swagger page. If not, navigate to http://localhost:5000/swagger. Enter in your desired dates using the Swagger interface and submit the request.

You can edit `holidays.json` to add and remove new public holidays. Restarting the application after changing holidays is not required. I hope that the format of the JSON file is self-explanatory. After editing the JSON file, keep an eye on the application logs. The application will proceed as normally as possible even with a corrupt or missing file, but errors will be reported.

## Contributions

This is the basic layout of the application to help understand logic flow and where future additions should go.

### BusinessDaysBetween.Api

Basic implementation of a web API for this project. Mediatr is used in the controller because it's a great pattern to keep the API layer thin and reduce coupling.

### BusinessDaysBetween.Business

"Business" logic for the project, split into the following:

* Application/ -- application logic, glue that binds things together. Command handlers in this case.
* Commands/ -- raw commands invoked to get into the business logic.
* Infrastructure/ -- dealing with the outside world: files, external APIs, etc.
* Services/ -- internal computations or things without state.
* ValueObjects/ -- data bags that don't have identifiers, not promoted to entities.
* (missing) Entities/ -- there are none!

If things start getting bigger, this could be split into multiple projects.

## Implementation notes

* As usual, working extensively with dates in .NET made me sad about .NET's time libraries. For this in production, or anything larger, I'd install Nodatime.
* I've conveniently ignored timezones, this may cause (very) subtle bugs.
* In the calculation, I'm not sure my idea of adjusting start and end dates so I can do inclusive maths was the best, because it requires too much nudging later. But I don't want this project to take all month, and the tests pass, so for now it'll do...

## TODO

I time capped myself at around 4-5 hours, but there's more work to do.

* Full integration tests over the web API.
* The `Holiday` class needs a lot of work. If we're reading from an external resource it'd be nicer to use a DTO of some sort, with a 'real' business object (or two) for validation, but this seemed like too much boilerplate (and too much time).
* I'd like to make things immutable where possible, but for now I didn't want to fight with ASP.NET model binding and JSON deserialisation to make it happen in a tidy way. Also related to the above point.
* Integration with some kind of free public holidays API or data source would make this much more useful.
* Validation, better exception handling, friendlier errors.
