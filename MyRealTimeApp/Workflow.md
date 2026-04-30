This project consists of  workflows:

-----Workflow 1 (user updates data in UI -> it gets updated in DB automatically)-----

UI sends GraphQL mutation to Banana Cake API
The Mutation.cs class receives payload and passes it to IProductService
ProductService receives request, validates checks and passes product entity to IProductRepository
ProductRepository attaches entity to AppDbContext and calls SaveChangesAsync
EF Core generates an INSERT/UPDATE SQL command & executes it in SSMS


-----Workflow 2 (admin updates in SSMS -> it gets reflected in UI)-----

Admin executes an UPDATE directly in database (SSMS)
The SqlListener intercepts the SQL Service Broker event
The listener converts raw SQL row data back into C# product object
The listener pushes Product to HotChocolate ITopicEventSender under topic "ProductUpdated"
The GraphQL Subscription.cs catches event and pushes it down the open WebSocket connection.
The website receives the JSON and updates the screen