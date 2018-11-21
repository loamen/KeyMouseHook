[![nuget][nuget-badge]][nuget-url]
[![stable](https://img.shields.io/badge/stable-stable-green.svg)](https://github.com/loamen/KeyMouseHook/) 
[![build](https://img.shields.io/shippable/5444c5ecb904a4b21567b0ff.svg)](https://travis-ci.org/loamen/KeyMouseHook)
[![license](https://img.shields.io/badge/license-MIT-red.svg?style=flat)](https://raw.githubusercontent.com/loamen/KeyMouseHook/master/LICENSE)
[![platforms](https://img.shields.io/badge/platform-Windows-yellow.svg?style=flat)]()
[![download_count](https://img.shields.io/github/downloads/loamen/KeyMouseHook/total.svg?style=plastic)](https://github.com/loamen/KeyMouseHook/releases) 
[![release](https://img.shields.io/github/release/loamen/KeyMouseHook.svg?style=flat)](https://github.com/loamen/KeyMouseHook/releases) 
<a target="_blank" href="https://shang.qq.com/wpa/qunwpa?idkey=419cea0774ab1aa37ae1a35eb0292482f9d8aa8decbab52eb62d9c5aa92c9c13"><img border="0" src="https://pub.idqqimg.com/wpa/images/group.png" alt="龙门信息①" title="龙门信息①"></a>

[nuget-badge]: https://img.shields.io/badge/nuget-v1.0.5-blue.svg
[nuget-url]: https://www.nuget.org/packages/KeyMouseHook
[source-url]: https://github.com/loamen/KeyMouseHook
[mousekeyhook-url]: https://github.com/gmamaladze/globalmousekeyhook
[inputsimulator-url]: https://github.com/michaelnoonan/inputsimulator
[readme-url]: https://github.com/loamen/KeyMouseHook/blob/master/README.md

![c#(winform)模拟键盘按键和鼠标点击操作类库](https://github.com/loamen/KeyMouseHook/raw/master/documents/images/keyboard-mouse-hook-logo.png)

[English][readme-url]

## 简介

这是一个基于[globalmousekeyhook][mousekeyhook-url] 和 [InputSimulator][inputsimulator-url] 的类似于按键精灵的模拟键盘按键和鼠标点击操作的扩展类库。可以检测并记录键盘和鼠标的活动，你可以录制你的键鼠操作的记录并进行回放，可模拟键盘输入和鼠标点击操作。
## 环境

* **Windows:** .Net Framework 4.0+

## 安装和源码

> nuget install KeyMouseHook

* [NuGet package][nuget-url]
* [Source code][source-url]

## 使用

```csharp
private readonly KeyMouseFactory eventHookFactory = new KeyMouseFactory(HookType.GlobalEvents);
private readonly KeyboardWatcher keyboardWatcher;
private readonly MouseWatcher mouseWatcher;
private List<MouseKeyEvent> _mouseKeyEvents;

public FormMain()
{
   InitializeComponent();

   keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
   keyboardWatcher.OnKeyboardInput += (s, e) =>
   {
	if (_mouseKeyEvents != null)
	    _mouseKeyEvents.Add(e);
   };

   mouseWatcher = eventHookFactory.GetMouseWatcher();
   mouseWatcher.OnMouseInput += (s, e) =>
   {
	if (_mouseKeyEvents != null)
	    _mouseKeyEvents.Add(e);
   };
}

private void StartWatch(IKeyboardMouseEvents events = null)
{
    _macroEvents = new List<MacroEvent>();
    keyboardWatcher.Start(events);
    mouseWatcher.Start(events);
}

private void StopWatch()
{
   keyboardWatcher.Stop();
   mouseWatcher.Stop();
}

private void Playback()
{
   var sim = new InputSimulator();
   //var sim = new KeyMouseSimulator();
   sim.PlayBack(_macroEvents);
}
```

```csharp
keyboardWatcher = eventHookFactory.GetKeyboardWatcher().Disable(MacroEventType.KeyDown | MacroEventType.KeyUp).Enable(MacroEventType.KeyPress);
mouseWatcher = eventHookFactory.GetMouseWatcher().Enable(MacroEventType.MouseDoubleClick | MacroEventType.MouseDragStarted).Disable(MacroEventType.MouseDragFinished | MacroEventType.MouseMove);
var sim = new InputSimulator().Enable(MacroEventType.MouseDoubleClick | MacroEventType.KeyPress).Disable(MacroEventType.MouseMove | MacroEventType.KeyDown | MacroEventType.KeyUp);
```


(源码里包含更详细的示例)

## 界面

![c#(winform)模拟键盘按键和鼠标点击操作类库](https://github.com/loamen/KeyMouseHook/raw/master/documents/images/screen-shots.png)

## 鸣谢

* [globalmousekeyhook][mousekeyhook-url] (MIT License)
* [InputSimulator][inputsimulator-url] (MIT License)

## 贡献代码

 - Fork并克隆到本机
 - 创建一个分支并添加你的代码
 - 发送一个Pull Request

## License

The MIT license see: [LICENSE](https://github.com/loamen/KeyMouseHook/blob/master/LICENSE)
