# JobScheduling
Deployment steps:
1) Create a Database on Microsoft SQL Server;
2) Open properties_table_creation_script SQL script which located in Database folder, set your Database name and execute it;
3) Open appsettings.json, set your server and database (Data Source and Initial Catalog) in connection string;
4) Fill out the EmailingCredentials property in appsettings.json with your Outlook post credentials to .NET SmtpClient for send message;
5) Uncomment SendEmailAsync in EmailingJob and enter recipient email to first argument of this method;
6) Run application;