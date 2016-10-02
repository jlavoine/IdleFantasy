using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class MissionTestHelper : MonoBehaviour {
        public GameObject MainCanvas;
        public GameObject MissionViewPrefab;

        private Mission mTestMission;

        void Start() {            
        }

        private void CreateTestMission_Offline() {
            MissionData testMissionData = new MissionData();
            testMissionData.DescriptionKey = "TEST_MISSION_DESC";
            testMissionData.Index = 0;

            MissionTaskData taskA = new MissionTaskData();
            taskA.DescriptionKey = "Task 1";
            taskA.PowerRequirement = 5000;
            taskA.StatRequirement = "TEST_STAT_1";

            MissionTaskData taskB = new MissionTaskData();
            taskB.DescriptionKey = "Task 2";
            taskB.PowerRequirement = 2000;
            taskB.StatRequirement = "TEST_STAT_2";

            MissionTaskData taskC = new MissionTaskData();
            taskC.DescriptionKey = "Task 3";
            taskC.PowerRequirement = 2000;
            taskC.StatRequirement = "TEST_STAT_2";

            //testMissionData.Tasks = new List<MissionTaskData>() { taskA };
            testMissionData.Tasks = new List<MissionTaskData>() { taskA, taskB, taskC };

            mTestMission = new Mission( testMissionData );

            GameObject missionUI = gameObject.InstantiateUI( MissionViewPrefab, MainCanvas );
            MissionView view = missionUI.GetComponent<MissionView>();
            view.Init( mTestMission );

            //List<MissionData> listMissions = new List<MissionData>();
            //listMissions.Add( testMissionData );                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
            //string missionAsJSON = JsonConvert.SerializeObject( listMissions );
            //UnityEngine.Debug.LogError( missionAsJSON );
        }

        void Update() {
            if ( Input.GetKeyDown( KeyCode.M ) ) {
                CreateMissionUI();
            }
        }

        private void CreateMissionUI() {
            CreateTestMission_FromBackend();
        }

        private void CreateTestMission_FromBackend() {
            //BackendManager.Backend.GetMission( "Test", OnGotMission );
        }

        /*
                // right now this is just for testing
        public void GetMission(string i_missionType, Callback<MissionData> i_callback ) {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( BackendConstants.MISSION_TYPE, i_missionType );

            MakeCloudCall( BackendConstants.GET_MISSIONS, cloudParams, ( results ) => {
                if ( results.ContainsKey( BackendConstants.DATA ) ) {
                    List<MissionData> listMissions = JsonConvert.DeserializeObject<List<MissionData>>( results[BackendConstants.DATA] );
                    i_callback( listMissions[0] );
                }
            } );
        }
        */

        private void OnGotMission( MissionData i_data ) {
            mTestMission = new Mission( i_data );
            GameObject missionUI = gameObject.InstantiateUI( MissionViewPrefab, MainCanvas );
            MissionView view = missionUI.GetComponent<MissionView>();
            view.Init( mTestMission );
        }
    }
}
