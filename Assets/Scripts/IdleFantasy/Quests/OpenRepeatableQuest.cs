using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class OpenRepeatableQuest : MonoBehaviour {
        public GameObject QuestMenuPrefab;

        public void OnClick() {
            OpenView();
        }

        private void OpenView() {
            RepeatableQuestModel model = new RepeatableQuestModel( PlayerManager.Data.GetRepeatableQuestForWorld( BackendConstants.WORLD_BASE ), AdManager.Instance );

            GameObject questUI = gameObject.InstantiateUI( QuestMenuPrefab );
            RepeatableQuestView view = questUI.GetComponent<RepeatableQuestView>();
            view.Init( model );
        }
    }
}
