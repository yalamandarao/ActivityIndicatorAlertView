using System;
using Android.App;
using Android.Content;
using ActivityIndicatorExample.Controls;
using ActivityIndicatorExample.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ActivityIndicatorAlertView), typeof(ActivityIndicatorAlertViewRenderer))]

namespace ActivityIndicatorExample.Droid.Renderers
{
    public class ActivityIndicatorAlertViewRenderer : ViewRenderer<ActivityIndicatorAlertView, Android.Views.View>
    {
        bool _disposed;
        ProgressDialog _dialog;

        ActivityIndicatorAlertView BaseElement
        {
            get => Element as ActivityIndicatorAlertView;
        }

        public ActivityIndicatorAlertViewRenderer(Context context) : base(context)
        {
        }

        #region Life-cycle methods

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;

                if (_dialog != null)
                {
                    _dialog.Dismiss();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null)
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
                SetNativeControl(new Android.Views.View(Context)
                {
                    Visibility = Android.Views.ViewStates.Gone
                });
            }

            UpdateTitle();
            UpdateMessage();
            UpdateIsRunning();
        }

        #endregion

        void UpdateTitle()
        {
            if (_dialog != null)
            {
                _dialog.SetTitle(BaseElement.Title);
            }
        }

        void UpdateMessage()
        {
            if (_dialog != null)
            {
                _dialog.SetMessage(BaseElement.Message);
            }
        }

        void UpdateIsRunning()
        {
            if (BaseElement.IsRunning)
            {
                CreateProgressDialog();
                _dialog.Show();
            }
            else
            {
                DismissProgressDialog();
            }
        }

        void CreateProgressDialog()
        {
            DismissProgressDialog();

            _dialog = new ProgressDialog(Context, Resource.Style.AppCompatDialogStyle);
            _dialog.SetCancelable(false);
            _dialog.SetCanceledOnTouchOutside(false);
            _dialog.SetMessage(BaseElement.Message);
        }

        void DismissProgressDialog()
        {
            if (_dialog != null)
            {
                _dialog.Dismiss();
                _dialog = null;
            }
        }
    }
}
