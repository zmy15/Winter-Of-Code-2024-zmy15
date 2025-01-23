# 南邮校科协 C#组 Winter Of Code 2024 基础代码仓库

你可以以该仓库的代码为起点开始 Winter Of Code 2024 项目。

## 食用方法

Fork 该仓库，新仓库的拥有者可以是你自己的账号，也可以放在 C#组 GitHub 组织下。

> [!NOTE]
> 如果你将 Fork 后的仓库置于**组织账号**下，请按如下格式命名：
> `Winter-Of-Code-2024-你的GitHub用户名`

然后在 Fork 出来的仓库中提交代码即可。

目前创建了两个用户，一个是普通用户`test`，密码为`123456`。一个是管理员`admin`，密码为`123456`。

[Getting Started](https://github.com/NJUPT-SAST-Csharp/Winter-Of-Code-2024/blob/main/Getting%20Started.md)

## 项目结构介绍

目前的项目中有多个文件夹:

- Assets : 一些图标，如应用的 logo。
- Controls : 自定义控件/用户控件。
  - ExpandableUserAvatar : 用于右上角鼠标悬浮在上方可以展开的头像
  - IconButton : 用于右上角头像展开后的卡片中的带有图标的按钮
- Helpers : 帮助类，用于使某些代码更直观
- Services : 应用服务类
  - SastImgAPI : 与后端交互的接口
  - AuthService : 用于管理登录的服务
  - API 文件夹 : 包含具体的接口定义。所有需要的接口，如列出图片，获取图片，上传，修改信息等等，都在这里。
- Themes : 存放控件的资源字典
- Views : 页面和 ViewModels
  - HomeView : 首页，目前是空的
  - SettingsView : 设置，目前是空的
  - ShellPage : 整个应用的框架。左边的导航栏，顶部的标题栏都在这里。其中包含一个 Frame，用于展示其它页面

## 你需要完成什么？

### 主要功能

- 图片浏览
  - 可以列出相册，并且能展示相册内的所有图片
- 上传图片
- 创建相册
- 修改相册和图片的描述，标签，分类

### 一些细小的功能

- 注册账号
- 应用左上角的返回按钮现在不起作用，请编写代码实现返回功能
  - Tips: ShellPage 中的 TitleBar 有 BackButtonClick 事件。Frame 有自带的返回方法
- 右上角用户头像处还不能显示用户的头像，可以通过对 ExpandableUserAvatar 进行扩展实现。
- 查看和修改用户资料
