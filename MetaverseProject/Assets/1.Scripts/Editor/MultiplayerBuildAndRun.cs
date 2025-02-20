using UnityEditor;
using UnityEngine;
using System.Linq;

public class MultiplayerBuildAndRun
{
    private const string BUILD_PATH_WIN = "Builds/Win64/";
    private const string BUILD_PATH_MAC = "Builds/Mac/";

    #region Windows Build
    [MenuItem("Tools/Run Multiplayer/Win64/1 Players")]
    static void PerformWin64Build1() => PerformBuild(BuildTarget.StandaloneWindows64, 1);

    [MenuItem("Tools/Run Multiplayer/Win64/2 Players")]
    static void PerformWin64Build2() => PerformBuild(BuildTarget.StandaloneWindows64, 2);

    [MenuItem("Tools/Run Multiplayer/Win64/3 Players")]
    static void PerformWin64Build3() => PerformBuild(BuildTarget.StandaloneWindows64, 3);

    [MenuItem("Tools/Run Multiplayer/Win64/4 Players")]
    static void PerformWin64Build4() => PerformBuild(BuildTarget.StandaloneWindows64, 4);
    #endregion

    #region Mac Build
    [MenuItem("Tools/Run Multiplayer/Mac/1 Players")]
    static void PerformMacBuild1() => PerformBuild(BuildTarget.StandaloneOSX, 1);

    [MenuItem("Tools/Run Multiplayer/Mac/2 Players")]
    static void PerformMacBuild2() => PerformBuild(BuildTarget.StandaloneOSX, 2);

    [MenuItem("Tools/Run Multiplayer/Mac/3 Players")]
    static void PerformMacBuild3() => PerformBuild(BuildTarget.StandaloneOSX, 3);

    [MenuItem("Tools/Run Multiplayer/Mac/4 Players")]
    static void PerformMacBuild4() => PerformBuild(BuildTarget.StandaloneOSX, 4);
    #endregion

    /// <summary>
    /// 플랫폼별 빌드를 수행하는 공통 함수
    /// </summary>
    static void PerformBuild(BuildTarget target, int playerCount)
    {
        string buildPath = target == BuildTarget.StandaloneWindows64 ? BUILD_PATH_WIN : BUILD_PATH_MAC;
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, target);

        for (int i = 1; i <= playerCount; i++)
        {
            string playerFolder = $"{buildPath}{GetProjectName()}{i}/";
            string playerExe = $"{playerFolder}{GetProjectName()}{(target == BuildTarget.StandaloneWindows64 ? ".exe" : ".app")}";

            BuildPipeline.BuildPlayer(GetScenePaths(), playerExe, target, BuildOptions.AutoRunPlayer);
        }
    }

    /// <summary>
    /// 프로젝트 이름을 가져오는 함수
    /// </summary>
    static string GetProjectName()
    {
        string[] pathParts = Application.dataPath.Split('/');
        return pathParts[pathParts.Length - 2];
    }

    /// <summary>
    /// 빌드 설정에서 활성화된 씬들의 경로를 가져오는 함수
    /// </summary>
    static string[] GetScenePaths()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }
}
