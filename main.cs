using BepInEx;
using Invector;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Video;

namespace funky_initial_video_replacer
{
    [BepInPlugin("initial.video.replacer", "IntroReplacer", "1.0.0")]
    public class main : BaseUnityPlugin
    {
        private void Awake()
        {
            SceneManager.activeSceneChanged += (Scene old, Scene newS) => {
                if (newS.name == "Intro")
                {
                    GameObject.FindFirstObjectByType<vDestroyGameObject>().enabled = false;
                }
            };
        }

        private void OnDestroy()
        {
            string IntroVideoSource = $"file://{Paths.PluginPath}/VideoReplacer/replacement.mp4";
            (new GameObject("temp")).AddComponent<tempBehaviour>().StartCoroutine(SetVideo(IntroVideoSource));
        }
        private IEnumerator SetVideo(string path)
        {
            Logger.LogInfo($"setting video to {path}");
            VideoPlayer player = GameObject.FindFirstObjectByType<VideoPlayer>();
            while (player == null)
            {
                player = GameObject.FindFirstObjectByType<VideoPlayer>();
                yield return null;
            }

            player.Pause();

            player.source = VideoSource.Url;
            player.url = path;
            player.playbackSpeed = 1;

            player.Prepare();

            while (!player.isPrepared)
            {
                yield return null;
            }
            player.Play();
            while (player.isPlaying)
            {
                yield return null;
            }
            SceneManager.LoadScene("Menu");
        }
    }
}
