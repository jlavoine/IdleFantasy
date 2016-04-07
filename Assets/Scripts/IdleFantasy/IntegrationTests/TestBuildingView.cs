using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class TestBuildingView : MonoBehaviour {

        void Start() {
            BuildingView viewToTest = FindObjectOfType<BuildingView>();
            BuildingData buildingData = BuildingLoader.GetData( BuildingLoader.TEST_BUILDING );
            Building testBuilding = new Building( buildingData );

            viewToTest.SetModel( testBuilding.GetViewModel() );

            IntegrationTest.Pass();
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
