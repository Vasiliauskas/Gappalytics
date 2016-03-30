namespace Gappalytics.Core
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    internal class AnalyticsClient : IAnalyticsClient
    {
        private readonly VariableBucket _sessionVariables;

        private string _referralSource = "(direct)";
        private string _medium = "(none)";
        private string _campaign = "(direct)";
        private string _domain;

        /// <summary>
        /// Use for non persisted user session mode (Does not guarantee uniqueness of users)
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="trackingCode"></param>
        public AnalyticsClient(string domain, string trackingCode)
            : this(domain, trackingCode, new Random(DateTime.Now.Millisecond).Next(1000000000), 1, null)
        {
        }

        /// <summary>
        /// Use for persisted user session mode can guarantee uniqueness by machine+user
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="trackingCode"></param>
        /// <param name="randomNumber">Random number that was generated on first launch</param>
        /// <param name="recentVisitCount">Recent visit count, must be incremented before passing in</param>
        /// <param name="firstVisitTimeStamp">Timestamp for first session</param>
        public AnalyticsClient(string domain, string trackingCode, int randomNumber, int recentVisitCount, DateTime? firstVisitTimestamp)
        {
            _sessionVariables = new VariableBucket();
            Timestamp = ConvertToUnixTimestamp(DateTime.Now).ToString();
            Domain = domain;
            RandomNumber = randomNumber.ToString();
            TrackingCode = trackingCode;
            FirstSessionTimestamp = firstVisitTimestamp.HasValue ?
                ConvertToUnixTimestamp(firstVisitTimestamp.Value).ToString() : Timestamp;
            VisitCount = recentVisitCount;
        }

        public string Domain
        {
            get { return _domain; }
            set
            {
                _domain = value;
                DomainHash = CalculateDomainHash(value);
            }
        }
        public string TrackingCode { get; set; }
        public string Timestamp { get; set; }
        public string FirstSessionTimestamp { get; set; }
        public string RandomNumber { get; set; }
        public int VisitCount { get; set; }

        public int DomainHash { get; private set; }

        public string CookieString { get { return GetCookieString(); } }

        public void SubmitPageView(string page, string title, VariableBucket pageVariables)
        {
            var client = CreateBrowser(page, title);

            var variables = _sessionVariables.MergeWith(pageVariables);

            if (variables.Any())
                client.QueryString["utme"] = variables.ToUtme();

            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    client.DownloadData(new Uri("__utm.gif", UriKind.Relative));
                }
                catch(Exception ex)
                {

                }
            });
        }

        public void SubmitEvent(string page, string title, string category, string action, string label, string value, VariableBucket pageVariables)
        {
            var client = CreateBrowser(page, title);

            client.QueryString["utmt"] = "event";
            client.QueryString["utme"] = FormatUtme(category, action, label, value);

            var variables = _sessionVariables.MergeWith(pageVariables);

            if (variables.Any())
                client.QueryString["utme"] += variables.ToUtme();

            ThreadPool.QueueUserWorkItem(state =>
            {
                client.DownloadDataAsync(new Uri("__utm.gif", UriKind.Relative));
            });
        }


        public void SetCustomVariable(int position, string key, string value)
        {
            _sessionVariables.Set(position, key, value);
        }

        public void ClearCustomVariable(int position)
        {
            _sessionVariables.Clear(position);
        }

        #region Private members

        private static string GetDefaultUserAgent()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return string.Format("Appalytics v{0}.{1}", version.Major, version.Minor);
        }

        private WebClient CreateBrowser(string page, string title)
        {
            Random randomNumber = new Random();
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.UserAgent, GetDefaultUserAgent());
            client.BaseAddress = "http://www.google-analytics.com/";

            client.QueryString["utmhn"] = Domain;
            client.QueryString["utmcs"] = "UTF-8";
            client.QueryString["utmsr"] = "1280x800";
            client.QueryString["utmvp"] = "1280x800";
            client.QueryString["utmsc"] = "24-bit";
            client.QueryString["utmul"] = "en-us";
            client.QueryString["utmdt"] = title;
            client.QueryString["utmhid"] = randomNumber.Next(1000000000).ToString();
            client.QueryString["utmac"] = TrackingCode;
            client.QueryString["utmn"] = randomNumber.Next(1000000000).ToString();
            client.QueryString["utmr"] = "-";
            client.QueryString["utmp"] = page;
            client.QueryString["utmwv"] = "5.3.5";
            client.QueryString["utmcc"] = CookieString;

            return client;
        }

        private static string FormatUtme(string category, string action, string label, string value)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("5({0}*{1}", EncodeUtmePart(category), EncodeUtmePart(action));

            if (!string.IsNullOrEmpty(label))
                builder.AppendFormat("*{0}", EncodeUtmePart(label));

            builder.Append(")");

            if (!string.IsNullOrEmpty(value))
                builder.AppendFormat("({0})", EncodeUtmePart(value));

            return builder.ToString();
        }

        internal static string EncodeUtmePart(string part)
        {
            return part.Replace("'", "'0").Replace(")", "'1").Replace("*", "'2");
        }

        private static int ConvertToUnixTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (int)span.TotalSeconds;
        }

        private int CalculateDomainHash(string domain)
        {
            int a = 1;
            int c = 0;
            int h;
            char chrCharacter;
            int intCharacter;

            a = 0;
            for (h = Domain.Length - 1; h >= 0; h--)
            {
                chrCharacter = char.Parse(domain.Substring(h, 1));
                intCharacter = (int)chrCharacter;
                a = (a << 6 & 268435455) + intCharacter + (intCharacter << 14);
                c = a & 266338304;
                a = c != 0 ? a ^ c >> 21 : a;
            }

            return a;
        }

        private string GetCookieString()
        {
            string utma = String.Format("{0}.{1}.{2}.{3}.{4}.{5}",
                                            DomainHash,
                                            RandomNumber,
                                            FirstSessionTimestamp, // timestamp of first visit
                                            Timestamp, // timestamp of previous (most recent visit)
                                            Timestamp, // timestamp of current visit
                                            VisitCount); // total visit count

            //referral informaiton
            string utmz = String.Format("{0}.{1}.{2}.{3}.utmcsr={4}|utmccn={5}|utmcmd={6}",
                                        DomainHash,
                                        Timestamp,
                                        "1",
                                        "1",
                                        _referralSource,
                                        _campaign,
                                        _medium);

            string utmcc = Uri.EscapeDataString(String.Format("__utma={0};+__utmz={1};",
                                                              utma,
                                                              utmz
                                                   ));

            return (utmcc);
        }

        #endregion
    }
}
