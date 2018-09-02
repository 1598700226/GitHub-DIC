using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Basler.Pylon;
using System.Diagnostics;

using System.IO;
using System.Xml;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Util;

using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

using ZedGraph;
using System.Drawing.Drawing2D; //用到Colorblend

using MyDIC.Algorithm;
using MyDIC.IO;

namespace MyDIC
{
    public partial class MainForm : Form
    {
        //******************************Basler相机相关变量****************************************//
        private Camera camera = null;
        private PixelDataConverter converter;
        private Stopwatch stopWatch = new Stopwatch();
                            //以下参数进入相机参数设置后才能更改，否则默认如下                                     
        public double m_double_ExposureTime = 30000;
        public int m_int_GainLevel = 8;
        public int m_int_CameraFrame = 200;
        public int m_int_SnapShotNumbers = 0;
        public int m_int_SnapShotTimes = 0; 
        public bool m_bool_IsSnapShot = false;                        ///是否连续采集标志位
        public bool m_bool_IsShow = false;                            ///是否显示照片，用来控制相机帧率                                                                      
        //**************************************************************************************//

        public bool m_bool_OpenFolder;                                  ///文件夹打开标志
        public bool m_bool_OpenCamera;                                  ///相机开启标志
        public int m_PicSizeMod;                                        ///0:12.5% 1:25% 2:50% 3:a zoom 4:100% 5:200% 6:400%
        public string m_str_FolderPath;                                 ///选择的文件夹路径

        public List<string> m_listStr_bmpPicPath;                       ///文件夹下面的所有BMP图片的完整路径(目前只在查看图片中使用)
        public int m_int_bmpPicOrder;                                   ///文件夹下面的所有BMP图片的序号(目前只在查看图片中使用)
        public int m_int_CamPicNumbers;                                 ///当前文件夹下拍摄照片数量
        public bool m_bool_IsSinglePic;                                 ///是否是单图片抓取
        public bool m_bool_IsMultiPic;                                  ///是否是多图片抓取
        public int m_int_CameraFileSequence;                            ///相机文件夹序列编号 从0开始
        public int m_int_Progress;                                      ///界面进度条的值 0~100
        public string[] filesName;                                        //图像文件夹内图像文件名
        bool isSingleImg;
        public List<string> m_listStr_bmpName;                       ///BMP文件名

        //单相机
        Panel m_PanelPic;                 //定义放置picturebox的panel
        PictureBox m_picboxImg;           //用于显示相机采集的图像
        Pen penDrawingpolygon;
        Graphics gDrawingpolygon;
        PictureBox pictureBoxDrawing;     //装载的picturebox画板
        public Bitmap bmpDrawing;         //画板图像

        //当前显示的图像
        Bitmap currentDisplayedBMP;
        String currentDisplayedBMP_FullfileName;
        String currentDisplayedBMP_FileName;

        //双相机
        SplitContainer splitContainerPictureBox = new SplitContainer();

        Panel m_PanelPicLeft = new Panel();//定义放置picturebox的panel
        Panel m_PanelPicRight = new Panel();

        PictureBox m_picboxLeft = new PictureBox();//定义用于显示相机图像的picturebox
        PictureBox m_picboxRight = new PictureBox();

        //PictureBox pictureBoxDrawingLeft;                               ///重新定义一个picBox画板
        //PictureBox pictureBoxDrawingRight;                               ///重新定义一个picBox画板

        //Bitmap bmpDrawingLeft;                                          ///
        //Bitmap bmpDrawingRight;                                          ///

        Pen penDrawingpolygonLeft;                                          ///    
        Pen penDrawingpolygonRight;                                          ///    

        Graphics gDrawingpolygonLeft;
        Graphics gDrawingpolygonRight;

        RegisteringPoints regPoints = new RegisteringPoints();
        MetaDateIO metaDateIO = new MetaDateIO();

        public MainForm()
        {
            m_bool_OpenFolder = false;
            m_bool_OpenCamera = false;
            m_PicSizeMod = 3;
            m_listStr_bmpPicPath = new List<string>();
            m_listStr_bmpName = new List<string>();
            m_int_bmpPicOrder = -1;
            m_int_CamPicNumbers = 0;
            m_bool_IsSinglePic = false;
            m_bool_IsMultiPic = false;
            m_int_CameraFileSequence = 0;
            m_int_Progress = 0;

            m_PanelPic = new Panel();
            m_picboxImg = new PictureBox();

            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _UpdateUI();
            __UpdateUI();

            m_tool_ProgressBar.Value = 0;

            //pictureBoxDrawing的鼠标事件
            pictureBoxDrawing.MouseWheel += new MouseEventHandler(pictureBoxDrawing_MouseWheel);  //滚轮事件
            pictureBoxDrawing.MouseEnter += new EventHandler(pictureBoxDrawing_MouseEnter);
            pictureBoxDrawing.MouseLeave += new EventHandler(pictureBoxDrawing_MouseLeave);
            pictureBoxDrawing.MouseUp += new MouseEventHandler(pictureBoxDrawing_MouseUp);
            pictureBoxDrawing.MouseDown += new MouseEventHandler(pictureBoxDrawing_MouseDown);
            pictureBoxDrawing.MouseMove += new MouseEventHandler(pictureBoxDrawing_MouseMove);
            //增加下拉列表操作事件
            comboBoxTemplateIMG.SelectedIndexChanged +=new EventHandler(comboBoxTemplateIMG_SelectedIndexChanged);
            //增加鼠标进入到右下相关计算那6个按钮处的事件
            m_btn_Control.MouseEnter += new EventHandler(m_btn_MouseEnter);
            m_btn_InputData.MouseEnter += new EventHandler(m_btn_MouseEnter);
            m_btn_EvaSetting.MouseEnter += new EventHandler(m_btn_MouseEnter);
            m_btn_MaskDef.MouseEnter += new EventHandler(m_btn_MouseEnter);
            m_btn_StartPts.MouseEnter += new EventHandler(m_btn_MouseEnter);
            m_btn_VisualSetting.MouseEnter += new EventHandler(m_btn_MouseEnter);
            //增加鼠标从到右下相关计算那6个按钮出去时的事件
            m_btn_Control.MouseLeave += new EventHandler(m_btn_MouseLeave);
            m_btn_InputData.MouseLeave += new EventHandler(m_btn_MouseLeave);
            m_btn_EvaSetting.MouseLeave += new EventHandler(m_btn_MouseLeave);
            m_btn_MaskDef.MouseLeave += new EventHandler(m_btn_MouseLeave);
            m_btn_StartPts.MouseLeave += new EventHandler(m_btn_MouseLeave);
            m_btn_VisualSetting.MouseLeave += new EventHandler(m_btn_MouseLeave);
        }

        private void CameraFrameTimer_Tick(object sender, EventArgs e)
        {
            m_bool_IsShow = true;
        }
        private void CameraSnapTimer_Tick(object sender, EventArgs e)
        {
            if (m_int_SnapShotNumbers <= 0)
            {
                CameraSnapTimer.Enabled = false;
                m_bool_IsMultiPic = false;
                return;
            }
            m_bool_IsMultiPic = true;
            m_txt_CatchPicNumbers.Text = "当前采集张数:" + (Convert.ToInt32(m_txt_CatchPicNumbers.Text.Substring(m_txt_CatchPicNumbers.Text.IndexOf(':') + 1)) + 1).ToString();
        }
        private void ProgressBarTimer_Tick(object sender, EventArgs e)
        {
            m_tool_ProgressBar.Value = m_int_Progress;
            if (m_tool_ProgressBar.Value == 100)
            {
                ProgressBarTimer.Enabled = false;
            }
        }

        void __UpdateUI()
        {
            m_btn_SelectFolder.Enabled = !m_bool_OpenCamera;
            m_btn_SavePicToAddress.Enabled = true;
            m_btn_CameraSet.Enabled = !m_bool_OpenCamera;
            m_btn_OpenCamera.Enabled = !m_bool_OpenCamera;
            m_btn_CloseCamera.Enabled = m_bool_OpenCamera;

            m_btn_WatchPics.Enabled = m_bool_OpenFolder;
            m_btn_WatchLastPics.Enabled = m_bool_OpenFolder;
            m_btn_NewFolder.Enabled = m_bool_OpenCamera && m_bool_OpenFolder;
            m_btn_ContinueFolder.Enabled = m_bool_OpenCamera && m_bool_OpenFolder;
            m_btn_CatchSinglePic.Enabled = m_bool_OpenCamera && m_bool_OpenFolder;

            m_menu_CameraSet.Enabled = !m_bool_OpenCamera;
            m_menu_CatchSinglePic.Enabled = m_bool_OpenCamera && m_bool_OpenFolder;
            m_menu_SnapShot.Enabled = m_bool_OpenCamera && m_bool_OpenFolder && m_bool_IsSnapShot;
             
            m_btn_StartCalibration.Enabled = true;
            m_btn_CalibrateSet.Enabled = true;
            m_btn_InPutCalibration.Enabled = true;

            m_btn_SetOrigin.Enabled = m_bool_OpenFolder;
            m_btn_SetPath.Enabled = m_bool_OpenFolder;
            m_btn_ShowPoint.Enabled = m_bool_OpenFolder;

            m_cob_PicShow.SelectedIndex = 0;
            m_cob_CameraMod.SelectedIndex = 0;
            m_cob_ShowPoint.SelectedIndex = 0;
            m_cob_PicSize.SelectedIndex = m_PicSizeMod;

            panel_Control.BringToFront();
        }
        void _UpdateUI()
        {
            MiddlePanelView();

            pictureBoxDrawing = new PictureBox();
            pictureBoxDrawing.Parent = m_picboxImg;
            pictureBoxDrawing.Size = m_picboxImg.Size;
            pictureBoxDrawing.Dock = DockStyle.None;
            pictureBoxDrawing.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            pictureBoxDrawing.Location = new System.Drawing.Point(0, 0);
            pictureBoxDrawing.Margin = new System.Windows.Forms.Padding(0);
            pictureBoxDrawing.Name = "pictureBoxDrawing";
            pictureBoxDrawing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBoxDrawing.BackColor = Color.Transparent;
            bmpDrawing = new Bitmap(10, 10);
            penDrawingpolygon = new Pen(System.Drawing.Color.Red, 3);
            gDrawingpolygon = Graphics.FromImage(bmpDrawing);       
        }
        void MiddlePanelView()
        {
            if (m_cob_CameraMod.SelectedIndex == m_cob_CameraMod.Items.IndexOf("单相机显示"))
            {
                isSingleImg = true;
                splitContainerPictureBox.Hide();
                m_PanelPic.Show();
                m_PanelPic.BringToFront();
                m_PanelPic.Parent = panelMiddle;
                m_PanelPic.Dock = DockStyle.Fill;
                m_PanelPic.BorderStyle = BorderStyle.FixedSingle;
                m_PanelPic.AutoScroll = true;
                m_PanelPic.AutoSize = true;

                m_picboxImg.Parent = m_PanelPic;
                m_picboxImg.Size = m_PanelPic.ClientSize;
                m_picboxImg.Dock = DockStyle.None;
                m_picboxImg.BackColor = System.Drawing.Color.Azure;
                m_picboxImg.Location = new System.Drawing.Point(0, 0);
                m_picboxImg.Margin = new System.Windows.Forms.Padding(0);
                m_picboxImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                m_picboxImg.SizeChanged += new System.EventHandler(m_picboxImg_SizeChanged);

                panelMiddle.Refresh();
            }
            if (m_cob_CameraMod.SelectedIndex == m_cob_CameraMod.Items.IndexOf("双相机显示"))
            {
                isSingleImg = false;
                m_PanelPic.Hide();
                splitContainerPictureBox.Show();
                splitContainerPictureBox.BringToFront();
                splitContainerPictureBox.Parent = panelMiddle;
                splitContainerPictureBox.Dock = DockStyle.Fill;
                splitContainerPictureBox.SplitterDistance = splitContainerPictureBox.Width / 2 - 3;
                splitContainerPictureBox.BorderStyle = BorderStyle.FixedSingle;
                m_PanelPicLeft.Parent = splitContainerPictureBox.Panel1;
                m_PanelPicRight.Parent = splitContainerPictureBox.Panel2;
                m_picboxLeft.Parent = m_PanelPicLeft;
                m_picboxRight.Parent = m_PanelPicRight;


                m_picboxLeft.Parent = splitContainerPictureBox.Panel1;
                m_picboxLeft.Size = m_PanelPic.ClientSize;
                m_picboxLeft.Dock = DockStyle.None;
                m_picboxLeft.BackColor = System.Drawing.Color.Azure;
                m_picboxLeft.Location = new System.Drawing.Point(0, 0);
                m_picboxLeft.Margin = new System.Windows.Forms.Padding(0);
                m_picboxLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;

                m_picboxRight.Parent = splitContainerPictureBox.Panel2;
                m_picboxRight.Size = m_PanelPic.ClientSize;
                m_picboxRight.Dock = DockStyle.None;
                m_picboxRight.BackColor = System.Drawing.Color.Black;
                m_picboxRight.Location = new System.Drawing.Point(0, 0);
                m_picboxRight.Margin = new System.Windows.Forms.Padding(0);
                m_picboxRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;

                panelMiddle.Refresh();
            }
        }

        void SelectTemplateImage()
        {
            String str = m_listStr_bmpPicPath[comboBoxTemplateIMG.SelectedIndex];
            currentDisplayedBMP = currentDisplayed(str);
            m_picboxImg.Image = currentDisplayedBMP;
         }
        void SelectCurrentImage()
        {
            //m_picboxLeft.Image = Image.FromFile(m_listStr_bmpPicPath[comboBoxCurrentIMG.SelectedIndex]);
        }
        Bitmap currentDisplayed(String str)
        {
            currentDisplayedBMP = new Bitmap(str);
            //m_picboxLeft.Image = currentDisplayedBMP;
            return currentDisplayedBMP;
        }

