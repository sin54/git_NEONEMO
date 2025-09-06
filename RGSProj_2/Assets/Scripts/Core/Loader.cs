using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    /// <summary>
    /// 씬을 로드하는 역할
    /// </summary>
    public static class Loader
    {
        private class LoadingMonoBehavior : MonoBehaviour { }
        public enum Scene
        {
            MainScene,
            LoadingScene,
            GameScene,
            GameOver,
            CutScene
        }

        private static Action onLoaderCallback;
        private static AsyncOperation loadingAsyncOperation;
        public static void Load(Scene scene)
        {

            onLoaderCallback = () =>
            {
                GameObject loadingGameObject = new GameObject("Loading Game Obj");
                loadingGameObject.AddComponent<LoadingMonoBehavior>().StartCoroutine(LoadSceneAsync(scene));
            };
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }
        public static void LoadWithNoLoadingScene(Scene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }

        private static IEnumerator LoadSceneAsync(Scene scene)
        {
            yield return null;

            loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
            loadingAsyncOperation.allowSceneActivation = false;

            float minLoadTime = 2f; // 최소 2초 로딩
            float timer = 0f;

            while (!loadingAsyncOperation.isDone)
            {
                timer += Time.unscaledDeltaTime;

                // 로딩이 90% 이상 되었고, 최소 시간도 지났다면 넘어가기
                if (loadingAsyncOperation.progress >= 0.9f && timer >= minLoadTime)
                {
                    loadingAsyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        public static float GetLoadingProgress()
        {
            if (loadingAsyncOperation!= null)
            {
                return loadingAsyncOperation.progress;
            }
            else
            {
                return 0f;
            }
        }
        public static void LoaderCallback()
        {
            if (onLoaderCallback != null)
            {
                onLoaderCallback();
                onLoaderCallback = null;
            }
        }
    }
}