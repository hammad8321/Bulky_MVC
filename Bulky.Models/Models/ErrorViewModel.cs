using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Bulky.Models.Models

{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
