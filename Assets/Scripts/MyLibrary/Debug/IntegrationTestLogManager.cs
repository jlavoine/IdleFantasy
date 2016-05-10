using UnityEngine;

#pragma warning disable 0219

namespace MyLibrary {
    public class IntegrationTestLogManager : MonoBehaviour {

        void Awake() {
            DontDestroyOnLoad( this );

            IMessageService messenger = new MyMessenger();
            IntegrationTestLogger logger = new IntegrationTestLogger( messenger );
        }
    }
}