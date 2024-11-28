using System.Diagnostics;
using System.IO;
using BepInEx;
using UnityEngine;

namespace SophisticatedStreamCam.Sceeps
{
    public class Streamer
    {
        private Process ffmpegProcess;
        public static RenderTexture activeRenderTexture; // Currently active RenderTexture
        public static RenderTexture secondaryRenderTexture; // Secondary RenderTexture for switching
        private bool useSecondaryTexture; // Flag to track which texture is active

        public Streamer(RenderTexture initialTexture)
        {
            activeRenderTexture = initialTexture;
        }

        public void SetSecondaryRenderTexture(RenderTexture texture)
        {
            secondaryRenderTexture = texture;
        }

        public void SwitchRenderTexture()
        {
            if (secondaryRenderTexture == null)
                return;

            // Toggle between primary and secondary textures
            useSecondaryTexture = !useSecondaryTexture;
            activeRenderTexture = useSecondaryTexture ? secondaryRenderTexture : activeRenderTexture;
        }

        public void StartStreaming(string twitchStreamKey)
        {
            if (ffmpegProcess != null && !ffmpegProcess.HasExited)
                return;

            string ffmpegPath = Path.Combine(Paths.PluginPath, "SophisticatedStreamCam", "ffmpeg", "ffmpeg.exe");
            if (!File.Exists(ffmpegPath))
                return;

            ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = $"-f rawvideo -pix_fmt rgba -s {activeRenderTexture.width}x{activeRenderTexture.height} " +
                                $"-i - -f flv rtmp://live.twitch.tv/app/{twitchStreamKey}",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            ffmpegProcess.Start();

            // Start a coroutine to feed the active RenderTexture to FFmpeg
            MonoBehaviour mb = new GameObject("StreamHandlerHelper").AddComponent<StreamHandlerHelper>();
            mb.StartCoroutine(StreamToFfmpeg(ffmpegProcess, activeRenderTexture));
        }

        public void StopStreaming()
        {
            if (ffmpegProcess != null && !ffmpegProcess.HasExited)
            {
                ffmpegProcess.Kill();
                ffmpegProcess.Dispose();
                ffmpegProcess = null;
            }
        }

        private System.Collections.IEnumerator StreamToFfmpeg(Process process, RenderTexture renderTexture)
        {
            var texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            while (process != null && !process.HasExited)
            {
                RenderTexture.active = renderTexture;
                texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                RenderTexture.active = null;

                byte[] frameData = texture2D.GetRawTextureData();
                process.StandardInput.BaseStream.Write(frameData, 0, frameData.Length);
                process.StandardInput.BaseStream.Flush();

                yield return new WaitForEndOfFrame();
            }
            Object.Destroy(texture2D);
        }
    }

    public class StreamHandlerHelper : MonoBehaviour { }
}
