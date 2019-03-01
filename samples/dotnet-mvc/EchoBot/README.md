# EchoBot README

This bot echoes back the text of messages sent to it.

It is set up as an ASP.NET Core MVC application.

- HTTP POST requests to `api/messages` are handled by `EchoBotController`.
- A bot adapter is created for each request. It authenticates and forwards incoming activities to the turn handler.