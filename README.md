Gappalytics README
==============

Gappalytics is a library for google analytics based page tracking for any desktop or mobile application
--------------

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

var page = _Session.CreatePageViewRequest("/Root/MyPage", "MyPage");
page.Send();
```
Travis build
--------------
[![Build Status](https://travis-ci.org/Vasiliauskas/Gappalytics.svg?branch=master)](https://travis-ci.org/Vasiliauskas/Gappalytics)