# Unity-Engine-Tools

![](https://img.shields.io/github/stars/Framvik/Unity-Engine-Tools) ![](https://img.shields.io/github/forks/Framvik/Unity-Engine-Tools) ![](https://img.shields.io/github/release/Framvik/Unity-Engine-Tools) ![](https://img.shields.io/github/issues/Framvik/Unity-Engine-Tools)

This project contains utility scripts and functionality based on the Unity engine and editor namespaces. It is collected inside a Package that can be added to any Unity project via the Package Manager.

## Installing The Package

This package references other git packages using the com.framvik.git-asset-deps package found at: 

https://github.com/Framvik/Unity-Git-Asset-Registry

Make sure to install it first by adding the following URL using the menu inside the Package Manager menu `+ > Add package from git URL`.

```
https://github.com/Framvik/Unity-Git-Asset-Registry.git?path=/Packages/com.framvik.git-asset-deps
```

Then you can simply add the following URL in the same way.

```
https://github.com/Framvik/Unity-Engine-Tools.git?path=/Packages/com.framvik.engine-tools
```

You may specify a specific version to add by adding `#v.x.x.x` at the end. <br>
ex:

```
https://github.com/Framvik/Unity-Engine-Tools.git?path=/Packages/com.framvik.engine-tools#v1.0.0
```