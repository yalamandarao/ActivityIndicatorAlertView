using System;
using System.ComponentModel;
using System.Drawing;
using ActivityIndicatorExample.Controls;
using ActivityIndicatorExample.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;

[assembly: ExportRenderer(typeof(ActivityIndicatorAlertView),
                          typeof(ActivityIndicatorAlertViewRenderer))]

namespace ActivityIndicatorExample.iOS.Renderers
{
    public class ActivityIndicatorAlertViewRenderer : ViewRenderer<ActivityIndicatorAlertView, UIView>
    {
        bool _disposed;
        AlertViewController _alert;

        ActivityIndicatorAlertView BaseElement
        {
            get => Element as ActivityIndicatorAlertView;
        }

        public ActivityIndicatorAlertViewRenderer()
        {
        }

        #region Life-cycle methods

        protected async override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;

                if (_alert != null)
                {
                    await _alert.DismissViewControllerAsync(true);
                    _alert.Dispose();
                    _alert = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null
                || BaseElement == null)
            {
                return;
            }

            if (e.PropertyName == ActivityIndicatorAlertView.TitleProperty.PropertyName)
            {
                UpdateTitle();
            }
            else if (e.PropertyName == ActivityIndicatorAlertView.MessageProperty.PropertyName)
            {
                UpdateMessage();
            }
            else if (e.PropertyName == ActivityIndicatorAlertView.IsRunningProperty.PropertyName)
            {
                UpdateIsRunning();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicatorAlertView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            if (Control == null)
            {
                SetNativeControl(new UIView
                {
                    Hidden = true
                });
            }

            UpdateTitle();
            UpdateMessage();
            UpdateIsRunning();
        }

        #endregion

        void UpdateTitle()
        {
            if (_alert != null)
            {
                _alert.TitleLabel.Text = BaseElement.Title;
            }
        }

        void UpdateMessage()
        {
            if (_alert != null)
            {
                _alert.MessageLabel.Text = BaseElement.Message;
            }
        }

        void UpdateIsRunning()
        {
            if (BaseElement.IsRunning)
            {
                CreateAlertViewController();
            }
            else
            {
                DismissAlertViewController();
            }
        }

        void CreateAlertViewController()
        {
            DismissAlertViewController();

            _alert = new AlertViewController
            {
                TitleText = BaseElement.Title,
                MessageText = BaseElement.Message
            };
            PresentAlert(_alert);
        }

        void PresentAlert(UIViewController alert)
        {
            var window = new UIWindow { BackgroundColor = Color.FromHex("#4C000000").ToUIColor() };
            window.RootViewController = new UIViewController();
            window.RootViewController.View.BackgroundColor = Color.Transparent.ToUIColor();
            window.WindowLevel = UIWindowLevel.Alert + 1;
            window.MakeKeyAndVisible();

            if (!UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                // For iOS 8, we need to explicitly set the size of the window
                window.Frame = new RectangleF(0,
                                              0,
                                              (float)UIScreen.MainScreen.Bounds.Width,
                                              (float)UIScreen.MainScreen.Bounds.Height);
            }

            window.RootViewController.PresentViewController(alert, true, null);
        }

        async void DismissAlertViewController()
        {
            if (_alert != null)
            {
                await _alert.DismissViewControllerAsync(true);
                _alert = null;
            }
        }

        public class AlertViewController : UIViewController
        {
            public UIView BackgroundView { get; set; }

            public string TitleText { get; set; }

            public string MessageText { get; set; }

            public UILabel TitleLabel { get; set; }

            public UILabel MessageLabel { get; set; }

            public UIActivityIndicatorView ActivityIndicator { get; set; }

            public override void LoadView()
            {
                base.LoadView();

                this.View.BackgroundColor = UIColor.Clear;

                BackgroundView = new UIView(
                  new CGRect((this.View.Frame.Width - 270.0f) / 2,
                             (this.View.Frame.Height - 144.0f) / 2,
                             270.0f,
                             114.0f));

                var rgb = 249.0f / 255.0f;
                BackgroundView.BackgroundColor = new UIColor(rgb, rgb, rgb, 0.9f);
                BackgroundView.Layer.CornerRadius = 15.0f;

                TitleLabel = new UILabel(new CGRect(0.0f, 8.0f, BackgroundView.Frame.Width, 30.0f));
                TitleLabel.TextAlignment = UITextAlignment.Center;
                TitleLabel.Font = UIFont.BoldSystemFontOfSize(15.0f);
                TitleLabel.Text = TitleText;
                BackgroundView.AddSubview(TitleLabel);

                MessageLabel = new UILabel(new CGRect(0.0f, 38.0f, BackgroundView.Frame.Width, 30.0f));
                MessageLabel.TextAlignment = UITextAlignment.Center;
                MessageLabel.Font = UIFont.SystemFontOfSize(UIFont.SmallSystemFontSize);
                MessageLabel.Text = MessageText;
                BackgroundView.AddSubview(MessageLabel);

                ActivityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
                ActivityIndicator.Frame = new CGRect((BackgroundView.Frame.Width - 16.0f) / 2,
                                                     68.0f,
                                                     16.0f,
                                                     16.0f);
                ActivityIndicator.StartAnimating();
                BackgroundView.AddSubview(ActivityIndicator);
                View.AddSubview(BackgroundView);
            }

        }
    }
}
