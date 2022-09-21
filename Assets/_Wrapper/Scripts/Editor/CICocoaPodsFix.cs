
using UnityEditor;
using UnityEditor.Callbacks;
/* #if UNITY_EDITOR_OSX
	using UnityEditor.iOS.Xcode;
#endif */

namespace Editor
{
    public static class XcodeSwiftVersionPostProcess
    {
/* #if UNITY_EDITOR_OSX
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                ModifyFrameworks(path);
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
#endif */
    }
}