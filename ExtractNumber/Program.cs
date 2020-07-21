using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;
using Tesseract;

namespace ExtractNumber
{
    class Program
    {
        static int x = 1690;
        static int y = 950;
        static int width = 90;
        static int height = 70;

        static Bitmap BitmapFrame;
        static Rectangle cropArea = new Rectangle(x, y, width, height);
        static TesseractEngine ocr = new TesseractEngine("./tessdata", "eng", EngineMode.Default);

        static void SetCropArea()
        {
            Console.WriteLine("x = ");
            x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("y = ");
            y = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("width = ");
            width = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("height = ");
            height = Convert.ToInt32(Console.ReadLine());
        }

        static void SaveFile(Bitmap img, string filename)
        {
            if (filename == null)
            {
                filename = x.ToString() + "_"
                                + y.ToString() + "_"
                                + width.ToString() + "_"
                                + height.ToString();
            }
            filename = Directory.GetCurrentDirectory() + "/cropImage/" + filename + ".png";
            img.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        }

        static void Main(string[] args)
        {
            string videoDir = Directory.GetCurrentDirectory() + "/video/HalfLife.mp4";
            VideoCapture capture = new VideoCapture(videoDir);
                ocr.DefaultPageSegMode = PageSegMode.SingleBlock;

            Pix pixImage;
            int frameCount = 0;
            int totalFrame = (int)capture.GetCaptureProperty(CapProp.FrameCount);
            Console.WriteLine(totalFrame);

            /* get each frame from the video */
            while (frameCount < totalFrame)
            {
                /* Crop area
                 * x:1680px  y:950px  boxsize: 105x70*/
    
                var frame = capture.QueryFrame();
                Console.WriteLine(frame.Width.ToString() + " " + frame.Height.ToString());
                BitmapFrame = Crop_frame(capture.QueryFrame(), cropArea).ToBitmap();
                frameCount += 1;
                
                SaveFile(BitmapFrame, frameCount.ToString());

                /* use Tesseract to recognize number */
                try
                {
                    pixImage = PixConverter.ToPix(BitmapFrame); //using Tesseract_463
                    var page = ocr.Process(pixImage);
                    var bulletStr = page.GetText(); // 識別後的內容
                    Console.WriteLine(frameCount.ToString() + " , String result : " + bulletStr);
                    page.Dispose();                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error message: " + ex.Message);
                }
            }
        }

        /* Crop out a square area */
        static Mat Crop_frame(Mat input, Rectangle crop_region)
        {
            Image<Bgr, Byte> buffer_im = input.ToImage<Bgr, Byte>();
            buffer_im.ROI = crop_region;

            Image<Bgr, Byte> cropped_im = buffer_im.Copy();

            return cropped_im.Mat;
        }
    }
}
