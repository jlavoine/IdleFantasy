using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class MissionInterfaceManager : Singleton<MissionInterfaceManager> {
        public GameObject MissionCanvas;
        public GameObject MissionViewPrefab;

        public void CreateUI( Mission i_mission ) {
            GameObject missionUI = gameObject.InstantiateUI( MissionViewPrefab, MissionCanvas );
            MissionView view = missionUI.GetComponent<MissionView>();
            view.Init( i_mission );
        }
    }
}