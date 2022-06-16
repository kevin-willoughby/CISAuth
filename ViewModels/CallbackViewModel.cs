using CIS2Auth.Dto;

namespace CIS2Auth.ViewModels
{
    public class CallbackViewModel
    {
        public string Code { get; set; }
        public string State { get; set; }
    }
}
