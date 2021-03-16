# ignium
Task Description : Faucet - cryptocurrency reward system which gives out small amounts of coins or tokens to users.

- Implement REST api that provides methods:
-- Get current faucet bitcoin balance and value (in $); [Done]
-- claim 0.001 bitcoin from the faucet, by provided email. [Done]
--- If faucet doesn't have required amount, don't claim anything. [Done]
--- Each email can only claim once per 24h. [Done]
--- Claim simply reduces faucet balance by claimed bitcoin amount. [Done]

- Initial balance at server overall start is 0. Upon further server restarts balance is persisted. [Done]
- Have Scheduled job running every 2 hours, adding $500 worth of bitcoin to the faucet. [Done, Used Quartz.net, cron settings saved in appsettins.json]
-- You'd need to get live bitcoin price to know how much bitcoin to add. Up to you where to get. [Done, used https://www.coindesk.com/coindesk-api]
- Scheduled job running every 24 hours, sending out email to admin about total claimed amount since last sent email. [Done, SMTP settings needs to be configured in appsettings.json]

- Store data in embedded DB with ORM on top. [Done, Used sqlLite, please set path to database in appsettings.json]
- Code must be async. [Done, tried to implement in whole project]
- Use dependency injection for components. [Done]
- Use logging. [Done, please set file path for log in appsettings.json]
- Generate REST api documentation. [Done, Used Swagger]
- Write meaningful integration tests for the REST api. [Done]
- Which components, tech to use - up to you.
- One project is enough for all the code; no need to create complex project hierarchy. [For integration test added second project.]
