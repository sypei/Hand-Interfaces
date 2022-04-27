---
uid: arcore-manual
---
# About ARCore XR Plug-in

The ARCore XR Plug-in package enables ARCore support via Unity's multi-platform XR API. This package implements the following XR subsystems:

* [Session](xref:arsubsystems-session-subsystem)
* [Camera](xref:arsubsystems-camera-subsystem)
* [Depth](xref:arsubsystems-depth-subsystem)
* [Input](xref:UnityEngine.XR.XRInputSubsystem)
* [Planes](xref:arsubsystems-plane-subsystem)
* [Raycast](xref:arsubsystems-raycast-subsystem)
* [Anchors](xref:arsubsystems-anchor-subsystem)
* [Face tracking](xref:arsubsystems-face-subsystem)
* [Image tracking](xref:arsubsystems-image-tracking-subsystem)
* [Environment probes](xref:arsubsystems-environment-probe-subsystem)
* [Occlusion](xref:arsubsystems-occlusion-subsystem)

This version of ARCore XR Plug-in uses ARCore 1.24 and supports the following functionality:

* Device localization
* Horizontal plane detection
* Vertical plane detection
* Point clouds
* Pass-through camera view
* Light estimation
* Anchors
* Oriented feature points
* Hit testing
* Session management
* ARCore APK on-demand installation
* 2D Image tracking
* Face tracking
* Environment probes
* Occlusion

This package does not support the following subsystems:

* [Object tracking](xref:arsubsystems-object-tracking-subsystem)
* [Participant](xref:arsubsystems-participant-subsystem)
* [Mesh](xref:arsubsystems-mesh-subsystem)
* [Body tracking](xref:UnityEngine.XR.ARSubsystems.XRHumanBodySubsystem)

# Installing ARCore XR Plug-in

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

You can also install the AR Foundation package, which uses the ARCore XR Plug-in and provides many useful scripts and prefabs. For more information, see the [AR Foundation documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.2).

# Using ARCore XR Plug-in

In most cases, you should use the scripts, prefabs, and assets provided by the AR Foundation package as the basis for your handheld AR apps rather than using ARCore APIs directly. The ARCore XR plug-in supports AR Foundation features on the Android platform by implementing the native endpoints required to target Google’s ARCore platform using Unity's multi-platform XR API.

