using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using Movies.Infrastructure.Models.Review;
using Movies.Infrastructure.Models.User;
using Movies.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Pages.Reviews
{
    [Authorize(Roles = "Reviewer")]
    public partial class EditReview
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        private IReviewService reviewService { get; set; }

        [Inject]
        private ICustomAuthentication authentication { get; set; }

        [Inject]
        private IMapper mapper { get; set; }

        [Parameter]
        public int Id { get; set; }

        //[CascadingParameter]
        //private Task<AuthenticationState> authenticationStateTask { get; set; }

        private UpdateReviewRequest reviewRequest { get; set; }
        private Result<ReviewResponse> result { get; set; }
        private Result<ReviewResponse> updateResult { get; set; }
        private Result<GetUserResponse> currentUser { get; set; }        

        protected override async Task OnParametersSetAsync()
        {            
            currentUser = await authentication.GetCurrentUserDataAsync();

            var toEdit = await reviewService.GetReviewAsync(Id);
            result = mapper.Map<Result<ReviewResponse>>(toEdit);

            reviewRequest = mapper.Map<Result<Review>, UpdateReviewRequest>(toEdit);

            await base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }

        private async Task EditReviewAsync()
        {
            var review = mapper.Map<Review>(reviewRequest);
            var getResponse = await reviewService.UpdateReviewAsync(Id, currentUser.Value.UserId, review);
            updateResult = mapper.Map<Result<ReviewResponse>>(getResponse);

            if (updateResult.ResultType == ResultType.Ok)
            {
                navigationManager.NavigateTo($"/movies/{getResponse.Value.MovieId}");
            }            
        }
    }
}
