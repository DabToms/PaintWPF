using System.Drawing;
using System.Runtime.InteropServices;

namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public void AvarageFilterTest()
    {
        byte[] buffer = new byte[] { 0,0,0,0,0,0,0,0,0,0,
                                     0,1,1,1,1,1,0,0,0,0,
                                     0,0,0,1,1,1,1,0,0,0,
                                     0,0,0,1,1,1,1,0,0,0,
                                     0,0,0,0,1,1,1,0,0,0,
                                     0,0,1,0,1,0,0,0,0,0,
                                     0,0,1,0,1,0,0,0,0,0,
                                     0,0,0,1,0,0,0,0,0,0,
                                     0,0,0,0,0,0,0,0,0,0,
                                     0,0,0,0,0,0,0,0,0,0,
                        };


        var filteredImage = Average(buffer, 10);

        Assert.Equal(filteredImage, new byte[] { 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,1,0,0,0,0,0,
                                                 0,0,0,0,0,1,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
            });
    }
    [Fact]
    public void HighPassFilterTest()
    {
        byte[] buffer = new byte[] { 0,0,0,1,1,1,0,0,0,0,
                                     0,1,1,1,1,1,0,1,1,1,
                                     0,0,0,1,1,1,0,1,1,1,
                                     0,0,0,1,1,1,0,1,1,1,
                                     0,0,0,0,0,1,0,0,0,0,
                                     0,0,1,0,1,0,0,0,0,0,
                                     0,0,1,0,1,0,0,0,0,0,
                                     0,0,0,1,0,0,0,0,0,0,
                                     0,0,0,0,0,0,0,0,0,0,
                                     0,0,0,0,0,0,0,0,0,0,
                        };


        var filteredImage = HighPass(buffer, 10);

        Assert.Equal(filteredImage, new byte[] { 0,0,255,1,1,1,255,0,255,255,
                                                 254,7,4,2,0,3,251,5,3,5,
                                                 253,254,251,2,0,3,250,3,0,3,
                                                 253,0,254,5,2,4,251,5,3,5,
                                                 254,255,254,252,251,5,253,254,253,
                                                 254,255,254,7,252,6,253,255,0,0,
                                                 0,0,254,6,251,6,254,0,0,0,
                                                 0,0,255,254,6,254,255,0,0,0,
                                                 0,0,0,255,255,255,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,0
            });
    }
    [Fact]
    public void GausianFilterTest()
    {
        byte[] buffer = new byte[] { 0,0,0,1,1,1,0,0,0,0,
                                     0,1,1,1,1,1,0,1,1,1,
                                     0,0,0,1,1,1,0,1,1,1,
                                     0,0,0,1,1,1,0,1,1,1,
                                     0,0,0,0,0,1,0,0,0,0,
                                     0,0,1,0,1,0,0,0,0,0,
                                     0,0,1,0,1,0,0,0,0,0,
                                     0,0,0,1,0,0,0,0,0,0,
                                     0,0,0,0,0,0,0,0,0,0,
                                     0,0,0,0,0,0,0,0,0,0,
                        };


        var filteredImage = Gaussian(buffer, 10);

        Assert.Equal(filteredImage, new byte[] { 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,0,
            });
    }
    public byte[] Average(byte[] bitmapSource, int width)
    {
        return Filter(bitmapSource, width, new List<int> { 1, 1, 1,
                                                           1, 1, 1,
                                                           1, 1, 1 });
    }
    public static byte[] HighPass(byte[] bitmapSource, int width)
    {
        return Filter(bitmapSource, width, new List<int> { -1, -1, -1,
                                                           -1,  8, -1,
                                                           -1, -1, -1 });
    }
    public static byte[] Gaussian(byte[] bitmapSource, int width)
    {
        return Filter(bitmapSource, width, new List<int> { 1, 2, 1,
                                                           2, 4, 2,
                                                           1, 2, 1 });
    }
    private static byte[] Filter(byte[] data, int width, List<int> filterMatrix)
    {
        var result = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            double sum = 0;
            int participated = 0;
            if (i - width - 1 >= 0)
            {
                sum += data[i - width - 1] * filterMatrix[0];
                participated += filterMatrix[0];
            }
            if (i - width >= 0)
            {
                sum += data[i - width] * filterMatrix[1];
                participated += filterMatrix[1];
            }
            if (i - width + 1 >= 0)
            {
                sum += data[i - width + 1] * filterMatrix[2];
                participated += filterMatrix[2];
            }
            if (i - 1 >= 0)
            {
                sum += data[i - 1] * filterMatrix[3];
                participated += filterMatrix[3];
            }

            sum += data[i] * filterMatrix[4];
            participated += filterMatrix[4];

            if (i + 1 < data.Length)
            {
                sum += data[i + 1] * filterMatrix[5];
                participated += filterMatrix[5];
            }
            if (i + width - 1 < data.Length)
            {
                sum += data[i + width - 1] * filterMatrix[6];
                participated += filterMatrix[6];
            }
            if (i + width < data.Length)
            {
                sum += data[i + width] * filterMatrix[7];
                participated += filterMatrix[7];
            }
            if (i + width + 1 < data.Length)
            {
                sum += data[i + width + 1] * filterMatrix[8];
                participated += filterMatrix[8];
            }
            result[i] = (byte)(sum / (participated == 0 ? 1 : participated));
        }
        return result;
    }
}