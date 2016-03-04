Gappalytics [![Build status](https://ci.appveyor.com/api/projects/status/06nsa81vvf7c9ymh?svg=true)](https://ci.appveyor.com/project/Vasiliauskas/gappalytics) [![Join the chat at https://gitter.im/Vasiliauskas/Gappalytics](https://badges.gitter.im/Vasiliauskas/Gappalytics.svg)](https://gitter.im/Vasiliauskas/Gappalytics?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
--------------


_This is a library for google analytics based page tracking for any desktop or mobile application_


To use Gappalytics library, create an instance of AnalyticsSession every time you run your application.

There are two modes of tracking
- Non persistent - meaning that each time you create a new session it will be treated as a new user as well
- Persistent - if your application can persist three specific tracking values then you can ensure user uniqueness
	- "First session timestamp"
	- "Current visit count (previous visit count + 1)"
	- "Random number created on first visit"

Usage:
```c#
var session = new AnalyticsSession("DOMAIN.COM", "UA-XXXXXXXX-X");* // non persistent
var session = new AnalyticsSession("DOMAIN.COM", "UA-XXXXXXXX-X", rndNumber, visitCount, firstVisitTimestamp);* // persistent

var page = _session.CreatePageViewRequest("/Root/MyPage", "MyPage");
page.Send();
```
