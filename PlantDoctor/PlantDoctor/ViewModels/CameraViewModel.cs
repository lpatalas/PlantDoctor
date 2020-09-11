using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlantDoctor.ViewModels
{
    public class CameraViewModel : BaseViewModel
    {
        private bool _isHealthy;
        public bool IsHealthy
        {
            get => _isHealthy;
            set
            {
                SetProperty(ref _isHealthy, value);
                OnPropertyChanged(nameof(HealthColor));
                OnPropertyChanged(nameof(HealthDescription));
            }
        }

        public Color HealthColor
            => IsHealthy ? Color.Green : Color.Red;

        public string HealthDescription
            => IsHealthy ? "✔ HEALTHY" : "✘ UNHEALTHY"; 

        private ImageSource _picture;
        public ImageSource Picture
        {
            get => _picture;
            set => SetProperty(ref _picture, value);
        }

        public ICommand TakePicture { get; }
        
        public CameraViewModel()
        {
            TakePicture = new Command(OnTakePicture);
        }

        private async void OnTakePicture()
        {
            var cameraOptions = new StoreCameraMediaOptions();
            var photo = await CrossMedia.Current.TakePhotoAsync(
                cameraOptions);

            if (photo != null)
            {
                Picture = ImageSource.FromStream(photo.GetStream);
                IsHealthy = CheckImage(photo);
            }
        }

        private bool CheckImage(MediaFile mediaFile)
        {
            // TODO: Put image recognition here
            return !IsHealthy;
        }
    }
}
