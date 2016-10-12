using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary {
    public class DebugPanel : MonoBehaviour {
        private Dictionary<string, DebugCommand> mCommands = new Dictionary<string, DebugCommand>();
        public Dictionary<string, DebugCommand> Commands { get { return mCommands; } }

        public ScrollRect ScrollArea;
        public Text TextArea;
        public InputField Input;

        void Start() {
            //DontDestroyOnLoad( gameObject );  // don't have to do this yet
            AddDefaultCommands();
        }

        protected virtual void AddDefaultCommands() {
            AddCommand( "help", "help", "Shows all commands", OnHelpCommand );
        }

        public void AddCommand( string i_command, string i_usage, string i_desc, Callback<string[]> i_callback ) {
            DebugCommand command = new DebugCommand( i_command, i_usage, i_desc, i_callback );
            Commands.Add( i_command, command );
        }

        private void OnHelpCommand( string[] i_args ) {
            EchoText( "The following commands exist: " );
            foreach ( KeyValuePair<string, DebugCommand> command in Commands ) {
                EchoText( command.Key + " -- " + command.Value.Usage + ": " + command.Value.Description );
            }
        }

        protected void EchoText( string i_text ) {
            TextArea.text += i_text + "\n";
            StartCoroutine( ClampTextToBottom() );
        }

        private IEnumerator ClampTextToBottom() {
            // this is a coroutine because apparently adding to a scroll area and moving the normalized position in the 
            // same frame doesn't work
            yield return null;
            ScrollArea.verticalNormalizedPosition = 0;
        }

        public void EnterCommand() {      
            string input = Input.text;
            if ( !string.IsNullOrEmpty( input ) ) {
                EchoText( "Executing command: " + input );

                TryCommand( input );
                ClearInput();
            }
        }

        private void TryCommand( string i_input ) {
            string[] args = i_input.Split( ' ' );
            if ( Commands.ContainsKey( args[0] ) ) {
                ExecuteCommand( args[0], args );                
            } else {
                CommandNotFound( args[0] );
            }
        }

        private void ExecuteCommand( string i_command, string[] i_args ) {
            Commands[i_command].OnCommand( i_args );
        }

        private void CommandNotFound( string i_command ) {
            EchoText( "No such command: " + i_command );
        }

        private void ClearInput() {
            Input.text = "";
        }

        protected void EchoImproperArgs() {
            EchoText( "Wrong usage of arguments! Type help to see usage of all commands." );
        }
    }
}