namespace HockeyPool.Components.BettingOverview
{
    public class UserPredictionsHistory : PredictionLog
    {
        public UserPredictionsHistory(PredictionLog predictionLog)
        {
            Id = predictionLog.Id;
            AspNetUserId = predictionLog.AspNetUserId;
            GuestTeamScore = predictionLog.GuestTeamScore;
            HomeTeamScore = predictionLog.HomeTeamScore;
            MatchupId = predictionLog.MatchupId;
            TimeStamp = predictionLog.TimeStamp;

        }

        public string HomeTeamCountryCode { get; set; }
        public string GuestTeamCountryCode { get; set; }
        public string UserName { get; set; }
        public bool CanShow { get; set; }
    }
}
