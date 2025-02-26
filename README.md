# ZeroFramework

用于支持Unity项目快速开发，基于[GameFramework](https://github.com/EllanJiang/GameFramework)理念，融合自有需求，构建更易用的工具合集。

Used to support the rapid development of Unity projects, based on the GameFramework concept, the integration of their own ideas, to build a collection of easy-to-use tools.

## Base Functions

框架中包含的基础功能，该部分功能在项目的基础包内，不可移除修改。

The basic functionality contained in the framework, which is in the base package of the project and cannot be removed.

Todo部分为正在开发，或待添加内容，如果需要使用则切换到preview分支。

The Todo section is under development, or to be added, and switches to the preview branch if needed.

## Custom Packages

可根据项目需求自定义移除添加。不过要注意的是部分组件包会依赖其他组件包，可以查看组件包中的README文档。

Custom removal can be added according to project requirements. Note, that some package depend on other packages; you can see the README documentation in the package.

* com.Config
* com.Debugger
* com.Localization
* com.Resources
* com.Scenes
* com.Setting
* com.Sound
* com.Timer(https://github.com/akbiggs/UnityTimer.git)
* com.UIFramework

## Built-in Plugins

框架内已经包含的第三方插件。

Third party plugins already included in the framework.

* UniTask(https://github.com/Cysharp/UniTask)
* SharpZipLib(https://github.com/icsharpcode/SharpZipLib)

## Pro-install

正确运行框架需要在项目中引入以下库，为必须安装。部分需要安装的库也会在组件包的README文档中说明，也需要下载引入到项目中。

Running the framework correctly requires the introduction of the following libraries in your project, which must be installed. Some of the libraries that need to be installed are also described in the README documentation of the component package and will also need to be downloaded into the project.

* HybridCLR(https://hybridclr.doc.code-philosophy.com/)
* YooAsset(https://www.yooasset.com/)