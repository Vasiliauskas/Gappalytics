using Gappalytics.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gappalytics.Sample
{
    public class AnalyticsHelper
    {
        private IAnalyticsSession _analyticsSession;

        private static AnalyticsHelper _Current;
        public static AnalyticsHelper Current
        {
            get
            {
                if (_Current == null)
                    Initialize();
                return _Current;
            }
        }

        public bool AnalyticsEnabled { get; set; }
        public bool IsTrial { get; set; }

        /// <summary>
        /// This method displays how to use non persistent mode of Gappalytics, each time you run will make a new unique user on GA dashboard
        /// For persistent mode, store first visit date, visit count (increment each time), and random number somewhere like app settings
        /// Use Constructor overload for Analytics sessions 
        /// </summary>
        private static void Initialize()
        {
            _Current = new AnalyticsHelper();

            // Replace UA code with your Google Analytics Tracking code
            _Current._analyticsSession = new AnalyticsSession("", "UA-XXXXXXX-X");
        }


        public void LogEvent(string eventName, string url)
        {
            if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(url))
                throw new InvalidOperationException("Parameters cannot contain empty values");
            try
            {
                var request = _analyticsSession.CreatePageViewRequest(url, eventName);
                request.Send();
            }
            catch (Exception ex)
            {
                ex.ToString();
                // Keep silent exceptions for robust scenarios
            }
        }
    }
}
