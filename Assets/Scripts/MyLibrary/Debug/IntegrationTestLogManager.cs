using UnityEngine;

#pragma warning disable 0219

namespace MyLibrary {
    public class IntegrationTestLogManager : MonoBehaviour {

        void Awake() {
            DontDestroyOnLoad( this );

            IntegrationTestLogger logger = new IntegrationTestLogger();            
        }
    }
}