using MyRealTimeApp.Core.Domain.Entities;
using HotChocolate;
using HotChocolate.Types;

//The GraphQL endpoint that holds the WebSocket connection open to push data to the UI.

namespace MyRealTimeApp.API.GraphQL
{
    public class Subscription
    {
        // The UI listens to this "ProductUpdated" topic
        [Subscribe]
        [Topic("ProductUpdated")]
        public Product OnProductUpdated([EventMessage] Product product)
        {
            return product;
        }
    }
}
