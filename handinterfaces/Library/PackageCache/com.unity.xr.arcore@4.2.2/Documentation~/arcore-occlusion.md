---
uid: arcore-occlusion
---
# Occlusion

ARCore provides support for occlusion based on environment depth images it generates every frame. These depth images provide a distance (in meters) from the device to any part of the environment in the camera field of view.

The [XROcclusionSubsystem](xref:UnityEngine.XR.ARSubsystems.XROcclusionSubsystem) provides access to two types of environment depth: [raw](xref:UnityEngine.XR.ARSubsystems.XROcclusionSubsystem.TryAcquireRawEnvironmentDepthCpuImage(UnityEngine.XR.ARSubsystems.XRCpuImage@)) and [smoothed](xref:UnityEngine.XR.ARSubsystems.XROcclusionSubsystem.TryAcquireSmoothedEnvironmentDepthCpuImage(UnityEngine.XR.ARSubsystems.XRCpuImage@)).

- **Raw**: The raw, unsmoothed depth data. This is useful for custom processing where raw data is necessary and corresponds to ARCore's [ArFrame_acquireRawDepthImage](https://developers.google.com/ar/reference/c/group/ar-frame#arframe_acquirerawdepthimage) function.
- **Smoothed**: Depth data with smoothing applied. This is useful when performing occlusion and corresponds to ARCore's [ArFrame_acquireDepthImage](https://developers.google.com/ar/reference/c/group/ar-frame#arframe_acquiredepthimage) function.

> [!NOTE]
> Both raw and smoothed are always available if depth is supported, regardless of the value of [environmentDepthTemporalSmoothingEnabled](xref:UnityEngine.XR.ARSubsystems.XROcclusionSubsystem.environmentDepthTemporalSmoothingEnabled).

## Requirements

Environment depth requires a device with depth support. See [ARCore supported devices](https://developers.google.com/ar/devices) for a list of devices that support depth ("Supports Depth API" in the Comments column).
