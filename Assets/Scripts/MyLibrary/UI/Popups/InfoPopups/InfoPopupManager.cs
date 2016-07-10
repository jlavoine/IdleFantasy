using UnityEngine;
using System.Collections.Generic;

namespace MyLibrary {
    public class InfoPopupManager {
        private const string POPUP_PANEL = "InfoPopupPanel";

        private List<QueuedInfoPopupData> mListPopups = new List<QueuedInfoPopupData>();

        private bool mShowingPopup = false;

        public GameObject PopupPanel { get { return GameObject.Find( POPUP_PANEL ); } }

        public InfoPopupManager() {
            SubscribeForMessages();            
        }        

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void SubscribeForMessages() {
            MyMessenger.AddListener<string, ViewModel>( InfoPopupEvents.QUEUE, QueueInfoPopup );
            MyMessenger.AddListener( InfoPopupEvents.CLOSE, OnPopupClosed );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<string, ViewModel>( InfoPopupEvents.QUEUE, QueueInfoPopup );
            MyMessenger.RemoveListener( InfoPopupEvents.CLOSE, OnPopupClosed );
        }

        public void QueueInfoPopup( string i_prefabName, ViewModel i_viewModel ) {
            QueuedInfoPopupData queuedPopup = new QueuedInfoPopupData( i_prefabName, i_viewModel );
            mListPopups.Add( queuedPopup );
            CheckToShowNextPopup();
        }

        public void CheckToShowNextPopup() {
            if ( !mShowingPopup && mListPopups.Count > 0 ) {
                ShowNextPopup();
            }
        }

        public void ShowNextPopup() {
            if ( PopupPanel != null ) {
                QueuedInfoPopupData nextQueuedPopupData = GetNextPopupData();
                GameObject popupObject = CreatePopup( nextQueuedPopupData );
                InitPopup( popupObject, nextQueuedPopupData );
                RemovedPopupDataFromList( nextQueuedPopupData );
            } else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "InfoPopupManager trying to show a popup, but the parent panel does not exist.", "InfoPopup" );
            }
        }

        private QueuedInfoPopupData GetNextPopupData() {
            return mListPopups[0];
        }

        private GameObject CreatePopup( QueuedInfoPopupData i_popupToShow ) {
            string nextPopupPrefabName = mListPopups[0].PrefabName;
            GameObject popup = PopupPanel.InstantiateUI( nextPopupPrefabName );
            mShowingPopup = true;

            return popup;
        }

        private void InitPopup( GameObject i_popup, QueuedInfoPopupData i_popupData ) {
            InfoPopupView infoView = i_popup.GetComponent<InfoPopupView>();
            infoView.Init( i_popupData.ViewModel );
        }

        private void RemovedPopupDataFromList( QueuedInfoPopupData i_popupData ) {
            mListPopups.Remove( i_popupData );
        }

        public void OnPopupClosed() {
            mShowingPopup = false;

            CheckToShowNextPopup();
        }
    }
}