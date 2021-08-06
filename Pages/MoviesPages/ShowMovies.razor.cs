using AutoMapper;
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
    public partial class ShowMovies
    {
        private Result<IEnumerable<MovieResponse>> movies;        

        private Result<GetUserResponse> currentUser;

        [Inject]
        private IMapper mapper { get; set; }

        [Inject]
        private ICustomAuthentication customAuthentication { get; set; }

        [Inject]
        private IMovieService movieService { get; set; }

        private bool showOnlyMyMovies { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadMoviesAsync(false);

            currentUser = await customAuthentication.GetCurrentUserDataAsync();                      
        }

        private async Task OnMovieDeletedAsync(MovieResponse movie)
        {           
            await LoadMoviesAsync(showOnlyMyMovies);
        }
      
        private async Task LoadMoviesAsync(bool showOnlyMyMovies)
        {
            var getMovies = new Result<IEnumerable<Movie>>();
            if (showOnlyMyMovies)
            {
                getMovies = await movieService.GetMoviesByProducerIdAsync(currentUser.Value.UserId);
            }
            else
            {
                getMovies = await movieService.GetAllMoviesAsync();
                movies = mapper.Map<Result<IEnumerable<MovieResponse>>>(getMovies);
            }
            movies = mapper.Map<Result<IEnumerable<MovieResponse>>>(getMovies);
        }

        private async Task OnShowOnlyMyMoviesAsync(ChangeEventArgs e)
        {
            showOnlyMyMovies = (bool)e.Value;
            await LoadMoviesAsync(showOnlyMyMovies);
        }        
    }
}
