Appalytics README
==============

Appalytics is a library for google analytics based page tracking for any desktop or mobile application
--------------

To simply use the library, create an instance of AnalyticsSessions any time you run your application.
There are two modes of tracking
- Non persistent - meaning that each time you create a new session it will be treated as a new user as well
- Persistent - if your application can persist three specific tracking values then you can ensure user uniqueness
-- "First session timestamp"
-- "Current visit count (previous visit count + 1)"
-- "Random number created on first visit"

Usage:

*var session = new AnalyticsSession("MYDOMAIN.COM", "UA-XXXXXXXX-X");*

*var page = _Session.CreatePageViewRequest("/Root/MyPage", "MyPage");*
*page.Send();*