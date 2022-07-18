#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEditor;
using UnityEditor.Callbacks;

namespace Editor
{
    public static class XcodeSwiftVersionPostProcess
    {
#if UNITY_IOS
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
#endif
    }
}