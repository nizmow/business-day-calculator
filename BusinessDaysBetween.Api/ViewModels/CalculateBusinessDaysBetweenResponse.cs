namespace BusinessDaysBetween.Api.ViewModels
{
    /// <summary>
    /// Response for the CalculateBusinessDaysBetween controller method
    /// </summary>
    /// <remarks>
    /// A single number is valid JSON, but adding a response object early allows us to add fields without
    /// breaking clients!
    /// </remarks>
    public class CalculateBusinessDaysBetweenResponse
    {
        public int Days { get; set; }
    }
}