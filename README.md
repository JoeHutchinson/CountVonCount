# Count Von Count
HTML scrapper for counting word occurrences 

![Sesame Street - Count Von Count](https://pbs.twimg.com/profile_images/2184519702/CountVonCount_400x400.jpg)

Takes a URL as input and will retrieve the HTML page count just visible words and updates its database. The user is then shown a word cloud of top 100 words and below a table of the same words and their respective counts.

# Key technologies
- ASP.NET 4.7
- Web API 2
- SQL Server
- C#

## Todo
- Resolve Enter on URL input error
- Validate user input
- DI into DB Store to aid testability
- Enhance smoke test to be E2E
- Encrypt using Always Encrypt and use SqlBulkCopy
- Hash the Id
