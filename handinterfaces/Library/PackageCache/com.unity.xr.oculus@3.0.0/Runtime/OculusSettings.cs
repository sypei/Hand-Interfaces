using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Management;

namespace Unity.XR.Oculus
{
    [System.Serializable]
    [XRConfigurationData("Oculus", "Unity.XR.Oculus.Settings")]
    public class OculusSettings : ScriptableObject
    {
        public enum StereoRenderingModeDesktop
        {
            /// <summary>
            /// Unity makes two passes across the scene graph, each one entirely indepedent of the other.
            /// Each pass has its own eye matrices and render target. Unity draws everything twice, which includes setting the graphics state for each pass.
            /// This is a slow and simple rendering method which doesn't require any special modification to shaders.
            /// </summary>
            MultiPass = 0,
            /// <summary>
            /// Unity uses a single texture array with two elements. Unity converts each call into an instanced draw call.
            /// Shaders need to be aware of this. Unity's shader macros handle the situation.
            /// </summary>
            SinglePassInstanced = 1,
        }

        public enum StereoRenderingModeAndroid
        {
            /// <summary>
            /// Unity makes two passes across the scene graph, each one entirely indepedent of the other.
            /// Each pass has its own eye matrices and render target. Unity draws everything twice, which includes setting the graphics state for each pass.
            /// This is a slow and simple rendering method which doesn't require any special modification to shaders.
            /// </summary>
            MultiPass = 0,
            /// <summary>
            /// Unity uses a single texture array with two elements.
            /// Multiview is very similar to Single Pass Instanced; however, the graphics driver converts each call into an instanced draw call so it requires less work on Unity's side.
            /// As with Single Pass Instanced, shaders need to be aware of the Multiview setting. Unity's shader macros handle the situation.
            /// </summary>
            Multiview = 2
        }

        /// <summary>
        /// The current stereo rendering mode selected for desktop-based Oculus platforms
        /// </summary>
        [SerializeField, Tooltip("The current stereo rendering mode selected for desktop-based Oculus platforms.")]
#if UNITY_2021_2_OR_NEWER
        public StereoRenderingModeDesktop m_StereoRenderingModeDesktop = StereoRenderingModeDesktop.SinglePassInstanced;
#else
        public StereoRenderingModeDesktop m_StereoRenderingModeDesktop = StereoRenderingModeDesktop.MultiPass;
#endif

        /// <summary>
        /// The current stereo rendering mode selected for Android-based Oculus platforms
        /// </summary>
        [SerializeField, Tooltip("The current stereo rendering mode selected for Android-based Oculus platforms.")]
#if UNITY_2021_2_OR_NEWER
        public StereoRenderingModeAndroid m_StereoRenderingModeAndroid = StereoRenderingModeAndroid.Multiview;
#else
        public StereoRenderingModeAndroid m_StereoRenderingModeAndroid = StereoRenderingModeAndroid.MultiPass;
#endif

        /// <summary>
        /// Enable or disable support for using a shared depth buffer. This allows Unity and Oculus to use a common depth buffer which enables Oculus to composite the Oculus Dash and other utilities over the Unity application.
        /// </summary>
        [SerializeField, Tooltip("Allows Unity and the Oculus runtime to share a common depth buffer for better scene integration with the Dash.")]
        public bool SharedDepthBuffer = true;

        /// <summary>
        /// Enable or disable Dash support. This inintializes the Oculus Plugin with Dash support which enables the Oculus Dash to composite over the Unity application.
        /// </summary>
        [SerializeField, Tooltip("Initialize the Oculus Plugin with Dash support which allows the Oculus Dash to composite over the Unity application.")]
        public bool DashSupport = true;

        /// <summary>
        /// If enabled, the GLES graphics driver will bypass validation code, potentially running faster.
        /// </summary>
        [SerializeField, Tooltip("If enabled, the GLES graphics driver will bypass validation code, potentially running faster at the expense of detecting and reporting errors. GLES only.")]
        public bool LowOverheadMode = false;

        /// <summary>
        /// If enabled, the depth buffer and MSAA contents will be discarded rather than resolved. This is an optimization that can possibly break rendering in certain cases. Vulkan only.
        /// </summary>
        [SerializeField, Tooltip("If enabled, the depth buffer and MSAA contents will be discarded rather than resolved. This is an optimization that can possibly break rendering in certain cases. Vulkan only.")]
        public bool OptimizeBufferDiscards = true;

