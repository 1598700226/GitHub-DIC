using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing;

using MyDIC.Algorithm;
using MyDIC.IO;

namespace MyDIC.Algorithm
{
    class SeedPointsMaping //找到种子点中x、y坐标最大值、最小值点，这4个点为边界点，根据边界点的位移情况，确定多边形内部点的位移
    {
        RegisteringPoints regPoints = new RegisteringPoints();
        MetaDateIO metaDateIO = new MetaDateIO();

        Bitmap ref_bmp;

        public Bitmap Ref_bmp
        {
            get { return ref_bmp; }
            set { ref_bmp = value; }
        }

        Bitmap cur_bmp;

        public Bitmap Cur_bmp
        {
            get { return cur_bmp; }
            set { cur_bmp = value; }
        }

        List<PointF> listNewVertexPoints;//变形图像上的定点坐标

        public List<PointF> ListNewVertexPoints
        {
            get { return listNewVertexPoints; }
            set { listNewVertexPoints = value; }
        }

        int image_num;

        public int Image_num
        {
            get { return image_num; }
            set { image_num = value; }
        }

        public SeedPointsMaping(List<PointF> lpf, Bitmap refImg, Bitmap curImg, int imageNum)
        {
            this.ref_bmp = refImg;
            this.cur_bmp = curImg;
            this.listNewVertexPoints = FindPolygonVertexs(lpf);
            this.image_num = imageNum;
        }

        //找到种子点x坐标、y坐标最大最小值点，返回4个坐标值点
        List<PointF> FindPolygonVertexs(List<PointF> listPolygonPoint)
        {
            List<PointF> fourCorners = new List<PointF>();
            List<PointF> fourNewCorners = new List<PointF>();
            PointF xMaxPoint = listPolygonPoint[0];
            PointF yMaxPoint = listPolygonPoint[0];
            PointF xMinPoint = listPolygonPoint[0];
            PointF yMinPoint = listPolygonPoint[0];
            foreach (PointF item in listPolygonPoint)
            {

                if (item.X > xMaxPoint.X)
                {
                    xMaxPoint = item;
                }
                else if (item.X < xMinPoint.X)
                {
                    xMinPoint = item;
                }
                if (item.Y > yMaxPoint.Y)
                {
                    yMaxPoint = item;
                }
                else if (item.Y < yMinPoint.Y)
                {
                    yMinPoint = item;
                }
            }
            fourCorners.Add(xMaxPoint);
            fourCorners.Add(xMinPoint);
            fourCorners.Add(yMaxPoint);
            fourCorners.Add(yMinPoint);

            //fourNewCorners = FindCornersNewCoordinates(fourCorners, ref_bmp, cur_bmp);
            return fourCorners;
        }

        double[] x;
        double[] y;
        PointF[] listNewFourPointCoordinates;
        PointF[] FindCornersNewCoordinates(List<PointF> fourCorners, Bitmap refImg, Bitmap curImg)
        {
            PointF[] newFourPointCoordinates = new PointF[4];

            int seedPointCount = fourCorners.Count;
            x = new double[seedPointCount];
            y = new double[seedPointCount];
            for (int i = 0; i < seedPointCount; i++)
            {
                x[i] = fourCorners[i].X;
                y[i] = fourCorners[i].Y;
            }
            int m_int_RelateSize = 31;
            int m_int_ExtendSize = 6;

            regPoints.Registration(refImg, curImg, fourCorners.Count, x, y, m_int_RelateSize, m_int_ExtendSize);
            listNewFourPointCoordinates = regPoints.PointFSub;
            return newFourPointCoordinates;
        }
    }
}
