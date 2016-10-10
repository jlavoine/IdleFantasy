
namespace MyLibrary {
    public class DebugCommand {
        public string Command;
        public string Description;
        public Callback<string[]> OnCommand;

        public DebugCommand( string i_commandName, string i_description, Callback<string[]> i_commandCallback ) {
            Command = i_commandName;
            Description = i_description;
            OnCommand = i_commandCallback;
        }
    }
}
