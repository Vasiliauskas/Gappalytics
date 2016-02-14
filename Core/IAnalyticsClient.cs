namespace Appalytics.Core
{
    internal interface IAnalyticsClient
    {
        string Domain { get; set; }

        string TrackingCode { get; set; }

        /// <summary>
        /// Works best if URL based page requests are sent, then analytics can provide content drilldown
        /// </summary>
        /// <param name="page">ROOT/MyPage/MySubpage</param>
        /// <param name="title"></param>
        /// <param name="pageVariables"></param>
        void SubmitPageView(string page, string title, CustomVariableBag pageVariables);

        void SubmitEvent(string page, string title, string category, string action, string label, string value, CustomVariableBag pageVariables);

        void SetCustomVariable(int position, string key, string value);

        void ClearCustomVariable(int position);
    }
}
