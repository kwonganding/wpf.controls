using System;
using System.Windows.Controls;

namespace Util.Controls
{
    /// <summary>
    /// 常用转换器的静态引用
    /// 使用实例：Converter={x:Static local:XConverter.TrueToFalseConverter}
    /// </summary>
    public sealed class XConverter
    {
        public static BooleanToVisibilityConverter BooleanToVisibilityConverter
        {
            get { return Singleton<BooleanToVisibilityConverter>.GetInstance(); }
        }

        public static EnabledToVisibilityConverter EnabledToVisibilityConverter
        {
            get { return Singleton<EnabledToVisibilityConverter>.GetInstance(); }
        }

        public static BoolToVisibilityHiddenConverter BoolToVisibilityHiddenConverter
        {
            get { return Singleton<BoolToVisibilityHiddenConverter>.GetInstance(); }
        }

        public static TrueToFalseConverter TrueToFalseConverter
        {
            get { return Singleton<TrueToFalseConverter>.GetInstance(); }
        }

        public static ThicknessToDoubleConverter ThicknessToDoubleConverter
        {
            get { return Singleton<ThicknessToDoubleConverter>.GetInstance(); }
        }

        public static DoubleToThicknessConverter DoubleToThicknessConverter
        {
            get { return Singleton<DoubleToThicknessConverter>.GetInstance(); }
        }

        public static BackgroundToForegroundConverter BackgroundToForegroundConverter
        {
            get { return Singleton<BackgroundToForegroundConverter>.GetInstance(); }
        }

        public static TreeViewMarginConverter TreeViewMarginConverter
        {
            get { return Singleton<TreeViewMarginConverter>.GetInstance(); }
        }

        public static PercentToAngleConverter PercentToAngleConverter
        {
            get { return Singleton<PercentToAngleConverter>.GetInstance(); }
        }

        public static NullToEnableConverter NullToEnableConverter
        {
            get { return Singleton<NullToEnableConverter>.GetInstance(); }
        }

        public static NullToVisibilityConverter NullToVisibilityConverter
        {
            get { return Singleton<NullToVisibilityConverter>.GetInstance(); }
        }

        public static CountOrNullToEnableConverter CountOrNullToEnableConverter
        {
            get { return Singleton<CountOrNullToEnableConverter>.GetInstance(); }
        }

        public static EnumDescriptionConverter EnumDescriptionConverter
        {
            get { return Singleton<EnumDescriptionConverter>.GetInstance(); }
        }

        public static EnumTypeToItemSourceConverter EnumTypeToItemSourceConverter
        {
            get { return Singleton<EnumTypeToItemSourceConverter>.GetInstance(); }
        }

        public static EnumsToItemSourceConverter EnumsToItemSourceConverter
        {
            get { return Singleton<EnumsToItemSourceConverter>.GetInstance(); }
        }

        public static ObjectTypeConverter ObjectTypeConverter
        {
            get { return Singleton<ObjectTypeConverter>.GetInstance(); }
        }

        public static LightBrushConverter LightBrushConverter
        {
            get { return Singleton<LightBrushConverter>.GetInstance(); }
        }

        public static BoolToTextConverter BoolToTextConverter
        {
            get { return Singleton<BoolToTextConverter>.GetInstance(); }
        }

        public static StringFormatConverter StringFormatConverter
        {
            get { return Singleton<StringFormatConverter>.GetInstance(); }
        }

        public static PointConverter PointConverter
        {
            get { return Singleton<PointConverter>.GetInstance(); }
        }

        public static SizeConverter SizeConverter
        {
            get { return Singleton<SizeConverter>.GetInstance(); }
        }

        public static ColorToBrushConverter ColorToBrushConverter
        {
            get { return Singleton<ColorToBrushConverter>.GetInstance(); }
        }

        public static BrushToColorConverter BrushToColorConverter
        {
            get { return Singleton<BrushToColorConverter>.GetInstance(); }
        }

        public static ColorToStringConverter ColorToStringConverter
        {
            get { return Singleton<ColorToStringConverter>.GetInstance(); }
        }

        public static BytesToImageSourceConverter BytesToImageSourceConverter
        {
            get { return Singleton<BytesToImageSourceConverter>.GetInstance(); }
        }

        public static StringLengthToVisibilityConverter StringLengthToVisibilityConverter
        {
            get { return Singleton<StringLengthToVisibilityConverter>.GetInstance(); }
        }

        public static NullAndVisibilityToCollapsedConverter NullAndVisibilityToCollapsedConverter
        {
            get { return Singleton<NullAndVisibilityToCollapsedConverter>.GetInstance(); }
        }

        public static VisibilityToCollapsedConverter VisibilityToCollapsedConverter
        {
            get { return Singleton<VisibilityToCollapsedConverter>.GetInstance(); }
        }

        public static StringLengthToBoolConverter StringLengthToBoolConverter
        {
            get { return Singleton<StringLengthToBoolConverter>.GetInstance(); }
        }

        public static CountOrNullToVisibilityConverter CountOrNullToVisibilityConverter
        {
            get { return Singleton<CountOrNullToVisibilityConverter>.GetInstance(); }
        }

        public static CountOrNullToCollapsedConverter CountOrNullToCollapsedConverter
        {
            get { return Singleton<CountOrNullToCollapsedConverter>.GetInstance(); }
        }

        public static BitmapImageSourceConverter BitmapImageSourceConverter
        {
            get { return Singleton<BitmapImageSourceConverter>.GetInstance(); }
        }

        public static VisibilityToStarGridLengthConverter VisibilityToStarGridLength
        {
            get { return Singleton<VisibilityToStarGridLengthConverter>.GetInstance(); }
        }

        public static CharacterWrapConverter CharacterWrapConverter
        {
            get { return Singleton<CharacterWrapConverter>.GetInstance(); }
        }
    }
}