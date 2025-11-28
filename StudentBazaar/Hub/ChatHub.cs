using Microsoft.AspNetCore.SignalR;

namespace StudentBazaar.Web.Hubs
{
    public class ChatHub : Hub
    {
        // اليوزر يدخل جروب المحادثة
        public async Task JoinConversation(string conversationKey)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationKey);
        }
    }
}
