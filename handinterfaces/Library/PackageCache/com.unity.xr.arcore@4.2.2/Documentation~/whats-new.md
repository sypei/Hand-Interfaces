---
uid: arcore-whats-new
---
# What's new in version 4.2

Summary of changes in ARCore XR Plug-in package version 4.2.

The main updates in this release include:

**Added**

- When you build an Android Player with ARCore enabled, this package checks the Gradle version and displays a warning if the Gradle version is too low. To disable this check and its warning notification, use the new public API [ARCoreSettings.ignoreGradleVersion](xref:UnityEditor.XR.ARCore.ARCoreSettings.ignoreGradleVersion).
- Added support for [session recording and playback](xref:arcore-manual#recording-and-playback).

**Updated**

- The following properties have been deprecated:
  - `ArSession.IsNull` => Compare to `null` instead.
  - `ArSession.Null` => Use [`default`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) instead.
  - `ArConfig.IsNull` => Compare to `null` instead.
  - `ArConfig.Null` => Use [`default`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) instead.
  - `ArCameraConfig.IsNull` => Compare to `null` instead.
  - `ArCameraConfig.Null` => Use [`default`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) instead.
  - `ArCameraConfigFilter.IsNull` => Compare to `null` instead.
  - `ArCameraConfigFilter.Null` => Use [`default`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) instead.
- Updated [XR Plug-in Management](https://docs.unity3d.com/Packages/com.unity.xr.management@4.0) dependency to 4.0.
- Updated [known limitations](xref:arcore-manual#known-limitations) to clarify depth image support behavior.

**Fixed**

- Improved handling for spaces in pathnames when building the reference image library which may cause the build to fail.
- You can now use reference images that are located outside the `Assets` folder.
- Fixed an issue where the [plane detection mode](xref:UnityEngine.XR.ARSubsystems.PlaneDetectionMode) was reported incorrectly. The [requestedPlaneDetectionMode](xref:UnityEngine.XR.ARSubsystems.XRPlaneSubsystem.requestedPlaneDetectionMode) and [currentPlaneDetectionMode](xref:UnityEngine.XR.ARSubsystems.XRPlaneSubsystem.currentPlaneDetectionMode) incorrectly reported that vertical plane detection was enabled or requested when only horizontal plane detection enabled/requested.
- Excluded tests from scripting API docs.

For a full list of changes and updates in this version, see the [ARCore XR Plug-in package changelog](xref:arcore-changelog).
