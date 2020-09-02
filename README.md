# Business Day Calculator

This project is an extremely useful business day calculator, that calculates the number of business days between two dates, EXCLUDING those two dates. It even takes into account public holidays! Right now you must enter all your holidays by editing a JSON file.

## Running the project

You must have .NET Core 3.1 or higher installed. Load the solution into Visual Studio and click the "Run" button, which should present you with the Swagger page. If not, navigate to http://localhost:5000/swagger. Enter in your desired dates using the Swagger interface and submit the request.

You can edit `holidays.json` to add and remove new public holidays. Restarting the application after changing holidays is not required. I hope that the format of the JSON file is self-explanatory.

## Implementation notes

* As usual, working extensively with dates in .NET made me sad about .NET's time libraries. For this in production, or anything larger, I'd install Nodatime.
* I've conveniently ignored timezones.
* I'm not sure my idea of adjusting start and end dates so I can do inclusive maths was the best, because it requires too much nudging later, but I don't want this project to take all month, and the tests pass, so for now it'll do...

