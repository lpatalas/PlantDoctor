using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PlantDoctor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PlantDoctor.CustomControls.CameraPreview), typeof(PlantDoctor.Droid.CameraPreviewRenderer))]
namespace PlantDoctor.Droid
{
    public class CameraPreviewRenderer
        : ViewRenderer<PlantDoctor.CustomControls.CameraPreview, PlantDoctor.Droid.CameraPreview>
    {
        CameraPreview cameraPreview;

        public CameraPreviewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<PlantDoctor.CustomControls.CameraPreview> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
                cameraPreview.Click -= OnCameraPreviewClicked;
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    cameraPreview = new CameraPreview(Context);
                    SetNativeControl(cameraPreview);
                }
                Control.Preview = Camera.Open();

                // Subscribe
                cameraPreview.Click += OnCameraPreviewClicked;
            }
        }

        void OnCameraPreviewClicked(object sender, EventArgs e)
        {
            if (cameraPreview.IsPreviewing)
            {
                cameraPreview.Preview.StopPreview();
                cameraPreview.IsPreviewing = false;
            }
            else
            {
                cameraPreview.Preview.StartPreview();
                cameraPreview.IsPreviewing = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.Preview.Release();
            }
            base.Dispose(disposing);
        }
    }
}