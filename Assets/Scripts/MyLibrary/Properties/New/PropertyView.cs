using UnityEngine;
using System;

namespace MyLibrary {
    public abstract class PropertyView : MonoBehaviour {
        public abstract void UpdateView();

        public string PropertyName;

        protected ViewModel mModel;

        protected Guid mPropertyID;

        public void SetModel( ViewModel i_model ) {
            mModel = i_model;
            if ( i_model == null ) {
                Debug.LogError( "PropertyView has null model: " + PropertyName );
            }

            bool modelHasProperty = mModel.HasProperty( PropertyName );
            if ( !modelHasProperty ) {
                mModel.CreateProperty( PropertyName );
            }

            Property property = mModel.GetProperty( PropertyName );
            Guid guid = property.GetID();
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
    }
}