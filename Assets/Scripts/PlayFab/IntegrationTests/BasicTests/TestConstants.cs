using System.Collections;
using MyLibrary;
using UnityEngine;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestConstants : IntegrationTestBase {

        protected override IEnumerator RunAllTests() {
            yield return InitConstants();

            TestInt();
            TestFloat();
            TestBool();
            TestString();
            TestVector3();
            TestColor();  
        }

        private void FailTest( string i_constantType ) {
            IntegrationTest.Fail( "Constant test failed for: " + i_constantType );
        }

        private void TestInt() {
            int value = Constants.GetConstant<int>( "TestInt" );
            if ( value != 10 ) {
                FailTest( "Int" );
            }
        }

        private void TestBool() {
            bool value = Constants.GetConstant<bool>( "TestBool" );
            if ( value != true ) {
                FailTest( "Bool" );
            }
        }

        private void TestFloat() {
            float value = Constants.GetConstant<float>( "TestFloat" );
            if ( value != 1.5f ) {
                FailTest( "Float" );
            }
        }

        private void TestString() {
            string value = Constants.GetConstant<string>( "TestString" );
            if ( value != "Hello" ) {
                FailTest( "String" );
            }
        }

        private void TestColor() {
            Color value = Constants.GetConstant<Color>( "TestColor" );
            Color expectedValue = new Color( 100.0f / 255, 100.0f / 255, 100.0f / 255, 0.0f );

            if ( value.a != expectedValue.a || value.b != expectedValue.b || value.g != expectedValue.g || value.r != expectedValue.r ) {
                FailTest( "Color" );
            }
        }

        private void TestVector3() {
            Vector3 value = Constants.GetConstant<Vector3>( "TestVector3" );
            Vector3 expectedValue = new Vector3( 1.0f, 2.0f, 3.0f );
            if ( value != expectedValue ) {
                FailTest( "Vector3" );
            }
        }
    }
}
