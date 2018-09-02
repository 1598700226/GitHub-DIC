using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDIC.Algorithm
{
    class SeedPoints
    {

        string camera_num;

        public string Camera_num
        {
            get { return camera_num; }
            set { camera_num = value; }
        }

        double ref_point_x;

        public double Ref_point_x
        {
            get { return ref_point_x; }
            set { ref_point_x = value; }
        }

        double ref_point_y;

        public double Ref_point_y
        {
            get { return ref_point_y; }
            set { ref_point_y = value; }
        }

        double cur_point_x;

        public double Cur_point_x
        {
            get { return cur_point_x; }
            set { cur_point_x = value; }
        }

        double cur_point_y;

        public double Cur_point_y
        {
            get { return cur_point_y; }
            set { cur_point_y = value; }
        }

        long image_num;

        public long Image_num
        {
            get { return image_num; }
            set { image_num = value; }
        }

        SeedPoints()
        {

        }
    }
}
