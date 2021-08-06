using Microsoft.AspNetCore.Components;
using Movies.Infrastructure.Models.Messages;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Pages.Chats
{
    public partial class Message
    {
        [Parameter]
        public ChatMessageResponse ChatMessage { get; set; }

        [Parameter]
        public EventCallback<ChatMessageResponse> OnReplyCallback { get; set; }

        [Parameter]
        public int Level { get; set; }

        private async Task OnReply()
        {
            await OnReplyCallback.InvokeAsync(ChatMessage);
        }

        private async Task OnChildReply(ChatMessageResponse response)
        {
            await OnReplyCallback.InvokeAsync(response);
        }
    }
}
