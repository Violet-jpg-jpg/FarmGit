using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace YFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        public string startScene = string.Empty;
        private CanvasGroup fadeCanvas;

        void Start()
        {
            StartCoroutine(LoadSceneSetActive(startScene));
            Debug.Log(SceneManager.GetActiveScene().name);
        }

        void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }
        
        private void OnTransitionEvent(string targetScene,Vector3 transitionPoint)
        {
            StartCoroutine(Transition(targetScene, transitionPoint));
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName">目标场景名字</param>
        /// <param name="transitionPoint">传送后点位</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName,Vector3 transitionPoint)
        {
            EventHandler.CallBeforeSceneUnLoadEvent();

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);

            EventHandler.CallMoveToPosition(transitionPoint);

            EventHandler.CallAfterSceneUnLoadEvent();
        }

        /// <summary>
        /// 加载一个场景并设置激活
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            SceneManager.SetActiveScene(newScene);
        }
    }
}
