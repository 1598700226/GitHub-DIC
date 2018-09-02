// ImageR.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "cv.h"  
#include <ctime>
#include "time.h"
#include <iostream>
#include <opencv2/core/core.hpp>  
#include <opencv2/highgui/highgui.hpp> 
#include <math.h>  
#include "highgui.h"
#include <opencv2/opencv.hpp>
#include <fstream>
//#include<stdio.h>  
//#include"Windows.h" 
//#pragma comment(lib,"highgui.lib")  
//#pragma comment(lib,"cxcore.lib")  
//#pragma comment(lib,"cv.lib") 
using namespace std; 
using namespace cv;

//void DrawTransRec(IplImage* img,int x,int y,int width,int height,CvScalar color,double alpha)  ;
int round_double(double number);
//
extern "C" _declspec(dllexport) void ImageRegistration(
	IplImage *temp, IplImage *cur, 
	int len, 
	double temp_x[], double temp_y[], 
	int **globalIntLocation, 
	double **subLocation,
	double **globalSubLocation,
	//double **ux, double **uy, 
	//double **vx, double **vy,
	//double **coefficientsMax,double **coefficientsMin,
	//bool **isCoefficientsConfidence,
	int m_subsetSize,int m_subsetExtendSize 
	//int &error_code
	)
{
	int subsetSize = m_subsetSize;
    int subsetExtendSize = m_subsetExtendSize;

	Mat tempImg = cvarrToMat(temp);
	Mat curImg = cvarrToMat(cur);
	Mat tempImgGray ;
	Mat curImgGray ;
    cvtColor(tempImg,tempImgGray,CV_BGR2GRAY);
	cvtColor(curImg,curImgGray,CV_BGR2GRAY);

	//cvNamedWindow("1", CV_WINDOW_AUTOSIZE);
	//imshow("1", tempImg);
	//cvNamedWindow("1", CV_WINDOW_AUTOSIZE);
	//imshow("1", curImg);
	//int n=0;
	for ( int i = 0; i < len; i++)  
	{
		int integerCoordinate_x = round_double(temp_x[i]);
		int integerCoordinate_y = round_double(temp_y[i]);
		double subCoordinate_x = temp_x[i]-integerCoordinate_x;
		double subCoordinate_y = temp_y[i]-integerCoordinate_y;

		Rect rect_t(integerCoordinate_x,integerCoordinate_y, subsetSize, subsetSize);
		//Rect rect_t(1050,590 , 31, 31);
		Mat image_t = tempImgGray(rect_t);
		Rect rect_i(integerCoordinate_x-subsetExtendSize, integerCoordinate_y-subsetExtendSize, subsetSize+2*subsetExtendSize, subsetSize+2*subsetExtendSize);
		//Rect rect_i(1047,587 ,37, 37);
		Mat image_i = curImgGray(rect_i);

		cv::Mat result;
		//CV_TM_CCOEFF_NORMED 归一化相关系数匹配法//CV_TM_CCORR_NORMED 归一化相关匹配法//CV_TM_CCOEFF 相关系数匹配法：1表示完美的匹配；-1表示最差的匹配。
		cv::matchTemplate(image_t, image_i, result, CV_TM_CCOEFF_NORMED);//CV_TM_CCOEFF_NORMED

		//for(int i=0; i<result.rows; ++i)
		//{
		//float *rRow = result.ptr<float>(i);
		//for(int j=0; j<result.cols; j++)
		//cout<<rRow[j];
		//}

		double minVal, maxVal;  
		cv::Point minLoc, maxLoc;  
		cv::minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc);  
		int xpeak = maxLoc.y;
		int ypeak = maxLoc.x;

		double epsi_x=(result.at<float>(xpeak-1,ypeak)-result.at<float>(xpeak+1,ypeak))/(2*(result.at<float>(xpeak-1,ypeak)+result.at<float>(xpeak+1,ypeak)-2*result.at<float>(xpeak,ypeak)));
		double epsi_y=(result.at<float>(xpeak,ypeak-1)-result.at<float>(xpeak,ypeak+1))/(2*(result.at<float>(xpeak,ypeak-1)+result.at<float>(xpeak,ypeak+1)-2*result.at<float>(xpeak,ypeak)));

		*((int*)globalIntLocation + i*2 + 0)=integerCoordinate_x + xpeak - subsetExtendSize; 
		*((int*)globalIntLocation + i*2 + 1)=integerCoordinate_y + ypeak - subsetExtendSize; 

		*((double*)subLocation + i*2 + 0)=xpeak - subsetExtendSize + epsi_x; 
		*((double*)subLocation + i*2 + 1)=ypeak - subsetExtendSize + epsi_y; 

		*((double*)globalSubLocation + i*2 + 0)=integerCoordinate_x + xpeak - subsetExtendSize + epsi_x; 
		*((double*)globalSubLocation + i*2 + 1)=integerCoordinate_y + ypeak - subsetExtendSize + epsi_y; 
		//n++;
	}
}

int round_double(double number)
{
	return (number > 0.0) ? (number + 0.5) : (number - 0.5); 
}
//2018.5.13 画出半透明的颜色区域，用于标注dic测量结果。
//  void DrawTransRec(IplImage* img,int x,int y,int width,int height,CvScalar color,double alpha)  
//  {  
////IplImage * pTemp=(IplImage*)cvClone(img);  
//      IplImage * rec=cvCreateImage(cvSize(width,height),img->depth,img->nChannels);  
//      cvRectangle(rec,cvPoint(0,0),cvPoint(width,height),color,-1);  
//      cvSetImageROI(img,cvRect(x,y,width,height));  
//      cvAddWeighted(img,alpha,rec,1-alpha,0.0,img);  
//      cvResetImageROI(img);  
//  }  


//void DrawPolygon(IplImage* image)  
//  {  
//
//  }  

extern "C" _declspec(dllexport) void ImageInterpolation(
	IplImage *temp, IplImage *cur, 
	int len, 
	double temp_x[], double temp_y[], 
	int **globalIntLocation, 
	double **subLocation,
	double **globalSubLocation,
	//double **ux, double **uy, 
	//double **vx, double **vy,
	//double **coefficientsMax,double **coefficientsMin,
	//bool **isCoefficientsConfidence,
	int m_subsetSize,int m_subsetExtendSize 
	//int &error_code
	)
{

}