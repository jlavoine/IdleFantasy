using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyLibrary {
    public class LoadSceneButton : MonoBehaviour {
        public string Scene;

        public void OnClick() {
            SceneManager.LoadScene( Scene );
        }
    }
}
