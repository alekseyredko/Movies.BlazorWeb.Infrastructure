using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using Movies.Infrastructure.Models.Producer;
using Movies.Infrastructure.Models.Reviewer;
using Movies.Infrastructure.Models.User;
using Movies.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Pages.Account
{
    public partial class AccountDetails
    {
        [Inject]
        private IProducerService producerService { get; set; }

        [Inject]
        private IReviewService reviewService { get; set; }

        [Inject]
        private IUserService userService { get; set; }

        [Inject]
        private ICustomAuthentication authentication { get; set; }
        
        [Inject]
        private IMapper mapper { get; set; }

        [Inject]
        private AuthenticationStateProvider authenticationProvider { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private ProducerRequest registerProducerRequest { get; set; }
        private RegisterReviewerRequest registerReviewerRequest { get; set; }

        private Result<GetUserResponse> user { get; set; }
        private Result<ProducerResponse> registerProducerResult { get; set; }
        private Result<RegisterReviewerResponse> registerReviewerResult { get; set; }

        private Result<ProducerResponse> producer { get; set; }
        private Result<ReviewerResponse> reviewer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            user = await authentication.GetCurrentUserDataAsync();

            if (user.ResultType == ResultType.Ok)
            {
                registerProducerRequest = new ProducerRequest();
                registerReviewerRequest = new RegisterReviewerRequest();

                var getProducer = await producerService.GetProducerAsync(user.Value.UserId);
                producer = mapper.Map<Result<ProducerResponse>>(getProducer);

                var getReviewer = await reviewService.GetReviewerAsync(user.Value.UserId);
                reviewer = mapper.Map<Result<ReviewerResponse>>(getReviewer);
            }
        }

        private async Task RegisterAsProducer()
        {           
            registerProducerResult = await authentication.TryRegisterAsProducerAsync(registerProducerRequest);

            if (registerProducerResult.ResultType == ResultType.Ok)
            {
                NavigationManager.NavigateTo("/");
            }
        }

        private async Task RegisterAsReviewer()
        {
            registerReviewerResult = await authentication.TryRegisterAsReviewerAsync(registerReviewerRequest);

            if (registerReviewerResult.ResultType == ResultType.Ok)
            {
                NavigationManager.NavigateTo("/");
            }
        }

    }
}