        /// <summary>
        /// Enables a latency optimization technique which can reduce simulation latency by several ms, depending on application workload.
        /// </summary>
        [SerializeField, Tooltip("Enables a latency optimization technique which can reduce simulation latency by several ms, depending on application workload.")]
        public bool PhaseSync = false;

        /// <summary>
        /// Allows the application to render with symmetric projection matrices which can improve performance when using multiview.
        /// </summary>
        [SerializeField, Tooltip("Allows the application to render with symmetric projection matrices.")]
        public bool SymmetricProjection = false;

        /// <summary>
        /// Enables a subsampled eye texture layout, which can improve performance when using FFR and reduce FFR related artifacts. Vulkan and Quest 2 only. Requires Unity 2020.3.11f1 or 2021.1.9f1 or higher, and will result in a black screen if enabled on earlier versions of Unity.
        /// </summary>
        [SerializeField, Tooltip("Enables a subsampled eye texture layout, which can improve performance when using FFR and reduce FFR related artifacts. Vulkan and Quest 2 only. Requires Unity 2020.3.11f1 or 2021.1.9f1 or higher, and will result in a black screen if enabled on earlier versions of Unity.")]
        public bool SubsampledLayout = false;

        /// <summary>
        /// Reduces tracked rendering latency by updating head and controller poses as late as possible before rendering. Vulkan only.
        /// </summary>
        [SerializeField, Tooltip("Experimental feature that reduces tracked rendering latency by updating head and controller poses as late as possible before rendering. Vulkan only.")]
        public bool LateLatching = false;

        /// <summary>
        /// Debug mode for Late Latching which will print information about the Late Latching system as well as any errors.
        /// </summary>
        [SerializeField, Tooltip("Debug mode for Late Latching which will print information about the Late Latching system as well as any errors.")]
        public bool LateLatchingDebug = false;

        /// A frame synthesis technology to allow your application to render at half frame rate, while still delivering a smooth experience. Note that this currently requires a custom version of the URP provided by Oculus in order to work, and should not be enabled if you aren't using that customized Oculus URP package.
        /// </summary>
        [SerializeField, Tooltip("A frame synthesis technology to allow your application to render at half frame rate, while still delivering a smooth experience. Note that this currently requires a custom version of the URP provided by Oculus in order to work, and should not be enabled if you aren't using that customized Oculus URP package.")]
        public bool SpaceWarp = false;

        /// <summary>
        /// Adds a Quest entry to the supported devices list in the Android manifest.
        /// </summary>
        [SerializeField, Tooltip("Adds a Quest entry to the supported devices list in the Android manifest.")]
        public bool TargetQuest = true;

        /// <summary>
        /// Adds a Quest 2 entry to the supported devices list in the Android manifest.
        /// </summary>
        [SerializeField, Tooltip("Adds a Quest 2 entry to the supported devices list in the Android manifest.")]
        public bool TargetQuest2 = true;

        /// <summary>
        /// Adds a PNG under the Assets folder as the splash screen image. If set, Oculus OS will display the system splash screen as a high quality compositor layer as soon as the app is starting to launch until the app submits the first frame.
        /// </summary>
        [SerializeField, Tooltip("Adds a PNG under the Assets folder as the splash screen image. If set, Oculus OS will display the system splash screen as a high quality compositor layer as soon as the app is starting to launch until the app submits the first frame.")]
        public Texture2D SystemSplashScreen;


        public ushort GetStereoRenderingMode()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return (ushort)m_StereoRenderingModeAndroid;
# else
            return (ushort)m_StereoRenderingModeDesktop;
#endif
        }
#if !UNITY_EDITOR
		public static OculusSettings s_Settings;

		public void Awake()
		{
			s_Settings = this;
		}
#else
        private void OnValidate()
        {
            if(SystemSplashScreen == null)
                return;

            string splashScreenAssetPath = AssetDatabase.GetAssetPath(SystemSplashScreen);
            if (Path.GetExtension(splashScreenAssetPath).ToLower() != ".png")
            {
                SystemSplashScreen = null;
                throw new ArgumentException("Invalid file format of System Splash Screen. It has to be a PNG file to be used by the Quest OS. The asset path: " + splashScreenAssetPath);
            }
        }
#endif
    }
}
