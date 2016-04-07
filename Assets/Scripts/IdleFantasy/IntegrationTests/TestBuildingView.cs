using UnityEngine;
using System.Collections.Generic;

namespace IdleFantasy {
    public class TestBuildingView : MonoBehaviour {

        void Start() {
            BuildingView viewToTest = FindObjectOfType<BuildingView>();
            Building testBuilding = GetMockBuilding();

            viewToTest.SetModel( testBuilding.GetViewModel() );
        }

        public static Building GetMockBuilding() {
            BuildingData data = new BuildingData();
            data.ID = "TEST_BUILDING";
            data.MaxLevel = 50;
            data.Size = 10;
            data.Units = new List<string>() { "TEST_UNIT" };
            data.Categories = new List<string>() { "TEST_CATEGORY" };
            data.ResourcesToUpgrade = new Dictionary<string, int>() { { "Wood", 10 } };

            return new Building( data );
        }
    }
}
