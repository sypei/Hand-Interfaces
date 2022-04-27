---
uid: arcore-upgrade-guide
---
# Upgrading to ARCore XR Plug-in version 4.2

To upgrade to ARCore XR Plug-in package version 4.2, you need to do the following:

- Update handling of null native ARCore objects.
- Use Unity 2020.3 or newer.

**Update handling of null native ARCore objects**

- The following properties have been deprecated:
  - `ArSession.IsNull` => Compare to `null` instead.
  - `ArSession.Null` => Use [`default`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) instead.
  - `ArConfig.IsNull` => Compare to `null` instead.
  - `ArConfig.Null` => Use [`default`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) instead.
  - `ArCameraConfig.IsNull` => Compare to `null` instead.
  - `ArCameraConfig.Null` => Use [`default`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) instead.
  - `ArCameraConfigFilter.IsNull` => Compare to `null` instead.
  - `ArCameraConfigFilter.Null` => Use [`default`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) instead.

**Use Unity 2020.3 or newer**

This version of the package requires Unity 2020.3 or newer.
