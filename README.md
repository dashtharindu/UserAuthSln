This contains SHA 512 password hashing with random salt and JWT bearer token generating methods. 
This project is using SQL server with Dapper

Please Create SQL database using the provided SQL file

Then you can test this api using POSTMAN or other app using following API calls,

https://localhost:44360/api/auth/register
Post

Application/Json
{
	"username":"hello",
	"password":"something"
}


===========================================
https://localhost:44360/api/auth/login
Post

Application/Json
{
	"username":"hello",
	"password":"something"
}

============================================

https://localhost:44360/api/auth/protected
Get
pass bearer access token in header(returns after above login call)


===========================================
You may need to change SQL connection string and Token key (40 or above random characters recomended in production env)
in appsettings.json file