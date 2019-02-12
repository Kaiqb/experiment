# Bots list

1. EchoBot (without counter)
    - For message activities, echoes back the user input text.
    - For other activities, mentions the activity type received.
    - Implements a simple adapter OnError handler.
1. EchoBotWithCounter - extends EchoBot
    - Creates a conversation data class for use with the state property accessor.
    - Adds conversation state and property accessor objects in Startup.cs.
    - Adds DI constructor and instance fields to the controller.
    - Manages the counter in the controller's turn handler code.
1. RichMedia - extends EchoBotWithCounter
    - State is needed. Will use a state flag and an enumeration to track the current "task".
    - Include AdaptiveCards NuGet package.