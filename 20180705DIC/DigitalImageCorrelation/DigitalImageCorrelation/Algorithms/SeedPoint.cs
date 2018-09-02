using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalImageCorrelation.Algorithms
{
    class SeedPoint
    {
        private double point_integer_x;
        public double Point_integer_x
        {
            get { return point_integer_x; }
            set { point_integer_x = value; }
        }

        private double point_integer_y;
        public double Point_integer_y
        {
            get { return point_integer_y; }
            set { point_integer_y = value; }
        }

        //private double point_displacement_integer_x;
        //public double Point_displacement_integer_x
        //{
        //    get { return point_displacement_integer_x; }
        //    set { point_displacement_integer_x = value; }
        //}

        //private double point_displacement_integer_y;
        //public double Point_displacement_integer_y
        //{
        //    get { return point_displacement_integer_y; }
        //    set { point_displacement_integer_y = value; }
        //}

        private double point_displacement_sub_x;
        public double Point_displacement_sub_x
        {
            get { return point_displacement_sub_x; }
            set { point_displacement_sub_x = value; }
        }

        private double point_displacement_sub_y;
        public double Point_displacement_sub_y
        {
            get { return point_displacement_sub_y; }
            set { point_displacement_sub_y = value; }
        }

        public SeedPoint(int x, int y)
        {
            

        }
    }
}
