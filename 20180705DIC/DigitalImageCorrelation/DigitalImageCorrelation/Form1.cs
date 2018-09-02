using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DigitalImageCorrelation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //定义画板，画板使用的picturebox与图形使用的picturebox大小相同
            pictureBoxDrawingLeft = new PictureBox();
            pictureBoxDrawingLeft.Width = pictureBoxVideoLeft.Width;
            pictureBoxDrawingLeft.Height = pictureBoxVideoLeft.Height;
            pictureBoxDrawingLeft.Parent = pictureBoxVideoLeft;
            pictureBoxDrawingLeft.Dock = DockStyle.None;
            pictureBoxDrawingLeft.BackColor = System.Drawing.Color.Black;
            pictureBoxDrawingLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            pictureBoxDrawingLeft.Location = new System.Drawing.Point(0, 0);
            pictureBoxDrawingLeft.Margin = new System.Windows.Forms.Padding(0);
            pictureBoxDrawingLeft.Name = "pictureBoxDrawingLeft";
            pictureBoxDrawingLeft.Size = pictureBoxVideoLeft.Size;
            pictureBoxDrawingLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBoxDrawingLeft.BackColor = Color.Transparent;
            bmpDrawingLeft = new Bitmap(10,10);
            penDrawingpolygon = new Pen(System.Drawing.Color.Red, 3);
            gDrawingpolygon = Graphics.FromImage(bmpDrawingLeft);

        }

        PictureBox pictureBoxDrawingLeft;
        public Bitmap bmpDrawingLeft;
        Pen penDrawingpolygon;
        public Graphics gDrawingpolygon;


        private void Form1_Load(object sender, EventArgs e)
        {
            //设置bombobox的状态
            comboBox1.Items.Add("Single Camera");
            comboBox1.Items.Add("Double Camera");
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Single Camera");
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            //定义显示图形的picturebox的大小
            splitContainerImages.SplitterDistance = (splitContainerImages.Width - splitContainerImages.SplitterWidth - splitContainerImages.Margin.Left- splitContainerImages.Margin.Right) / 2;
            pictureBoxVideoLeft.Size = panelShowImageLeft.Size;
            pictureBoxVideoRight.Size = panelShowImageRight.Size;

            pictureBoxDrawingLeft.MouseWheel += new MouseEventHandler(pictureBoxDrawingLeft_MouseWheel);
            pictureBoxDrawingLeft.MouseEnter += new EventHandler(pictureBoxDrawingLeft_MouseEnter);
            pictureBoxDrawingLeft.MouseLeave += new EventHandler(pictureBoxDrawingLeft_MouseLeave);
            pictureBoxDrawingLeft.MouseUp += new MouseEventHandler(this.pictureBoxDrawingLeft_MouseUp);
            pictureBoxDrawingLeft.MouseDown += new MouseEventHandler(this.pictureBoxDrawingLeft_MouseDown);
            pictureBoxDrawingLeft.MouseMove += new MouseEventHandler(this.pictureBoxDrawingLeft_MouseMove);
            pictureBoxDrawingLeft.Paint += new PaintEventHandler(pictureBoxDrawingLeft_Paint);

        }

        private void ButtonStopDrawing_Click(object sender, EventArgs e)
        {
            if (isDrawAble == false)
            {
                isDrawAble = true;
                isMoving = false;
                ButtonStopDrawing.ToolTipText = "Drawing";

            }
            if (isMoving == false)
            {
                isDrawAble = false;
                isMoving = true;
                ButtonStopDrawing.ToolTipText = "Moving";

            }
        }


        OpenFileDialog openFileDialog = new OpenFileDialog();
        private List<string> fullFileName = new List<string>();
        private List<string> fileName = new List<string>();
        List<Bitmap> listBmpFile = new List<Bitmap>();
        double zoomRate;        //图像放缩比例
        double currentBMPLeft_Width_Height_ratio;
        double pictureboxVideoLeft_Width_Height_ratio;
        Bitmap openedBMP_ThumbLeft; //压缩尺寸的图像
        bool isOpenTestImageLeft;
        Bitmap openedBMPLeft;
        Bitmap currentBMPLeft;
        int w1 = 0;
        int h1 = 0;

        Size openedBMP_ThumbLeft_size = new Size(1,1);
        private void toolStripButton_loagImage_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = @"C:\TEMPIMG\MI";
            //openFileDialog.InitialDirectory = Application.StartupPath; ;
            openFileDialog.Title = "打开图像文件";

            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = true;
            openFileDialog.ReadOnlyChecked = true;
            openFileDialog.ShowReadOnly = true;

            openFileDialog.DefaultExt = "bmp";
            openFileDialog.Filter = "bmp格式文件(*.bmp)|*.bmp|tif格式文件(*.tif)|*.tif|jpg格式文件(*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //清除list缓存
                fullFileName.Clear();
                fileName.Clear();
                listBmpFile.Clear();
            }
            if (openFileDialog.SafeFileName.ToUpper().Contains(".BMP") || openFileDialog.SafeFileName.ToUpper().Contains(".TIF")
                     || openFileDialog.SafeFileName.ToUpper().Contains(".JPG"))
            {
                FileInfo fi = new FileInfo(openFileDialog.FileNames[0]);
                for (int i = 0; i < openFileDialog.SafeFileNames.Length; i++)
                {
                    listBmpFile.Add(new Bitmap(openFileDialog.FileNames[i]));
                }
                openedBMPLeft = listBmpFile[0];
                currentBMPLeft = openedBMPLeft;
            }
            //计算放缩比例，按panelLeft的尺寸放缩。
            currentBMPLeft_Width_Height_ratio = (double)openedBMPLeft.Width / (double)openedBMPLeft.Height;
            pictureboxVideoLeft_Width_Height_ratio = (double)panelShowImageLeft.Width / (double)panelShowImageLeft.Height;
            if (currentBMPLeft_Width_Height_ratio > pictureboxVideoLeft_Width_Height_ratio)
            {
                zoomRate = (double)panelShowImageLeft.Width / (double)openedBMPLeft.Width;
            }
            else
            {
                zoomRate = (double)panelShowImageLeft.Height / (double)openedBMPLeft.Height;
            }
            //放缩图片
            w1 = (int)(openedBMPLeft.Width * zoomRate);
            h1 = (int)(openedBMPLeft.Height * zoomRate);

            openedBMP_ThumbLeft_size = new Size(w1, h1);
            openedBMP_ThumbLeft = new Bitmap(openedBMPLeft, w1, h1);
            pictureBoxVideoLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;//为避免缩放picturebox引起绘图的缩放，缩放image
            pictureBoxVideoLeft.Image = openedBMP_ThumbLeft;

            //pictureBoxDrawing与pictureBoxVideo配对
            pictureBoxDrawingLeft.Location = pictureBoxVideoLeft.Location;
            bmpDrawingLeft = new Bitmap(w1, h1);
            pictureBoxDrawingLeft.Image = bmpDrawingLeft;
            pictureBoxDrawingLeft.BringToFront();
            pictureBoxDrawingLeft.Focus();


        }
        private void pictureBoxDrawingLeft_MouseLeave(object sender, EventArgs e)
        {
            //if (pictureBoxDrawingLeft.Focused == false)
            {
                //pictureBoxDrawingLeft.Focus();
                this.Cursor = Cursors.Arrow;
                // update_picture_position();
            }
        }
        private void pictureBoxDrawingLeft_MouseEnter(object sender, EventArgs e)
        {
            if(isDrawAble)
            {
                this.Cursor = Cursors.Cross;
            }
            if (isMoving)
            {
                this.Cursor = Cursors.Hand;
            }
        }

        float zoomMax = 1.0F;//最大放大倍率
        //float zoomMin = 0.3F;
        void pictureBoxDrawingLeft_MouseWheel(object sender, MouseEventArgs e)
        {
            panelShowImageLeft.VerticalScroll.Value = 0;
            panelShowImageLeft.HorizontalScroll.Value = 0;
            if (((pictureBoxVideoLeft.Width > currentBMPLeft.Width * zoomMax) && e.Delta > 0) || ((currentBMPLeft.Width * zoomRate < panelShowImageLeft.Width) && e.Delta < 0))
            {
            }
            else
            {
                if (e.Delta > 0)
                {
                    zoomRate = zoomRate + 0.1;//放大倍率每次滚动滚轮时加0.1
                }
                else
                {
                    zoomRate = zoomRate - 0.1;
                }

                if (pictureBoxVideoLeft.Width != 0 && pictureBoxVideoLeft.Height != 0 && zoomRate > 0.1)
                {
                    w1 = (int)(openedBMPLeft.Width * zoomRate);
                    h1 = (int)(openedBMPLeft.Height * zoomRate);
                    Size thumbSize = new Size(w1, h1);

                    openedBMP_ThumbLeft = new Bitmap(openedBMPLeft, w1, h1);
                    pictureBoxVideoLeft.Image = openedBMP_ThumbLeft;

                    bmpDrawingLeft = new Bitmap(bmpDrawingLeft, w1, h1);
                    pictureBoxDrawingLeft.Image = bmpDrawingLeft;
                    pictureBoxDrawingLeft.Location = pictureBoxVideoLeft.Location;
                    pictureBoxDrawingLeft.BringToFront();
                    pictureBoxDrawingLeft.Focus();
                    //pictureBoxDrawingLeft.Update();
                    //pictureBoxDrawingLeft.Invalidate();

                }
            }
        }

        int xPos;
        int yPos;

        Point polygonPoint = new Point();
        private void pictureBoxDrawingLeft_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            if(e.Button == MouseButtons.Right)
            {
                isDrawAble = false;
                //listPolygonPoint.Clear();
            }
            //PointF checkPoint = new PointF(e.X, e.Y);

            isDrawingLockedLeft = false;
            pictureBoxDrawingLeft.Invalidate();
        }

        bool isMouseDown = false;
        bool isDrawAble = true;
        bool isMoving = true;
        bool isPicMovable = false;
        bool isFirstPoint = false;
        int polygonCount = 0;
        bool isDrawingLockedLeft = true;//锁定是否画图，避免重绘占用计算资源。
        bool isMouseLeftClick = false;//右键单击停止画多边形
        bool isMouseRightClick = false;
        private void pictureBoxDrawingLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDrawAble)//绘制多边形
            {
                if (e.Button == MouseButtons.Left)
                {
                    isMouseDown = true;
                    isMouseLeftClick = true;
                    this.Cursor = Cursors.Cross;

                    polygonPoint = new Point(e.X, e.Y);
                    listPolygonPoint.Add(polygonPoint);
                }
                if (e.Button == MouseButtons.Right)
                {
                    isMouseRightClick = true;
                    isDrawAble = false;

                    polygonPoint = new Point(e.X, e.Y);
                    listPolygonPoint.Add(polygonPoint);
                }
            }
            if(isMoving == true)//不绘制时移动
            {
                xPos = e.X;//当前x坐标.
                yPos = e.Y;//当前y坐标.
            }
            isDrawingLockedLeft = false;
            pictureBoxDrawingLeft.Invalidate();
        }

        Point currentPoint = new Point();
        List<PointF> listPolygonPoint = new List<PointF>();
        private void pictureBoxDrawingLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && !isDrawAble)//移动图片
            {
                pictureBoxVideoLeft.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                pictureBoxVideoLeft.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
                pictureBoxDrawingLeft.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                pictureBoxDrawingLeft.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
            }
            if (isDrawAble)//draw polygon
            {
                //Console.WriteLine("isDrawPolygon");
                currentPoint = new Point(e.X, e.Y);
            }
            isDrawingLockedLeft = false;
            pictureBoxDrawingLeft.Invalidate();
        }


        Bitmap bmpDrawingTemp;
        Graphics g1;
        Pen p1;
        public void pictureBoxDrawingLeft_Paint(object sender, PaintEventArgs e)
        {
            if (!isDrawingLockedLeft)
            {
                if (isDrawAble)
                {
                    //Console.WriteLine("pictureBoxDrawingLeft_Paint +if (isDrawPolygon) ");
                    if (isMouseLeftClick)
                    {
                        PointF[] ps = new PointF[listPolygonPoint.Count + 1];
                        for (int i = 0; i < listPolygonPoint.Count; i++)
                        {
                            ps[i] = listPolygonPoint[i];
                            //Console.WriteLine("listPolygonPoint[i]" + listPolygonPoint[i].ToString());
                        }
                        ps[listPolygonPoint.Count] = currentPoint;
                        bmpDrawingTemp = new Bitmap(bmpDrawingLeft.Width, bmpDrawingLeft.Height);
                        g1 = Graphics.FromImage(bmpDrawingTemp);
                        g1.DrawPolygon(penDrawingpolygon, ps);
                        pictureBoxDrawingLeft.Image = bmpDrawingTemp;
                        isDrawingLockedLeft = false;
                    }
                    if (isMouseRightClick)
                    {
                        PointF[] ps = new PointF[listPolygonPoint.Count];
                        if(listPolygonPoint.Count >1)
                        {
                            for (int i = 0; i < listPolygonPoint.Count; i++)
                            {
                                ps[i] = listPolygonPoint[i];
                                //Console.WriteLine("listPolygonPoint[i]" + listPolygonPoint[i].ToString());
                            }
                            bmpDrawingTemp = new Bitmap(bmpDrawingLeft.Width, bmpDrawingLeft.Height);
                            g1 = Graphics.FromImage(bmpDrawingTemp);
                            g1.DrawPolygon(penDrawingpolygon, ps);
                            pictureBoxDrawingLeft.Image = bmpDrawingTemp;
                            isDrawingLockedLeft = false;
                        }
                    }
                }
            }
            isDrawingLockedLeft = true;
        }

        //设定间隔为10*10
        int gridInterval_X = 10;
        int gridInterval_Y = 10;
        List<PointF> listPointGrid = new List<PointF>();
        List<PointF> listPointGridinPolygon = new List<PointF>();
        void SetGridInPolygon(Bitmap bmp, int grid_x, int grid_y)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int maxEdge = 30;
            int w1 = (w - 2 * maxEdge) / grid_x;
            int h1 = (w - 2 * maxEdge) / grid_y;
            int w2;
            int h2;
            Point[,] psGrid = new Point[w1, h1];
            List<PointF> listPolygonPoint_orginal = convert2_Origin_Value(listPolygonPoint);
            for (int i = maxEdge; i< w-maxEdge; i=i+gridInterval_X)
            {
                //float orginal_i = convert2_Origin_Value(i);
                for (int j = maxEdge; j < h - maxEdge; j = j + gridInterval_Y)
                {
                    float orginal_j = convert2_Origin_Value(j);
                    listPointGrid.Add(new PointF(i, j));

                    PointF checkPoint = new PointF(i, j);

                    bool isInPloygon = IsInPolygon(checkPoint, listPolygonPoint_orginal);
                    if(isInPloygon)
                    {
                        listPointGridinPolygon.Add(checkPoint);
                    }
                }
            }
        }

        private void toolStripButtonSetGrid_Click(object sender, EventArgs e)
        {
            SetGridInPolygon(currentBMPLeft, gridInterval_X, gridInterval_Y);
        }

        /// <summary>  
        /// 判断点是否在多边形内.  
        /// ----------原理----------  
        /// 注意到如果从P作水平向左的射线的话，如果P在多边形内部，那么这条射线与多边形的交点必为奇数，  
        /// 如果P在多边形外部，则交点个数必为偶数(0也在内)。  
        /// </summary>  
        /// <param name="checkPoint">要判断的点</param>  
        /// <param name="polygonPoints">多边形的顶点</param>  
        /// <returns></returns>  
        public static bool IsInPolygon(PointF checkPoint, List<PointF> polygonPoints)
        {
            bool inside = false;
            int pointCount = polygonPoints.Count;
            PointF p1, p2;
            for (int i = 0, j = pointCount - 1; i < pointCount; j = i, i++)//第一个点和最后一个点作为第一条线，之后是第一个点和第二个点作为第二条线，之后是第二个点与第三个点，第三个点与第四个点...  
            {
                p1 = polygonPoints[i];
                p2 = polygonPoints[j];
                if (checkPoint.Y < p2.Y)
                {//p2在射线之上  
                    if (p1.Y <= checkPoint.Y)
                    {//p1正好在射线中或者射线下方  
                        if ((checkPoint.Y - p1.Y) * (p2.X - p1.X) > (checkPoint.X - p1.X) * (p2.Y - p1.Y))//斜率判断,在P1和P2之间且在P1P2右侧  
                        {
                            //射线与多边形交点为奇数时则在多边形之内，若为偶数个交点时则在多边形之外。  
                            //由于inside初始值为false，即交点数为零。所以当有第一个交点时，则必为奇数，则在内部，此时为inside=(!inside)  
                            //所以当有第二个交点时，则必为偶数，则在外部，此时为inside=(!inside)  
                            inside = (!inside);
                        }
                    }
                }
                else if (checkPoint.Y < p1.Y)
                {
                    //p2正好在射线中或者在射线下方，p1在射线上  
                    if ((checkPoint.Y - p1.Y) * (p2.X - p1.X) < (checkPoint.X - p1.X) * (p2.Y - p1.Y))//斜率判断,在P1和P2之间且在P1P2右侧  
                    {
                        inside = (!inside);
                    }
                }
            }
            return inside;
        }

        public PointF convert2_Original_PointF(PointF p)
        {
            PointF new_P = new PointF((float)(p.X / zoomRate), (float)(p.Y / zoomRate));
            return new_P;
        }

        public float convert2_Thumb_Value(float d)
        {
            float new_D = (float)(d * zoomRate);
            return new_D;
        }
        public float convert2_Thumb_Value(int d)
        {
            int new_D = (int)(d * zoomRate);
            return new_D;
        }
        public float convert2_Origin_Value(float d)
        {
            float new_D = (float)(d / zoomRate);
            return new_D;
        }
        public int convert2_Origin_Value(int d)
        {
            int new_D = (int)(d / zoomRate);
            return new_D;
        }
        
        List<PointF>  convert2_Origin_Value(List<PointF> listPolygonPoint)
        {
            List<PointF> listPolygonPoint_orginal = new List<PointF>();
            for (int i = 0; i< listPolygonPoint.Count; i++)
            {
                PointF pf = new PointF(convert2_Origin_Value(listPolygonPoint[i].X), convert2_Origin_Value(listPolygonPoint[i].Y));
                listPolygonPoint_orginal.Add(pf);
            }
            return listPolygonPoint_orginal;
        }
    }
}
