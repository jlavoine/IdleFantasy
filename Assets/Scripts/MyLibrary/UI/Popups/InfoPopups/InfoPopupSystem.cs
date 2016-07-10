using UnityEngine;

#pragma warning disable 0219

namespace MyLibrary {
    public class InfoPopupSystem : MonoBehaviour {

        void Start() {
            DontDestroyOnLoad( gameObject );

            InfoPopupManager manager = new InfoPopupManager();
        }
    }
}
