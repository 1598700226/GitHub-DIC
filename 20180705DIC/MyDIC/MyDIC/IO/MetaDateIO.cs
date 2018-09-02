using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace MyDIC.IO
{
    class MetaDateIO
    {
        public enum data_type
        {
            Global_Integer_Pixel_Coordinate_X,//全局整像素位移
            Global_Integer_Pixel_Coordinate_Y,
            Global_Sub_Pixel_Coordinate_X,//全局亚像素位移
            Global_Sub_Pixel_Coordinate_Y,
        };

        public void Write2File(byte[] buffer, int sequence_num, data_type dataType)
        {
            //FileStream 操作字节的
            //1.创建FileStream类对象
            //string fullfilename =  "C:\\testResults\\mask.mc";
            string fullfilename = Environment.CurrentDirectory + "\\testResults\\mask.mc";
            FileStream fsWrite = new FileStream(fullfilename, FileMode.OpenOrCreate, FileAccess.Write);
            fsWrite.Write(buffer, 0, buffer.Length);    //每次读取2M放到字节数组里面
            fsWrite.Close();
            fsWrite.Dispose();
        }
        //读取数据， sequence_num读取的数据序列，data_type读取的数据类型，包括全局整像素坐标，全局亚像素
        public string ReadFile(int sequence_num, data_type dataType)
        {
            string fullfilename = Environment.CurrentDirectory + "\\testResults\\mask.mc";
            FileStream fsread = new FileStream(fullfilename,FileMode.OpenOrCreate,FileAccess.Read);
            byte[] buffer=new byte[1024*1024*2];    //定义一个2M的字节数组
            //返回本次实际读取到的有效字节数
            int r=fsread.Read(buffer,0,buffer.Length);    //每次读取2M放到字节数组里面
            //将字节数组中每一个元素按照指定的编码格式解码成字符串
            string s=Encoding.Default.GetString(buffer,0,r);	
            //关闭流
            fsread.Close();
            //释放流所占用的资源
            fsread.Dispose();
            return s;
        }

    }
}