Use the ARCore XR plug-in APIs when you need access to Android ARCore-specific features. The ARCoreFaceRegions sample in the [AR Foundations repository](https://github.com/Unity-Technologies/arfoundation-samples#ARCoreFaceRegions) provides an example of using an ARCore feature.

See [Using AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.2/manual/index.html#using-ar-foundation) for general information about developing AR apps in Unity.

## Build Settings

You can flag the ARCore XR Plug-in as **Required** or **Optional** in the Project Settings window.

ARCore is set to **Required** by default. If you set the ARCore plug-in to **Optional** instead, the Google Play store lets users install your app on devices that don't support ARCore, or devices that support ARCore but don't have it installed. Use the **Optional** setting when creating an app that provides different experiences depending on whether ARCore is available.

To create an `ARCoreSettings` Asset and assign it to your build settings, open the Project Settings window in **Edit &gt; Project Settings**, then navigate to the **XR Plug-in Management** menu and enable the **ARCore** plug-in provider:

![XR Plug-in Management](images/arcore-xrmanagement.png "ARCore in XR Management")

When you enable this setting, Unity creates an `ARCoreSettings` asset that you can access in the **XR Plug-in Management &gt; ARCore** settings:

![ARCore Settings](images/arcore-project-settings.png "ARCore Settings")

> [!NOTE]
> If you set ARCore as **Required** and install your app on a device that does not support ARCore -- which you can do using Unity's **Build and Run** feature or by "side-loading" via USB -- the device will incorrectly report that ARCore is available. (This is because the Google Play Store prevents the installation of apps that require ARCore on unsupported devices, so these apps always assume they're running on a supported device.)


## Session

ARCore implements `XRSessionSubsystem.GetAvailabilityAsync`.  Use this function to determine if the device ARCore is currently running on is supported. ARCore's list of supported devices is frequently updated to include additional devices.  For a full list of devices that ARCore supports, see [ARCore supported devices](https://developers.google.com/ar/discover/supported-devices).

If ARCore isn't already installed on a device, your app needs to check with the Google Play store to see if there's a version of ARCore that supports that device. To do this, use `GetAvailabilityAsync` to return a `Promise` that you can use in a coroutine. For ARCore, this check can take some time.

If the device is supported, but ARCore is not installed or requires an update, call `XRSessionSubsystem.InstallAsync`, which also returns a `Promise`.

For more information, see [ARSubsystems session documentation](xref:arsubsystems-session-subsystem).

## Depth subsystem

Ray casts return a `Pose` for the item the ray cast hits. When you use a ray cast against feature points, the pose orientation provides an estimate for the surface the feature point might represent.

The depth subsystem doesn't require any additional resources, so it doesn't affect performance.

ARCore's depth subsystem only produces one [`XRPointCloud`](xref:UnityEngine.XR.ARSubsystems.XRPointCloud).

For more information, see [ARSubsystems depth subsystem](xref:arsubsystems-depth-subsystem).

## Plane tracking

ARCore supports plane subsumption. This means that you can include a plane inside another. Unity keeps the included (subsumed) plane and doesn't update it.

ARCore provides boundary points for all its planes.

The ARCore plane subsystem requires additional CPU resources and can use a lot of energy. The horizontal and vertical plane detection require additional resources when enabled. To save energy, disable plane detection when your app doesn't need it.

Setting the plane detection mode to `PlaneDetectionMode.None` works in the same way as using `Stop` for a subsystem.

For more information, see the [ARSubsystems plane subsystem documentation](xref:arsubsystems-plane-subsystem).

## Image tracking

To use image tracking on ARCore, you must create a reference image library. To do this, see the [AR Subsystems documentation on image tracking](xref:arsubsystems-image-tracking-subsystem).

When you build the Player for Android, the ARCore build code creates a  `imgdb` file for each reference image library. ARCore creates these files in your project's `StreamingAssets` folder, in a subdirectory called `HiddenARCore`, so Unity can access them at runtime.

You can use .jpg or .png files as AR reference images in ARCore. If a reference image in the `XRReferenceImageLibrary` isn't a .jpg or .png, the ARCore build processor attempts to convert the Texture to a .png so that ARCore can use it.

When you export a  `Texture2D` to .png, it can fail if the Texture's [Texture Import Settings](https://docs.unity3d.com/Manual/class-TextureImporter.html) have **Read/Write Enabled** disabled and **Compression** is set to **None**.

To use the Texture at runtime (not as a source Asset for the reference image), create a separate .jpg or .png copy for the source Asset. This reduces the performance impact of the Texture Import Settings at runtime.

### Reference image dimensions

To improve image detection in ARCore you can specify the image dimensions. When you specify the dimensions for a reference image, ARCore receives the image's width, and then determines the height from the image's aspect ratio.

## Face tracking

For information about face tracking, see the [ARSubsystems Face Tracking](xref:arsubsystems-face-subsystem) documentation.

The ARCore face subsystem provides face tracking methods that allow access to "regions". Regions are specific to ARCore. ARCore provides access to the following regions that define features on a face:

- Nose tip
- Forehead left
- Forehead right

Each region has a [Pose](xref:UnityEngine.Pose) associated with it. To access face regions, obtain an instance of the [ARCoreFaceSubsystem](xref:UnityEngine.XR.ARCore.ARCoreFaceSubsystem) using the following script:

```csharp
XRFaceSubsystem faceSubsystem = ...
#if UNITY_ANDROID
var arcoreFaceSubsystem = faceSubsystem as ARCoreFaceSubsystem;
if (arcoreFaceSubsystem != null)
{
    var regionData = new NativeArray<ARCoreFaceRegionData>(0, Allocator.Temp);
    arcoreFaceSubsystem.GetRegionPoses(faceId, Allocator.Temp, ref regionData);
    using (regionData)
    {
        foreach (var data in regionData)
        {
            Debug.LogFormat("Region {0} is at {1}", data.region, data.pose);
        }
    }
}
#endif
```

## Light estimation

ARCore light estimation can operate in two modes:

- `LightEstimationMode.AmbientIntensity`: Providers color correction and average pixel intensity information.
- `LightEstimationMode.EnvironmentalHDR`: Provides an estimated Main Light Direction, HDR Color, and the ambient SphericalHarmonicsL2 (see [SphericalHarmonicsL2](https://docs.unity3d.com/ScriptReference/Rendering.SphericalHarmonicsL2.html) for more information on Spherical Harmonics).

You can’t use both modes at the same time.

ARCore's [Face tracking](xref:UnityEngine.XR.ARCore.ARCoreFaceSubsystem) and [Environment probes](xref:UnityEngine.XR.ARSubsystems.XREnvironmentProbeSubsystem) use or affect the light estimation mode.  If one or both of these subsystems is present and `enabled`, it changes the light estimation mode behavior depending on the configuration:

| Functionality      | Supported light estimation modes                                       | Modifiable |
|--------------------|------------------------------------------------------------------------|------------|
| Face tracking      | `LightEstimationMode.AmbientIntensity`, `LightEstimationMode.Disabled` | Yes        |
| Environment probes | `LightEstimationMode.EnvironmentalHDR`                                 | No         |

* [Face tracking](xref:UnityEngine.XR.ARCore.ARCoreFaceSubsystem): ARCore doesn't support `LightEstimationMode.EnvironmentalHDR` when face tracking is enabled and rendering won't work when this mode is specified. To prevent errors, you can only set `LightEstimationMode.AmbientIntensity` or `LightEstimationMode.Disabled` when face tracking is enabled, or ARCore enforces `LightEstimationMode.Disabled`.

*  [Environment probes](xref:UnityEngine.XR.ARSubsystems.XREnvironmentProbeSubsystem): For ARCore environment probes to update the cubemap, the light estimation mode must be set to `LightEstimationMode.EnvironmentalHDR`. This also allows ARCore to take ownership of the setting.

## Camera configuration

[XRCameraConfiguration](xref:UnityEngine.XR.ARSubsystems.XRCameraConfiguration) contains an `IntPtr` field `nativeConfigurationHandle`, which is a platform-specific handle. For ARCore, this handle is the pointer to the `ArCameraConfiguration`. The native object is managed by Unity. Do not manually destroy it.

## Occlusion

The ARCore implementation of [XROcclusionSubsystem](xref:arsubsystems-occlusion-subsystem) supports [Environment Depth Texture](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.2/api/UnityEngine.XR.ARFoundation.AROcclusionManager.html#UnityEngine_XR_ARFoundation_AROcclusionManager_environmentDepthTexture) but does not support the other Textures related to human segmentation.

## Recording and playback

ARCore allows you to record an ArSession to an `.mp4` and play it back at a later time. To support this feature, the [ARCoreSessionSubsystem](xref:UnityEngine.XR.ARCore.ARCoreSessionSubsystem) exposes the following methods:

* [StartRecording](xref:UnityEngine.XR.ARCore.ARCoreSessionSubsystem.StartRecording(UnityEngine.XR.ARCore.ArRecordingConfig))
* [StopRecording](xref:UnityEngine.XR.ARCore.ARCoreSessionSubsystem.StopRecording)
* [StartPlayback](xref:UnityEngine.XR.ARCore.ARCoreSessionSubsystem.StartPlayback(System.String))
* [StopPlayback](xref:UnityEngine.XR.ARCore.ARCoreSessionSubsystem.StopPlayback)

To start a recording, supply an [ArRecordingConfig](xref:UnityEngine.XR.ARCore.ArRecordingConfig). This specifies the file name that Unity saves the recording as, as well as other options. Call `StopRecording` to stop recording. When Unity stops recording, it creates the `.mp4` file as specified in the `ArRecordingConfig`. This contains the camera feed and sensor data required by ARCore.

To play back a video, use the `StartPlayback` method, and specify an `.mp4` file created during an earlier recording.

To start or stop a recorded file in ARCore, the [ARCoreSessionSubsystem](xref:UnityEngine.XR.ARCore.ARCoreSessionSubsystem) pauses the session. Pausing and resuming a session can take between 0.5 and 1.0 seconds.

**Note**: Video recordings contain sensor data, but not the computed results. ARCore does not always produce the same output, which means trackables might not be consistent between playbacks of the same recording. For example, multiple playbacks of the same recording might give different plane detection results.

# Technical details

## Requirements

This version of ARCore XR Plug-in is compatible with the following versions of the Unity Editor:

* 2020.3
* 2021.1
* 2021.2

## Known limitations

* Color Temperature in degrees Kelvin is not currently supported.
* The [XROcclusionSubsystemDescriptor](xref:UnityEngine.XR.ARSubsystems.XROcclusionSubsystemDescriptor) properties [supportsEnvironmentDepthImage](xref:UnityEngine.XR.ARSubsystems.XROcclusionSubsystemDescriptor.supportsEnvironmentDepthImage) and [supportsEnvironmentDepthConfidenceImage](xref:UnityEngine.XR.ARSubsystems.XROcclusionSubsystemDescriptor.supportsEnvironmentDepthConfidenceImage) require a session before support can be determined. If there is no session, then these properties return `false`. They might return `true` later when a session has been established.

## Package contents

This version of ARCore XR Plug-in includes:

* A shared library which provides implementation of the XR Subsystems listed above
* A shader used for rendering the camera image
* A plug-in metadata file
