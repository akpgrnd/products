# Products.Api
This is a dotnet 8 application. Please run it as usual. In the given time I didn't manage to do everything that a production ready project needs. It didn't need any configuration, or containerization. All data is hard coded mock. I could have used an SQL engine but didn't see a point. I hope it would be fine given the 2-3 hour completion estimate.

I wanted to show good use of Integration testing and code architecture when unit testing becomes reserved for testing ofprecise behaviour or edge cases. Integration testing setup, however, would also be slightly different in production. Similar to what I started but I would make it more flexible/configurable, with injection of fake services into dotnet pipeline (such as config sources, authentication/authorzation dependencies, etc.) 

# Testing
There is a swagger endpoint. To authorize a request use any of the keys from Products.Module/UserRetrieval/UsersRepository.cs, for example "d57ffca5-26cf-4a51-b165-fdbaad3a296d".
