using System;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// The deprecated ARCore implementation of the <c>XRImageTrackingSubsystem</c>. Do not use this. Use the <c>ARCoreImageTrackingSubsystem</c> instead.
    /// </summary>
    [Obsolete("ARCoreImageTrackingProvider has been deprecated. Use ARCoreImageTrackingSubsystem instead (UnityUpgradable) -> UnityEngine.XR.ARCore.ARCoreImageTrackingSubsystem", true)]
    public sealed class ARCoreImageTrackingProvider : XRImageTrackingSubsystem
    {
    }
}
