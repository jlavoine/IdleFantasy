using NUnit.Framework;

#pragma warning disable 0414

namespace MyLibrary {
    public class PropertySystemTests {

        [Test]
        public void UncreatedPropertyIsNull() {
            PropertySet propertySet = new PropertySet();

            Property property = propertySet.GetProperty( "None" );

            Assert.IsNull( property );
        }

        [Test]
        public void DoesNotHaveUncreatedProperty() {
            PropertySet propertySet = new PropertySet();

            bool hasProperty = propertySet.HasProperty( "None" );

            Assert.IsFalse( hasProperty );
        }

        [Test]
        public void CreatedPropertyNotNull() {
            PropertySet propertySet = new PropertySet();

            propertySet.CreateProperty( "NewProperty" );
            Property newProperty = propertySet.GetProperty( "NewProperty" );

            Assert.NotNull( newProperty );
        }

        [Test]
        public void HasCreatedProperty() {
            PropertySet propertySet = new PropertySet();

            propertySet.CreateProperty( "NewProperty" );
            bool hasProperty = propertySet.HasProperty( "NewProperty" );

            Assert.IsTrue( hasProperty );
        }

        [Test]
        public void SetPropertyCreatesProperty() {
            PropertySet propertySet = new PropertySet();

            propertySet.SetProperty( "NewProperty", 0 );
            bool hasProperty = propertySet.HasProperty( "NewProperty" );

            Assert.IsTrue( hasProperty );
        }

        [Test]
        public void GetPropertyValueReturnsExpectedValue() {
            PropertySet propertySet = new PropertySet();

            propertySet.SetProperty( "NewProperty", 0 );
            int propertyValue = propertySet.GetPropertyValue<int>( "NewProperty" );

            Assert.AreEqual( propertyValue, 0 );
        }

        [Test]
        public void GetPropertyWithWrongTypeReturnsDefault() {
            PropertySet propertySet = new PropertySet();

            propertySet.SetProperty( "NewProperty", 0 );
            string propertyValue = propertySet.GetPropertyValue<string>( "NewProperty" );

            Assert.AreEqual( default( string ), propertyValue );
        }
    }
}