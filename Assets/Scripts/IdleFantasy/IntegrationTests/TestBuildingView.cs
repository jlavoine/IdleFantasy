using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace IdleFantasy {
    public class TestBuildingView : MonoBehaviour {

        Building mTestBuilding;

        void Start() {
            UnitData unitData = GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, GenericDataLoader.TEST_UNIT );
            Unit testUnit = new Unit( unitData );

            BuildingView viewToTest = FindObjectOfType<BuildingView>();
            BuildingController controllerToTest = FindObjectOfType<BuildingController>();

            MajorResourcesView[] views = FindObjectsOfType<MajorResourcesView>();
            MockPlayerData mockPlayer = new MockPlayerData();
            mockPlayer.Gold = 10000;

            BuildingData buildingData = GenericDataLoader.GetData<BuildingData>( GenericDataLoader.BUILDINGS, GenericDataLoader.TEST_BUILDING );
            mTestBuilding = new Building( buildingData, testUnit );

            foreach ( MajorResourcesView view in views ) {
                view.SetModel( mockPlayer.GetViewModel() );
            }

            viewToTest.SetModel( mTestBuilding.GetViewModel() );
            controllerToTest.Init( mTestBuilding, mockPlayer );

            SetUpMajorResourcesView();

            //Test();
            //IntegrationTest.Pass();
        }

        void SetUpMajorResourcesView() {


     
        }

        void Update() {
            int msElapsed = (int)(Time.deltaTime * 1000);
            TimeSpan timeElapsedAsSpan = new TimeSpan( 0, 0, 0, 0, msElapsed );
            mTestBuilding.Tick( timeElapsedAsSpan );
        }

        public void Test() {
            UnitData data = new UnitData();
            data.ID = "TEST_UNIT";
            data.Stats = new Dictionary<string, int>() { { "Str", 1 }, { "Int", 4 } };
            data.BaseProgressPerSecond = 1.2f;

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            string test = JsonConvert.SerializeObject( data, Formatting.Indented, settings );
            Debug.Log( test );
        }

        /*public static Building GetMockBuilding() {
            BuildingData data = new BuildingData();
            data.ID = "TEST_BUILDING";
            data.MaxLevel = 50;
            data.Size = 10;
            data.Units = new List<string>() { "TEST_UNIT" };
            data.Categories = new List<string>() { "TEST_CATEGORY" };
            data.ResourcesToUpgrade = new Dictionary<string, int>() { { "Wood", 10 } };

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            string test = JsonConvert.SerializeObject( data, Formatting.Indented, settings );
            Debug.Log( test );

            return new Building( data );
        }*/
    }
}
