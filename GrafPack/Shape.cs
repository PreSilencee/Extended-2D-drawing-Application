using System;
using System.Drawing;

namespace GrafPack
{
    abstract class Shape
    {
        Point firstPt, secondPt, thirdPt, fourthPt;

        int width, height;

        Point midpoint;


        public enum Type { Square, Triangle, Rectangle, Circle}
        string type;

        public Shape() { }

        public void setFirstPt(int x, int y)
        {
            firstPt.X = x;
            firstPt.Y = y;
        }

        public void setFirstPt(Point pt)
        {
            firstPt = pt;
        }

        public Point getFirstPt()
        {
            return firstPt;
        }

        public void setSecondPt(int x, int y)
        {
            secondPt.X = x;
            secondPt.Y = y;
        }

        public void setSecondPt(Point pt)
        {
            secondPt = pt;
        }

        public Point getSecondPt()
        {
            return secondPt;
        }

        public void setThirdPt(int x, int y)
        {
            thirdPt.X = x;
            thirdPt.Y = y;
        }

        public void setThirdPt(Point pt)
        {
            thirdPt = pt;
        }

        public Point getThirdPt()
        {
            return thirdPt;
        }

        public void setFourthPt(int x, int y)
        {
            fourthPt.X = x;
            fourthPt.Y = y;
        }

        public void setFourthPt(Point pt)
        {
            fourthPt = pt;
        }

        public Point getFourthPt()
        {
            return fourthPt;
        }

        public void setWidth(int w)
        {
            width = w;
        }

        public int getWidth()
        {
            return width;
        }

        public void setHeight(int h)
        {
            height = h;
        }

        public int getHeight()
        {
            return height;
        }

        public void setMidPoint(int x, int y)
        {
            midpoint.X = x;
            midpoint.Y = y;
        }

        public Point getMidPoint()
        {
            return midpoint;
        }

        public void setType(string type)
        {
            this.type = type;
        }

        public string getType()
        {
            return type;
        }

        public float GetAngle(PointF mouseLocation, PointF currentLocation, PointF centrePoint)
        {
            float angle = (float)Math.Atan2(mouseLocation.Y - centrePoint.Y, mouseLocation.X - centrePoint.X);
            float angle1 = (float)Math.Atan2(currentLocation.Y - centrePoint.Y, currentLocation.X - centrePoint.X);
            return angle - angle1;
        }

        public float EditAngle(float initialAngle, float angle)
        {
            int angleOut = (int)(initialAngle + angle);

            if(angleOut >= 360) { angleOut = angleOut % 360; }
            if(angleOut < 0) { angleOut = 360 - (-angleOut % 360); }

            return angleOut;
        }

        public PointF RotatePoint(float angle, Point pt)
        {
            float cosa = (float)Math.Cos(angle * Math.PI / 180.0);
            float sina = (float)Math.Sin(angle * Math.PI / 180.0);
            float X = cosa * (pt.X - getMidPoint().X) - sina * (pt.Y - getMidPoint().Y) + getMidPoint().X;
            float Y = sina * (pt.X - getMidPoint().X) + cosa * (pt.Y - getMidPoint().Y) + getMidPoint().Y;

            return new PointF(X, Y);
        }

        public abstract void Draw(Graphics g, Pen p);

        public abstract bool Contains(Point location);

        public abstract void DrawCentrePoint(Graphics g, Pen p, SolidBrush b);

        


    }
    class MySquare : Shape
    {
        //get the difference of x
        double xDiff{ get { return getFourthPt().X - getFirstPt().X; }}

        //get the difference of y
        double yDiff { get { return getFourthPt().Y - getFirstPt().Y; } }

        //get the midpoint of x
        double xMid{ get { return (getFourthPt().X + getFirstPt().X) / 2; } }

        //get the midpoint of y
        double yMid{ get { return (getFourthPt().Y + getFirstPt().Y) / 2; } }

        //constructor for square
        public MySquare(Point keyPt, Point oppPt)
        {
            setFirstPt(keyPt.X, keyPt.Y);
            setFourthPt(oppPt.X, oppPt.Y);
            setType(Type.Square.ToString());
            setWidth(getFourthPt().X - getFirstPt().X);
            setHeight(getFourthPt().Y - getFirstPt().Y);
            setMidPoint(Math.Abs((int)xMid), Math.Abs((int)yMid));
        }