        void __UpdateTreeView(string FolderPath)
        {
            if(!m_bool_OpenFolder)
                return;

            m_treeView.Nodes.Clear();
            m_treeView.ImageList = DirectoryIcon;

            string[] strArrayNode = FolderPath.Split('\\');
            TreeNode rootNode = new TreeNode(strArrayNode[0], 0, 0);
            rootNode.Tag = strArrayNode[0];
            rootNode.Name = strArrayNode[0];
            this.m_treeView.Nodes.Add(rootNode);

            if (strArrayNode.Length > 1 && strArrayNode[1] != "")
                InsertTreeNode(rootNode, strArrayNode, 1);

            rootNode.Expand();  
        }

        void __UpdateCombox(string FolderPath)
        {
            if (!m_bool_OpenFolder)
                return;

            m_treeView.Nodes.Clear();
            m_treeView.ImageList = DirectoryIcon;

            string[] strArrayNode = FolderPath.Split('\\');
            TreeNode rootNode = new TreeNode(strArrayNode[0], 0, 0);
            rootNode.Tag = strArrayNode[0];
            rootNode.Name = strArrayNode[0];
            this.m_treeView.Nodes.Add(rootNode);

            if (strArrayNode.Length > 1 && strArrayNode[1] != "")
                InsertTreeNode(rootNode, strArrayNode, 1);

            rootNode.Expand();
        }
        void InsertTreeNode(TreeNode rNode, string[] strArrayNode,int i)
        {
            if (i >= strArrayNode.Length)
            {
                return;
            }
            TreeNode rootNode = new TreeNode(strArrayNode[i], 0, 0);
            rootNode.Tag = strArrayNode[i];
            rootNode.Name = strArrayNode[i];
            rNode.Nodes.Add(rootNode);
            rNode.Expand();  
            i++;
            InsertTreeNode(rootNode, strArrayNode, i);
        }
        void __PicBoxShowIndexChanged(string strpicPath)
        {
            Image img = Image.FromFile(strpicPath);
            double width = img.Width;
            double height = img.Height;

            switch (m_PicSizeMod)
            {
                case 0:
                    width = width * 0.125;
                    height = height * 0.125;
                    break;
                case 1:
                    width = width * 0.25;
                    height = height * 0.25;
                    break;
                case 2: 
                    width = width * 0.5;
                    height = height * 0.5;
                    break;
                case 3:
                    break;
                case 4:
                    width = width * 1;
                    height = height * 1;
                    break;
                case 5:
                    width = width * 2;
                    height = height * 2;
                    break;
                case 6:
                    width = width * 4;
                    height = height * 4;
                    break;
            }

            m_picboxImg.Width = Convert.ToInt32(width);
            m_picboxImg.Height = Convert.ToInt32(height);
            m_picboxImg.Image = img;
        }
        void __PicBoxShowWatchPics(string strpicPath)
        {
            Bitmap img = currentDisplayed(strpicPath);

            double width = img.Width;
            double height = img.Height;

            switch (m_PicSizeMod)
            {
                case 0:
                    width = width * 0.125;
                    height = height * 0.125;
                    break;
                case 1:
                    width = width * 0.25;
                    height = height * 0.25;
                    break;
                case 2:
                    width = width * 0.5;
                    height = height * 0.5;
                    break;
                case 3:
                    break;
                case 4:
                    width = width * 1;
                    height = height * 1;
                    break;
                case 5:
                    width = width * 2;
                    height = height * 2;
                    break;
                case 6:
                    width = width * 4;
                    height = height * 4;
                    break;
            }

            if (show_height != 0 && show_width != 0 && m_picboxImg.Image != null)
            {
                m_picboxImg.Width = Convert.ToInt32(show_width);
                m_picboxImg.Height = Convert.ToInt32(show_height);
            }
            else
            {
                m_picboxImg.Width = Convert.ToInt32(width);
                m_picboxImg.Height = Convert.ToInt32(height);
            }
            //img = new Bitmap(img, new Size(100, 100));       

            m_picboxImg.Image = img;

            Bitmap bitmapdrawFont = new Bitmap(m_picboxImg.Image.Width, m_picboxImg.Image.Height);
            Graphics g = Graphics.FromImage(bitmapdrawFont);
            Brush brush = new SolidBrush(Color.Red);
            Font drawFont = new Font("Arial", 5, FontStyle.Bold, GraphicsUnit.Millimeter);

            string strStepno = "Step no:" + " " + m_int_bmpPicOrder;
            g.DrawString(strStepno, drawFont, brush, 10, 10);//在图片上显示当前这张图片序号
            string strLaststepno = "Last step no:" + " " + (m_listStr_bmpPicPath.Count - 1).ToString();
            g.DrawString(strLaststepno, drawFont, brush, 300, 10);//在图片上显示最后的一张图片序号
            string strAcqtime = "Rel.acq.time:" + " " + "00:00:00.000";
            g.DrawString(strAcqtime, drawFont, brush, 10, 40);//在图片上显示相对时间，固定相对时间为00：00：00.000
            string strStepacqtime = "Step " + m_int_bmpPicOrder + " acq.time:" + m_listStr_bmpPicPath[m_int_bmpPicOrder].Substring(m_listStr_bmpPicPath[m_int_bmpPicOrder].LastIndexOf("\\"));
            g.DrawString(strStepacqtime, drawFont, brush, 300, 40);//在图片上显示该图片名称
            pictureBoxDrawing.Image = bitmapdrawFont;
        }
        void __UpdateCameraHZ()
        {
            CameraFrameTimer.Enabled = false;
            CameraFrameTimer.Interval = (int)(1000.0 / (double)m_int_CameraFrame);    //  Camera HZ
            if (m_bool_OpenCamera)
            {
                CameraFrameTimer.Enabled = true;
            }
        }
        void __StartCameraSnap()
        {
            CameraSnapTimer.Enabled = false;
            CameraSnapTimer.Interval = m_int_SnapShotTimes;
            CameraSnapTimer.Enabled = true;
        }
        void __ListbInfoAdd(string str)
        {
            m_listb_info.Items.Insert(0, str);
        }
        private void m_treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (m_str_FolderPath != m_treeView.SelectedNode.FullPath)
            {
                m_str_FolderPath = m_treeView.SelectedNode.FullPath;
                m_listb_info.Items.Insert(0, "更换保存路径为:" + m_str_FolderPath);//相机路径更换了则显示...
                m_int_CamPicNumbers = 0;
                m_txt_CatchPicNumbers.Text = "当前采集张数:" + m_int_CamPicNumbers;
            }
        }

