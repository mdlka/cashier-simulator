using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YellowSquad.CashierSimulator.Application.Editor
{
    internal class DisableDepthClearInBuild : IProcessSceneWithReport
    {
        public int callbackOrder => 1;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            if (UnityEngine.Application.isPlaying) 
                return;
            
            foreach (GameObject rootGameObject in scene.GetRootGameObjects())
                foreach (Camera camera in rootGameObject.GetComponentsInChildren<Camera>())
                    if (camera.clearFlags == CameraClearFlags.Depth)
                        camera.clearFlags = CameraClearFlags.Nothing;
        }
    }
}
