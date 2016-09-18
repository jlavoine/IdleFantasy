using UnityEngine;
using UnityEngine.SceneManagement;

namespace IdleFantasy {
    public class Startup : MonoBehaviour {

        void Start() {
            Screen.SetResolution( 1024, 768, false );

            SceneManager.LoadScene( SceneList.LOGIN );
        }
    }
}