        private void m_cob_PicSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_PicSizeMod = m_cob_PicSize.SelectedIndex;
            if (m_listStr_bmpPicPath.Count >= 0 && m_int_bmpPicOrder >= 0 && !m_bool_OpenCamera)
            {
                __PicBoxShowIndexChanged(m_listStr_bmpPicPath[m_int_bmpPicOrder]);
            }
            else
            {
                double width = m_picboxImg.Width;
                double height = m_picboxImg.Height;
                switch (m_PicSizeMod)
                {
                    case 0:
                        width = width * 0.125;
                        height = height * 0.125;
                        break;
                    case 1:
                        width = width * 0.25;
                        height = height * 0.25;
                        break;
                    case 2:
                        width = width * 0.5;
                        height = height * 0.5;
                        break;
                    case 3:
                        break;
                    case 4:
                        width = width * 1;
                        height = height * 1;
                        break;
                    case 5:
                        width = width * 2;
                        height = height * 2;
                        break;
                    case 6:
                        width = width * 4;
                        height = height * 4;
                        break;
                }
                m_picboxImg.Width = (int)width;
                m_picboxImg.Height = (int)height;
            }
            __UpdateUI();
        }
        public bool m_bool_ShowPoint = false; //是否显示相关计算点
        private void m_cob_ShowPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (m_cob_ShowPoint.SelectedIndex)
            {
                case 0: m_bool_ShowPoint = false; break;
                case 1: m_bool_ShowPoint = true; break;
                default: break;
            }
            if (listPointGridinPolygon.Count() > 0 && m_bool_ShowPoint)
            {
                using (Graphics g = Graphics.FromImage(bmpDrawingTemp))
                {
                    foreach (PointF item in listPointGridinPolygon)
                    {
                        Point p1 = new Point((int)(item.X * width_tzoom - 3), (int)(item.Y * heigth_tzoom));
                        Point p2 = new Point((int)(item.X * width_tzoom + 3), (int)(item.Y * heigth_tzoom));
                        Point p3 = new Point((int)(item.X * width_tzoom), (int)(item.Y * heigth_tzoom - 3));
                        Point p4 = new Point((int)(item.X * width_tzoom), (int)(item.Y * heigth_tzoom + 3));

                        g.DrawLine(new Pen(Color.Yellow, 2), p1, p2);
                        g.DrawLine(new Pen(Color.Yellow, 2), p3, p4);
                    }
                }
            }
        }


        private void m_btn_SelectFolder_Click(object sender, EventArgs e)
        {
            string fullfilename = Environment.CurrentDirectory + "\\Config.xml";
            string defaultfilePath = "";
            if (File.Exists(fullfilename))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(fullfilename);
                XmlNodeList xmlNoteList = xd.GetElementsByTagName("Configrations");
                //XmlNodeList pathNode
                //string str;
                foreach (XmlElement item in xmlNoteList)
	            {
                    defaultfilePath = item.ChildNodes[0].InnerXml.ToString();
	            }
            }

            //首次defaultfilePath为空，按FolderBrowserDialog默认设置（即桌面）选择  
            if (defaultfilePath != "")
            {
                //设置此次默认目录为上一次选中目录  
                FolderSelectDlg.SelectedPath = defaultfilePath;
            }

            if (FolderSelectDlg.ShowDialog() == DialogResult.OK)
            {
                defaultfilePath = FolderSelectDlg.SelectedPath;
                m_bool_OpenFolder = true;
                m_str_FolderPath = FolderSelectDlg.SelectedPath;

                m_listb_info.Items.Clear();
                m_listb_info.Items.Insert(0, "选择文件夹" + m_str_FolderPath + "成功");

                __UpdateUI();
                __UpdateTreeView(m_str_FolderPath);
                GetFolderImageFiles(m_str_FolderPath);

                comboBoxTemplateIMG.SelectedIndex = 0;//设置模板图像与当前图像选择下拉框
                comboBoxCurrentIMG.SelectedIndex = 0;

                if (isSingleImg)
                {
                    if (m_listStr_bmpPicPath.Count <= 0)
                        return;//如果该文件夹下没有图片则退出

                    m_int_bmpPicOrder = 0;
                    __PicBoxShowWatchPics(m_listStr_bmpPicPath[m_int_bmpPicOrder]);
                }
                else
                {
                    //Image img = Image.FromFile(m_listStr_bmpPicPath[0]);
                    //m_picboxImg.Image = img;
                }

                //保存图片文件路径
                string imagePath = defaultfilePath;
               // if (!File.Exists(fullfilename))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNode header = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    //创建头
                    xmlDoc.AppendChild(header);
                    XmlElement rootNode = xmlDoc.CreateElement("Configrations");
                    XmlElement xs = xmlDoc.CreateElement("TestImagePath");
                    xs.InnerText = imagePath;
                    rootNode.AppendChild(xs);
                    xmlDoc.AppendChild(rootNode);
                    xmlDoc.Save(fullfilename);
                }
            }
            pictureBoxDrawing.BringToFront();
            pictureBoxDrawing.Focus();
        }

        private void m_btn_SavePicToAddress_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            if (m_picboxImg.Image == null)
            {
                MessageBox.Show("没有图片!");
                return;
            }
            sf.Title = "保存图片";
            sf.Filter = @"bmp|*.bmp";

            if (sf.ShowDialog() == DialogResult.OK)
            {
                m_picboxImg.Image.Save(sf.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                __ListbInfoAdd("保存图片成功!");
            }
        }
        private void m_btn_WatchPics_Click(object sender, EventArgs e)
        {
            string strFullPath;
           
            if (m_treeView.SelectedNode == null)
            {
                strFullPath = m_str_FolderPath;
            }
            else
            {
                strFullPath = m_treeView.SelectedNode.FullPath;
            }//如果没有在TreeView中选择，那么默认为最开始选择的文件夹

            //m_listStr_bmpPicPath.Clear();//每次先把List<string>内容清空，不然会出现重复和混乱


            GetFolderImageFiles(strFullPath);

            if (m_listStr_bmpPicPath.Count <= 0)
                return;//如果该文件夹下没有图片则退出

            m_int_bmpPicOrder++;
            m_int_bmpPicOrder = (m_int_bmpPicOrder >= m_listStr_bmpPicPath.Count ? 0 : m_int_bmpPicOrder);
            __PicBoxShowWatchPics(m_listStr_bmpPicPath[m_int_bmpPicOrder]);
            string strpicName = m_listStr_bmpPicPath[m_int_bmpPicOrder].Split('\\')[m_listStr_bmpPicPath[m_int_bmpPicOrder].Split('\\').Length - 1];
            m_listb_info.Items.Insert(0, "图片"+ strpicName+ "打开成功");//这部分功能是点击一下“查看图片”，Picbox会更新一张选中文件夹下的图片
        }

        void GetFolderImageFiles(string strFullPath)
        {
            m_listStr_bmpPicPath.Clear();
            m_listStr_bmpName.Clear();
            comboBoxTemplateIMG.Items.Clear();
            comboBoxCurrentIMG.Items.Clear();
            DirectoryInfo theFolder = new DirectoryInfo(strFullPath);
            foreach (FileInfo item in theFolder.GetFiles())
            {
                if (item.FullName.Substring(item.FullName.Length - 3).ToLower() == "bmp")
                {
                    m_listStr_bmpPicPath.Add(item.FullName);
                    m_listStr_bmpName.Add(item.Name);
                    comboBoxTemplateIMG.Items.Add(item.Name);
                    comboBoxCurrentIMG.Items.Add(item.Name);
                }
            }//将选中文件夹内的图片完整路径放在List<string>中以供调用
        }
        private void m_btn_WatchLastPics_Click(object sender, EventArgs e)
        {
            string strFullPath;

            if (m_treeView.SelectedNode == null)
            {
                strFullPath = m_str_FolderPath;
            }
            else
            {
                strFullPath = m_treeView.SelectedNode.FullPath;
            }//如果没有在TreeView中选择，那么默认为最开始选择的文件夹

            m_listStr_bmpPicPath.Clear();//每次先把List<string>内容清空，不然会出现重复和混乱
            DirectoryInfo theFolder = new DirectoryInfo(strFullPath);
            foreach (FileInfo item in theFolder.GetFiles())
            {
                if (item.FullName.Substring(item.FullName.Length - 3).ToLower() == "bmp")
                {
                    m_listStr_bmpPicPath.Add(item.FullName);
                }
            }//将选中文件夹内的图片完整路径放在List<string>中以供调用

            if (m_listStr_bmpPicPath.Count <= 0)
                return;//如果该文件夹下没有图片则退出

            m_int_bmpPicOrder--;
            if (m_int_bmpPicOrder < 0)
            {
                m_int_bmpPicOrder = m_listStr_bmpPicPath.Count - 1;
            }
            m_int_bmpPicOrder = (m_int_bmpPicOrder >= m_listStr_bmpPicPath.Count ? 0 : m_int_bmpPicOrder);
            __PicBoxShowWatchPics(m_listStr_bmpPicPath[m_int_bmpPicOrder]);
            string strpicName = m_listStr_bmpPicPath[m_int_bmpPicOrder].Split('\\')[m_listStr_bmpPicPath[m_int_bmpPicOrder].Split('\\').Length - 1];
            m_listb_info.Items.Insert(0, "图片" + strpicName + "打开成功");//这部分功能是点击一下“查看图片”，Picbox会更新一张选中文件夹下的图片
        }

        private void m_btn_OpenCamera_Click(object sender, EventArgs e)
        {
            try
            {
                List<ICameraInfo> allCameras = CameraFinder.Enumerate();
                if (allCameras.Count <= 0)
                {
                    MessageBox.Show("未找到相机设备!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }

                camera = new Camera(allCameras[0]);

                camera.CameraOpened += Configuration.AcquireContinuous;  ////这句不能省略，否则Basler ace2500-14gm这款相机可能打开了并采集了但也不显示

                camera.ConnectionLost += OnConnectionLost;
                camera.CameraOpened += OnCameraOpened;
                camera.CameraClosed += OnCameraClosed;
                camera.StreamGrabber.GrabStarted += OnGrabStarted;
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                camera.StreamGrabber.GrabStopped += OnGrabStopped;

                // Open the connection to the camera device.
                camera.Open();

                camera.Parameters[PLCamera.PixelFormat].SetValue("Mono8");
                camera.Parameters[PLCamera.ExposureTimeAbs].SetValue(m_double_ExposureTime);
                camera.Parameters[PLCamera.GainRaw].SetValue(m_int_GainLevel);
                ContinuousShot();

                m_bool_OpenCamera = true;
                m_picboxImg.Width = 1296;
                m_picboxImg.Height = 972;
                __ListbInfoAdd(allCameras[0][CameraInfoKey.FullName].ToString());
                m_listb_info.Items.Insert(0, "相机已打开");//相机已打开

                if (m_picboxImg.Image != null)
                    m_picboxImg.Image = null;
                //更新相机帧率
                __UpdateCameraHZ();
                //刷新界面
                __UpdateUI();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ContinuousShot()
        {
            try
            {
                // Start the grabbing of images until grabbing is stopped.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
        // Shows exceptions in a message box.
        void ShowException(Exception exception)
        {
            MessageBox.Show("Exception caught:\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OnConnectionLost(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(OnConnectionLost), sender, e);
                return;
            }

            // Close the camera object.
            DestroyCamera();
        }
        private void OnGrabStarted(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(OnGrabStarted), sender, e);
                return;
            }
            // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed.
            stopWatch.Reset();
        }
        private void OnGrabStopped(Object sender, GrabStopEventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<GrabStopEventArgs>(OnGrabStopped), sender, e);
                return;
            }
            // Reset the stopwatch.
            stopWatch.Reset();
            // If the grabbed stop due to an error, display the error message.
            if (e.Reason != GrabStopReason.UserRequest)
            {
                MessageBox.Show("A grab error occured:\n" + e.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnCameraOpened(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(OnCameraOpened), sender, e);
                return;
            }
        }
        private void OnCameraClosed(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(OnCameraClosed), sender, e);
                return;
            }
        }
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
                // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
                BeginInvoke(new EventHandler<ImageGrabbedEventArgs>(OnImageGrabbed), sender, e.Clone());
                return;
            }

            try
            {
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid && m_bool_IsShow)
                {
                    m_bool_IsShow = false;
                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        stopWatch.Restart();

                        Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format24bppRgb);
                        // Lock the bits of the bitmap.
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        // Place the pointer to the buffer of the bitmap.
                        converter = new PixelDataConverter();
                        converter.OutputPixelFormat = PixelType.RGB8packed;

                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO

                        bitmap.UnlockBits(bmpData);

                        // Assign a temporary variable to dispose the bitmap after assigning the new bitmap to the display control.
                        Bitmap bitmapOld = m_picboxImg.Image as Bitmap;
                        // Provide the display control with the new bitmap. This action automatically updates the display.
                        m_picboxImg.Image = bitmap;
                        //是否需要进行图像保存
                        if (m_bool_IsSinglePic)
                        {
                            m_bool_IsSinglePic = false;
                            DateTime dtNow = System.DateTime.Now;  // 获取系统当前时间
                            string strDateTime = dtNow.Year.ToString() + "_"
                                                + dtNow.Month.ToString() + "_"
                                                + dtNow.Day.ToString() + "_"
                                                + dtNow.Hour.ToString() + "_"
                                                + dtNow.Minute.ToString() + "_"
                                                + dtNow.Second.ToString() + "_"
                                                + dtNow.Millisecond.ToString();

                            string stfFileName = m_str_FolderPath + "\\" + strDateTime + ".bmp";  // 默认的图像保存名称
                            RgbToGrayScale((Bitmap)(m_picboxImg.Image)).Save(stfFileName, ImageFormat.Bmp);
                        }
                        if (m_bool_IsSnapShot && m_int_SnapShotNumbers > 0 && m_int_SnapShotTimes > 0 && m_bool_IsMultiPic == true)
                        {
                            DateTime dtNow = System.DateTime.Now;  // 获取系统当前时间
                            string strDateTime = dtNow.Year.ToString() + "_"
                                               + dtNow.Month.ToString() + "_"
                                               + dtNow.Day.ToString() + "_"
                                               + dtNow.Hour.ToString() + "_"
                                               + dtNow.Minute.ToString() + "_"
                                               + dtNow.Second.ToString() + "_"
                                               + dtNow.Millisecond.ToString();

                            string stfFileName = m_str_FolderPath + "\\" + strDateTime + ".bmp";  // 默认的图像保存名称
                            RgbToGrayScale((Bitmap)(m_picboxImg.Image)).Save(stfFileName, ImageFormat.Bmp);

                            m_int_SnapShotNumbers--;
                            m_bool_IsMultiPic = false;
                        }



                        if (bitmapOld != null)
                        {
                            // Dispose the bitmap.
                            bitmapOld.Dispose();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }
        public static Bitmap RgbToGrayScale(Bitmap original)
        {
            if (original != null)
            {
                // 将源图像内存区域锁定  
                Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
                BitmapData bmpData = original.LockBits(rect, ImageLockMode.ReadOnly,
                     original.PixelFormat);

                // 获取图像参数  
                int width = bmpData.Width;
                int height = bmpData.Height;
                int stride = bmpData.Stride;  // 扫描线的宽度  
                int offset = stride - width * 3;  // 显示宽度与扫描线宽度的间隙  
                IntPtr ptr = bmpData.Scan0;   // 获取bmpData的内存起始位置  
                int scanBytes = stride * height;  // 用stride宽度，表示这是内存区域的大小  

                // 分别设置两个位置指针，指向源数组和目标数组  
                int posScan = 0, posDst = 0;
                byte[] rgbValues = new byte[scanBytes];  // 为目标数组分配内存  
                Marshal.Copy(ptr, rgbValues, 0, scanBytes);  // 将图像数据拷贝到rgbValues中  
                // 分配灰度数组  
                byte[] grayValues = new byte[width * height]; // 不含未用空间。  
                // 计算灰度数组  
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        double temp = rgbValues[posScan++] * 0.11 +
                            rgbValues[posScan++] * 0.59 +
                            rgbValues[posScan++] * 0.3;
                        grayValues[posDst++] = (byte)temp;
                    }
                    // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel  
                    posScan += offset;
                }

                // 内存解锁  
                Marshal.Copy(rgbValues, 0, ptr, scanBytes);
                original.UnlockBits(bmpData);  // 解锁内存区域  

                // 构建8位灰度位图  
                Bitmap retBitmap = BuiltGrayBitmap(grayValues, width, height);
                return retBitmap;
            }
            else
            {
                return null;
            }
        }
        private static Bitmap BuiltGrayBitmap(byte[] rawValues, int width, int height)
        {
            // 新建一个8位灰度位图，并锁定内存区域操作  
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                 ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            // 计算图像参数  
            int offset = bmpData.Stride - bmpData.Width;        // 计算每行未用空间字节数  
            IntPtr ptr = bmpData.Scan0;                         // 获取首地址  
            int scanBytes = bmpData.Stride * bmpData.Height;    // 图像字节数 = 扫描字节数 * 高度  
            byte[] grayValues = new byte[scanBytes];            // 为图像数据分配内存  

            // 为图像数据赋值  
            int posSrc = 0, posScan = 0;                        // rawValues和grayValues的索引  
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grayValues[posScan++] = rawValues[posSrc++];
                }
                // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel  
                posScan += offset;
            }

            // 内存解锁  
            Marshal.Copy(grayValues, 0, ptr, scanBytes);
            bitmap.UnlockBits(bmpData);  // 解锁内存区域  

            // 修改生成位图的索引表，从伪彩修改为灰度  
            ColorPalette palette;
            // 获取一个Format8bppIndexed格式图像的Palette对象  
            using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                palette = bmp.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            // 修改生成位图的索引表  
            bitmap.Palette = palette;

            return bitmap;
        }  
        private void Stop()
        {
            // Stop the grabbing.
            try
            {
                camera.StreamGrabber.Stop();
                CameraFrameTimer.Enabled = false;
                CameraSnapTimer.Enabled = false;
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
        private void DestroyCamera()
        {
            // Disable all parameter controls.
            try
            {
                if (camera != null)
                {

                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }

            // Destroy the camera object.
            try
            {
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }

        private void m_btn_CloseCamera_Click(object sender, EventArgs e)
        {
            m_bool_OpenCamera = false;         
            Stop();
            DestroyCamera();
            __UpdateUI();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bool_OpenCamera = false;
            if (camera != null)
            {
                Stop();
                DestroyCamera();
            }
        }
        private void m_btn_CameraSet_Click(object sender, EventArgs e)
        {
            SingleCameraSetting SCS = new SingleCameraSetting(this.m_double_ExposureTime, this.m_int_GainLevel, this.m_int_CameraFrame, 
                                                                this.m_bool_IsSnapShot, this.m_int_SnapShotNumbers, this.m_int_SnapShotTimes);
            SCS.ShowDialog();
            if (SCS.m_bool_IsOK)
            {
                this.m_double_ExposureTime = SCS.m_double_ExposureTime;
                this.m_int_GainLevel = SCS.m_int_GainLevel;
                this.m_int_CameraFrame = SCS.m_int_CameraFrame;
                this.m_bool_IsSnapShot = SCS.m_bool_IsSnapShot;
                this.m_int_SnapShotNumbers = SCS.m_int_SnapShotNumbers;
                this.m_int_SnapShotTimes = SCS.m_int_SnapShotTimes;
                __UpdateCameraHZ();
                __UpdateUI();
                m_listb_info.Items.Insert(0, "相机参数设置成功...");//相机参数设置成功...
            }
            else
            {
                m_listb_info.Items.Insert(0, "取消相机参数设置...");//取消相机参数设置...
            }
        }
        private void m_btn_CatchSinglePic_Click(object sender, EventArgs e)
        {
            m_bool_IsSinglePic = true;  //当这个变量为ture时 回调函数中就采集当前流中的一幅图像
            m_int_CamPicNumbers++;
            m_txt_CatchPicNumbers.Text = "当前采集张数:" + m_int_CamPicNumbers;
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FolderSelectDlg.ShowDialog() == DialogResult.OK)
            {
                m_bool_OpenFolder = true;
                m_str_FolderPath = FolderSelectDlg.SelectedPath;

                m_listb_info.Items.Clear();
                m_listb_info.Items.Insert(0, "选择文件夹" + m_str_FolderPath + "成功");

                __UpdateUI();
                __UpdateTreeView(m_str_FolderPath);
            }
        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            if (m_picboxImg.Image == null)
            {
                MessageBox.Show("没有图片!");
                return;
            }
            sf.Title = "保存图片";
            sf.Filter = @"bmp|*.bmp";

            if (sf.ShowDialog() == DialogResult.OK)
            {
                m_picboxImg.Image.Save(sf.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                __ListbInfoAdd("保存图片成功!");
            }
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_bool_OpenCamera = false;
            if (camera != null)
            {
                Stop();
                DestroyCamera();
            }
            this.Close();
            this.Dispose();
        }
        private void 查看图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strFullPath;

            if (m_treeView.SelectedNode == null)
            {
                strFullPath = m_str_FolderPath;
            }
            else
            {
                strFullPath = m_treeView.SelectedNode.FullPath;
            }//如果没有在TreeView中选择，那么默认为最开始选择的文件夹

            m_listStr_bmpPicPath.Clear();//每次先把List<string>内容清空，不然会出现重复和混乱
            DirectoryInfo theFolder = new DirectoryInfo(strFullPath);
            foreach (FileInfo item in theFolder.GetFiles())
            {
                if (item.FullName.Substring(item.FullName.Length - 3).ToLower() == "bmp")
                {
                    m_listStr_bmpPicPath.Add(item.FullName);
                }
            }//将选中文件夹内的图片完整路径放在List<string>中以供调用

            if (m_listStr_bmpPicPath.Count <= 0)
                return;//如果该文件夹下没有图片则退出

            m_int_bmpPicOrder++;
            m_int_bmpPicOrder = (m_int_bmpPicOrder >= m_listStr_bmpPicPath.Count ? 0 : m_int_bmpPicOrder);
            __PicBoxShowWatchPics(m_listStr_bmpPicPath[m_int_bmpPicOrder]);
            string strpicName = m_listStr_bmpPicPath[m_int_bmpPicOrder].Split('\\')[m_listStr_bmpPicPath[m_int_bmpPicOrder].Split('\\').Length - 1];
            m_listb_info.Items.Insert(0, "图片" + strpicName + "打开成功");//这部分功能是点击一下“查看图片”，Picbox会更新一张选中文件夹下的图片
        }
        private void 相机参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SingleCameraSetting SCS = new SingleCameraSetting(this.m_double_ExposureTime, this.m_int_GainLevel, this.m_int_CameraFrame,
                                                                this.m_bool_IsSnapShot, this.m_int_SnapShotNumbers, this.m_int_SnapShotTimes);
            SCS.ShowDialog();
            if (SCS.m_bool_IsOK)
            {
                this.m_double_ExposureTime = SCS.m_double_ExposureTime;
                this.m_int_GainLevel = SCS.m_int_GainLevel;
                this.m_int_CameraFrame = SCS.m_int_CameraFrame;
                this.m_bool_IsSnapShot = SCS.m_bool_IsSnapShot;
                this.m_int_SnapShotNumbers = SCS.m_int_SnapShotNumbers;
                this.m_int_SnapShotTimes = SCS.m_int_SnapShotTimes;
                __UpdateCameraHZ();
                __UpdateUI();
                m_listb_info.Items.Insert(0, "相机参数设置成功...");//相机参数设置成功...
            }
            else
            {
                m_listb_info.Items.Insert(0, "取消相机参数设置...");//取消相机参数设置...
            }
        }
        private void 单帧采集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_bool_IsSinglePic = true;  //当这个变量为ture时 回调函数中就采集当前流中的一幅图像
            m_int_CamPicNumbers++;
            m_txt_CatchPicNumbers.Text = "当前采集张数:" + m_int_CamPicNumbers;
        }
        private void 连续采集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_int_SnapShotNumbers == 0)
            {
                MessageBox.Show("请设置连续采集时间", "MyDIC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (m_int_SnapShotTimes == 0)
            {
                MessageBox.Show("请设置连续采集时间", "MyDIC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            __StartCameraSnap();
        }
        private void m_btn_NewFolder_Click(object sender, EventArgs e)
        {
            m_int_CameraFileSequence = 0;
            string str_CameraFile = m_str_FolderPath +"\\"+ "Collection " + m_int_CameraFileSequence;
            __CheckFileExit(str_CameraFile);  //不存在则创建          
        }
        void __CheckFileExit(string path)
        {
            if (Directory.Exists(path))
            {
                m_int_CameraFileSequence++;
                string str_CameraFile = m_str_FolderPath + "\\" + "Collection " + m_int_CameraFileSequence;
                __CheckFileExit(str_CameraFile);
            }
            else
            {
                Directory.CreateDirectory(path);
                __UpdateTreeView(path);     
            }
        }
        private void m_btn_ContinueFolder_Click(object sender, EventArgs e)
        {
            if (FolderSelectDlg.ShowDialog() == DialogResult.OK)
            {
                m_bool_OpenFolder = true;
                m_str_FolderPath = FolderSelectDlg.SelectedPath;
                m_listb_info.Items.Insert(0, "选择文件夹" + m_str_FolderPath + "成功");
                __UpdateUI();
                __UpdateTreeView(m_str_FolderPath);
            }
        }

        double show_width = 0; //当前显示控件的尺寸宽
        double show_height = 0; //当前显示控件的尺寸高
        void pictureBoxDrawing_MouseWheel(object sender, MouseEventArgs e)
        {
            if (pictureBoxDrawing.Image == null)
                return;
            m_PanelPic.VerticalScroll.Value = 0;
            m_PanelPic.HorizontalScroll.Value = 0;
            if ((this.m_picboxImg.Width > 100 && this.m_picboxImg.Height > 85) || e.Delta > 0)
            {
                m_picboxImg.Width = (int)((double)m_picboxImg.Width * (1 + (double)e.Delta / 1200));
                m_picboxImg.Height = (int)((double)m_picboxImg.Height * (1 + (double)e.Delta / 1200));
                show_width = m_picboxImg.Width;
                show_height = m_picboxImg.Height;
            }
        }

        private void m_btn_SetPath_Click(object sender, EventArgs e)
        {
            if (m_bool_OpenCamera)
            {
                MessageBox.Show("请先关闭相机", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (m_picboxImg.Image == null)
            {
                MessageBox.Show("请打开一副BMP图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else 
            {
                __ListbInfoAdd("开始选择区域");

                if (m_picboxImg == null)
                    return;

                bmpDrawing = new Bitmap(currentDisplayedBMP.Width, currentDisplayedBMP.Height);
                pictureBoxDrawing.Image = bmpDrawing;
                pictureBoxDrawing.BringToFront();
                pictureBoxDrawing.Focus();

                listPolygonPoint.Clear();
                isDrawAble = true;              //可以画图
                isCompletePath = false;         //未完成画图

                pen_zoom = (double)m_picboxImg.Width / m_picboxImg.Image.Width;
                width_tzoom = (float)m_picboxImg.Width / m_picboxImg.Image.Width;
                heigth_tzoom = (float)m_picboxImg.Height / m_picboxImg.Image.Height;

                //pictureBoxDrawing绘制事件
                pictureBoxDrawing.Paint += new PaintEventHandler(pictureBoxDrawing_Paint);
            }
        }

        private void m_picboxImg_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBoxDrawing == null)
                return;
            if (m_picboxImg.Image != null) //如果显示有图片，则算出显示与实际得比例
            {
                width_tzoom = (float)m_picboxImg.Width / m_picboxImg.Image.Width;
                heigth_tzoom = (float)m_picboxImg.Height / m_picboxImg.Image.Height;
            }
            pictureBoxDrawing.Image = null;
            pictureBoxDrawing.Size = m_picboxImg.Size;
            pictureBoxDrawing.Location = m_picboxImg.Location;

            currentDisplayedBMP = new Bitmap(m_picboxImg.Width, m_picboxImg.Height);
            pictureBoxDrawing.Image = currentDisplayedBMP; ;

            pictureBoxDrawing.BringToFront();
            pictureBoxDrawing.Focus();

            if (isCompletePath)
            {
                ps = new PointF[listPolygonPoint.Count];
                if (listPolygonPoint.Count > 1)
                {
                    for (int i = 0; i < listPolygonPoint.Count; i++)
                    {
                        ps[i] = listPolygonPoint[i];
                        ps[i].X = (int)(ps[i].X  * width_tzoom);
                        ps[i].Y = (int)(ps[i].Y  * heigth_tzoom);
                        //Console.WriteLine("listPolygonPoint[i]" + listPolygonPoint[i].ToString());
                    }
                    bmpDrawingTemp = new Bitmap(currentDisplayedBMP.Width, currentDisplayedBMP.Height);
                    g1 = Graphics.FromImage(bmpDrawingTemp);
                    penDrawingpolygon = new Pen(System.Drawing.Color.Red, (int)(3 / pen_zoom * width_tzoom));
                    g1.DrawPolygon(penDrawingpolygon, ps);
                    pictureBoxDrawing.Image = bmpDrawingTemp;
                    isDrawingLocked = false;
                }
                if (listPointGridinPolygon.Count() > 0 && m_bool_ShowPoint)
                {
                    using (Graphics g = Graphics.FromImage(bmpDrawingTemp)) 
                    {
                        foreach (PointF item in listPointGridinPolygon)
                        {
                            Point p1 = new Point((int)(item.X * width_tzoom - 3), (int)(item.Y * heigth_tzoom));
                            Point p2 = new Point((int)(item.X * width_tzoom + 3), (int)(item.Y * heigth_tzoom));
                            Point p3 = new Point((int)(item.X * width_tzoom), (int)(item.Y * heigth_tzoom - 3));
                            Point p4 = new Point((int)(item.X * width_tzoom), (int)(item.Y * heigth_tzoom + 3));

                            g.DrawLine(new Pen(Color.Yellow, 2), p1, p2);
                            g.DrawLine(new Pen(Color.Yellow, 2), p3, p4);
                        }                   
                    }
                }
            }
        }

        /************************************画布事件********************************************/
        bool isDrawAble = false;//是否可以画图
        bool isDrawing = false;//是否正在画图
        bool isDrawingLocked = true;//锁定是否画图，避免重绘占用计算资源。
        bool isMouseLeftClick = false;//右键单击停止画多边形
        bool isMouseRightClick = false;
        bool isCompletePath = false;

        double pen_zoom = 0;  //代表显示宽与图片宽比例
        float width_tzoom = 0;  //只在画图的时候用到，代表显示宽与图片宽比例
        float heigth_tzoom = 0; //只在画图的时候用到，代表显示高与图片高比例

        Point currentPoint = new Point();
        Point polygonPoint = new Point();
        List<PointF> listPolygonPoint = new List<PointF>();//所有 鼠标点下的 点的集合（已经转换为相对与图片大小一样的坐标）
        List<PointF> listPointGridinPolygon = new List<PointF>();//多边形内点的集合（已经转换为相对与图片大小一样的坐标）
        int[] listMask;          //覆盖面积 覆盖处该像素为1 否则为0
        byte[] listMaskStream;
        int gridInterval_X = 20; //相关计算点的x距离(像素)默认是20
        int gridInterval_Y = 20; //相关计算点的y距离(像素)默认是20

        Bitmap bmpDrawingTemp;
        Graphics g1;
        private void pictureBoxDrawing_MouseEnter(object sender, EventArgs e)
        {
            //pictureBoxDrawing.BringToFront();
            //pictureBoxDrawing.Focus();
            if (m_bool_OpenCamera)
                return;
            if (m_picboxImg.Image == null)
                return;
            if (isDrawAble)
            {
                this.Cursor = Cursors.Cross;
            }
        }
        private void pictureBoxDrawing_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
        private void pictureBoxDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isDrawAble = false;
                isCompletePath = true;
                this.Cursor = Cursors.Arrow;
                //listPolygonPoint.Clear();
                __ListbInfoAdd("完成区域选择");
                if (isCompletePath)
                {
                    gridInterval_X = m_int_SetSize;
                    gridInterval_Y = m_int_SetSize;
                    SetGridInPolygon((Bitmap)m_picboxImg.Image, gridInterval_X, gridInterval_Y);
                }
                else
                {
                    MessageBox.Show("请先选择计算区域!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (listPointGridinPolygon.Count() > 0 && m_bool_ShowPoint)
                {
                    Graphics g = Graphics.FromImage(bmpDrawingTemp);
                    foreach (PointF item in listPointGridinPolygon)
                    {
                        Point p1 = new Point((int)(item.X * width_tzoom - 3), (int)(item.Y * heigth_tzoom));
                        Point p2 = new Point((int)(item.X * width_tzoom + 3), (int)(item.Y * heigth_tzoom));
                        Point p3 = new Point((int)(item.X * width_tzoom), (int)(item.Y * heigth_tzoom - 3));
                        Point p4 = new Point((int)(item.X * width_tzoom), (int)(item.Y * heigth_tzoom + 3));

                        g.DrawLine(new Pen(Color.Yellow, 2), p1, p2);
                        g.DrawLine(new Pen(Color.Yellow, 2), p3, p4);
                    }
                }
                seedPointCount = listPointGridinPolygon.Count;
                __ListbInfoAdd("计算点个数为：" + seedPointCount.ToString());
            }
            //PointF checkPoint = new PointF(e.X, e.Y);
            isDrawing = false;
            isDrawingLocked = false;
            pictureBoxDrawing.Invalidate();
        }

        private void pictureBoxDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDrawAble)//绘制多边形
            {
                isDrawing = true;
                if (e.Button == MouseButtons.Left)
                {
                    isMouseLeftClick = true;
                    isMouseRightClick = false;
                    this.Cursor = Cursors.Cross;

                    polygonPoint = new Point((int)(e.X / width_tzoom), (int)(e.Y / heigth_tzoom));
                    listPolygonPoint.Add(polygonPoint);
                }
                if (e.Button == MouseButtons.Right)
                {
                    isMouseRightClick = true;
                    isMouseLeftClick = false;
                    isDrawAble = false;

                    polygonPoint = new Point((int)(e.X / width_tzoom), (int)(e.Y / heigth_tzoom));
                    listPolygonPoint.Add(polygonPoint);
                }
            }
            isDrawingLocked = false;
            pictureBoxDrawing.Invalidate();
        }
        private void pictureBoxDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawAble)//draw polygon
            {
                //Console.WriteLine("isDrawPolygon");
                currentPoint = new Point(e.X, e.Y);
            }
            isDrawingLocked = false;
            pictureBoxDrawing.Invalidate();
        }
        PointF[] ps;
        public void pictureBoxDrawing_Paint(object sender, PaintEventArgs e)
        {
            if (!isDrawingLocked)
            {
                if (isDrawAble)
                {
                    //Console.WriteLine("pictureBoxDrawingLeft_Paint +if (isDrawPolygon) ");
                    if (isMouseLeftClick)
                    {
                        ps = new PointF[listPolygonPoint.Count + 1];
                        for (int i = 0; i < listPolygonPoint.Count; i++)
                        {
                            ps[i].X = listPolygonPoint[i].X * width_tzoom;
                            ps[i].Y = listPolygonPoint[i].Y * heigth_tzoom;
                            //Console.WriteLine("listPolygonPoint[i]" + listPolygonPoint[i].ToString());
                        }
                        ps[listPolygonPoint.Count] = currentPoint;
                        bmpDrawingTemp = new Bitmap(currentDisplayedBMP.Width, currentDisplayedBMP.Height);
                        g1 = Graphics.FromImage(bmpDrawingTemp);
                        g1.DrawPolygon(penDrawingpolygon, ps);
                        pictureBoxDrawing.Image = bmpDrawingTemp;
                        isDrawingLocked = false;
                    }
                    if (isMouseRightClick)
                    {
                        ps = new PointF[listPolygonPoint.Count];

                        if (listPolygonPoint.Count > 1)
                        {
                            for (int i = 0; i < listPolygonPoint.Count; i++)
                            {
                                ps[i].X = listPolygonPoint[i].X * width_tzoom;
                                ps[i].Y = listPolygonPoint[i].Y * heigth_tzoom;
                                //Console.WriteLine("listPolygonPoint[i]" + listPolygonPoint[i].ToString());
                            }
                            bmpDrawingTemp = new Bitmap(currentDisplayedBMP.Width, currentDisplayedBMP.Height);
                            g1 = Graphics.FromImage(bmpDrawingTemp);
                            g1.DrawPolygon(penDrawingpolygon, ps);
                            pictureBoxDrawing.Image = bmpDrawingTemp;
                            isDrawingLocked = false;
                        }
                    }
                }
            }
            isDrawingLocked = true;
        }

        int seedPointCount = 0;

        //DIC相关参数设置
        int m_int_ExtendSize = 3;  //搜索扩展区域 2*m_int_ExtendSize
        int m_int_RelateSize = 31; //相关搜索框的大小 m_int_RelateSize*m_int_RelateSize
        int m_int_SetSize = 20;    //每个搜索点之间的间隔 m_int_SetSize*m_int_SetSize
        //DIC相关参数设置
        private void m_btn_DICSetting_Click(object sender, EventArgs e)
        {
            DICSetting DICS = new DICSetting(m_int_ExtendSize, m_int_RelateSize, m_int_SetSize);
            DICS.ShowDialog();
            if (DICS.isOK == true)
            {
                m_int_ExtendSize = DICS.m_int_ExtendSize;
                m_int_RelateSize = DICS.m_int_RelateSize;
                m_int_SetSize = DICS.m_int_SetSize;
                __ListbInfoAdd("完成设置!");
                __ListbInfoAdd("搜索扩展区域：" + m_int_ExtendSize.ToString());
                __ListbInfoAdd("搜索框的大小：" + m_int_RelateSize.ToString());
                __ListbInfoAdd("搜索点间的间隔：" + m_int_SetSize.ToString());
            }
            else
            {
                __ListbInfoAdd("取消设置!");
            }
        }
        void SetGridInPolygon(Bitmap bmp, int grid_x, int grid_y)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int maxEdge = grid_x;
            int w1 = (w - 2 * maxEdge) / grid_x;
            int h1 = (w - 2 * maxEdge) / grid_y;
            Point[,] psGrid = new Point[w1, h1];
            listPointGridinPolygon.Clear();
            for (int i = maxEdge; i < w - maxEdge; i = i + grid_x)
            {
                //float orginal_i = convert2_Origin_Value(i);
                for (int j = maxEdge; j < h - maxEdge; j = j + grid_y)
                {
                    PointF checkPoint = new PointF(i, j);

                    bool isInPloygon = IsInPolygon(checkPoint, listPolygonPoint);
                    if (isInPloygon)
                    {
                        listPointGridinPolygon.Add(checkPoint);
                    }
                }
            }
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
        /****************************************************************************************/

        /**********************************相机标定部分******************************************/
        public int m_int_CalibraRowCornerNums = 6; //x geshu
        public int m_int_CalibraColCornerNums = 9; //y geshu
        public double m_double_CalibraRowLength = 6; //x hang = ?mm
        public double m_double_CalibraColLength = 6; //y lie = ?mm

            //*!!*//*!!*//*!!*//*!!*//*!!*//*!!*//*!!*//*!!*//*!!*/更改了EmguCV中的旋转向量与平移向量Data为NUll的BUG，重写CalibrateCamera
            public static double CalibrateCamera(
            MCvPoint3D32f[][] objectPoints,
            PointF[][] imagePoints,
            Size imageSize,
            IInputOutputArray cameraMatrix,
            IInputOutputArray distortionCoeffs,
            CalibType calibrationType,
            MCvTermCriteria termCriteria,
            out Mat[] rotationVectors,
            out Mat[] translationVectors)
            {
                System.Diagnostics.Debug.Assert(objectPoints.Length == imagePoints.Length,
                   "The number of images for objects points should be equal to the number of images for image points");
                int imageCount = objectPoints.Length;

                using (VectorOfVectorOfPoint3D32F vvObjPts = new VectorOfVectorOfPoint3D32F(objectPoints))
                using (VectorOfVectorOfPointF vvImgPts = new VectorOfVectorOfPointF(imagePoints))
                {
                    double reprojectionError = 0;
                    using (VectorOfMat rVecs = new VectorOfMat())
                    using (VectorOfMat tVecs = new VectorOfMat())
                    {
                        reprojectionError = CvInvoke.CalibrateCamera(
                           vvObjPts,
                           vvImgPts,
                           imageSize,
                           cameraMatrix,
                           distortionCoeffs,
                           rVecs,
                           tVecs,
                           calibrationType,
                           termCriteria);

                        rotationVectors = new Mat[imageCount];
                        translationVectors = new Mat[imageCount];
                        for (int i = 0; i < imageCount; i++)
                        {
                            rotationVectors[i] = new Mat();
                            using (Mat matR = rVecs[i])
                                matR.CopyTo(rotationVectors[i]);
                            translationVectors[i] = new Mat();
                            using (Mat matT = tVecs[i])
                                matT.CopyTo(translationVectors[i]);
                        }
                    }
                    return reprojectionError;
                }
            }
            //*!!*//*!!*//*!!*//*!!*//*!!*//*!!*//*!!*//*!!*//*!!*/更改了EmguCV中的旋转向量与平移向量Data为NUll的BUG，重写CalibrateCamera

        private void m_btn_StartCalibration_Click(object sender, EventArgs e)
        {
            m_int_CameraFileSequence = 0;
            string str_CameraFile = m_str_FolderPath + "\\" + "Calibration " + m_int_CameraFileSequence;
            __CheckFileExit(str_CameraFile);  //不存在则创建   
        }

        private void m_btn_CalibrateSet_Click(object sender, EventArgs e)
        {
            CalibrationSetting CS = new CalibrationSetting(m_int_CalibraRowCornerNums, m_int_CalibraColCornerNums, m_double_CalibraRowLength, m_double_CalibraColLength);
            CS.ShowDialog();
            if (CS.isOK == true)
            {
                m_int_CalibraRowCornerNums = CS.m_int_RowCornerNums;
                m_int_CalibraColCornerNums = CS.m_int_ColCornerNums;
                m_double_CalibraRowLength = CS.m_double_RowLength;
                m_double_CalibraColLength = CS.m_double_ColLength;
                __ListbInfoAdd("完成设置!");
                __ListbInfoAdd("角点个数：" + m_int_CalibraRowCornerNums.ToString() + "," + m_int_CalibraColCornerNums.ToString());
                __ListbInfoAdd("每格长度："+ m_double_CalibraRowLength.ToString() + "mm," + m_double_CalibraColLength.ToString() + "mm");
            }
            else
            {
                __ListbInfoAdd("取消设置!");
            }
        }

        public double[] m_double_K = new double[5];
        public double m_double_fx = 0;             //x方向相机焦距（单位像素）
        public double m_double_fy = 0;             //y方向相机焦距（单位像素）
        public double m_double_ux = 0;             //主点x坐标，查阅相关资料越接近图片像素长度一半越好
        public double m_double_uy = 0;             //主点y坐标，查阅相关资料越接近图片像素长度一半越好
        public double m_double_kx = 0;             //x方向像素距离与实际物理距离的比值（单位mm/像素）1个像素占多少mm
        public double m_double_ky = 0;             //y方向像素距离与实际物理距离的比值（单位mm/像素）1个像素占多少mm
        public double m_double_error = 0;          //标定的总误差值（单位像素）
        private void m_menu_CornerFind_Click(object sender, EventArgs e)
        {
            Thread Thread_CornerFind = new Thread(new ThreadStart(CornerFind));  //创建线程
            ProgressBarTimer.Enabled = true;
            Thread_CornerFind.Start();
        }
        void CornerFind()
        {
            m_int_Progress = 0;
            string strFullPath = m_str_FolderPath;
            if (strFullPath == null)
            {
                MessageBox.Show("请选择一个标定序列!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            m_listStr_bmpPicPath.Clear();//每次先把List<string>内容清空，不然会出现重复和混乱
            DirectoryInfo theFolder = new DirectoryInfo(strFullPath);
            foreach (FileInfo item in theFolder.GetFiles())
            {
                if (item.FullName.Substring(item.FullName.Length - 3).ToLower() == "bmp")
                {
                    m_listStr_bmpPicPath.Add(item.FullName);
                }
            }//将选中文件夹内的图片完整路径放在List<string>中以供调用

            if (m_listStr_bmpPicPath.Count <= 0)
            {
                MessageBox.Show("该文件夹下没有图片可供角点检测", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;//如果该文件夹下没有图片则退出
            }

            int bmpPicOrder = 0;
            List<string> bmpPicPath = new List<string>();
            bmpPicPath = m_listStr_bmpPicPath;
            int nImage = bmpPicPath.Count;                    
            MCvPoint3D32f[][] object_points = new MCvPoint3D32f[nImage][];//保存标定板上角点的三维坐标
            int width = m_int_CalibraRowCornerNums; //width of chessboard no. squares in width - 1  
            int height = m_int_CalibraColCornerNums; // heght of chess board no. squares in heigth - 1  
            double lengthRow = m_double_CalibraRowLength; //标定板每格长度（mm）
            double lengthCol = m_double_CalibraColLength; //标定板每格宽度（mm）
            double[] Kxy = new double[nImage];
            Image<Bgr, byte> img;
            Image<Gray, byte> ImgGray;
            Size image_size = new Size();                   //图像尺寸
            PointF[][] corner_count = new PointF[nImage][]; //角点数组

            m_int_Progress = 10;

            while (bmpPicOrder < nImage)
            {
                string str = bmpPicPath[bmpPicOrder];
                img = new Image<Bgr, byte>(str);
                ImgGray = img.Convert<Gray, byte>();
                image_size.Width = img.Cols;
                image_size.Height = img.Rows;

                Size patternSize = new Size(width, height); //size of chess board to be detected 

                // PointF[] corners = new PointF[width * height];

                VectorOfPointF cor = new VectorOfPointF(width * height);
                CvInvoke.FindChessboardCorners(ImgGray, patternSize, cor, CalibCbType.AdaptiveThresh);
                corner_count[bmpPicOrder] = cor.ToArray();

                if (corner_count[bmpPicOrder].Count() == width * height) //chess board found  
                {
                    //make mesurments more accurate by using FindCornerSubPixel  
                    ImgGray.FindCornerSubPix(new PointF[1][] { corner_count[bmpPicOrder] }, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));
                }
                else
                {
                    MessageBox.Show("    图像序列中存在不符合要求的图片!\n  请删除:" + bmpPicPath[bmpPicOrder], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Kxy[bmpPicOrder] = CalculateRatio(corner_count[bmpPicOrder], width, height, lengthRow, lengthCol);
                bmpPicOrder++;    
                Invoke(new MessageDelegate(ListbInfoShow), new object[] { "第< " + bmpPicOrder + " >幅图片完成检测" });           
                m_int_Progress = (int)(10 + (double)bmpPicOrder / (double)nImage * 60);
            }

            //开始标定
            //摄像机内参数矩阵
            Mat cameraMatrix = new Mat(3, 3, DepthType.Cv32F, 1);
            //畸变矩阵
            //摄像机的5个畸变系数：k1,k2,p1,p2,k3
            Mat distCoeffs = new Mat(1, 5, DepthType.Cv32F, 1);
            //旋转矩阵R
            Mat[] rotateMat = new Mat[nImage];
            for (int i = 0; i < nImage; i++)
            {
                rotateMat[i] = new Mat(3, 3, DepthType.Cv32F, 1);
            }
            //平移矩阵T         
            Mat[] transMat = new Mat[nImage];
            for (int i = 0; i < nImage; i++)
            {
                transMat[i] = new Mat(3, 1, DepthType.Cv32F, 1);
            }
            //初始化标定板上角点的三维坐标
            List<MCvPoint3D32f> object_list = new List<MCvPoint3D32f>();
            for (int k = 0; k < nImage; k++)
            {
                object_list.Clear();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        object_list.Add(new MCvPoint3D32f((int)(j * lengthRow), (int)(i * lengthCol), 0f));
                    }
                }
                object_points[k] = object_list.ToArray();
            }
            //相机标定
            //获取到棋盘标定图的内角点图像坐标之后，使用CalibrateCamera函数进行相机标定，计算相机内参和外参矩阵
            /*
            第一个参数objectPoints，为世界坐标系中的三维点。在使用时，应该输入一个三维坐标点的向量的向量MCvPoint3D32f[][]，即需要依据棋盘上单个黑白矩阵的大小，计算出（初始化）每一个内角点的世界坐标。
            第二个参数imagePoints，为每一个内角点对应的图像坐标点。和objectPoints一样，应该输入PointF[][]类型变量；
            第三个参数imageSize，为图像的像素尺寸大小，在计算相机的内参和畸变矩阵时需要使用到该参数；
            第四个参数cameraMatrix为相机的内参矩阵。输入一个Mat cameraMatrix即可，如Mat cameraMatrix=Mat(3,3,CV_32FC1,Scalar::all(0));
            第五个参数distCoeffs为畸变矩阵。输入一个Mat distCoeffs=Mat(1,5,CV_32FC1,Scalar::all(0))即可 
            第六个参数CalibType相机标定类型
            第七个参数criteria是最优迭代终止条件设定
            第八个参数out Mat[]类型的旋转矩阵
            第九个参数out Mat[]类型的平移矩阵
            */
            //在使用该函数进行标定运算之前，需要对棋盘上每一个内角点的空间坐标系的位置坐标进行初始化
            //标定的结果是生成相机的内参矩阵cameraMatrix、相机的5个畸变系数distCoeffs
            //另外每张图像都会生成属于自己的平移向量和旋转向量
            m_int_Progress = 80;
            //CvInvoke.CalibrateCamera(object_points, corner_count, image_size, cameraMatrix,
            //        distCoeffs, CalibType.FixK3, new MCvTermCriteria(30, 0.1), out rotateMat, out transMat);
            CalibrateCamera(object_points, corner_count, image_size, cameraMatrix,
                    distCoeffs, CalibType.Default, new MCvTermCriteria(30, 0.1), out rotateMat, out transMat);

            PointF[][] new_corner = new PointF[nImage][];
            double[] temp_error = new double[nImage];
            for (int i = 0; i < nImage; i++)
            {
                new_corner[i] = CvInvoke.ProjectPoints(object_points[i], rotateMat[i], transMat[i], cameraMatrix, distCoeffs, null, 0); //计算重投影点
                temp_error[i] = CalculationError(new_corner[i], corner_count[i]);                                                       //计算每张图的重投影误差
            }
            
            
            double[] temp_m = new double[cameraMatrix.Height * cameraMatrix.Width];
            Marshal.Copy(cameraMatrix.DataPointer, temp_m, 0, cameraMatrix.Height * cameraMatrix.Width);
            m_double_fx = temp_m[0];    //相机内参
            m_double_ux = temp_m[2];    //相机内参
            m_double_fy = temp_m[4];    //相机内参   
            m_double_uy = temp_m[5];    //相机内参
            double[] temp_d = new double[distCoeffs.Height * distCoeffs.Width];
            Marshal.Copy(distCoeffs.DataPointer, temp_d, 0, distCoeffs.Height * distCoeffs.Width);
            for (int i = 0; i < distCoeffs.Height * distCoeffs.Width; i++)
            {
                m_double_K[i] = temp_d[i];  //相机畸变系数K(1~5)
            }
            for (int i = 0; i < nImage; i++)  
            {
                m_double_kx += Kxy[i];
            }
            m_double_kx = m_double_kx / nImage;                      //相机Kx                     
            m_double_ky = m_double_kx * (m_double_fy / m_double_fx); //相机Ky     //矫正kx ky根据fx fy
            m_int_Progress = 90;
            double[][] temp_r = new double[nImage][];                //每张图的旋转向量
            for (int i = 0; i < nImage; i++)
            {
                temp_r[i] = new double[rotateMat[i].Rows * rotateMat[i].Cols];
                Marshal.Copy(rotateMat[i].DataPointer, temp_r[i], 0, rotateMat[i].Rows * rotateMat[i].Cols);
            }
            double[][] temp_t = new double[nImage][];                //每张图的平移向量
            for (int i = 0; i < nImage; i++)
            {
                temp_t[i] = new double[transMat[i].Rows * transMat[i].Cols];
                Marshal.Copy(transMat[i].DataPointer, temp_t[i], 0, transMat[i].Rows * transMat[i].Cols);
            }


            //导出标定结果
            if (File.Exists(strFullPath + "\\Calibration_Result.xml"))
            {
                DialogResult dr = MessageBox.Show("此标定文件中已存在标定数据，是否进行覆盖?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel)
                {
                    if (LoadCalibrateDataXml(strFullPath + "\\Calibration_Result.xml"))
                        Invoke(new MessageDelegate(ListbInfoShow), new object[] { "载入数据成功" }); 
                    else
                        Invoke(new MessageDelegate(ListbInfoShow), new object[] { "载入数据失败，请选择正确的XML文件" }); 
                }
                else
                {
                    CreatCalibrateDataXml(strFullPath, temp_r, temp_t, temp_error, nImage);
                }
            }
            else
            {
                CreatCalibrateDataXml(strFullPath, temp_r, temp_t, temp_error, nImage);
            }
            Invoke(new MessageDelegate(ListbInfoShow), new object[] { "完成相机标定" });
            m_int_Progress = 100;
            //CvInvoke.CalibrateCamer();
        }
        /// <summary>
        /// 计算像素点间的实际物理距离（单位mm）
        /// </summary>
        /// <param name="Corners">检测到的角点值</param>
        /// <param name="width">宽方向角点个数</param>
        /// <param name="height">高方向角点个数</param>
        double CalculateRatio(PointF[] Corners,int width,int height,double length_x,double length_y)
        {
            int i = 0;
            double pixl_x = 0;
            double pixl_y = 0;
            double pixl_xy = 0;
            double length_xy = 0;

            double K = 0;
            m_double_ky = 0;
            for (i = 1; i < height; i++)
            {
                pixl_x = abs_double(Corners[i * width - 1].X - Corners[i * width].X);
                pixl_y = abs_double(Corners[i * width].Y - Corners[i * width - 1].Y);
                pixl_xy = Math.Sqrt(Math.Pow(pixl_x, 2) + Math.Pow(pixl_y, 2));
                length_xy = Math.Sqrt(Math.Pow(length_x*width, 2) + Math.Pow(length_y, 2));
                K += length_xy / pixl_xy;
            }
            K = K / (height - 1);
            return K;
        }
        double abs_double(double value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }
        double CalculationError(PointF[] new_corner, PointF[] corner_count)
        {
            if (new_corner.Count() != corner_count.Count())
                return 999.9;
            double R = 0;
            int num = new_corner.Count();
            for (int i = 0; i < num; i++)
            {
                R += Math.Sqrt(Math.Pow(new_corner[i].X - corner_count[i].X, 2) + Math.Pow(new_corner[i].Y - corner_count[i].Y, 2));
            }
            R = R / num;
            return R;
        }
        /// <summary>
        /// 在str文件夹下，创建XML文件，并把标定数据放进去
        /// </summary>
        /// <param name="str">文件夹path</param>
        void CreatCalibrateDataXml(string str, double[][] temp_r, double[][] temp_t, double[] temp_error, int nImage)
        {
            string filename = str + "\\Calibration_Result.xml";
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode header = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            //创建头
            xmlDoc.AppendChild(header);
            //创建一级节点
            XmlElement rootNode = xmlDoc.CreateElement("Cameras");
            //创建二级节点
            XmlElement xs = xmlDoc.CreateElement("Camera");
            xs.SetAttribute("id", "gige-2500-14gm");
            //创建三级节点
            XmlElement xinfo = xmlDoc.CreateElement("相机内参");
            //创建四级节点
            XmlElement xfx = xmlDoc.CreateElement("fx");
            xfx.InnerText = m_double_fx.ToString();
            XmlElement xfy = xmlDoc.CreateElement("fy");
            xfy.InnerText = m_double_fy.ToString();
            XmlElement xux = xmlDoc.CreateElement("ux");
            xux.InnerText = m_double_ux.ToString();
            XmlElement xuy = xmlDoc.CreateElement("uy");
            xuy.InnerText = m_double_uy.ToString();
            XmlElement xk15 = xmlDoc.CreateElement("k1-5");
            xk15.InnerText = m_double_K[0].ToString() + "," + m_double_K[1].ToString() + "," + m_double_K[2].ToString() + "," + m_double_K[3].ToString() + "," + m_double_K[4].ToString();
            XmlElement xkx = xmlDoc.CreateElement("kx");
            xkx.InnerText = m_double_kx.ToString();
            XmlElement xky = xmlDoc.CreateElement("ky");
            xky.InnerText = m_double_ky.ToString();
            //保存四级节点到三级节点
            xinfo.AppendChild(xfx);
            xinfo.AppendChild(xfy);
            xinfo.AppendChild(xux);
            xinfo.AppendChild(xuy);
            xinfo.AppendChild(xk15);
            xinfo.AppendChild(xkx);
            xinfo.AppendChild(xky);
            double sumR = 0;
            foreach (double item in temp_error)
            {
                sumR = sumR + item;
            }
            sumR = sumR / nImage;
            XmlElement xSunR = xmlDoc.CreateElement("总体平均像素误差");
            xSunR.InnerText = sumR.ToString();
            xinfo.AppendChild(xSunR);
            //保存三级节点到二级节点
            xs.AppendChild(xinfo);

            //创建四级节点 每张图片的旋转向量与平移向量
            for (int i = 0; i < nImage; i++)
            {
                string strpic = "Picture" + i;
                XmlElement xpic = xmlDoc.CreateElement(strpic);
                XmlElement xr = xmlDoc.CreateElement("Rotate");
                xr.InnerText = temp_r[i][0].ToString() + "," + temp_r[i][1].ToString() + "," + temp_r[i][2].ToString();
                XmlElement xt = xmlDoc.CreateElement("Trans");
                xt.InnerText = temp_t[i][0].ToString() + "," + temp_t[i][1].ToString() + "," + temp_t[i][2].ToString();
                XmlElement xer = xmlDoc.CreateElement("平均像素误差");
                xer.InnerText = temp_error[i].ToString();
                xpic.AppendChild(xr); //保存四级节点到三级节点
                xpic.AppendChild(xt); //保存四级节点到三级节点  
                xpic.AppendChild(xer);//保存四级节点到三级节点 
                xs.AppendChild(xpic);
            }
            //保存二级节点到一级节点
            rootNode.AppendChild(xs);
            //保存一级节点到xml文件中
            xmlDoc.AppendChild(rootNode);
            xmlDoc.Save(filename);
        }
        /// <summary>
        /// 读取str路径下的XML文件，该文件包含标定数据
        /// </summary>
        /// <param name="str">XML文件路径</param>
        bool LoadCalibrateDataXml(string str)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(str);

            XmlNodeList xmlNoteList = xd.GetElementsByTagName("相机内参");
            if (xmlNoteList.Count <= 0)
            {
                return false;
            }
            try
            {
                foreach (XmlElement item in xmlNoteList) //遍历所有<相机内参></相机内参>的节点
                {
                    XmlNodeList xnl = item.ChildNodes;      //获取<相机内参></相机内参>的节点内容
                    m_double_fx = Convert.ToDouble(xnl[0].InnerXml);
                    m_double_fy = Convert.ToDouble(xnl[1].InnerXml);
                    m_double_ux = Convert.ToDouble(xnl[2].InnerXml);
                    m_double_uy = Convert.ToDouble(xnl[3].InnerXml);
                    string[] k = xnl[4].InnerXml.ToString().Split(',');
                    for (int i = 0; i < k.Length; i++)
                    {
                        m_double_K[i] = Convert.ToDouble(k[i]);
                    }
                    m_double_kx = Convert.ToDouble(xnl[5].InnerXml);
                    m_double_ky = Convert.ToDouble(xnl[6].InnerXml);
                    m_double_error = Convert.ToDouble(xnl[7].InnerXml);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public delegate void MessageDelegate(string message);  //声明委托类型
        private void ListbInfoShow(string message)
        {
            __ListbInfoAdd(message);
        }

        private void m_menu_NowPicCorner_Click(object sender, EventArgs e)
        {
            Thread Thread_NowPicCornerFind = new Thread(new ThreadStart(NowPicCornerFind));  //创建线程
            ProgressBarTimer.Enabled = true;
            Thread_NowPicCornerFind.Start();
        }
        void NowPicCornerFind()
        {
            if (m_picboxImg.Image == null)
            {
                MessageBox.Show("请打开一副BMP图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            m_int_Progress = 0;
            string str = m_listStr_bmpPicPath[m_int_bmpPicOrder];
            Image<Bgr, byte> img = new Image<Bgr, byte>(str);
            Image<Gray, byte> ImgGray = img.Convert<Gray, byte>();

            int width = m_int_CalibraRowCornerNums; //width of chessboard no. squares in width - 1  
            int height = m_int_CalibraColCornerNums; // heght of chess board no. squares in heigth - 1  
            Size patternSize = new Size(width, height); //size of chess board to be detected 
            m_int_Progress = 10;
            // PointF[] corners = new PointF[width * height];
            VectorOfPointF cor = new VectorOfPointF(width * height);
            m_int_Progress = 20;
            CvInvoke.FindChessboardCorners(ImgGray, patternSize, cor, CalibCbType.AdaptiveThresh);
            m_int_Progress = 40;
            PointF[] corners = cor.ToArray();

            if (corners.Count() == width * height) //chess board found  
            {
                m_int_Progress = 60; 
                //make mesurments more accurate by using FindCornerSubPixel  
                ImgGray.FindCornerSubPix(new PointF[1][] { corners }, new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));
                m_int_Progress = 80;
                //CvInvoke.DrawChessboardCorners(img, patternSize, cor, true);//非必需，仅用做测试

                //fill line colour array  
                Random R = new Random();
                Bgr[] line_colour_array = new Bgr[corners.Length];
                for (int i = 0; i < corners.Length; i++)
                {
                    line_colour_array[i] = new Bgr(R.Next(255, 255), R.Next(150, 150), R.Next(255, 255));
                }

                //dram the results  
                img.Draw(new CircleF(corners[0], 8), new Bgr(Color.Red), 3);
                for (int i = 1; i < corners.Length; i++)
                {
                    img.Draw(new LineSegment2DF(corners[i - 1], corners[i]), line_colour_array[i], 3);
                    img.Draw(new CircleF(corners[i], 8), new Bgr(Color.Yellow), 3);
                }
                m_int_Progress = 100;
                Invoke(new picShowDelegate(picShow), new object[] {img});
            }
        }
        public delegate void picShowDelegate(Image<Bgr, byte> img);  //声明委托类型
        private void picShow(Image<Bgr, byte> img)  //本例中的线程要通过这个方法来访问主线程中的控件
        {
            m_picboxImg.Image = img.Bitmap;
            //m_picboxLeft.Image = img.Bitmap;
            __ListbInfoAdd("显示角点完成!");
        }

        private void m_menu_InputData_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            if (opf.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (LoadCalibrateDataXml(opf.FileName))
                        __ListbInfoAdd("载入数据成功");
                    else
                        __ListbInfoAdd("载入数据失败，请选择正确的XML文件");
                }
                catch (Exception)
                {
                    __ListbInfoAdd("载入数据失败，请选择正确的XML文件");
                }
            }
        }
        private void m_btn_InPutCalibration_Click(object sender, EventArgs e)
        {
            firstStep: OpenFileDialog opf = new OpenFileDialog();
            if (opf.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream stream = new FileStream(opf.FileName, FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    //this.textbox1.Text = reader.ReadLine(); //一次性读取一行
                    UI.CalibrationDataInput CalibrationWindow = new UI.CalibrationDataInput(opf.FileName, reader.ReadToEnd());
                    CalibrationWindow.ShowDialog();
                    reader.Close();
                    stream.Close();

                    if (CalibrationWindow.m_bool_lastStep)
                    {
                        goto firstStep;
                    }
                    if (CalibrationWindow.m_bool_Cancel)
                    {
                        __ListbInfoAdd("取消标定文件导入");
                        return;
                    }
                    if(LoadCalibrateDataXml(opf.FileName))
                        __ListbInfoAdd("载入数据成功");
                    else
                        __ListbInfoAdd("载入数据失败，请选择正确的XML文件");
                }
                catch (Exception)
                {
                    __ListbInfoAdd("载入数据失败，请选择正确的XML文件");
                }
            }                    
        }

        private void m_menu_CalibrationSetting_Click(object sender, EventArgs e)
        {
            CalibrationSetting CS = new CalibrationSetting(m_int_CalibraRowCornerNums, m_int_CalibraColCornerNums, m_double_CalibraRowLength, m_double_CalibraColLength);
            CS.ShowDialog();
            if (CS.isOK == true)
            {
                m_int_CalibraRowCornerNums = CS.m_int_RowCornerNums;
                m_int_CalibraColCornerNums = CS.m_int_ColCornerNums;
                m_double_CalibraRowLength = CS.m_double_RowLength;
                m_double_CalibraColLength = CS.m_double_ColLength;
                __ListbInfoAdd("完成设置!");
                __ListbInfoAdd("角点个数：" + m_int_CalibraRowCornerNums.ToString() + "," + m_int_CalibraColCornerNums.ToString());
                __ListbInfoAdd("每格长度：" + m_double_CalibraRowLength.ToString() + "mm," + m_double_CalibraColLength.ToString() + "mm");
            }
            else
            {
                __ListbInfoAdd("取消设置!");
            }
        }

        private void m_btn_SetOrigin_Click(object sender, EventArgs e)
        {

        }

        Bitmap templateImage;
        Bitmap currentImage;
        //Image<Gray, byte> templateImageGray;
        //Image<Gray, byte> currentImageGray;
        String templateImageName;
        String currentImageName;
        double[] x;
        double[] y;
        PointF[] arrayGlobal_PointGridinPolygonSub; //多边形内相关计算点的全局亚像素结果值
        PointF[] arrayPointGridinPolygonSub;        //多边形内相关计算点的亚像素“变形”结果值

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (isSingleImg)
            {
                pictureBoxDrawing.Image = null;

                templateImageName = m_listStr_bmpPicPath[comboBoxTemplateIMG.SelectedIndex];
                currentImageName = m_listStr_bmpPicPath[comboBoxCurrentIMG.SelectedIndex];
                //
                //listPointGridinPolygon
                //templateImage = new Image<Bgr, byte>(templateImageName);
                //currentImage = new Image<Bgr, byte>(currentImageName);
                //templateImageGray = templateImage.Convert<Gray, byte>();
                //currentImageGray = currentImage.Convert<Gray, byte>();
                //Rectangle 
                //IOutputArray result = null;
                //CvInvoke.MatchTemplate(templateImage, currentImage, result, TemplateMatchingType.CcoeffNormed);

                List<PointF> ListPF = new List<PointF>();
                List<PointF> ListBianjie = new List<PointF>();
                if (listPolygonPoint.Count() <= 0)
                {
                    return;
                }
                int xMax = (int)listPolygonPoint[0].X;
                int yMax = (int)listPolygonPoint[0].Y;
                int xMin = (int)listPolygonPoint[0].X;
                int yMin = (int)listPolygonPoint[0].Y;
                foreach (PointF item in listPolygonPoint)
                {
                    if (item.X > xMax)
                    {
                        xMax = (int)item.X;
                    }
                    else if (item.X < xMin)
                    {
                        xMin = (int)item.X;
                    }
                    if (item.Y > yMax)
                    {
                        yMax = (int)item.Y;
                    }
                    else if (item.Y < yMin)
                    {
                        yMin = (int)item.Y;
                    }
                }
                ListBianjie.Add(new Point(xMin, yMin));
                ListBianjie.Add(new Point(xMax, yMin));                
                ListBianjie.Add(new Point(xMax, yMax));//求得多边形的最大矩形的四个顶点，放入新的边界List中
                ListBianjie.Add(new Point(xMin, yMax));
                int w = m_picboxImg.Image.Width;
                int h = m_picboxImg.Image.Height;
                int maxEdge = m_int_SetSize;
                ListPF.Clear();
                for (int i = maxEdge; i < w - maxEdge; i = i + m_int_SetSize)
                {
                    //float orginal_i = convert2_Origin_Value(i);
                    for (int j = maxEdge; j < h - maxEdge; j = j + m_int_SetSize)
                    {
                        PointF checkPoint = new PointF(i, j);

                        bool isInPloygon = IsInPolygon(checkPoint, ListBianjie);
                        if (isInPloygon)
                        {
                            ListPF.Add(checkPoint);
                        }
                    }
                }//将需要的间隔点放入ListPF中，带入相关计算,,,找到矩形内的种子点

                int seedPointCount = ListPF.Count;
                x = new double[seedPointCount];
                y = new double[seedPointCount];
                for(int i = 0; i<seedPointCount; i++)
                {
                    x[i] = ListPF[i].X;
                    y[i] = ListPF[i].Y;
                }
                
                templateImage = new Bitmap(templateImageName);
                currentImage = new Bitmap(currentImageName);

                //计算匹配点
                //SeedPointsMaping spMaping = new SeedPointsMaping(listPolygonPoint, templateImage, currentImage, 1);
                regPoints.Registration(templateImage, currentImage, seedPointCount, x, y, m_int_RelateSize, m_int_ExtendSize);//计算举矩形内的种子点
                arrayGlobal_PointGridinPolygonSub = regPoints.PointFSub;//得到全局亚像素坐标
                arrayPointGridinPolygonSub = regPoints.PointFSub;//得到亚像素坐标
                //ListPF对应的点在ListPointGridinPolygonSub中
                float fx = ListPF[0].X;//计算矩形的x，y点个数
                int xnum = 0;
                foreach (PointF item in ListPF)
                {
                    if (item.X == fx)
                    {
                        xnum++;
                    }
                    else
                    {
                        break;
                    }
                }

                int ynum = arrayPointGridinPolygonSub.Count() / xnum; //得到ListPF中长宽的像素点个数
                PointF[,] PF = new PointF[xnum, ynum];
                for (int i = 0; i < ynum; i++)
                {
                    for (int j = 0; j < xnum; j++)
                    {
                        PF[j, i] = arrayPointGridinPolygonSub[i * xnum + j];
                    }
                }//将ListPF转为二维数组，方便插值计算

                PointF[,] templateAllPF = new PointF[(int)(ListPF[ListPF.Count() - 1].X - ListPF[0].X) + 1, (int)(ListPF[ListPF.Count() - 1].Y - ListPF[0].Y) + 1];//构建模板图片矩形内所有像素点的二维数组
                for (int i = 0; i < ListPF[ListPF.Count() - 1].Y - ListPF[0].Y + 1; i++)
                {
                    for (int j = 0; j < ListPF[ListPF.Count() - 1].X - ListPF[0].X + 1; j++)
                    {
                        templateAllPF[j, i].X = ListPF[0].X + j;
                        templateAllPF[j, i].Y = ListPF[0].Y + i;
                    }
                }
                PointF[,] currentAllPF = new PointF[(int)(ListPF[ListPF.Count() - 1].X - ListPF[0].X) + 1, (int)(ListPF[ListPF.Count() - 1].Y - ListPF[0].Y) + 1];//构建当前图片矩形内所有像素点的二维数组
                for (int i = 0; i < ListPF[ListPF.Count() - 1].X - ListPF[0].X + 1; i++)
                {
                    for (int j = 0; j < ListPF[ListPF.Count() - 1].Y - ListPF[0].Y + 1; j++)
                    {
                        int xn = j / maxEdge;
                        int yn = i / maxEdge;
                        int xx = j % maxEdge;
                        int yy = i % maxEdge;
                        if(xn == xnum -1 && yn == ynum -1) //剔除xy都是边界的情况
                        {
                            currentAllPF[i, j].X = (float)(PF[xn, yn].X);
                            currentAllPF[i, j].Y = (float)(PF[xn, yn].Y);                          
                        }
                        else if (xn == xnum - 1)//剔除x是边界的情况
                        {
                            currentAllPF[i, j].X = (float)(
                                                    PF[xn, yn].X * (1 - (double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge) +              //双线性插值                                     
                                                    PF[xn, yn + 1].X * (1 - (double)xx / (double)maxEdge) * ((double)yy / (double)maxEdge)) ;
                            currentAllPF[i, j].Y = (float)(
                                                    PF[xn, yn].Y * (1 - (double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge) +
                                                    PF[xn, yn + 1].Y * (1 - (double)xx / (double)maxEdge) * ((double)yy / (double)maxEdge)) ;
                        }
                        else if (yn == ynum - 1)//剔除y是边界的情况
                        {
                            currentAllPF[i, j].X = (float)(
                                                    PF[xn, yn].X * (1 - (double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge) +
                                                    PF[xn + 1, yn].X * ((double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge));
                            currentAllPF[i, j].Y = (float)(
                                                    PF[xn, yn].Y * (1 - (double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge) +
                                                    PF[xn + 1, yn].Y * ((double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge));
                        }
                        else//正常插值的情况
                        {
                            currentAllPF[i, j].X = (float)(
                                                    PF[xn, yn].X * (1 - (double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge) +
                                                    PF[xn + 1, yn].X * ((double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge) +
                                                    PF[xn, yn + 1].X * (1 - (double)xx / (double)maxEdge) * ((double)yy / (double)maxEdge) +
                                                    PF[xn + 1, yn + 1].X * ((double)xx / (double)maxEdge) * ((double)yy / (double)maxEdge));
                            currentAllPF[i, j].Y = (float)(
                                                    PF[xn, yn].Y * (1 - (double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge) +
                                                    PF[xn + 1, yn].Y * ((double)xx / (double)maxEdge) * (1 - (double)yy / (double)maxEdge) +
                                                    PF[xn, yn + 1].Y * (1 - (double)xx / (double)maxEdge) * ((double)yy / (double)maxEdge) +
                                                    PF[xn + 1, yn + 1].Y * ((double)xx / (double)maxEdge) * ((double)yy / (double)maxEdge));
                        }
                    }
                }

                double diffmax = 0;//最大变化量
                double diffmin = 9999;//最小变化量
                Point[] drawPt = new Point[templateAllPF.Length];
                double[] drawPtValue = new double[templateAllPF.Length];
                for (int i = 0; i < ListPF[ListPF.Count() - 1].Y - ListPF[0].Y + 1; i++)
                {
                    for (int j = 0; j < ListPF[ListPF.Count() - 1].X - ListPF[0].X + 1; j++)
                    {
                        //double a = Math.Sqrt(Math.Pow((double)(templateAllPF[j, i].X - currentAllPF[j, i].X), 2) + Math.Pow((double)(templateAllPF[j, i].Y - currentAllPF[j, i].Y), 2));
                        double a = Math.Sqrt(Math.Pow((double)(currentAllPF[j, i].X), 2) + Math.Pow((double)(currentAllPF[j, i].Y), 2));
                        if (diffmax < a)
                        {
                            diffmax =  a;
                        }
                        if (diffmin > a)
                        {
                            diffmin = a;
                        }
                        drawPtValue[i * (int)(ListPF[ListPF.Count() - 1].X - ListPF[0].X + 1) + j] = a;
                        drawPt[i * (int)(ListPF[ListPF.Count() - 1].X - ListPF[0].X + 1) + j].X = (int)templateAllPF[j, i].X;
                        drawPt[i * (int)(ListPF[ListPF.Count() - 1].X - ListPF[0].X + 1) + j].Y = (int)templateAllPF[j, i].Y;
                    }
                }
                double ciff = diffmax - diffmin;
                using (Graphics g = Graphics.FromImage(bmpDrawingTemp))
                {
                    GetMask(this.m_picboxImg.Image);
                    for (int i = 0; i < drawPtValue.Length; i++)
                    {
                        Color cor;
                        int R, G, B;
                        double ptValue = drawPtValue[i];
                        if (diffmax - ptValue < ciff / 2.0)  //将三通道先归一化，再做一次函数计算，得到RGB分量
                        {
                            R = (int)(-180 / ciff * (diffmax - ptValue) + 160);
                            G = (int)(360 / ciff * (diffmax - ptValue) + 70);
                            B = (int)(-220 / ciff * (diffmax - ptValue) + 220);
                        }
                        else
                        {
                            R = (int)(350 / ciff * (diffmax - ptValue) - 105);
                            G = (int)(-400 / ciff * (diffmax - ptValue) + 450);
                            B = (int)(-220 / ciff * (diffmax - ptValue) + 220);
                        }
                        cor = Color.FromArgb(120, R, G, B);

                        if (listMask[drawPt[i].X + drawPt[i].Y * m_picboxImg.Image.Width] == 1)
                            g.DrawRectangle(new Pen(cor), drawPt[i].X * width_tzoom, drawPt[i].Y * heigth_tzoom, 1, 1);
                         
                    }
                    string str1 = (diffmin + ciff * 1 / 7).ToString("#0.000");
                    string str2 = (diffmin + ciff * 2 / 7).ToString("#0.000");
                    string str3 = (diffmin + ciff * 3 / 7).ToString("#0.000");
                    string str4 = (diffmin + ciff * 4 / 7).ToString("#0.000");
                    string str5 = (diffmin + ciff * 5 / 7).ToString("#0.000");
                    string str6 = (diffmin + ciff * 6 / 7).ToString("#0.000");
                    string str7 = (diffmin + ciff * 7 / 7).ToString("#0.000");
                    ShowColorDisplament(str7, str6, str5, str4, str3, str2, str1);
                    pictureBoxDrawing.Image = bmpDrawingTemp;
                }
                ClearMemory();//释放内存，否则运行过后程序可能会卡
            }
        } //相关计算按钮事件

        private void m_cob_CameraMod_SelectedIndexChanged(object sender, EventArgs e)
        {
            MiddlePanelView();
        }
        private void comboBoxTemplateIMG_SelectedIndexChanged(object sender, EventArgs e)
        {         
            SelectTemplateImage();
        }
        private void comboBoxCurrentIMG_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectCurrentImage();
        }
        private void buttonShow_Click(object sender, EventArgs e)
        {
            //DICResultShow d = new DICResultShow(listPointGridinPolygon, ListPointGridinPolygonSub);
            //d.Show();
        }
        /// <summary>
        /// 获得目标图片中所画区域的位置，1为所画区域内部，0为外部
        /// </summary>
        /// <param name="img">目标图片</param>
        /// <returns></returns>
        public void GetMask(Image img)
        {
            listMask = new int[img.Width * img.Height];
            listMaskStream = new byte[img.Width * img.Height];
            if (listPolygonPoint.Count() <= 0)
            {
                MessageBox.Show("未选择区域", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int xMax = (int)listPolygonPoint[0].X;
            int yMax = (int)listPolygonPoint[0].Y;
            int xMin = (int)listPolygonPoint[0].X;
            int yMin = (int)listPolygonPoint[0].Y;
            foreach (PointF item in listPolygonPoint)
            {
                if (item.X > xMax)
                {
                    xMax = (int)item.X;
                }
                else if (item.X < xMin)
                {
                    xMin = (int)item.X;
                }
                if (item.Y > yMax)
                {
                    yMax = (int)item.Y;
                }
                else if (item.Y < yMin)
                {
                    yMin = (int)item.Y;
                }
            }           
            for (int i = 0; i < listMask.Count(); i++)
            {
                listMask[i] = 0;
            }
            int w = xMax;
            int h = yMax;
            for (int i = xMin; i < w; i = i + 1)
            {
                for (int j = yMin; j < h; j = j + 1)
                {
                    PointF checkPoint = new PointF(i, j);
                    bool isInPloygon = IsInPolygon(checkPoint, listPolygonPoint);
                    if (isInPloygon)
                    {
                        listMask[i + j * m_picboxImg.Image.Width] = 1;
                        listMaskStream[i + j * m_picboxImg.Image.Width] = 1;
                    }
                }
            }
            //metaDateIO.Write2File(listMaskStream);
            //string s = metaDateIO.ReadFile();
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="process"></param>
        /// <param name="minimumWorkingSetSize"></param>
        /// <param name="maximumWorkingSetSize"></param>
        /// <returns></returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        private void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }
        /// <summary>
        /// 在m_picboxImg上画出颜色带并给出每种颜色对应的值
        /// </summary>
        /// <param name="txtCorlor1">颜色1（红色）对应的值</param>
        /// <param name="txtCorlor2">颜色2对应的值</param>
        /// <param name="txtCorlor3">颜色3对应的值</param>
        /// <param name="txtCorlor4">颜色4对应的值</param>
        /// <param name="txtCorlor5">颜色5对应的值</param>
        /// <param name="txtCorlor6">颜色6对应的值</param>
        /// <param name="txtCorlor7">颜色7（紫色）对应的值</param>
        /// <returns></returns>
        public bool ShowColorDisplament(string txtCorlor1, string txtCorlor2, string txtCorlor3, string txtCorlor4, string txtCorlor5, string txtCorlor6, string txtCorlor7)
        {
            int showColor_width = 50;
            int showColor_height = m_picboxImg.Height;
            PictureBox m_picboxShowColor; //显示颜色代表的位移的图片框
            m_picboxShowColor = new PictureBox();
            m_picboxShowColor.Parent = m_picboxImg;
            m_picboxShowColor.Size = new Size(showColor_width, showColor_height);
            m_picboxShowColor.Dock = DockStyle.None;
            m_picboxShowColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            m_picboxShowColor.Location = new System.Drawing.Point(m_picboxImg.Width - showColor_width, 0);
            m_picboxShowColor.Margin = new System.Windows.Forms.Padding(0);
            m_picboxShowColor.Name = "m_picboxShowColor";
            m_picboxShowColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            m_picboxShowColor.BackColor = Color.Transparent;
            m_picboxShowColor.BringToFront();
            m_picboxShowColor.Focus();

            Point startPoint2 = new Point(m_picboxImg.Width - showColor_width, 0);
            Point endPoint2 = new Point(m_picboxImg.Width - showColor_width, m_picboxImg.Height);
            Color[] myColors = {
                                    Color.FromArgb(200,255,80,0),//红
                                    Color.FromArgb(200,253,99,71),
                                    Color.FromArgb(200,255,250,205),  
                                    Color.FromArgb(200,72,255,204), 
                                    Color.FromArgb(200,176,224,230),
                                    Color.FromArgb(200,147,112,219),
                                    Color.FromArgb(200,153,50,204) //紫
                               }; //7种颜色的渐变
            float[] myPositions = { 0.0f, .17f, .34f, .50f, .67f, .76f, 1.0f };//所占百分比
            ColorBlend myBlend = new ColorBlend();
            myBlend.Colors = myColors;
            myBlend.Positions = myPositions;
            LinearGradientBrush lgBrush2 = new LinearGradientBrush(startPoint2,
               endPoint2,
               Color.FromArgb(200, 255, 80, 0),
               Color.FromArgb(200, 153, 50, 204));
            lgBrush2.InterpolationColors = myBlend;

            Bitmap colorShow = new Bitmap(showColor_width, showColor_height);
            using (Graphics g = Graphics.FromImage(colorShow))
            {
                g.FillRectangle(lgBrush2, 0, 0, showColor_width, showColor_height);
            }//画出颜色带

            //-------------------------在颜色带上标注Displament-------------------------------//
            int w = showColor_height / 8;
            using (Brush brush = new SolidBrush(Color.Black))
            using (Graphics g = Graphics.FromImage(colorShow))
            {
                Rectangle rect1 = new Rectangle(0, w * 1, showColor_width, 20);
                g.DrawString(txtCorlor1, Font, brush, rect1, StringFormat.GenericDefault);
                Rectangle rect2 = new Rectangle(0, w * 2, showColor_width, 20);
                g.DrawString(txtCorlor2, Font, brush, rect2, StringFormat.GenericDefault);
                Rectangle rect3 = new Rectangle(0, w * 3, showColor_width, 20);
                g.DrawString(txtCorlor3, Font, brush, rect3, StringFormat.GenericDefault);
                Rectangle rect4 = new Rectangle(0, w * 4, showColor_width, 20);
                g.DrawString(txtCorlor4, Font, brush, rect4, StringFormat.GenericDefault);
                Rectangle rect5 = new Rectangle(0, w * 5, showColor_width, 20);
                g.DrawString(txtCorlor5, Font, brush, rect5, StringFormat.GenericDefault);
                Rectangle rect6 = new Rectangle(0, w * 6, showColor_width, 20);
                g.DrawString(txtCorlor6, Font, brush, rect6, StringFormat.GenericDefault);
                Rectangle rect7 = new Rectangle(0, w * 7, showColor_width, 20);
                g.DrawString(txtCorlor7, Font, brush, rect7, StringFormat.GenericDefault);
            }
            m_picboxShowColor.Image = colorShow;
            return true;
        }



        private void m_btn_MouseEnter(object sender, EventArgs e)
        {
            UI.ControlUI.m_btn_MouseEnter(sender);
        }
        private void m_btn_MouseLeave(object sender, EventArgs e)
        {
            UI.ControlUI.m_btn_MouseLeave(sender);
        }

        public Panel ActivePanel = new Panel();//显示当前活动的面板
        private void m_btn_Control_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel_Control.Name)
                return;
            ActivePanel = panel_Control;

            panel_Control.BringToFront();
            panel_InputData.SendToBack();
            panel_EvaSetting.SendToBack();
            panel_MaskDef.SendToBack();
            panel_StartPts.SendToBack();
            panel_VisualSetting.SendToBack();         
        }

        private void m_btn_InputData_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel_InputData.Name)
                return;
            ActivePanel = panel_InputData;

            panel_Control.SendToBack();
            panel_InputData.BringToFront();
            panel_EvaSetting.SendToBack();
            panel_MaskDef.SendToBack();
            panel_StartPts.SendToBack();
            panel_VisualSetting.SendToBack();  
        }

        private void m_btn_EvaSetting_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel_EvaSetting.Name)
                return;
            ActivePanel = panel_EvaSetting;

            panel_Control.SendToBack();
            panel_InputData.SendToBack();
            panel_EvaSetting.BringToFront();
            panel_MaskDef.SendToBack();
            panel_StartPts.SendToBack();
            panel_VisualSetting.SendToBack();  
        }

        private void m_btn_MaskDef_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel_MaskDef.Name)
                return;
            ActivePanel = panel_MaskDef;

            panel_Control.SendToBack();
            panel_InputData.SendToBack();
            panel_EvaSetting.SendToBack();
            panel_MaskDef.BringToFront();
            panel_StartPts.SendToBack();
            panel_VisualSetting.SendToBack();  
        }

        private void m_btn_StartPts_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel_StartPts.Name)
                return;
            ActivePanel = panel_StartPts;

            panel_Control.SendToBack();
            panel_InputData.SendToBack();
            panel_EvaSetting.SendToBack();
            panel_MaskDef.SendToBack();
            panel_StartPts.BringToFront();
            panel_VisualSetting.SendToBack();  
        }

        private void m_btn_VisualSetting_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel_VisualSetting.Name)
                return;
            ActivePanel = panel_VisualSetting;

            panel_Control.SendToBack();
            panel_InputData.SendToBack();
            panel_EvaSetting.SendToBack();
            panel_MaskDef.SendToBack();
            panel_StartPts.SendToBack();
            panel_VisualSetting.BringToFront();  
        }
        /****************************************************************************************/      
    }
}
