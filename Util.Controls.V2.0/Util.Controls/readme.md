>Util.Controls 常用操作说明：
*** 

## 基础常用
* xaml引用：`xmlns:utc="clr-namespace:Util.Controls;assembly=Util.Controls"`
* 常用静态转换器引用：Converter={x:Static xly:XConverter.TrueToFalseConverter}
* 自定义标题交互：shell:WindowChrome.IsHitTestVisibleInChrome="True"

## 附加属性相关
* 启用动画，很好玩的一个附件属性：utc:UtilProperty.AllowsAnimation="{Binding IsMouseOver,RelativeSource={RelativeSource Self}}"

## FIcon：字体图标
* 字体文件目前在Util.Controls，因为WPF中字体文件必须要内嵌到项目中，暂时选择在这个地方。
* 一般在TextBlock中使用：Style="{StaticResource FIcon}"

## FImage：同时支持图片资源和字体图表的图片控件
* 属性Source设置资源的数据源，支持FIcon图标，例如：&#xe64e;同FIcon使用方法一样；支持图片资源路径（相对路径、绝对路径，如\img\a.jpg）

## ThumbnailImage：缩略图
直接继承自Image的图片控件，支持多种数据源的缩略图显示，如本地图片、网络图片、视频等
* ThumbnailSource：缩略图数据源：文件物理路径，或者URL地址
* ThumbnailType：缩略图类型，默认Image图片
* AsyncEnable：是否启用异步加载，网络图片建议启用，本地图可以不需要。默认不起用异步
* CacheEnable：是否启用缓存,默认false不启用

## AnimatedGIF：可以播放Gif动画的图片控件
* GIFSource：设置gif图片资源路径，可以通过其提供的接口开启或停止动画。

## Button的样式及附加属性，主要是集成了字体图标
* DefaultButtonStyle：默认样式，不需要设置
* TransparencyButtonStyle：无背景的按钮
* LinkButtonStyle：link标签样式的按钮
* 字体图标相关可以通过附加属性设置，如utc:UtilProperty.FIcon，具体可以参考示例代码；
* 颜色、圆角等可以通过附加属性设置，具体可以参考示例代码；
* 可以通过附加属性FIconAnimationEnable启用图表的鼠标移入旋转动画，如：utc:UtilProperty.FIconAnimationEnable="True"

## DatePicker，日期选择框支持4种样式：
* DefaultDatePicker：默认样式，若要默认样式需要设置，一般在App.xaml中处理；
* ClearButtonDatePicker：带清除按钮的样式，支持自定义样式扩展
* LabelDatePicker：带Label标签的样式，支持自定义样式扩展
* LabelClearButtonDatePicker：同时支持Label标签和清除按钮的样式

## CheckBox，提供两种样式，又添加了一个不错的样式BulletCheckBoxStyle
* DefaultCheckBox：作为默认样式
* SimpleCheckBox：更简单轻量的样式，主要用于列表，没有内容
* BulletCheckBoxStyle：移动端很流行的像子弹的复选样式

## RadioButton，提供两种样式：
* DefaultRadioButton：作为默认样式
* BoxRadioButton，box形状的样式，可以参考使用示例

##TextBox，输入控件，和DatePicker类似提供多种样式，并且可以灵活扩展，具体可也参考代码示例
* DefaultTextBox：作为默认样式
* ClearButtonTextBox：带清除按钮的样式，支持自定义样式扩展
* LabelTextBox：带Label标签的样式，支持自定义样式扩展
* LabelClearButtonTextBox：同时支持Label标签和清除按钮的样式
* LabelOpenFileTextBox：集成选择文件的输入框，通过Tag设置文件过滤器，如Tag="文本文件(*.txt)|*.txt|所有文件(*.*)|*.*"
* LabelOpenFolderTextBox：集成选择文件夹的输入框
* LabelSaveFileTextBox：集成选择文件保存路径的输入框，通过Tag设置文件过滤器，如Tag="文本文件(*.txt)|*.txt|所有文件(*.*)|*.*"
* LabelStyle：像Label一样，只读但支持复制的文本样式

## RichTextBox，只有一个DefaultRichTextBox作为默认样式

## ToggleButton，提供两个样式	DefaultToggleButton,	FIconToggleButton

