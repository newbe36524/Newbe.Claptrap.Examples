namespace Newbe.Claptrap.Ticketing.Web.Models
{
    public class BlazorJsonResponse
    {
        /// <summary>
        /// Status of response, 0 for success, others for failed
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// error message, only if status is not '0'
        /// </summary>
        public string Message { get; set; }
    }
}