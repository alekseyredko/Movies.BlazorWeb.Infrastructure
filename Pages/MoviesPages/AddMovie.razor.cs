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
    public partial class AddMovie
    {
        [Inject]
        private IMovieService movieService { get; set; }

        [Inject]
        private ICustomAuthentication customAuthentication { get; set; }

        [Inject]
        private IMapper mapper { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }
     
        private Result<GetUserResponse> currentUser;      

        private Result<MovieResponse> addResult;

        private MovieRequest movieRequest;

        protected override async Task OnParametersSetAsync()
        {
            movieRequest = new MovieRequest();
            currentUser = await customAuthentication.GetCurrentUserDataAsync();
              
            await base.OnParametersSetAsync();
        }       

        private async Task AddMovieAsync()
        {
            var request = mapper.Map<Movie>(movieRequest);

            var response = await movieService.AddMovieAsync(currentUser.Value.UserId, request);

            addResult = mapper.Map<Result<Movie>, Result<MovieResponse>>(response);

            if (addResult.ResultType == ResultType.Ok)
            {
                navigationManager.NavigateTo($"movies/{addResult.Value.MovieId}");
            }

        }
    }
}
