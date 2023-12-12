using System;

namespace PaintWPF.ViewModels;
internal partial class PaintViewModel : ViewModelBase
{
    public int _RGB_R_Value = 0;
    public int RGB_R_Value
    {
        get => _RGB_R_Value;
        set
        {
            _RGB_R_Value = value;
            OnPropertyChanged(nameof(RGB_R_Value));

            var cmyk = Providers.ColorConverter.RGBToCMYK(RGB_R_Value, RGB_G_Value, RGB_B_Value);
            _CMYK_C_Value = cmyk.c;
            _CMYK_M_Value = cmyk.m;
            _CMYK_Y_Value = cmyk.y;
            _CMYK_K_Value = cmyk.k;
            OnPropertyChanged(nameof(CMYK_C_Value));
            OnPropertyChanged(nameof(CMYK_M_Value));
            OnPropertyChanged(nameof(CMYK_Y_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));


            var hsv = Providers.ColorConverter.RGBToHSV(RGB_R_Value, RGB_G_Value, RGB_B_Value);
            _HSV_H_Value = hsv.h;
            _HSV_S_Value = hsv.s;
            _HSV_V_Value = hsv.v;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public int _RGB_G_Value;
    public int RGB_G_Value
    {
        get => _RGB_G_Value;
        set
        {
            _RGB_G_Value = value;
            OnPropertyChanged(nameof(RGB_G_Value));

            var cmyk = Providers.ColorConverter.RGBToCMYK(RGB_R_Value, RGB_G_Value, RGB_B_Value);
            _CMYK_C_Value = cmyk.c;
            _CMYK_M_Value = cmyk.m;
            _CMYK_Y_Value = cmyk.y;
            _CMYK_K_Value = cmyk.k;
            OnPropertyChanged(nameof(CMYK_C_Value));
            OnPropertyChanged(nameof(CMYK_M_Value));
            OnPropertyChanged(nameof(CMYK_Y_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

            var hsv = Providers.ColorConverter.RGBToHSV(RGB_R_Value, RGB_G_Value, RGB_B_Value);
            _HSV_H_Value = hsv.h;
            _HSV_S_Value = hsv.s;
            _HSV_V_Value = hsv.v;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public int _RGB_B_Value = 0;
    public int RGB_B_Value
    {
        get => _RGB_B_Value;
        set
        {
            _RGB_B_Value = value;
            OnPropertyChanged(nameof(RGB_B_Value));

            var cmyk = Providers.ColorConverter.RGBToCMYK(RGB_R_Value, RGB_G_Value, RGB_B_Value);
            _CMYK_C_Value = cmyk.c;
            _CMYK_M_Value = cmyk.m;
            _CMYK_Y_Value = cmyk.y;
            _CMYK_K_Value = cmyk.k;
            OnPropertyChanged(nameof(CMYK_C_Value));
            OnPropertyChanged(nameof(CMYK_M_Value));
            OnPropertyChanged(nameof(CMYK_Y_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

            var hsv = Providers.ColorConverter.RGBToHSV(RGB_R_Value, RGB_G_Value, RGB_B_Value);
            _HSV_H_Value = hsv.h;
            _HSV_S_Value = hsv.s;
            _HSV_V_Value = hsv.v;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public float _CMYK_C_Value;
    public float CMYK_C_Value
    {
        get => _CMYK_C_Value;
        set
        {
            _CMYK_C_Value = value;
            OnPropertyChanged(nameof(CMYK_C_Value));

            var rgb = Providers.ColorConverter.CMYKToRGB(CMYK_C_Value, CMYK_M_Value, CMYK_Y_Value, CMYK_K_Value);
            _RGB_R_Value = rgb.r;
            _RGB_G_Value = rgb.g;
            _RGB_B_Value = rgb.b;
            OnPropertyChanged(nameof(RGB_R_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
            OnPropertyChanged(nameof(RGB_B_Value));

            var hsv = Providers.ColorConverter.CMYKToHSV(CMYK_C_Value, CMYK_M_Value, CMYK_Y_Value, CMYK_K_Value);
            _HSV_H_Value = hsv.h;
            _HSV_S_Value = hsv.s;
            _HSV_V_Value = hsv.val;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public float _CMYK_M_Value;
    public float CMYK_M_Value
    {
        get => _CMYK_M_Value;
        set
        {
            _CMYK_M_Value = value;
            OnPropertyChanged(nameof(CMYK_M_Value));

            var rgb = Providers.ColorConverter.CMYKToRGB(CMYK_C_Value, CMYK_M_Value, CMYK_Y_Value, CMYK_K_Value);
            _RGB_R_Value = rgb.r;
            _RGB_G_Value = rgb.g;
            _RGB_B_Value = rgb.b;
            OnPropertyChanged(nameof(RGB_R_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
            OnPropertyChanged(nameof(RGB_B_Value));

            var hsv = Providers.ColorConverter.CMYKToHSV(CMYK_C_Value, CMYK_M_Value, CMYK_Y_Value, CMYK_K_Value);
            _HSV_H_Value = hsv.h;
            _HSV_S_Value = hsv.s;
            _HSV_V_Value = hsv.val;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public float _CMYK_Y_Value;
    public float CMYK_Y_Value
    {
        get => _CMYK_Y_Value;
        set
        {
            _CMYK_Y_Value = value;
            OnPropertyChanged(nameof(CMYK_Y_Value));

            var rgb = Providers.ColorConverter.CMYKToRGB(CMYK_C_Value, CMYK_M_Value, CMYK_Y_Value, CMYK_K_Value);
            _RGB_R_Value = rgb.r;
            _RGB_G_Value = rgb.g;
            _RGB_B_Value = rgb.b;
            OnPropertyChanged(nameof(RGB_R_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
            OnPropertyChanged(nameof(RGB_B_Value));

            var hsv = Providers.ColorConverter.CMYKToHSV(CMYK_C_Value, CMYK_M_Value, CMYK_Y_Value, CMYK_K_Value);
            _HSV_H_Value = hsv.h;
            _HSV_S_Value = hsv.s;
            _HSV_V_Value = hsv.val;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public float _CMYK_K_Value;
    public float CMYK_K_Value
    {
        get => _CMYK_K_Value;
        set
        {
            _CMYK_K_Value = value;
            OnPropertyChanged(nameof(CMYK_K_Value));

            var rgb = Providers.ColorConverter.CMYKToRGB(CMYK_C_Value, CMYK_M_Value, CMYK_Y_Value, CMYK_K_Value);
            _RGB_R_Value = rgb.r;
            _RGB_G_Value = rgb.g;
            _RGB_B_Value = rgb.b;
            OnPropertyChanged(nameof(RGB_R_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
            OnPropertyChanged(nameof(RGB_B_Value));

            var hsv = Providers.ColorConverter.CMYKToHSV(CMYK_C_Value, CMYK_M_Value, CMYK_Y_Value, CMYK_K_Value);
            _HSV_H_Value = hsv.h;
            _HSV_S_Value = hsv.s;
            _HSV_V_Value = hsv.val;
            OnPropertyChanged(nameof(HSV_H_Value));
            OnPropertyChanged(nameof(HSV_S_Value));
            OnPropertyChanged(nameof(HSV_V_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public float _HSV_H_Value;
    public float HSV_H_Value
    {
        get => _HSV_H_Value;
        set
        {
            _HSV_H_Value = value;
            OnPropertyChanged(nameof(HSV_H_Value));

            var rgb = Providers.ColorConverter.HSVToRGB(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            _RGB_R_Value = rgb.r;
            _RGB_G_Value = rgb.g;
            _RGB_B_Value = rgb.b;
            OnPropertyChanged(nameof(RGB_R_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
            OnPropertyChanged(nameof(RGB_B_Value));

            var cmyk = Providers.ColorConverter.HSVToCMYK(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            _CMYK_C_Value = cmyk.c;
            _CMYK_M_Value = cmyk.m;
            _CMYK_Y_Value = cmyk.y;
            _CMYK_K_Value = cmyk.k;
            OnPropertyChanged(nameof(CMYK_C_Value));
            OnPropertyChanged(nameof(CMYK_M_Value));
            OnPropertyChanged(nameof(CMYK_Y_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public float _HSV_S_Value;
    public float HSV_S_Value
    {
        get => _HSV_S_Value;
        set
        {
            _HSV_S_Value = value;
            OnPropertyChanged(nameof(HSV_S_Value));

            var rgb = Providers.ColorConverter.HSVToRGB(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            _RGB_R_Value = rgb.r;
            _RGB_G_Value = rgb.g;
            _RGB_B_Value = rgb.b;
            OnPropertyChanged(nameof(RGB_R_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
            OnPropertyChanged(nameof(RGB_B_Value));

            var cmyk = Providers.ColorConverter.HSVToCMYK(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            _CMYK_C_Value = cmyk.c;
            _CMYK_M_Value = cmyk.m;
            _CMYK_Y_Value = cmyk.y;
            _CMYK_K_Value = cmyk.k;
            OnPropertyChanged(nameof(CMYK_C_Value));
            OnPropertyChanged(nameof(CMYK_M_Value));
            OnPropertyChanged(nameof(CMYK_Y_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

            this.ReloadBruchFromRgb();
        }
    }

    public float _HSV_V_Value;
    public float HSV_V_Value
    {
        get => _HSV_V_Value;
        set
        {
            _HSV_V_Value = value;
            OnPropertyChanged(nameof(HSV_V_Value));

            var rgb = Providers.ColorConverter.HSVToRGB(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            _RGB_R_Value = rgb.r;
            _RGB_G_Value = rgb.g;
            _RGB_B_Value = rgb.b;
            OnPropertyChanged(nameof(RGB_R_Value));
            OnPropertyChanged(nameof(RGB_G_Value));
            OnPropertyChanged(nameof(RGB_B_Value));

            var cmyk = Providers.ColorConverter.HSVToCMYK(HSV_H_Value, HSV_S_Value, HSV_V_Value);
            _CMYK_C_Value = cmyk.c;
            _CMYK_M_Value = cmyk.m;
            _CMYK_Y_Value = cmyk.y;
            _CMYK_K_Value = cmyk.k;
            OnPropertyChanged(nameof(CMYK_C_Value));
            OnPropertyChanged(nameof(CMYK_M_Value));
            OnPropertyChanged(nameof(CMYK_Y_Value));
            OnPropertyChanged(nameof(CMYK_K_Value));

            this.ReloadBruchFromRgb();
        }
    }
}