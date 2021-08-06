using AutoMapper;
using Microsoft.AspNetCore.Components;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using Movies.Infrastructure.Models.Review;
using Movies.Infrastructure.Models.User;
using Movies.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Pages.Reviews
{    
    public partial class ShowReviews
    {
        [Inject]
        private IReviewService reviewService { get; set; }

        [Inject]
        private ICustomAuthentication customAuthentication { get; set; }        

        [Inject]
        private IMapper mapper { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Parameter]
        public int? MovieId { get; set; }

        private Result<IEnumerable<ReviewResponse>> reviews { get; set; }
        private Result<GetUserResponse> currentUser { get; set; }
        private bool showOnlyMyReviews { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            await LoadReviewsAsync(false);

            currentUser = await customAuthentication.GetCurrentUserDataAsync();

            await base.OnParametersSetAsync();
        }

       
        private async Task LoadReviewsAsync(bool showOnlyMyReviews)
        {
            var getReviews = new Result<IEnumerable<Review>>();

            if (MovieId.HasValue)
            {
                getReviews = await reviewService.GetMovieReviewsAsync(MovieId.Value);
                if (showOnlyMyReviews && getReviews.ResultType == ResultType.Ok)
                {
                    getReviews.Value = getReviews.Value.Where(x => x.ReviewerId == currentUser.Value.UserId);
                }
            }
            else
            {
                if (showOnlyMyReviews)
                {
                    getReviews = await reviewService.GetReviewerReviewsAsync(currentUser.Value.UserId);
                }
                else
                {
                    getReviews = await reviewService.GetAllReviewsAsync();
                }
            }
            reviews = mapper.Map<Result<IEnumerable<Review>>, Result<IEnumerable<ReviewResponse>>>(getReviews);
        }

        private async Task OnShowOnlyMyReviewsAsync(ChangeEventArgs e)
        {
            showOnlyMyReviews = (bool)e.Value;
            await LoadReviewsAsync(showOnlyMyReviews);
        }

        private async Task OnReviewDeletedAsync(ReviewResponse review)
        {
            await LoadReviewsAsync(showOnlyMyReviews);
        }
    }
}
