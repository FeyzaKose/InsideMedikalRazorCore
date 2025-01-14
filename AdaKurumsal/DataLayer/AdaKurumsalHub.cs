using Microsoft.AspNetCore.SignalR;

namespace AdaKurumsal.DataLayer
{
    public class AdaKurumsalHub : Hub
    {
        public async Task JoinLanguageGroup(string language)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, language);
        }

        public async Task LeaveLanguageGroup(string language)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, language);
        }
    }
}