## ProgressBar，进度条样式，有两种样式方式：
* LoopProcessBar：环形进度样式，最大值必须为1，Value为实际进度值；
* SimpleProgressBar：简单条形样式

## Separator 分割线，提供垂直和水平两种样式
	VerticalSeparatorStyle,	HorizontalSeparatorStyle

## ComboBox，下拉选择输入框，其中ComboBoxItem默认为SimpleComboBoxItemStyle，只支持文本选项，可以扩展、重写该样式即可。如果需要自定义模板显示的使用DefaultComboBoxItemStyle
* DefaultComboBox：作为默认样式
* ClearButtonComboBox：带清除按钮的样式，支持自定义样式扩展
* LabelComboBox：带Label标签的样式，支持自定义样式扩展
* LabelClearButtonComboBox：同时支持Label标签和清除按钮的样式
* 绑定一个枚举类型：ItemsSource="{Binding Converter={x:Static utc:XConverter.EnumTypeToItemSourceConverter}, ConverterParameter={x:Type domain:EnumDataType}}"
                                             SelectedValuePath="Value" SelectedValue="{Binding DataType,UpdateSourceTrigger=PropertyChanged}"
* RadioBoxComboBox 把选项平铺出来的一种样式，单个项类似Radio的Box样式

## MultiComboBox，下拉多选输入框，选项样式可以重写。
* DefaultMultiComboBox：作为默认样式
* ClearButtonMultiComboBox：带清除按钮的样式，支持自定义样式扩展
* LabelMultiComboBox：带Label标签的样式，支持自定义样式扩展
* LabelClearButtonMultiComboBox：同时支持Label标签和清除按钮的样式

## PasswordBox，密码输入框样式。
* DefaultPasswordBox：作为默认样式
* ClearButtonPasswordBox：带清除按钮的样式，支持自定义样式扩展
* LabelPasswordBox：带Label标签的样式，支持自定义样式扩展
* LabelClearButtonPasswordBox：同时支持Label标签和清除按钮的样式

## Menu菜单相关样式
* FIconMenuItem：使用FIcon作为图标的菜单项样式；
* DefaultMenuItem：继承自FIconMenuItem，简单文本Header的菜单项，为默认MenuItem样式，可以用自定义扩展；
* TransparentHeaderMenuItem：主要作为一级菜单，背景透明的菜单项样式；
* DefaultMenu：默认菜单样式；
* DefaultContextMenu：默认右键菜单样式；
* TextBoxContextMenu：文本操作的右键菜单样式，已经内嵌入所有文本输入组件内了；

## TabControl控件样式，其中TabItem提供了两种样式，也可以自己定义
* DefaultTabControl：TabControl的默认样式，需要注意的亮点：Padding作为Header的偏移位置；Panel.ZIndex来控制头部分割线的垂直顺序，Header中按钮的Panel.ZIndex固定为2;
* FIconTabItemStyle：集成了FIcon的TabItem样式，是DefaultTabControl的默认ItemContainerStyle样式
* TransparencyFIconTabItemStyle：背景透明的TabItem样式，当使用该样式的时候，就需要设置TabControl的Panel.ZIndex小于2的一个值

## ListBox，简单的列表集合控件样式
* DefaultListBox：默认ListBox样式，默认不支持虚拟化
* VirtualListBox：支持数据虚拟化的ListBox样式
* DefaultListBoxItem：默认的ListBoxItem项样式
* RadioButtonListBoxItem：集成RadioButton的ListBoxItem样式，参照可以自定义自己的样式

## ListView，数据列表ListView样式
* DefaultListView：默认的ListView样式，支持ListView.View（内嵌GridView），如：‘<GridView ColumnHeaderContainerStyle="{StaticResource DefaultGridViewColumnHeader}">‘，具体参考示例
* 其他都为内部辅助样式，其中在使用ListView.View需要显示引用的是DefaultGridViewColumnHeader样式。

## DataGrid，比较常用的数据列表
* DefaultDataGridRow：DataGrid的默认样式，默认已经开启了数据虚拟化。默认AlternationCount=2，GridLinesVisibility=All
* SmallDataGridColumnHeader，主要用于简单的自定义列，不需要额外操作，比如自定义状态列、CheckBox的数据选中状态列等
* 其他样式都属于内部样式，有需要可以自己分别自定义

