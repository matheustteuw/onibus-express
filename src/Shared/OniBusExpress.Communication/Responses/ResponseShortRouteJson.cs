namespace OniBusExpress.Communication.Responses
{
    public class ResponseShortRouteJson
    {
        public Guid Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string EstimatedDuration { get; set; } = string.Empty;
    }
}
