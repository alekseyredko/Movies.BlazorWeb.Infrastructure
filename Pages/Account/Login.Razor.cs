using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Movies.Data.Results;
using Movies.Infrastructure.Models;
using Movies.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Pages.Account
{
    public partial class Login
    {
        [Inject]
        ICustomAuthentication customAuthentication { get; set; }
        
        [Inject]
        NavigationManager NavigationManager { get; set; }

       
        private LoginUserRequest loginUserRequest { get; set; }
        private Result<LoginUserResponse> response;

        [CascadingParameter] 
        Task<AuthenticationState> authenticationStateTask { get; set; }


        protected override Task OnInitializedAsync()
        {
            loginUserRequest = new LoginUserRequest();
            return base.OnInitializedAsync();
        }

        private async Task LogUsername()
        {                   
            response = await customAuthentication.TryLoginAsync(loginUserRequest);
            if (response.ResultType == ResultType.Ok)
            {
                NavigationManager.NavigateTo("/", true);
            }
        }
    }
}
