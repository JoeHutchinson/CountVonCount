# Count Von Count
HTML scrapper for counting word occurrences 

![Sesame Street - Count Von Count](https://pbs.twimg.com/profile_images/2184519702/CountVonCount_400x400.jpg)

Takes a URL as input and will retrieve the HTML page count just visible words and updates its database. The user is then shown a word cloud of top 100 words and below a table of the same words and their respective counts.

## Key technologies
- ASP.NET 4.7
- Web API 2
- SQL Server
- C#

## How to run
1. Create TestDB on your local machine
2. Execute DB_Create.sql to create tables and TVP
3. Build and run 
4. Enter any URL and click the Go button

## To do
- Resolve Enter on URL input error
- Validate user input
- DI into DB Store to aid testability
- Enhance smoke test to be E2E
- Encrypt using Always Encrypt and use SqlBulkCopy
- Hash the Id
