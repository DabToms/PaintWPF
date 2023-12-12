using PaintWPF.Commands;
using PaintWPF.Models;
using PaintWPF.Providers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace PaintWPF.ViewModels;
internal class DigitalFiltersViewModel : ViewModelBase
{
    public ICommand NavigatePaintCommand { get; }

    public int RValue { get; set; } = 0;
    public int GValue { get; set; } = 0;
    public int BValue { get; set; } = 0;
    private Bitmap bitmap;
    public int _imageWidth;
    public int ImageWidth
    {
        get => _imageWidth;
        set
        {
            _imageWidth = value;
            OnPropertyChanged(nameof(ImageWidth));
        }
    }
    public int _imageHeight;
    public int ImageHeight
    {
        get => _imageHeight;
        set
        {
            _imageHeight = value;
            OnPropertyChanged(nameof(ImageHeight));
        }
    }
    private BitmapSource _imageSource;
    public BitmapSource ImageSource
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            OnPropertyChanged(nameof(ImageSource));
        }
    }
    private int _brightnessLevel;
    public int BrightnessLevel
    {
        get => _brightnessLevel;
        set
        {
            _brightnessLevel = value;
            OnPropertyChanged(nameof(BrightnessLevel));
        }
    }
    public List<string> GrayScaleModeList { get; set; } = new List<string> { "RGB Avg", "RChanel" };
    public string SelectedGrayScaleMode { get; set; }
    public ICommand ApplyGrayness { get; }

    public ICommand ApplyBrightness { get; }

    public ICommand AddCommand { get; }
    public ICommand SubtractCommand { get; }
    public ICommand MultiplyCommand { get; }
    public ICommand DivideCommand { get; }

    public ICommand AverageFilterCommand { get; }
    public ICommand MedianFilterCommand { get; }
    public ICommand SobelFilterCommand { get; }
    public ICommand HighPassFilterCommand { get; }
    public ICommand GausianFilterCommand { get; }


    public DigitalFiltersViewModel(INavigationService paintNavigationService)
    {
        NavigatePaintCommand = new NavigateCommand(paintNavigationService);
        ApplyBrightness = new Command(() =>
        {
            if (ImageSource != null)
                ImageSource = PointTransformation.ChangeBrightness(ImageSource, this.BrightnessLevel);
        });
        AddCommand = new Command(() =>
        {
            if (ImageSource != null)
                ImageSource = PointTransformation.Add(ImageSource, System.Drawing.Color.FromArgb(RValue, GValue, BValue));
        });
        SubtractCommand = new Command(() =>
        {
            if (ImageSource != null)
                ImageSource = PointTransformation.Subtract(ImageSource, System.Drawing.Color.FromArgb(RValue, GValue, BValue));
        });
        MultiplyCommand = new Command(() =>
        {
            if (ImageSource != null)
                ImageSource = PointTransformation.Multiply(ImageSource, System.Drawing.Color.FromArgb(RValue, GValue, BValue));
        });
        DivideCommand = new Command(() =>
        {
            if (ImageSource != null)
                ImageSource = PointTransformation.Divide(ImageSource, System.Drawing.Color.FromArgb(RValue, GValue, BValue));
        });
        ApplyGrayness = new Command(() =>
        {
            if (ImageSource != null)
            {
                switch (this.SelectedGrayScaleMode)
                {
                    case "RGB Avg":
                        ImageSource = PointTransformation.ConvertToGrayscaleRGBAvarage(ImageSource);
                        break;
                    case "RChanel":
                        ImageSource = PointTransformation.ConvertToGrayscaleRChannel(ImageSource);
                        break;
                }
            }
        });


        AverageFilterCommand = new Command(() =>
            {
                if (ImageSource != null)
                    ImageSource = FilterProvider.Average(ImageSource);
            }); 

        MedianFilterCommand = new Command(() =>
            {
                if (ImageSource != null)
                    ImageSource = FilterProvider.Median(ImageSource);
            }); 
        
        SobelFilterCommand = new Command(() =>
            {
                if (ImageSource != null)
                    ImageSource = FilterProvider.Sobel(ImageSource);
            }); 
        
        HighPassFilterCommand = new Command(() =>
            {
                if (ImageSource != null)
                    ImageSource = FilterProvider.HighPass(ImageSource);
            });

        GausianFilterCommand = new Command(() =>
            {
                if (ImageSource != null)
                    ImageSource = FilterProvider.Gaussian(ImageSource);
            });

    }
    public void LoadImage(string path)
    {
        Bitmap bpm = null;
        string format = File.ReadLines(path).First().Split(null as char[], StringSplitOptions.RemoveEmptyEntries).First().TrimStart('�');
        switch (format)
        {
            case "PNG":
                var source = ImageLoader.LoadPNGFileToBitmap(path);
                this.ImageSource = source;
                this.ImageWidth = source.PixelWidth;
                this.ImageHeight = source.PixelHeight;
                return;
            case "P1":
                bpm = ImageLoader.LoadP1FileToBitmap(path);
                break;
            case "P2":
                bpm = ImageLoader.LoadP2FileToBitmap(path);
                break;
            case "P3":
                bpm = ImageLoader.LoadP3FileToBitmap(path);
                break;
            case "P4":
                bpm = ImageLoader.LoadP4FileToBitmap(path);
                break;
            case "P5":
                bpm = ImageLoader.LoadP5FileToBitmap(path);
                break;
            case "P6":
                bpm = ImageLoader.LoadP6FileToBitmap(path);
                break;
        }
        this.ImageWidth = bpm.Width;
        this.ImageHeight = bpm.Height;
        this.ImageSource = BmpToBmpImg(bpm);
        this.bitmap = bpm;
    }
    private BitmapImage BmpToBmpImg(Bitmap bitmap)
    {
        var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Bmp);
        stream.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = stream;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        return bitmapImage;
    }

    public Bitmap? BitmapImageToBitmap(BitmapImage? bitmapImage)
    {
        if (bitmapImage == null)
        {
            return null;
        }
        var memoryStream = new MemoryStream();
        var bitmapEncoder = new BmpBitmapEncoder();
        bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapImage));
        bitmapEncoder.Save(memoryStream);
        return new Bitmap(memoryStream);
    }
}
