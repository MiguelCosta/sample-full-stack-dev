using System.Collections.Generic;

namespace GigHub.Core.ViewModels
{
    public class SendCodeViewModel
    {
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public string SelectedProvider { get; set; }
    }
}
