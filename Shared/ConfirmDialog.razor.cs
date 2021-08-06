using Microsoft.AspNetCore.Components;
using Movies.Data.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.BlazorWeb.Infrastructure.Shared
{
    public partial class ConfirmDialog
    {
        [Parameter]
        public Result Result { get; set; } 

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public EventCallback<bool> OnClose { get; set; }

        [Parameter]
        public ModalDialogType DialogType { get; set; }      

        private Task ModalCancel()
        {
            return OnClose.InvokeAsync(false);
        }

        private Task ModalOk()
        {
            return OnClose.InvokeAsync(true);
        }

        public enum ModalDialogType
        {
            Ok,
            OkCancel,
            DeleteCancel
        }
    }
}
