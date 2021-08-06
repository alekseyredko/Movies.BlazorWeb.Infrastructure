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
    public partial class ShowMovie
    {
        [Inject]
        private IMovieService movieService { get; set; }       
        
        [Inject]
        private ICustomAuthentication customAuthentication { get; set; }

        [Inject]
        private IMapper mapper { get; set; }

        [Parameter]
        public int Id { get; set; }


        private Result<GetUserResponse> currentUser;
        private Result<MovieResponse> movie;      
        private string addReviewLink;

        protected override async Task OnParametersSetAsync()
        {                        
            addReviewLink = $"/movies/{Id}/add-review";
            await base.OnInitializedAsync();            
        }

        protected override async Task OnInitializedAsync()
        {
            var getMovie = await movieService.GetMovieAsync(Id);

            movie = mapper.Map<Result<MovieResponse>>(getMovie);

            currentUser = await customAuthentication.GetCurrentUserDataAsync();
            await base.OnInitializedAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }
    }
}