## ScrollViewer：滑动条样式，作为默认样式这种，一般不需要显示使用。
* DefaultScrollViewer：默认ScrollViewer样式

## TreeView：树形列表
* DefaultTreeView：默认的TreeView样式，默认已经开启数据虚拟化，对于多各层级一般需要定制模板HierarchicalDataTemplate

## Window，窗体组件，添加了一个窗体控件WindowBase
* WindowBase使用示例：utc:WindowBase x:Class="Util.ControlsTest.WinNT" xmlns:utc="clr-namespace:Util.Controls;assembly=Util.Controls"，后面会考虑添加窗体模板的
* 几个关键指标：CaptionHeight 标题栏高度；FIcon 窗体字体图标FIcon；FIconSize 窗体字体图标大小
* Header是一个ControlTemplate，可以自定义，具体使用参考使用示例
* DefaultWindowStyle 默认WindowBase窗体样式，如果有自定义标题，自定义标题交互设置：shell:WindowChrome.IsHitTestVisibleInChrome="True"
* NoTransparencyWindowStyle，这是一个高级的需要定制的非透明窗体，主要是应用与程序主窗体，这个主窗体可能会包含其他winform的组件。
* 去掉了NoTransparencyWindowStyle样式，如果有这个需求，使用默认样式，然后设置几个基本属性即可，例：WindowState="Maximized" AllowsTransparency="False" Effect="{x:Null}" Margin="0"，
	标题栏通过重写Header的模板来实现，需要主要注意的是，如果Header上有交互操作，需要设置附加属性：shell:WindowChrome.IsHitTestVisibleInChrome="True"


## MessageBox，消息提示框控件，用于代替默认的提示窗体，该MessageBox内容是可以自适应窗体大小的，
* 目前提供的几个静态方法：Error,Info,Warning,Question
* 使用示例：Util.Controls.MessageBox.Error("获取数据发生错误：" + ex.AllMessage());

## 表示等待、忙的状态控件：
* ProgressRing，类似Win10风格的一个控件，通过IsActive来控制是否运行。
* BusyBox，一个很简单的的表示忙的状态控件，通过IsActive来控制是否运行。

## 新增一个PopMessageBox控件，以Popup方式弹出的一个消息框，非独占，默认3秒后自己消失
* 使用和上面的MessageBox类似，目前提供的几个静态方法：Error,Info,Warning
* 使用示例：Util.Controls.PopMessageBox.Info("Hello World!");

## 新增一个非常实用的转换器：EnumTypeToItemSourceConverter，把一个枚举类型的值作为数据源
* 使用：' ItemsSource="{Binding Converter={x:Static utc:XConverter.EnumTypeToItemSourceConverter}, ConverterParameter={x:Type utc:EnumThumbnail}}" '
* 同时必须注意设置SelectedValuePath="Value"，然后直接使用SelectedValue设置、获取枚举对象即可
* 目前在示例项目中有ComboBox的的示例

## 2016.6更新  **************************************************
* 所有输入组建的Label部分，增加附加属性控制Label宽度，默认60，使用示例：utc:UtilProperty.LabelWidth="80"

## TreeComboBox 下拉树形选择控件，只有一个属性SelectItem可以获取选中项，设置的话目前只能通过样式绑定数据来控制。
* DefaultTreeComboBoxItemTemplate，默认树的项模板，只支持NodeX结构
* DefaultTreeComboBoxItemContainerStyle，默认树的项模板，只支持NodeX结构（默认已经绑定选中状态到IsSelected属性）

## Slider控件样式
* SimpleSliderStyle 简单的Slider样式，此样式不支持刻度线
* FIconThumbSliderStyle 用字体图标做滑块的一种样式，其他和上面类似。

## RangeSlider 范围选择控件，这是一个新开发的控件，支持范围值选择
* 该控件继承自RangeBase，Minimum，Maximum的使用和slider一样。由于是两个值，属性StartValue，EndValue分别描述开始值和结束值。

待完成项：

树控件选项还是有bug，还有就是选中后的GUI交互方式

，容器控件

