using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Shared.Entities
{
    public partial class Entity<T> 
    {
        [Inject]
        NavigationManager navigationManager { get; set; }

        [Parameter]
        public EventCallback<T> OnDelete { get; set; }

        [Parameter]
        public EventCallback<T> OnEdit { get; set; }

        [Parameter]
        public T Value { get; set; }

        [Parameter]
        public RenderFragment<T> Template { get; set; }
        
        [Parameter]
        public bool CanEditAndDelete { get; set; }

        private bool deleteDialogOpen { get; set; }


        protected override async Task OnParametersSetAsync()
        {            
            await base.OnParametersSetAsync();
        }

        private void ShowDeleteDialog()
        {
            deleteDialogOpen = true;
            this.StateHasChanged();
        }

        private async Task OnEditAsync()
        {
            await OnEdit.InvokeAsync(Value);
        }

        private async Task OnDeleteAsync(bool confirm)
        {
            if (confirm)
            {
                await OnDelete.InvokeAsync(Value);

                this.StateHasChanged();
            }
            deleteDialogOpen = false;
        }
    }
}
