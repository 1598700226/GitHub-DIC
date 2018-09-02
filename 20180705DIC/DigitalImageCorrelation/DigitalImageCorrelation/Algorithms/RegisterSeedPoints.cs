using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu;
using Emgu.CV;
using Emgu.Util;
using ZedGraph;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System.Drawing;

namespace DigitalImageCorrelation.Algorithms
{
    class RegisterSeedPoints
    {
        public int subset_size;
        public int searching_size;
        public double coefficient_threshold;

        [DllImport("ImageR.dll", CallingConvention = CvInvoke.CvCallingConvention)]
        //input: 模板图像，当前图像，模板图像坐标x, 模板图像坐标y, 子区域大小，搜索区域大小，匹配系数阈值
        //output： u, v, ux, uy, vx, vy, error_code
        //    public static extern void ImageRegistion(IplImage* templ1, IplImage* image1, ref double temp_x[], ref double temp_y[], ref int &subsetSize, ref double &subsetExtendSize,
        //ref double* current_u, ref double* current_v, ref double* current_ux, ref double* current_uy, ref double current_vx[], ref double current_vy[],
        //int &error_code)
        //IntPtr src, ref bool n, ref bool s, ref bool e, ref bool w,
        //ref int LN, ref int LS, ref int LE, ref int LW);
    }
}
