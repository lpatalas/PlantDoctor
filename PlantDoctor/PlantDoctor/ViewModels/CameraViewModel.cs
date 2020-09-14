using Plugin.Media;
using Plugin.Media.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
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

        public Xamarin.Forms.Color HealthColor
            => IsHealthy ? Xamarin.Forms.Color.Green : Xamarin.Forms.Color.Red;

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
            else
            {
                Picture = null;
            }
        }

        private bool CheckImage(MediaFile mediaFile)
        {
            var image = LoadCroppedImage(mediaFile.Path);
            // TODO: Send to API
            return !IsHealthy;
        }

        private SixLabors.ImageSharp.Image LoadCroppedImage(string filePath)
        {
            var image = SixLabors.ImageSharp.Image.Load(filePath);
            if (image.Width != image.Height)
            {
                var cropRect = image.Bounds();
                if (image.Width < image.Height)
                {
                    cropRect.Inflate(0, -(image.Height - image.Width) / 2);
                }
                else
                {
                    cropRect.Inflate(-(image.Width - image.Height) / 2, 0);
                }

                image.Mutate(x => x.Crop(cropRect));
            }

            return image;
        }
    }
}
