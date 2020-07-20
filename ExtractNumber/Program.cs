using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ExtractNumber
{
    class Program
    {
        static int x = 1680;
        static int y = 950;
        static int width = 105;
        static int height = 70;
        
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
                filename = Directory.GetCurrentDirectory() + "/cropImage/"
                                        + x.ToString() + "_"
                                        + y.ToString() + "_"
                                        + width.ToString() + "_"
                                        + height.ToString() + ".png";
            }
            img.Save(filename, ImageFormat.Png);
        }

        static void Main(string[] args)
        {
            string videoDir = Directory.GetCurrentDirectory() + "/video/HalfLife.mp4";

            VideoCapture capture = new VideoCapture(videoDir);
            int frameCount = 0;
            int totalFrame = (int)capture.GetCaptureProperty(CapProp.FrameCount); // 影片中的影格總數;
            Console.WriteLine(totalFrame);

            Bitmap BitmapFrame;
            Rectangle cropArea = new Rectangle(x, y, width, height);

            /* get each frame from the video */
            while (frameCount < totalFrame)
            {

                /* calculate total process time */
                // beforDT = System.DateTime.Now;

                /* Crop area
                 * x:1680px  y:950px  boxsize: 105x70*/
    
                var frame = capture.QueryFrame();
                Console.WriteLine(frame.Width.ToString() + " " + frame.Height.ToString());
                BitmapFrame = Crop_frame(capture.QueryFrame(), cropArea).ToBitmap();
                SaveFile(BitmapFrame, null);

                frameCount += 1;


                /* image processing */
                // BitmapFrame = imageProcess.NegativePicture(BitmapFrame); //turn into negative image
                // BitmapFrame = imageProcess.ResizeImage(BitmapFrame, 120, 76); // enlarge image(x2)
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
