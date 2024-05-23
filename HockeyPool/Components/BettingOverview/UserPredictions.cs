namespace HockeyPool.Components.BettingOverview
{
    public class UserPredictions
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<Prediction> Predictions { get; set; }
    }
}
