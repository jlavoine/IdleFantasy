using UnityEngine;
using UnityEngine.SceneManagement;

namespace IdleFantasy {
    public class Startup : MonoBehaviour {

        void Start() {
            #if UNITY_STANDALONE
            Screen.SetResolution( 1024, 768, false );
            #endif

            SceneManager.LoadScene( SceneList.LOGIN );
        }
    }
}