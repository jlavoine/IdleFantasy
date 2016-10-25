using UnityEngine;
using System;

namespace MyLibrary {
    public abstract class PropertyView : MonoBehaviour {
        public abstract void UpdateView();

        public string PropertyName;

        private IViewModel mModel;
        protected IViewModel Model {
            get { return mModel; }
        }

        protected Guid mPropertyID;

        public void SetModel( IViewModel i_model ) {
            mModel = i_model;
            if ( i_model == null ) {
                Debug.LogError( "PropertyView has null model: " + PropertyName );
            }

            bool modelHasProperty = mModel.HasProperty( PropertyName );
            if ( !modelHasProperty ) {
                mModel.CreateProperty( PropertyName );
            }

            Property property = mModel.GetProperty( PropertyName );
            Guid guid = property.ID;
            SetPropertyID( guid, !modelHasProperty );
        }

        void OnDestroy() {
            Messenger.RemoveListener( "SetDirty_" + mPropertyID, UpdateView );
        }

        public void SetPropertyID( Guid i_id, bool i_isNewProperty ) {
            mPropertyID = i_id;

            Messenger.AddListener( "SetDirty_" + i_id, UpdateView );

            if ( !i_isNewProperty ) {
                UpdateView();
            }
        }

        protected T GetValue<T>() {
            return Model.GetPropertyValue<T>( PropertyName );
        }
    }
}