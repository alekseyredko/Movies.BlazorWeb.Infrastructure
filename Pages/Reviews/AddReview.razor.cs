using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class AddReview
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
        public int MovieId { get; set; }      

        private ReviewRequest reviewRequest { get; set; }        
        private Result<ReviewResponse> result { get; set; }
        private Result<GetUserResponse> currentUser { get; set; }
             
        protected override async Task OnParametersSetAsync()
        {            
            await base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {            
            reviewRequest = new ReviewRequest();
            currentUser = await authentication.GetCurrentUserDataAsync();

            await base.OnInitializedAsync();
        }               

        private async Task AddReviewAsync()
        {
            var review = mapper.Map<Review>(reviewRequest);
            var getResponse  = await reviewService.AddReviewAsync(MovieId, currentUser.Value.UserId, review);
            result = mapper.Map<Result<ReviewResponse>>(getResponse);

            if (result.ResultType == ResultType.Ok)
            {
                navigationManager.NavigateTo($"/movies/{MovieId}");
            }
        }
    }
}
