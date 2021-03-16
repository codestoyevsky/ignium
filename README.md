# ignium.io 
Task Description: Faucet - cryptocurrency reward system which gives out small amounts of coins or tokens to users.

- Implement REST API that provides methods:

  - Get current faucet bitcoin balance and value (in $); [Done :white_check_mark:]

  - claim 0.001 bitcoin from the faucet, by provided email. [Done :white_check_mark:]

    - If the faucet doesn't have the required amount, don't claim anything. [Done :white_check_mark:]

    - Each email can only claim once per 24h. [Done :white_check_mark:]

    - Claim simply reduces faucet balance by claimed bitcoin amount. [Done :white_check_mark:]

- Initial balance at the server overall start is 0. Upon further server restarts, the balance is persisted. [Done :white_check_mark:]

- Have Scheduled job running every 2 hours, adding $500 worth of bitcoin to the faucet. [Done :white_check_mark:, Used Quartz.net, CRON settings saved in appsettins.json]

  - You'd need to get live bitcoin price to know how much bitcoin to add. Up to you where to get. [Done :white_check_mark: , used https://www.coindesk.com/coindesk-api]

- Scheduled job running every 24 hours, sending out an email to an admin about total claimed amount since last sent email. [Done :white_check_mark:, SMTP settings need to be configured in appsettings.json]

- Store data in embedded DB with ORM on top. [Done :white_check_mark:, Used SQLite, please set path to the database in appsettings.json]

- Code must be async. [Done :white_check_mark:, tried to implement in the whole project]

- Use dependency injection for components. [Done :white_check_mark:] 
 
- Use logging. [Done :white_check_mark:, please set the file path for log in appsettings.json]

- Generate REST API documentation. [Done :white_check_mark:, Used Swagger]

- Write meaningful integration tests for the REST API. [Done :white_check_mark:]

- Which components, tech to use - up to you. - One project is enough for all the code; no need to create a complex project hierarchy. [For integration test added the second project.]
