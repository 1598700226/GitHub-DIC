using System;
using System.Collections.Generic;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System.Drawing;


namespace MyDIC.Algorithm
{
    class RegisteringPoints
    {

        //Pinvoke 跨平台，今后需要改成c++ invoke
        [DllImport("ImageR.dll", CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "ImageRegistration")]
        //input: reference image，current image, coordinate_x, coordinate_y, subset size, searching size 1(大范围搜索), searching size 2(小范围搜索)，匹配系数阈值
        //output： u, v, ux, uy, vx, vy, error_code
        public static extern void ImageRegistration(
            IntPtr src, IntPtr curImg, 
            int len, 
            [In, Out] double[] temp_x, [In, Out] double[] temp_y, 
            ref int global_intLoc, ref double subLoc, ref double golbal_subLoc, 
            int m_subsetSize, int m_subsetExtendSize
            );

        //定义变量，用于外部访问
        private int subset_size;//配准图像上选取的子区域大小

        public int Subset_size
        {
            get { return subset_size; }
            set { subset_size = value; }
        }
        private int searching_size;//搜索区域大小

        public int Searching_size
        {
            get { return searching_size; }
            set { searching_size = value; }
        }
        private double coefficient_max;//相关系数最大值

        public double Coefficient_max
        {
            get { return coefficient_max; }
            set { coefficient_max = value; }
        }

        private double coefficient_min;//相关系数最大值

        public double Coefficient_min
        {
            get { return coefficient_min; }
            set { coefficient_min = value; }
        }

        private int[,] globalIntLocation;//配准得到的新的正像素坐标

        public int[,] GlobalIntLocation
        {
            get { return globalIntLocation; }
            set { globalIntLocation = value; }
        }
        private double[,] subLocation;

        public double[,] SubLocation//配准得到的新的压像素坐标
        {
            get { return subLocation; }
            set { subLocation = value; }
        }
        private double[,] globalSubLocation;

        public double[,] GlobalSubLocation//配准得到的新的压像素坐标
        {
            get { return globalSubLocation; }
            set { globalSubLocation = value; }
        }

        //PointF[]类型
        Point[] pointFGlobalInteger;

        public Point[] PointFGlobalInteger
        {
            get { return pointFGlobalInteger; }
            set { pointFGlobalInteger = value; }
        }
        PointF[] pointFSub;

        public PointF[] PointFSub
        {
            get { return pointFSub; }
            set { pointFSub = value; }
        }
        PointF[] pointFGlobalSub;

        public PointF[] PointFGlobalSub
        {
            get { return pointFGlobalSub; }
            set { pointFGlobalSub = value; }
        }

        //调用dll中的方法
        //public int[,] intLocation;
        //public double[,] subLocation;

        //public List<Point> listIntLocation = new List<Point>();
        //public List<PointF> listSubLocation = new List<PointF>();

        //Image<Bgr, Byte> TemplateImage;
        //Image<Bgr, Byte> currentImage;

        public void Registration(Bitmap tempImg, Bitmap curImg, int len, double[] x, double[] y, int m_subsetSize, int m_subsetExtendSize)
        {
            pointFGlobalSub = new PointF[len];
            PointFSub = new PointF[len];
            pointFGlobalInteger = new Point[len];
            Image<Bgr, Byte> templateImage = new Image<Bgr, byte>(tempImg);
            Image<Bgr, Byte> currentImage = new Image<Bgr, byte>(curImg); 
            globalIntLocation = new int[len, 2];
            subLocation = new double[len, 2];
            globalSubLocation = new double[len, 2];
            //using (currentImage = new Image<Bgr, byte>(tempImg))
            {
                ImageRegistration(
                    templateImage, currentImage, 
                    len, 
                    x, y,
                    ref globalIntLocation[0, 0], ref subLocation[0, 0], ref globalSubLocation[0, 0], 
                    m_subsetSize, m_subsetExtendSize
                    );
                for (int i = 0; i < len; i++)
                {
                    pointFGlobalInteger[i].X = globalIntLocation[i, 0];
                    pointFGlobalInteger[i].Y = globalIntLocation[i, 1];

                    PointFSub[i].X = (float)subLocation[i, 0];
                    PointFSub[i].Y = (float)subLocation[i, 1];

                    pointFGlobalSub[i].X = (float)(globalSubLocation[i, 0] );
                    pointFGlobalSub[i].Y = (float)(globalSubLocation[i, 1] );
                }
            }
        }
    }
}
