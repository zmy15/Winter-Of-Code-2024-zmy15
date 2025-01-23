# SAST C# Group Winter Of Code | Getting Started

## 环境配置

- **<u>操作系统:</u>**Windows 11 / Windows 10 20H2 or newer ( >10.0.19041.0 ) ( x64 or ARM64 )
  - 不支持其它操作系统，并且极大概率不能通过 Wine 及其它模拟层运行
- **<u>IDE:</u>** Visual Studio 2022 17.12 or newer
  - Rider 对 WinUI 3 项目支持很差，可能会出现 **无法调试/运行、XAML代码补全/语法检查不可用** 等问题。
  - Visual Studio Code 支持比 Rider 更烂。
- **<u>Visual Studio 工作负载:</u>**保证勾选`Windows 应用程序开发`。如果你的设备尚未安装Git for Windows，可以在 单个组件 中找到它。

## 获取代码

1. Fork [NJUPT-SAST-Csharp/Winter-Of-Code-2024](https://github.com/NJUPT-SAST-Csharp/Winter-Of-Code-2024) 仓库到你的账户中。

   > [!WARNING]
   >
   > 如果在2025/1/24以前已经Fork过了，请在你Fork的仓库中点击上方的Sync Fork按钮，更新分支。
   >
   > 我们发布了一些关于后端连接的 bugfix

   ![image](https://github.com/user-attachments/assets/c25ee4ac-d333-46c5-8121-f015cd8a04c5)

2. 以你喜欢的方式Clone你Fork后的新仓库。**请不要使用 Download ZIP！**

   - 命令行`git clone **从仓库页面>Code>Clone>HTTPS或者SSH栏复制的链接**`

     会将仓库克隆至工作目录下的`Winter-Of-Code-2024`

   - GitHub Desktop / Visual Studio 2022

   - GitHub CLI

   **在Clone前请确认仓库地址是你Fork后的新仓库**，而不是`NJUPT-SAST-Csharp/Winter-Of-Code-2024`

   此步骤很可能需要你有合理的网络加速手段，如果可以，**请将代理软件的TUN模式/通透代理模式打开**，或者去学习**为Git设置代理**的正确姿势。

3. 打开仓库文件夹，找到`Winter-Of-Code-2024.sln`，使用Visual Studio 2022即可打开项目。

## 编译、部署&运行

打开项目后，确认启动项目为 `SastImg.Client (Package)` 。（如果出现无法启动的问题，那么很有可能你选择了错误的启动项目）

按下`F5`即可编译+部署+调试，初次启动可能会花费很长时间还原 NuGet 包（大约会在硬盘上存储数百MB的包缓存），并且没有任何进度条或状态提示，请耐心等待。还原 NuGet 包只会经历一次，在此后的开发过程中，你通常不会再经历这一可能漫长的过程。根据组内其它人测试，此步骤不需要任何网络加速手段即可顺利完成（多亏了微软在境内部署的镜像服务器）。

不出意外的话，你将能看到只有基础代码的程序启动，以及它的主界面：

![image](https://github.com/user-attachments/assets/7db080d2-e0b5-4d18-8091-2eb42622b025)

在右上角的用户头像处可以登录，鼠标在上方悬浮即可展开面板。目前创建了两个用户：`test` 密码`123456` 和管理员权限的用户：`admin` 密码`123456`。显示登录成功Dialog就表明我部署在Azure上的服务还没欠费（）

## 关于SAST Image

SAST Image 是一套图库系统，由前任组长完成（虽然没有完全完工，这次或许可以当作集成测试（？），所以出现任何调用API的异常欢迎询问我或其它组员）。

你可以访问 [NJUPT-SAST-Csharp/SAST-Image-Re: Continuation of SAST-Image, which is combination of SastImg Frontend and Backend.](https://github.com/NJUPT-SAST-Csharp/SAST-Image-Re) 并给它一个可爱的Star～(∠・ω< )⌒★。本次项目所使用的小幅修改版后端位于该仓库的`Shirasagi0012/Winter-Of-Code`分支下。

### 功能

- 用户相关：
  - 登录&注册，修改密码&用户名
  - 用户头像、用户主页头图、用户简介
- 图片管理相关： 按照 分类 -> 相册 -> 图片 的分类管理。并且每个图片可以拥有一个/多个标签。
  - 分类 Category：一个分类由名字和描述组成。
    - （**仅限管理员**）创建分类，修改分类的名字和描述
    - 获取全部分类
  - 相册 Album：一个相册由标题、描述、创建者的用户ID、所属的分类和其它人的访问权限构成。
    - 删除与恢复最近删除的相册
    - 修改访问权限，标题，描述，协作者，封面等
    - 获取一个分类下有哪些相册
    - 创建相册
  - 图片 Image：一个图片由图像（原图和自动生成的缩略图）、标题、标签、点赞数、点赞者组成
    - 向相册添加图片，删除图片与恢复最近删除的图片
    - 下载图片原图或缩略图
    - 获取一个相册里有哪些图片
    - 点赞和取消点赞
  - 标签 Tag：一个标签由它的名字组成
    - 创建，删除标签，修改一个标签的名字

其中，不同的分类、相册、图片、标签各自使用唯一存在的数字ID标识（是一个`long`型的数字）。

### WebAPI

这些功能均已实现在后端中，提供了一套REST风格的WebAPI以供客户端使用。本次Winter Of Code项目的SAST Image桌面客户端正是通过调用后端提供的WebAPI来完成以上功能。本次WOC<u>**并不需要**你具有WebAPI甚至后端开发相关知识</u>，但是了解WebAPI是什么，以及Query、Body、Path分别是啥，如何调用WebAPI，对理解和解决某些bug有很大帮助。

在项目的Services\API文件夹中，有自动生成的后端WebAPI的本地调用接口。你可以访问 [SAST Image | Scalar API Reference](http://sastwoc2024.shirasagi.space:5265/scalar/SastImgApi) 以了解所有API，它们与在Services\API文件夹中接口内的函数一一对应，并且后者具有注释。

## 开始开发

### 需求

**<u>尽可能多</u>**的在客户端中实现SAST Image后端提供的功能。当然，你需要优先着手完成其中更重要的功能：

#### 主要功能

- 图片浏览
  - 可以列出相册，并且能展示相册内的所有图片
- 上传图片
- 创建相册
- 修改相册和图片的描述，标签，分类

#### 一些细小的功能

- 注册账号
- 应用左上角的返回按钮现在不起作用，请编写代码实现返回功能
  - Tips: ShellPage 中的 TitleBar 有 BackButtonClick 事件。Frame 有自带的返回方法
- 右上角用户头像处还不能显示用户的头像，可以通过对 ExpandableUserAvatar 进行扩展实现。
- 查看和修改用户资料

通过考核并不需要你像个超人一样完成全部功能，我觉得把以上的功能实现得七七八八就非常好了（）

### Quick Start

在这一节，我将带着各位完成一个基础功能——获取指定ID的图片，并把它显示在界面上的`<Image>`控件里。这一部分更多的是帮助各位理解整个框架是怎么work的，而不是像主仓库那样的基础代码。

#### 准备工作

1. 按照前述说明，成功运行目前的基础框架代码。

2. 安装 [WinUI 3 Gallery | Microsoft Store](https://apps.microsoft.com/detail/9P3JFPWWDZRC?hl=en-us&gl=US&ocid=pdpshare) 和 [CommunityToolkit Gallery | Microsoft Store](https://apps.microsoft.com/detail/9NBLGGH4TLCQ?hl=en-us&gl=US&ocid=pdpshare)。前者提供了WinUI 3的部分设计规范和多数控件的demo以及对应的代码；后者是Windows Community Toolkit，一个社区开发的用于补充 UWP / WinUI / Uno 的工具包，的文档，为WinUI 3提供了一些重要但缺失的功能和控件。

3. 参考资料：微软编写的Windows应用开发文档。[开发 Windows 桌面应用 - Windows apps | Microsoft Learn](https://learn.microsoft.com/zh-cn/windows/apps/develop/)

   关于设计规范、控件用法等内容主要在“设计”章节内。在WinUI 3 Gallery应用中的控件demo页面可以跳转到文档中对应的页面。

#### 添加页面

##### 添加`NavigationViewItem`

目前左侧导航栏有三个按钮，主页，设置，和跳转到GitHub。在这一节中，我将演示如何向导航栏添加一个打开新页面的按钮。

导航栏的XAML在`ShellPage.xaml`中，和整个主界面大多数元素一样。`<NavigationView>`是导航栏的主体，你可以自行查询它的用法。导航栏的按钮分为上半部分的`MenuItems`和下半部分的`FooterMenuItems`。在`MenuItems`依葫芦画瓢添加一个`测试页面`的按钮：

```xaml
<NavigationView ...>
    <NavigationView.MenuItems>
        <NavigationViewItem ... />
        
        <NavigationViewItem
            Content="测试页面"
            Tag="Test">
            <NavigationViewItem.Icon>
                <FontIcon Glyph="&#xE789;" />
            </NavigationViewItem.Icon>
        </NavigationViewItem>
        
    </NavigationView.MenuItems>
    <NavigationView.FooterMenuItems ... />
    <Frame x:Name="MainFrame" />
</NavigationView>
```

对于图标，你可以在[WinUI 3 Gallery - Iconography Sample](winui3gallery://item/Iconography) 查看所有图标（如果你已经安装WinUI 3 Gallery，点击该链接可以直接跳转），右下角有图标对应的字形。

Tag是用来标识`NavigationView`中的每一个Item的，稍后会用上。它是`object`类型，可以放任何东西，这里为方便使用设置为"Test"字符串。

此时运行，应当可以看见左侧多了一个按钮。点击它界面不会有任何反应，也不会切换到新页面。接下来完成切换新页面的代码

##### 页面切换

首先得有一个新页面。创建一个叫`TestView`的新页面，以及它对应的ViewModel，并且为便于实现数据绑定，ViewModel需要继承自`ObservableObject`。你可以在[MVVM Toolkit Sample App | Microsoft Store](https://apps.microsoft.com/detail/9NKLCF1LVZ5H?hl=en-us&gl=US&ocid=pdpshare)中学习如何使用`CommunityToolkit.MVVM`工具包。

我们要在`NavigationView`中按下按钮时切换页面。这里已经监听了`NavigationView`的`ItemInvoked`事件，来到`ShellPage.xaml`的后台代码，依葫芦画瓢添加切换页面的逻辑。这里需要使用前面的Tag来区分选择的是哪个页面。

```csharp
private async void NavigationView_ItemInvoked (NavigationView sender, NavigationViewItemInvokedEventArgs args)
{
    if ( args.InvokedItemContainer is NavigationViewItem item )
    {
        switch ( item.Tag )
        {
     // ...
            case "Test":
                MainFrame.Navigate(typeof(TestView));
                break;
        }
    };
}
```

这样，点击按钮时，会使`MainFrame`切换到`TestView`。

##### 完成`TestView`

这个`TestView`将用于获取所有的图片的ID（通过`App.API.Image.GetImagesAsync()`这一方法），以供选择某一张图片，然后切换到展示图片的另一个页面（稍后完成）。因此，我们需要一个列表用于展示所有获取到的图片ID，可以使用`ListView`这一控件。

首先，我们要在ViewModel中实现获取图片列表的逻辑：

```csharp
public partial class TestViewModel : ObservableObject
{
    public ObservableCollection<ImageDto> Images { get; } = [];
    
    [ObservableProperty]
    private ImageDto selectedImage;

    public async Task<bool> GetAllImagesAsync()
    {
        Images.Clear();
        var imagesRequest = await App.API!.Image.GetImagesAsync(null,null,null);
        if (!imagesRequest.IsSuccessful) return false; // 如果获取失败，返回 false

        foreach (var image in imagesRequest.Content)
        {
            Images.Add(image);
        }
        return true;
    }
}
```

这里使用 `ObservableCollection`，以在更新列表的时候提供更新通知。由`GetImagesAsync`产生的请求的Content为`ImageDto`类型，当光标在`var`处时按F12可以快速跳转至定义。

接着完成界面。这里只给出核心的`<ListView>`部分。记得在后台代码new一个ViewModel并且作为属性暴露出来。

```xaml
<ListView ItemsSource="{x:Bind ViewModel.Images}" SelectedItem="{x:Bind ViewModel.SelectedImage, Mode=TwoWay}">
    <ListView.ItemTemplate>
        <DataTemplate xmlns:api="using:SastImg.Client.Service.API" x:DataType="api:ImageDto">
            <StackPanel>
                <TextBlock Text="{x:Bind Title}" Style="{ThemeResource SubtitleTextBlockStyle}"/>
                <TextBlock Text="{x:Bind UploadedAt}"/>
                <TextBlock Text="{x:Bind Id}"/>
            </StackPanel>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

这时打开程序，切换到测试页面，应该就能看见获取的图片列表了。（可能需要登录）

花了20分钟稍微完善一下（留给你们了）：

![image](https://github.com/user-attachments/assets/a9d9f843-cd96-4f1a-9014-9c9b5849f7fc)

#### 显示图片

在获取到图片ID后，接下来我们要把它显示出来，可以使用[`<Image>`控件](winui3gallery://item/Image)。

在一个新页面`ImageView.xaml`里面展示图片：

```xaml
<Image x:Name="img" Stretch="Uniform"
       HorizontalAlignment="Stretch" VerticalAlignment="Stretch"/>
```

接下来完成它的ViewModel。它的ViewModel需要实现从后端取得需要的图片。使用一个可观察属性存储图像数据，还有一个获取指定ID图片的方法，这个方法使用`GetImageAsync`获取图片的原图(kind参数为0)或缩略图（为1）。

```csharp
[ObservableProperty]
private Byte[]? imageData;

public async Task<bool> ShowImageAsync(long id)
{
    var imageResponse = await App.API?.Image.GetImageAsync(id, 0);
    if (!imageResponse.IsSuccessful) return false;

    using var m = new MemoryStream();
    await imageResponse.Content.CopyToAsync(m);
    ImageData = m.ToArray();
    return true;
}
```

在后台代码中，我们要实现当ViewModel里的图像数据更新时，将`<Image>`更新为这个图像。那么怎样触发更新呢？之前我们使用`x:Bind`，会自动使用`ObservableProperty`的更新机制，现在我们需要手动利用它。很简单，只需要注册`ObservableObject`的事件`PropertyChanged`即可（这就是为什么ViewModel通常继承自`ObservableObject`，或者使用`CommunityToolkit.Mvvm`的`[ObservableObject]`特性激活源生成器生成对应代码。）

在构造函数中注册一个异步事件处理函数：

```csharp
ViewModel.PropertyChanged += async (sender, e) =>
{
    if (e.PropertyName == nameof(ViewModel.ImageData)) // 如果属性的名字是“ImageData”
    {
        await UpdateImageAsync();
    }
};
```

其中的`UpdateImageAsync`是用来将`Byte[] ImageData`解码为`Image`能接受的`BitmapImage`，并且设置`Image`的图片。

```csharp
private async Task UpdateImageAsync()
{
    if (ViewModel.ImageData is null)
    {
        img.Source = null;
        return;
    }
    var s = new MemoryStream(ViewModel.ImageData);
    var bitmap = new BitmapImage();
    await bitmap.SetSourceAsync(s.AsRandomAccessStream());
    img.Source = bitmap;
}
```

---

目前，我们还没有让这个页面知道应该显示哪张图片。我在这里会实现从`TestView`的列表里选择一张图，然后自动跳转至`ImageView`，显示该ID的图片。为此，我们需要实现这个ID在两个页面之间的传递。目前，这些页面都处在一个`Frame`中，而`Frame`本身就提供了这一功能。`Page`有几个函数：

- `protected virtual void OnNavigatedTo(NavigationEventArgs e)` : 在页面卸载并且不再是`Frame`的当前页面之后立即调用。
- `protected virtual void OnNavigatedFrom(NavigationEventArgs e)` : 在页面被加载并且成为`Frame`的当前页面时立即调用。
- `protected virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)` : 在页面卸载并且不再是`Frame`的当前页面**前一刻**调用。
- 没有`OnNavigatingTo`

`Frame`的`Navigate`函数（前面用过）支持传递参数，这个参数将会保存在`NavigationEventArgs`的`Parameter`属性里，可以通过重写以上三个方法获取。

为了在切换到`ImageView`时获取接下来要传递的ID，我们需要重写`ImageView`的`OnNavigatedTo`方法：

```csharp
protected override async void OnNavigatedTo(NavigationEventArgs e)
{
    if(e.Parameter is ImageModel model)
    {
        var isSuccess = await ViewModel.ShowImageAsync(model.Id);
    }
}
```

#### 导航到新页面

完成了页面的编码，接下来要实现导航到这个新页面。由于需要传递参数，很显然不能以导航栏的按钮为入口。我们以点击`ListView`中展示的图片信息为入口跳转到`ImageView`，使用`Frame.Navigate`方法。

##### 获取`Frame`

首先我们需要拿到`ShellPage`里的`Frame`，会稍微有些麻烦。一种解决办法是在App类中添加一个静态的ShellPage引用：

```csharp
public static ShellPage? Shell;
```

然后在创建窗口时，使用这个ShellPage：

```csharp
Shell = new ShellPage();
MainWindow = new Window()
{
    SystemBackdrop = new MicaBackdrop(),
    Title = "SAST Image",
    Content = Shell
};
```

在XAML里定义的对象默认是private的，我们需要把`MainFrame`变成public或internal的：

在`ShellPage.xaml`中找到MainFrame，修改它：

```xaml
<Frame x:Name="MainFrame" x:FieldModifier="public" />
```

接下来就可以使用`App.Shell.MainFrame`访问它了。

##### 导航

使用ListView的ItemClick事件即可，很简单：

```csharp
private void image_list_ItemClick(object sender, ItemClickEventArgs e)
{
    if (e.ClickedItem is not ImageDto c) return; // 点击的对象是ImageDto的话（因为ClickItem是object类型的，
                 // 为保险需要提前判断一下，并把它转换成ImageDto c）
    App.Shell.MainFrame.Navigate(typeof(ImageView), c.Id); // 除了页面类型，还可以传递参数！
}
```

还需要设置ListView的`IsItemClickEnabled`为True。

```xaml
<ListView ... x:Name="image_list" ItemClick="image_list_ItemClick" IsItemClickEnabled="True" ... />
```

#### 返回原来的页面

进入`ImageView`后，暂时没有办法返回原来的页面（左上角那个返回按钮的逻辑没有完成，交给你们了）。`Frame`提供了`GoBack`方法，可以返回上一个页面。我们可以在`ImageView`中添加一个按钮，实现返回。

```xaml
<Grid>
    <Button Width="48" Height="48" Margin="24" Canvas.ZIndex="3"
            VerticalAlignment="Top" HorizontalAlignment="Left"
            Click="Button_Click">
			Back
    </Button>
    <Image x:Name="img" Stretch="Uniform" Canvas.ZIndex="1"
                   HorizontalAlignment="Stretch" VerticalAlignment="Stretch"/>
</Grid>
```

这个按钮被放置在左上角，设置了外边距以使其稍远离边缘。接着设置它的点击事件：

```csharp
private void Button_Click(object sender, RoutedEventArgs e)
{
    if(App.Shell.MainFrame.CanGoBack) // 先判断能否返回
    {
        App.Shell.MainFrame.GoBack(); 
    }
}
```

花了20分钟稍微完善一下（留给你们了）：

![image](https://github.com/user-attachments/assets/c547e061-1306-463b-b131-4a455df7e94a)

### 调试

到此，我已经带着各位完成了一些基础功能，这些代码会在我自己Fork的仓库中放着[Shirasagi0012/Winter-Of-Code-2024: The base code of NJUPT SAST C# Group's WOC 2024](https://github.com/Shirasagi0012/Winter-Of-Code-2024)。<u>这些代码只是供你参考，而非是让你基于此继续完成。</u>当然，你可以自由使用这些代码。

有一些调试的技巧：

1. **善用断点**

   比如网络请求有时会挂掉，导致报错，你可以在判断请求是否完成的地方加一个断点，然后观察变量的值：

   ![image](https://github.com/user-attachments/assets/31ffecec-9744-4064-8f7c-13ea4ce6c92e)

   对于`IApiResponse`（也就是所有请求的返回类型)，它如果出错了，可以在其Error属性中看见具体的错误，便于你排除错误。

   如果你嫌断点太多，总是中断，可以为断点设置条件：

   ![image](https://github.com/user-attachments/assets/83bd5cd9-12d0-40a5-b81f-d3aa8a62992c)

   右键一个断点即可为其设置条件，比如这个断点我设置了只有当IsSuccessful为false时中断，便于查看错误信息。

2. **获取某些含糊不清的报错的真相**

   由于WinUI 3作为Windows平台原生的UI框架，它是非托管的，应用通过WinRT API来使用它。在编程时不大能感知到这一点，因为CsWinRT等部件已经为我们完成了所有相关的代码生成和自动封送等，让你能拿到的所有东西都是托管世界中的，但是当报错发生时就不一定了（笑）。

   如果WinUI 3框架调用了你的代码（比如XAML里声明一个元素，框架去调用该对象的构造函数），而你的代码抛出了异常，这个异常会先被封送到非托管代码中，然后等它被你的程序捕获时，很可能只有一些看不懂的信息（包括但不限于明明有调试器但是程序还是崩溃、COMException、诡异的HResult代码等）。有一些有效的方法可以让你获得真正的错误信息：

   - 打开“输出”窗口。`Ctrl+Alt+O`

   - 开启First Chance Exception，在异常发生的第一时刻就捕获它，而不是等它去非托管世界绕一圈再回来，指望框架保留了原来的报错信息。但是这么做会导致程序调试时因异常而频繁暂停，因此只在需要时启用它/设置好条件。

   - 启用混合模式调试。在调试设置中开启Enable Native Code Debugging。然后调试时观察输出窗口，可能会多一些报错信息。

     ![image](https://github.com/user-attachments/assets/f41e59d4-97e2-4a93-9033-a63490add6b6)

     ![image](https://github.com/user-attachments/assets/a0493230-d2f7-434a-9df0-e3568343fcae)

     可以看见调试时输出窗口多了一些消息。在非UI线程操作UI界面产生的报错也会在这里显示（开启混合模式调试后）。

     > [!WARNING]
     >
     > 开启混合模式调试会导致热重载失效

   - 如果你发现报错发生在这里：
     
     ![image](https://github.com/user-attachments/assets/ca01b708-bb3e-4583-ad4d-e468eccdaa1c)

     很懵逼，不是吗？其实只是它没把错误给你抛出来……。查看`e`参数的`Exception`属性，这就是它原本的报错信息。

     想要在错误发生处单步调试/还原现场？

     - 开启First Chance Exception

     - 阅读报错堆栈，找到调用者的代码在哪里，然后加个断点

       ![image](https://github.com/user-attachments/assets/2047bb4e-b253-4c14-adae-4b44af1667ae)

       比如这个`HttpIOException`，它实际上是在`ShowImageAsyc`函数里发生的，你可以去那里加个try catch然后打个断点，查明发生错误的原因。

     不得不说能把原本的报错信息藏着也是🍬了（其实输出窗口里面还是会把信息打印出来的）

   当然，报错模糊是WinUI 3框架本身的问题，不得不说相比去年同期还是有了不少改善的（去年WOC如果上WinUI 3的话，我大概就要在debug中亖了😇）。

## 结语

WinUI 3 相关资料可能比较匮乏，主要依靠微软爸爸的文档。其实80%以上的 UWP 资料也适用于 WinUI 3，搜索不到可以换个关键词试试。

希望这篇因个人原因咕了几天的 Getting Started 能有一些帮助。

如果在开发过程中遇到诡异的Bug，可联系SAST C#组全体成员，可以优先找我（话说回来24号了还在给基础代码仓库推BugFix，接下来真的不会再出大问题吗）。

总之，Good luck & Have fun！

Shirasagi
