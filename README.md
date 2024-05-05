# My solution on Labb 4 – MVC & Razor in Webbapplikationer i C#, ASP.NET

# Setup
In the Solutions Explorer click the arrow next to `Connected Services`
Right-Click `SQL Server Database`
Choose Edit (or Connect)
In the first box `Connection string name` fill in:
```console
ConnectionStrings:DefaultConnection
```
In the second box `Connection string value` fill in:
<YourConnectionStringToYourDB>
> You will have to provide your own connection string to your database above

<br>

In the `Package Manager Console` write:
```console
add-migration -OutputDir Data/Migrations "Initial migration"
```
then write:
```console
update-database
```
Now you can start the project and since it's the first time it is running it will also the function `Initialize` that's found inside the `DbInitializer` class
