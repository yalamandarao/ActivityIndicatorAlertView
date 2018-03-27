using System;
using Xamarin.Forms;


namespace ActivityIndicatorExample.Controls
{
    public class ActivityIndicatorAlertView: View
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: nameof(Title),
            returnType: typeof(string),
            declaringType: typeof(ActivityIndicatorAlertView),
            defaultValue: "");

        public static readonly BindableProperty MessageProperty = BindableProperty.Create(
            propertyName: nameof(Message),
            returnType: typeof(string),
            declaringType: typeof(ActivityIndicatorAlertView),
            defaultValue: "");

        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(
            propertyName: nameof(IsRunning),
            returnType: typeof(bool),
            declaringType: typeof(ActivityIndicatorAlertView),
            defaultValue: default(bool));

        public string Title 
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
            
        }

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MenuProperty, value);
        }

        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

    }
}
