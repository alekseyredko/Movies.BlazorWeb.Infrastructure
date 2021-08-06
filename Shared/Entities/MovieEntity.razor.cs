using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using Movies.Infrastructure.Models.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Shared.Entities
{
    public partial class MovieEntity: ComponentBase
    {        
        [Inject]
        private IMovieService movieService { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }

        [Parameter]
        public EventCallback<MovieResponse> OnActionDoneAsync { get; set; }

        [Parameter]
        public MovieResponse Movie { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        
        private bool canEditAndDelete { get; set; }       

        private bool confirmActionDialogOpen { get; set; }

        private int userId { get; set; }

        private string movieLink { get; set; }
                

        protected override async Task OnParametersSetAsync()
        {
            var state = await authenticationStateTask;

            if (state.User.Identity != null && state.User.Identity.IsAuthenticated)
            {
                var claim = state.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (claim != null)
                {
                    userId = int.Parse(claim.Value);

                    if (state.User.IsInRole(Enum.GetName(UserRoles.Producer)) && Movie.ProducerId == userId)
                    {
                        canEditAndDelete = true;
                    }
                    else
                    {
                        canEditAndDelete = false;
                    }
                }   
            }

            movieLink = $"/movies/{Movie.MovieId}";

            await base.OnParametersSetAsync();
        }

        private void ShowDeleteDialog()
        {            
            confirmActionDialogOpen = true;
            DynamicRender = CreateComponent("Delete",
                "Are you sure?",
                ConfirmDialog.ModalDialogType.DeleteCancel,
                null,
                OnDeletedAsync);
        }       

        private void GoToEditMovie()
        {
            navigationManager.NavigateTo($"/movies/{Movie.MovieId}/edit");
        }

        private async Task OnDeletedAsync(bool confirm)
        {
            if (confirm)
            {
                var result = await movieService.DeleteMovieAsync(userId, Movie.MovieId);

                if (result.ResultType == ResultType.Ok)
                {                    
                    await OnActionDoneAsync.InvokeAsync(Movie);
                    confirmActionDialogOpen = false;
                }
                else
                {
                    DynamicRender = CreateComponent("Error", null, ConfirmDialog.ModalDialogType.Ok, result, OnConfirmAsync);
                }                                
            }
               
        }

        private async Task OnConfirmAsync(bool confirm)
        {
            confirmActionDialogOpen = false;
        }

        private RenderFragment DynamicRender { get; set; }

        private RenderFragment CreateComponent(string title, string text, ConfirmDialog.ModalDialogType dialogType, Result result, Func<bool, Task> task)
        {
            EventCallback<bool> callback = new EventCallbackFactory().Create<bool>(this, task);
            return new RenderFragment((builder) =>
            {
                builder.OpenComponent(0, typeof(ConfirmDialog));
                builder.AddAttribute(1, "Title", title);
                builder.AddAttribute(2, "Text", text);
                builder.AddAttribute(3, "DialogType", dialogType);
                builder.AddAttribute(4, "Result", result);
                builder.AddAttribute(4, "OnClose", callback);
                builder.CloseComponent();
            });
           
        }
    }
}
