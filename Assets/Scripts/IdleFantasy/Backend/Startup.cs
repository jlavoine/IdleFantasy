using UnityEngine;
using UnityEngine.SceneManagement;

namespace IdleFantasy {
    public class Startup : MonoBehaviour {

        void Start() {
            SceneManager.LoadScene( "Login" );
        }
    }
}