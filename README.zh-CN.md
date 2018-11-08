[![nuget][nuget-badge]][nuget-url]
[![stable](https://img.shields.io/badge/stable-stable-green.svg)](https://github.com/loamen/KeyMouseHook/) 
[![build](https://img.shields.io/shippable/5444c5ecb904a4b21567b0ff.svg)](https://travis-ci.org/loamen/KeyMouseHook)
[![license](https://img.shields.io/badge/license-MIT-red.svg?style=flat)](https://raw.githubusercontent.com/loamen/KeyMouseHook/master/LICENSE)
[![platforms](https://img.shields.io/badge/platform-Windows-yellow.svg?style=flat)]()
<a target="_blank" href="https://shang.qq.com/wpa/qunwpa?idkey=419cea0774ab1aa37ae1a35eb0292482f9d8aa8decbab52eb62d9c5aa92c9c13"><img border="0" src="https://pub.idqqimg.com/wpa/images/group.png" alt="龙门信息①" title="龙门信息①"></a>

[nuget-badge]: https://img.shields.io/badge/nuget-v1.0.0-blue.svg
[nuget-url]: https://www.nuget.org/packages/KeyMouseHook
[source-url]: https://github.com/loamen/KeyMouseHook

![c#(winform)模拟键盘按键和鼠标点击操作类库](documents/images/keyboard-mouse-hook-logo.png)

<a href="README.md" target="_blank">English</a> <br/>

## 简介

这是一个基于[globalmousekeyhook][MouseKeyHook-Url] 和 [InputSimulator][InputSimulator-Url] 的类似于按键精灵的模拟键盘按键和鼠标点击操作的扩展类库。可以检测并记录键盘和鼠标的活动，你可以录制你的键鼠操作的记录并进行回放，可模拟键盘输入和鼠标点击操作。
## 环境

* **Windows:** .Net Framework 4.6+

## 安装和源码

<pre>
  nuget install KeyMouseHook
</pre>

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

public void StartWatch(IKeyboardMouseEvents events = null)
{
    _macroEvents = new List<MacroEvent>();
    keyboardWatcher.Start(events);
    mouseWatcher.Start(events);
}

public void StopWatch()
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

(源码里包含更详细的示例)

## 界面

![c#(winform)模拟键盘按键和鼠标点击操作类库](documents/images/screen-shots.png)

## 鸣谢

* [globalmousekeyhook][MouseKeyHook-Url] (MIT License)
* [InputSimulator][InputSimulator-Url] (MIT License)

[MouseKeyHook-Url]: https://github.com/gmamaladze/globalmousekeyhook "MouseKeyHook"
[InputSimulator-Url]: https://github.com/michaelnoonan/inputsimulator "InputSimulator"

## License

The MIT license see: [LICENSE](LICENSE)
