namespace Appalytics.Core
{
    public interface IAnalyticsPageViewRequest
    {
        void Send();

        void SendEvent(string category, string action, string label, string value);

        void SetCustomVariable(int position, string key, string value);

        void ClearCustomVariable(int position);
    }
}
