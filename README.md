# Products.Api

This is a .NET8 application which uses Minimal API. 

I wanted to create clean, modular code and show good use of integration testing where unit tests become reserved for testing of precise behaviour or edge cases. Hopefully, the code is self-explanatory and does not require more comments or documentation. 

# Running

Please run it as usually from command line or Visual Studio.

Swagger is integrated.

Product endpoints are 
- /products - which returns list of all products
- /products/{colour} - which returns list of products by specified colours. The "colour" parameter only accepts letters a to z, must have length between 3 and 15 characters, and is case insensitive.

Authentication scheme is custom and require an Api key passed in Authorization header. For header values use any of the keys from Products.Module/UserRetrieval/UsersRepository.cs, for example "d57ffca5-26cf-4a51-b165-fdbaad3a296d".

# Next improvements? 

I hope this demo application shows a good balance between of functionality and required effort. Aspects such as sufficient logging, configuration sources,  advanced health checks or API versioning have been left aside, as well as docker configuration. Without having detailed requirements, working on those aspects would add little value. 

Also, everything that looks like a boiler plate code, such as Integration testing configuration, should be modularized and extracted as a package.