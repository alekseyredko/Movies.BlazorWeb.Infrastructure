using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Shared.InputTemplate
{
    public partial class FormTemplate<TItem, TResult>
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public TResult Result { get; set; }

        [Parameter]
        public IEnumerable<RenderFragment> RenderFragments { get; set; }
    }
}
