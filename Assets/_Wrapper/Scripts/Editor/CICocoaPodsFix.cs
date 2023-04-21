
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_EDITOR_OSX
	using UnityEditor.iOS.Xcode;
#endif

namespace Editor
{
    public static class XcodeSwiftVersionPostProcess
    {
#if UNITY_EDITOR_OSX
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                ModifyFrameworks(path);
                DisableBitcodeOnIos(buildTarget, path);
            }
        }

        private static void ModifyFrameworks(string path)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);

            var project = new PBXProject();
            project.ReadFromFile(projPath);

            string targetGuid = project.GetUnityFrameworkTargetGuid();

            project.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

            //string mainTargetGuid = project.GetUnityMainTargetGuid();
            //project.SetBuildProperty(mainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

            project.WriteToFile(projPath);
        }

    /// <summary>
    /// Disables bitcode compilation on iOS platform.
    /// </summary>
    // From https://support.unity3d.com/hc/en-us/articles/207942813-How-can-I-disable-Bitcode-support-
    private static void DisableBitcodeOnIos(BuildTarget buildTarget, string path)
    {
        if (buildTarget != BuildTarget.iOS)
        {
            return;
        }

        string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

        var pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        string target = pbxProject.TargetGuidByName("Unity-iPhone");
        pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

        pbxProject.WriteToFile(projectPath);
    }

#endif
    }
}