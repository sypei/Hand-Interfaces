# ARCore XR SDK Package

The purpose of this package is to provide ARCore XR Support. This package provides the necessary sdk libraries for users to build Applications that work with ARCore on Android.

# Building From Source

:warning: Be sure you have installed [Git Large File Support](https://git-lfs.github.com/) before cloning this repository.

:warning: The first time you build requires an internet connection and you must be on Unity's network (or VPN).

From a terminal window:

1. Clone this repo:
```
git clone git@github.cds.internal.unity3d.com:unity/arfoundation.git
```
2. Get the submodules:
```
git submodule update --init --recursive
```
3. Change directory to `com.unity.xr.arcore/Source~`:
```
cd com.unity.xr.arcore/Source~
```
4. Build the source using `bee`.
  - On Windows: ```bee.exe```
  - On Mac: ```mono bee.exe```
