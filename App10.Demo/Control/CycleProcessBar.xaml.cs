﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App10.Demo.Control;

public partial class CycleProcessBar : UserControl
{
    public CycleProcessBar()
    {
        InitializeComponent();
    }

    public double CurrentValue
    {
        set => SetValue(value);
    }

    /// <summary>
    /// 设置百分百，输入整数，自动除100
    /// </summary>
    /// <param name="percentValue"></param>
    private void SetValue(double percentValue)
    {
        /*****************************************
          方形矩阵边长为34，半长为17
          环形半径为14，所以距离边框3个像素
          环形描边3个像素
        ******************************************/
        var angel = percentValue * 360 / 100; //角度
        const double radius = 14; //环形半径

        //起始点
        const double leftStart = 17;
        const double topStart = 3;

        //结束点
        double endLeft = 0;
        double endTop = 0;

        //数字显示
        PART_Text.Content = percentValue.ToString("0") + "%";

        /***********************************************
        * 整个环形进度条使用Path来绘制，采用三角函数来计算
        * 环形根据角度来分别绘制，以90度划分，方便计算比例
        ***********************************************/
        var isLargeCircle = false; //是否优势弧，即大于180度的弧形

        //小于90度
        if (angel <= 90)
        {
            /*****************
                      *
                      *   *
                      * * ra
               * * * * * * * * *
                      *
                      *
                      *
            ******************/
            var ra = (90 - angel) * Math.PI / 180; //弧度
            endLeft = leftStart + Math.Cos(ra) * radius; //余弦横坐标
            endTop = topStart + radius - Math.Sin(ra) * radius; //正弦纵坐标
        }
        else if (angel <= 180)
        {
            /*****************
                      *
                      *  
                      * 
               * * * * * * * * *
                      * * ra
                      *  *
                      *   *
            ******************/

            var ra = (angel - 90) * Math.PI / 180; //弧度
            endLeft = leftStart + Math.Cos(ra) * radius; //余弦横坐标
            endTop = topStart + radius + Math.Sin(ra) * radius; //正弦纵坐标
        }
        else if (angel <= 270)
        {
            /*****************
                      *
                      *  
                      * 
               * * * * * * * * *
                    * *
                   *ra*
                  *   *
            ******************/
            isLargeCircle = true; //优势弧
            var ra = (angel - 180) * Math.PI / 180;
            endLeft = leftStart - Math.Sin(ra) * radius;
            endTop = topStart + radius + Math.Cos(ra) * radius;
        }
        else if (angel < 360)
        {
            /*****************
                  *   *
                   *  *  
                 ra * * 
               * * * * * * * * *
                      *
                      *
                      *
            ******************/
            isLargeCircle = true; //优势弧
            var ra = (angel - 270) * Math.PI / 180;
            endLeft = leftStart - Math.Cos(ra) * radius;
            endTop = topStart + radius - Math.Sin(ra) * radius;
        }
        else
        {
            isLargeCircle = true; //优势弧
            endLeft = leftStart - 0.001; //不与起点在同一点，避免重叠绘制出非环形
            endTop = topStart;
        }

        var arcEndPt = new Point(endLeft, endTop); //结束点
        var arcSize = new Size(radius, radius);
        const SweepDirection direction = SweepDirection.Clockwise; //顺时针弧形
        //弧形
        var arcSegment = new ArcSegment(arcEndPt, arcSize, 0, isLargeCircle, direction, true);

        //形状集合
        var pathSegmentCollection = new PathSegmentCollection { arcSegment };

        //路径描述
        var pathFigure = new PathFigure
        {
            StartPoint = new Point(leftStart, topStart), //起始地址
            Segments = pathSegmentCollection
        };

        //路径描述集合
        var pathFigureCollection = new PathFigureCollection { pathFigure };

        //复杂形状
        var pathGeometry = new PathGeometry
        {
            Figures = pathFigureCollection
        };

        //Data赋值
        PART_Bar.Data = pathGeometry;
        //达到100%则闭合整个
        if (angel == 360)
            PART_Bar.Data = Geometry.Parse(PART_Bar.Data.ToString() + " z");
    }
}