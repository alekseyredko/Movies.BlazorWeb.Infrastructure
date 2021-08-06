using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using Movies.Infrastructure.Models.Movie;
using Movies.Infrastructure.Models.User;
using Movies.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Pages.MoviesPages
{
    [Authorize(Roles = "Producer")]
    public partial class EditMovie
    {
        [Inject]
        private IMovieService movieService { get; set; }

        [Inject]
        private ICustomAuthentication customAuthentication { get; set; }

        [Inject]
        private IMapper mapper { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Parameter]
        public int Id { get; set; }

        private string movieName;

        private string duration;

        private Result<GetUserResponse> currentUser;

        private Result<MovieResponse> result;

        private Result<MovieResponse> updateResult;

        protected override async Task OnParametersSetAsync()
        {
            var movie = await movieService.GetMovieAsync(Id);

            result = mapper.Map<Result<Movie>, Result<MovieResponse>>(movie);
            currentUser = await customAuthentication.GetCurrentUserDataAsync();

            if (result.ResultType == ResultType.Ok)
            {
                movieName = result.Value.MovieName;
                duration = result.Value.Duration.ToString();
            }

            await base.OnParametersSetAsync();
        }

        private async Task UpdateMovieAsync()
        {
            if (TimeSpan.TryParse(duration, out TimeSpan timeSpan))
            {
                var request = new Movie
                {
                    MovieName = movieName,
                    Duration = timeSpan,
                    ProducerId = currentUser.Value.UserId
                };

                var response = await movieService.UpdateMovieAsync(currentUser.Value.UserId, Id, request);

                updateResult = mapper.Map<Result<Movie>, Result<MovieResponse>>(response);

                if (updateResult.ResultType == ResultType.Ok)
                {
                    navigationManager.NavigateTo($"movies/{updateResult.Value.MovieId}");
                }
            }

        }
    }
}
