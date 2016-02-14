namespace Appalytics.Core
{
    public class AnalyticsPageViewRequest : IAnalyticsPageViewRequest
    {
        private readonly IAnalyticsClient _analyticsClient;
        private readonly string _page;
        private readonly string _title;
        private readonly CustomVariableBag _customVariables;

        internal AnalyticsPageViewRequest(IAnalyticsClient analyticsClient, string page, string title)
        {
            _analyticsClient = analyticsClient;
            _page = page;
            _title = title;
            _customVariables = new CustomVariableBag();
        }

        public void Send()
        {
            _analyticsClient.SubmitPageView(_page, _title, _customVariables);
        }

        public void SendEvent(string category, string action, string label, string value)
        {
            _analyticsClient.SubmitEvent(_page, _title, category, action, label, value, _customVariables);
        }

        public void SetCustomVariable(int position, string key, string value)
        {
            _customVariables.Set(position, key, value);
        }

        public void ClearCustomVariable(int position)
        {
            _customVariables.Clear(position);
        }
    }
}
