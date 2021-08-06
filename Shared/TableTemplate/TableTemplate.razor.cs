using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Movies.Data.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Shared.TableTemplate
{
    public partial class TableTemplate<TItem>
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Parameter]
        public RenderFragment TableHeader { get; set; }

        [Parameter]
        public RenderFragment<TItem> RowTemplate { get; set; }
           
        [Parameter]
        public Result<IEnumerable<TItem>> Items { get; set; }

        private bool shouldRender = true;

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var parameter = parameters.GetValueOrDefault<Result<IEnumerable<TItem>>>("Items");

            if (parameter != null && parameter.ResultType == ResultType.Ok)
            {
                if (Items != null && Items.ResultType == ResultType.Ok)
                {
                    if (parameter.Value.Count() == Items.Value.Count())
                    {
                        shouldRender = false;
                        await base.SetParametersAsync(parameters);
                        return;
                    }
                }
            }
            shouldRender = true;
            await base.SetParametersAsync(parameters);
        }

        protected override bool ShouldRender() => shouldRender;
    }
}
