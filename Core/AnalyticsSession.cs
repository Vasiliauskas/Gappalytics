namespace Gappalytics.Core
{
    using System;

    public class AnalyticsSession : IAnalyticsSession
    {
        private readonly AnalyticsClient _analyticsClient;

        public AnalyticsSession(string domain, string trackingCode)
        {
            _analyticsClient = new AnalyticsClient(domain, trackingCode);
        }

        public AnalyticsSession(string domain, string trackingCode, int userRandomNumber, int visitCount, DateTime? firstVisitTimeStamp)
        {
            _analyticsClient = new AnalyticsClient(domain, trackingCode, userRandomNumber, visitCount, firstVisitTimeStamp);
        }

        public IAnalyticsPageViewRequest CreatePageViewRequest(string page, string title)
        {
            return new AnalyticsPageViewRequest(_analyticsClient, page, title);
        }

        public void SetCustomVariable(int position, string key, string value)
        {
            _analyticsClient.SetCustomVariable(position, key, value);
        }
    }
}
