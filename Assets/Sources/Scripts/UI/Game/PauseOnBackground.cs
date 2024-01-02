using Agava.WebUtility;
using Shared;
using UnityEngine;

namespace UI.Game
{
    public class PauseOnBackground : MonoBehaviour
    {
        private void Awake()
        {
            WebApplication.InBackgroundChangeEvent += InBackground =>
            {
                if (TimeHandler.TimeStopped == true) return;

                if (UIState.CurrentState == State.Game && InBackground == true)
                    UIState.ChangeState(State.Pause);
            };
        }
    }
}