using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace TheBookClub.Services.NotificationService
{
    public class NotificationHub : Hub
    {
        // A thread-safe dictionary to map user IDs to connection IDs
        private static readonly ConcurrentDictionary<string, string> UserConnections = new();

        public override Task OnConnectedAsync()
        {
            // Get the user ID from the connection context
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                UserConnections[userId] = Context.ConnectionId;
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // Remove the user from the dictionary when they disconnect
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                UserConnections.TryRemove(userId, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string userId, string message)
        {
            // Check if the user is connected
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }
    }
}