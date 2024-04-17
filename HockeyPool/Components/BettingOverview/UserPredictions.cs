namespace HockeyPool.Components.BettingOverview
{
    public class UserPredictions
    {
        public Guid userId { get; set; }
        public string userName { get; set; }
        public List<Prediction> predictions { get; set; }

    }
}
