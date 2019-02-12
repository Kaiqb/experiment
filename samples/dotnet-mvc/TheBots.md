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
1. RichMedia - extends EchoBot
    - Include AdaptiveCards NuGet package.
    - State is not needed:
        - Switch on user input text:
        - If it matches a "task", display the rich media attachment.
        - Display the suggested actions every turn.
    - Check ConversationUpdate for new users
        - On a new user, display suggested actions.
    - All rich media attachments are defined in Resources\Attachments.cs.
    - The Adaptive card JSON is in Resources\FlightDetails.json.
