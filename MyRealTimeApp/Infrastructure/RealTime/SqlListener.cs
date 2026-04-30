using MyRealTimeApp.Core.Domain.Entities;
using HotChocolate.Subscriptions;
using Microsoft.Extensions.Hosting;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

//The background service that continuously listens directly
//to SSMS for row changes and pushes them to GraphQL.
//uses SqlTableDependency to attach directly to the SQL Server Service Broker


namespace MyRealTimeApp.Infrastructure.RealTime
{
    public class SqlListener(string connectionString, ITopicEventSender eventSender) : IHostedService
    {
        private SqlTableDependency<Product>? _dependency;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Connects to the "Products" table in SSMS
            _dependency = new SqlTableDependency<Product>(connectionString, "Products");
            _dependency.OnChanged += Changed;
            _dependency.Start();

            return Task.CompletedTask;
        }

        private async void Changed(object sender, RecordChangedEventArgs<Product> e)
        {
            // If a row is inserted, updated, or deleted, send it to the WebSocket topic
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await eventSender.SendAsync("ProductUpdated", e.Entity);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _dependency?.Stop();
            return Task.CompletedTask;
        }
    }
}