        //override method to detect the square
        public override bool Contains(Point location)
        {
            //detect the normal square
            bool result1 = location.X >= getFirstPt().X && location.X <= getFourthPt().X
                        && location.Y >= getFirstPt().Y && location.Y <= getFourthPt().Y;

            //detect the square that reverse draw by the user
            bool result2 = location.X >= getThirdPt().X && location.X <= getSecondPt().X
                        && location.Y >= getThirdPt().Y && location.Y <= getSecondPt().Y;

            //detect the square (diamond)
            //divide into two triangle and find their area
            int triangleArea1 = Area(getFirstPt(), getSecondPt(), getFourthPt());
            int triangleArea2 = Area(getFirstPt(), getThirdPt(), getFourthPt());

            //find total area 
            int totalTriangleArea = triangleArea1 + triangleArea2;

            //get each area with the current mouse location
            //location = mouse
            //first triangle
            int area1 = Area(location, getSecondPt(), getFourthPt());
            int area2 = Area(getFirstPt(), location, getFourthPt());
            int area3 = Area(getFirstPt(), getSecondPt(), location);

            //get each area with the current mouse location
            //location = mouse
            //second triangle
            int area4 = Area(location, getThirdPt(), getFourthPt());
            int area5 = Area(getFirstPt(), location, getFourthPt());
            int area6 = Area(getFirstPt(), getThirdPt(), location);

            //get total area
            int totalArea = area1 + area2 + area3 + area4 + area5 + area6;

            //detect the abnormal square
            bool result3 = location.X >= getSecondPt().X && location.X <= getFirstPt().X
                        && location.Y >= getThirdPt().Y && location.Y <= getFirstPt().Y;

            //detect the reverse abnormal square
            bool result4 = location.X >= getFirstPt().X && location.X <= getSecondPt().X
                        && location.Y >= getFirstPt().Y && location.Y <= getThirdPt().Y;

            //return true if mouse in the circle
            return result1 || result2 || result3 || result4 || totalTriangleArea == totalArea || totalTriangleArea == (totalArea + 1);
        }

        //override method to draw square
        public override void Draw(Graphics g, Pen p)
        {
            g.DrawLine(p, getFirstPt(), getSecondPt());
            g.DrawLine(p, getSecondPt(), getFourthPt());
            g.DrawLine(p, getFourthPt(), getThirdPt());
            g.DrawLine(p, getThirdPt(), getFirstPt());

        }

        public int Area(Point first, Point second, Point third)
        {
            return (int)Math.Abs((first.X * (second.Y - third.Y) + second.X * (third.Y - first.Y) + third.X * (first.Y - second.Y)) / 2.0);
        }

        public override void DrawCentrePoint(Graphics g, Pen p, SolidBrush b)
        {
            setMidPoint((int)xMid, (int)yMid);
            Rectangle midRectangle = new Rectangle(getMidPoint().X - 5, getMidPoint().Y - 5, 10, 10);
            g.DrawEllipse(p, midRectangle);
            g.FillEllipse(b, midRectangle);
        }

    }

    class MyTriangle : Shape
    {
        public MyTriangle(Point keyPt, Point oppPt)
        {
            setFirstPt(keyPt.X, keyPt.Y);
            setFourthPt(oppPt.X, oppPt.Y);
            setType(Type.Triangle.ToString());
        }

        //override method to draw triangle
        public override void Draw(Graphics g, Pen p)
        {
            g.DrawLine(p, getSecondPt(), getThirdPt());
            g.DrawLine(p, getFourthPt(), getThirdPt());
            g.DrawLine(p, getFourthPt(), getSecondPt());
        }

        //override method to detect the triangle
        public override bool Contains(Point location)
        {
            //find the area of triangle
            int triangleArea = Area(getSecondPt(), getThirdPt(), getFourthPt());

            //get each area with the current mouse location
            //location = mouse
            int area1 = Area(location, getThirdPt(), getFourthPt());
            int area2 = Area(getSecondPt(), location, getFourthPt());
            int area3 = Area(getSecondPt(), getThirdPt(), location);

            //find total area
            int totalArea = area1 + area2 + area3;

            //return true if mouse location is in the triangle
            return triangleArea == totalArea || triangleArea == (totalArea + 1);
        }

        public int Area(Point first, Point second, Point third)
        {
            return (int)Math.Abs((first.X * (second.Y - third.Y) + second.X * (third.Y - first.Y) + third.X * (first.Y - second.Y)) / 2.0);
        }

