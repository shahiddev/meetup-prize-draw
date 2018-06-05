# meetup-prize-draw

A simple .NET Core sample application showing how to create an in-browser prize draw application for a [meetup](https://www.meetup.com) event, inspired by [dotnetoxford/prizedraw](https://github.com/dotnetoxford/PrizeDraw)

A demo version of the sample is running at [meetupprizedraw.azurewebsites.net/](http://meetupprizedraw.azurewebsites.net/).

The presenter screen can be shown by pressing `P`. This screen shows the list of winners, and also allows you to specify a [Slack webhook url](https://api.slack.com/incoming-webhooks) to have winners posted to Slack. For convenience, the webhook url is persisted in local storage :-)