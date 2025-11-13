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
        public CanvasGroup fadeCanvas;
        private bool isFade;

        void Start()
        {
            StartCoroutine(LoadSceneSetActive(startScene));
            fadeCanvas = FindObjectOfType<CanvasGroup>();
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
            if(!isFade)
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

            yield return Fade(1);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);

            EventHandler.CallMoveToPosition(transitionPoint);

            EventHandler.CallAfterSceneUnLoadEvent();
            
            yield return Fade(0);
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

        /// <summary>
        /// 淡入淡出场景
        /// </summary>
        /// <param name="targetAlpha">1是黑，0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvas.blocksRaycasts = true;//是否遮挡鼠标
            float speed = Mathf.Abs(targetAlpha - fadeCanvas.alpha) / Settings.fadeDuration;
            while (!Mathf.Approximately(fadeCanvas.alpha, targetAlpha))
            {
                fadeCanvas.alpha = Mathf.MoveTowards(fadeCanvas.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
            fadeCanvas.blocksRaycasts = false;
            isFade = false;
        }
    }
}
