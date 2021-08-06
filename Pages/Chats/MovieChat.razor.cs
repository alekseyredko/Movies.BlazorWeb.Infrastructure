using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using Movies.Infrastructure.Models.Messages;
using Movies.Infrastructure.Models.Reviewer;
using Movies.Infrastructure.Models.User;
using Movies.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Movies.BlazorWeb.Infrastructure.Pages.Chats
{
    [Authorize(Roles = "Reviewer")]
    public partial class MovieChat
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        private ChatMessageService messageService { get; set; }

        [Inject]
        private IReviewService reviewService{ get; set; }

        [Inject]
        private ICustomAuthentication customAuthentication { get; set; }

        [Inject]
        private IMapper mapper { get; set;  }

        private HubConnection connection;
        private List<ChatMessageResponse> messages;
        private ChatMessageResponse parentMessage;
        private ChatMessageRequest comment;
        private Result<GetUserResponse> currentUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            messages = new List<ChatMessageResponse>();
            comment = new ChatMessageRequest();
            currentUser = await customAuthentication.GetCurrentUserDataAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            connection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri($"/chat"))
                .WithAutomaticReconnect()
                .Build();

            connection.On<ChatMessageRequest>("ReceiveMessageAsync", request =>
            {
                messageService.AddMessage(messages, request);
                StateHasChanged();

                comment = new ChatMessageRequest();
                parentMessage = null;
            });

            await connection.StartAsync();

            await connection.SendAsync("AddUserToGroup", $"{Id}");
        }

        private async Task PostComment()
        {
            if (parentMessage != null)
            {
                comment.ParentMessageId = parentMessage.ChatMessageId;
            }

            var getReviewer = await reviewService.GetReviewerAsync(currentUser.Value.UserId);

            var mapped = mapper.Map<Result<ReviewerResponse>>(getReviewer);

            if (mapped.ResultType == ResultType.Ok)
            {
                comment.Reviewer = mapped.Value;
            }

            await connection.SendAsync("SendMessageAsync", comment, $"{Id}");
        }

        private void GetMessageToReply(ChatMessageResponse response)
        {
            parentMessage = response;
        }
    }
}
