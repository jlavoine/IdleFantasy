using UnityEngine;

namespace IdleFantasy {
    public class MissionTestHelper : MonoBehaviour {
        public GameObject MainCanvas;
        public GameObject MissionViewPrefab;

        private Mission mTestMission;

        void Start() {
            CreateTestMission();
        }

        private void CreateTestMission() {
            MissionData testMissionData = new MissionData();
            testMissionData.DescriptionKey = "TEST_MISSION_DESC";

            mTestMission = new Mission( testMissionData );
        }

        void Update() {
            if ( Input.GetKeyDown( KeyCode.M ) ) {
                CreateMissionUI();
            }
        }

        private void CreateMissionUI() {
            GameObject missionUI = gameObject.InstantiateUI( MissionViewPrefab, MainCanvas );
            MissionView view = missionUI.GetComponent<MissionView>();
            view.Init( mTestMission );
        }
    }
}
