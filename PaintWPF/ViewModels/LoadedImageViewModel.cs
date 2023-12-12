using PaintWPF.Commands;
using PaintWPF.Providers;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PaintWPF.ViewModels;
public class LoadedImageViewModel : ViewModelBase
{
    private ImageSource _loadedImage;

    public ImageSource LoadedImage
    {
        get => _loadedImage;
        set
        {
            _loadedImage = value;
            OnPropertyChanged(nameof(LoadedImage));
        }
    }

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

    public LoadedImageViewModel(string path)
    {
        LoadFile(path);
    }

    public void LoadFile(string path)
    {
        Bitmap bpm = null;
        string format = File.ReadLines(path).First().Split(null as char[], StringSplitOptions.RemoveEmptyEntries).First();
        switch (format)
        {
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
        this.LoadedImage = BmpToBmpImg(bpm);
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
}