        public override void DrawCentrePoint(Graphics g, Pen p, SolidBrush b)
        {
            setMidPoint((getSecondPt().X + getThirdPt().X + getFourthPt().X) / 3, (getSecondPt().Y + getThirdPt().Y + getFourthPt().Y) / 3);
            Rectangle midRectangle = new Rectangle(getMidPoint().X - 5, getMidPoint().Y - 5, 10, 10);
            g.DrawEllipse(p, midRectangle);
            g.FillEllipse(b, midRectangle);
        }
    }

    class MyRectangle : Shape
    {
        public MyRectangle(Point keyPt, Point oppPt)
        {
            setFirstPt(keyPt.X, keyPt.Y);
            setFourthPt(oppPt.X, oppPt.Y);
            setType(Type.Rectangle.ToString());
            setWidth(getFourthPt().X - getFirstPt().X);
            setHeight(getFourthPt().Y - getFirstPt().Y);
        }

        //override method to detect the rectangle
        public override bool Contains(Point location)
        {
            //detect the normal rectangle
            bool result1 = location.X >= getFirstPt().X && location.X <= getFourthPt().X
                        && location.Y >= getFirstPt().Y && location.Y <= getFourthPt().Y;

            //detect the reverse draw rectangle
            bool result2 = location.X >= getSecondPt().X && location.X <= getThirdPt().X
                        && location.Y >= getSecondPt().Y && location.Y <= getThirdPt().Y;
            
            //return true if mouse location is in the rectangle
            return result1 || result2;

        }

        //override method to draw rectangle
        public override void Draw(Graphics g, Pen p)
        {
            g.DrawLine(p, getFirstPt(), getSecondPt());
            g.DrawLine(p, getSecondPt(), getFourthPt());
            g.DrawLine(p, getFourthPt(), getThirdPt());
            g.DrawLine(p, getThirdPt(), getFirstPt());
        }

        public override void DrawCentrePoint(Graphics g, Pen p, SolidBrush b)
        {
            setMidPoint((getFirstPt().X + getFourthPt().X) / 2, (getFirstPt().Y + getFourthPt().Y) / 2);
            Rectangle midRectangle = new Rectangle(getMidPoint().X - 5, getMidPoint().Y - 5, 10, 10);
            g.DrawEllipse(p, midRectangle);
            g.FillEllipse(b, midRectangle);
        }

    }

    class MyCircle : Shape
    {
        public MyCircle(Point keyPt, Point oppPt)
        {
            setFirstPt(keyPt.X, keyPt.Y);
            setFourthPt(oppPt.X, oppPt.Y);
            setMidPoint((int)(keyPt.X + oppPt.X)/2, (int)(keyPt.Y + oppPt.Y)/2);
            setType(Type.Circle.ToString());
        }

        //override method to detect circle
        public override bool Contains(Point location)
        {
            //set mid point 
            setMidPoint((getFirstPt().X + getFourthPt().X) / 2, (getFirstPt().Y + getFourthPt().Y) / 2);
           
            //find radius
            int radius = (int)Math.Sqrt(Math.Pow(getFirstPt().X - getMidPoint().X, 2) + (Math.Pow(getFirstPt().Y - getMidPoint().Y, 2)));

            //get result
            int result = (int)((Math.Pow(location.X - getMidPoint().X, 2) / Math.Pow(radius, 2)) + (Math.Pow(location.Y - getMidPoint().Y, 2) / Math.Pow(radius, 2)));

            //if result is less than, mouse location is in the circle
            return result < 1;
        }

        //override method to draw circle
        public override void Draw(Graphics g, Pen p)
        {
            int diameter = Math.Max(Math.Abs(getFirstPt().X - getFourthPt().X), 
                Math.Abs(getFirstPt().Y - getFourthPt().Y));

            g.DrawEllipse(p, Math.Min(getFirstPt().X, getFourthPt().X), 
                Math.Min(getFirstPt().Y, getFourthPt().Y), diameter, diameter);
        }

        public override void DrawCentrePoint(Graphics g, Pen p, SolidBrush b)
        {
            setMidPoint((getFirstPt().X + getFourthPt().X) / 2, (getFirstPt().Y + getFourthPt().Y) / 2);
            Rectangle midRectangle = new Rectangle(getMidPoint().X - 5, getMidPoint().Y - 5, 10, 10);
            g.DrawEllipse(p, midRectangle);
            g.FillEllipse(b, midRectangle);
        }
    }

}
