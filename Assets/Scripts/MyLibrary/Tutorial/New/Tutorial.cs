using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary {
    public abstract class Tutorial : MonoBehaviour {
        public const string END_STEP_MESSAGE = "EndStep";
        public const string TUTORIAL_FINISHED = "TutorialFinished";

        protected abstract bool ShouldStartTutorial();

        public string TutorialName;
        public List<GameObject> TutorialSteps;

        private int mCurrentStep;
        public int CurrentStep {
            get { return mCurrentStep; }
            private set { mCurrentStep = value; }
        }

        private GameObject mCurrentStepObject;

        void Start() {
            SubscribeToMessages();

            if ( ShouldStartTutorial() ) {
                StartTutorial();
            } else {
                Destroy( gameObject );
            }
        }

        void OnDestroy() {
            UnsubscribeFromMessages();
        }

        private void FinishTutorial() {
            MyMessenger.Send( TUTORIAL_FINISHED, TutorialName );
            Destroy( gameObject );
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener( END_STEP_MESSAGE, OnEndStep );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener( END_STEP_MESSAGE, OnEndStep );
        }

        private void StartTutorial() {
            CurrentStep = 0;
            StartCoroutine( BeginTutorialStep( CurrentStep ) ); 
        }

        private IEnumerator BeginTutorialStep( int i_stepNum ) {
            TutorialStep step = InstantiateStepObject( i_stepNum );

            yield return null;

            DisableAllInvalidButtons( step.StepName );            
        }

        private void DisableAllInvalidButtons( string i_stepName ) {
            Button[] allButtons = GameObject.FindObjectsOfType<Button>();
            foreach ( Button button in allButtons ) {
                GameObject gameObject = button.gameObject;
                TutorialComponent tutComponent = gameObject.GetComponent<TutorialComponent>();
                if ( tutComponent == null || !tutComponent.ValidForTutorials.Contains( i_stepName ) ) {
                    button.interactable = false;
                }
            }
        }

        private void EnableAllButtons() {
            Button[] allButtons = GameObject.FindObjectsOfType<Button>();
            foreach ( Button button in allButtons ) {
                button.interactable = true;
            }
        }

        private TutorialStep InstantiateStepObject( int i_stepNum ) {
            mCurrentStepObject = gameObject.InstantiateUI( TutorialSteps[i_stepNum], GameObject.FindGameObjectWithTag( "TutorialCanvas" ) );
            return mCurrentStepObject.GetComponent<TutorialStep>();
        }

        private void OnEndStep() {
            Destroy( mCurrentStepObject );
            EnableAllButtons();
            CurrentStep++;

            if ( CurrentStep >= TutorialSteps.Count ) {
                FinishTutorial();                
            } else {
                StartCoroutine( BeginTutorialStep( CurrentStep ) );
            }
        }
    }
}
