using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GrafPack
{
    public partial class Main : Form
    {
        //main menu;
        private MainMenu mainMenu;

        //menu item
        private MenuItem moveItem = new MenuItem();
        private MenuItem rotateItem = new MenuItem();
        private MenuItem rotateManualItem = new MenuItem();
        private MenuItem rotate30Item = new MenuItem();
        private MenuItem rotate45Item = new MenuItem();
        private MenuItem rotate60Item = new MenuItem();
        private MenuItem deleteItem = new MenuItem();

        // bitmap and graphics for background
        private Bitmap bitmap;
        private Graphics gB;

        //list of shape
        private List<Shape> shapeList;

        private int clickNumber = 0;

        //mouse click
        private bool selectMouseClickStatus = false;
        private bool selectSquareMCStatus = false;
        private bool selectTriangleMCStatus = false;
        private bool selectRectangleMCStatus = false;
        private bool selectCircleMCStatus = false;

        //rubber-banding
        private bool selectRubberBandingStatus = false;
        private bool selectSquareRBStatus = false;
        private bool selectTriangleRBStatus = false;
        private bool selectRectangleRBStatus = false;
        private bool selectCircleRBStatus = false;

        //move shape status
        private bool moveShapeStatus = false;

        //rotate shape status
        private bool rotateShapeStatus = false;

        //select shape status
        private bool selectShapeStatus = false;
        public enum Type { Square, Triangle, Rectangle, Circle }

        private Shape selectedShape;
        private Shape isSelectingShape;

        private bool isSelecting = false;
        private bool isMove = false;
        private bool isRotate = false;

        private bool isNew = false;
        private bool isRefresh = false;

        //initialize for drag
        private Point one = Point.Empty;
        private Point two = Point.Empty;
        private Point StartPoint = Point.Empty;
        private Point EndPoint = Point.Empty;

        //move
        private PointF StartShapePoint = Point.Empty;
        private PointF EndShapePoint = Point.Empty;
        private PointF StartMovePoint = Point.Empty;

        //rotate
        private PointF StartRotatePoint = Point.Empty;
        private float rotateAngle;
        private float angle;
        private float initialAngle;



        public Main()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer, true);

            WindowState = FormWindowState.Maximized;
            BackColor = Color.White;
            moveItem.Text = "&Move";
            rotateItem.Text = "&Rotate";
            rotateManualItem.Text = "&Rotate Manually";
            rotate30Item.Text = "&Rotate 30°";
            rotate45Item.Text = "&Rotate 45°";
            rotate60Item.Text = "&Rotate 60°";
            deleteItem.Text = "&Delete";

            Menu = getMenu();
            shapeList = new List<Shape>();

        }

        public MainMenu getMenu()
        {
            mainMenu = new MainMenu();
            MenuItem fileItem = new MenuItem();
            MenuItem newItem = new MenuItem();
            MenuItem exitItem = new MenuItem();

            fileItem.Text = "&File";
            newItem.Text = "&New";
            exitItem.Text = "&Exit";

            MenuItem createItem = new MenuItem();
            createItem.Text = "&Create";

            //mouse-click
            MenuItem mouseClickItem = new MenuItem();
            MenuItem squareItemMC = new MenuItem();
            MenuItem triangleItemMC = new MenuItem();
            MenuItem rectangleItemMC = new MenuItem();
            MenuItem circleItemMC = new MenuItem();

            mouseClickItem.Text = "&Mouse Click";
            squareItemMC.Text = "&Square";
            triangleItemMC.Text = "&Triangle";
            rectangleItemMC.Text = "&Rectangle";
            circleItemMC.Text = "&Circle";

            //rubber-banding
            MenuItem rubberBandingItem = new MenuItem();
            MenuItem squareItemRB = new MenuItem();
            MenuItem triangleItemRB = new MenuItem();
            MenuItem rectangleItemRB = new MenuItem();
            MenuItem circleItemRB = new MenuItem();

            rubberBandingItem.Text = "&Rubber-banding";
            squareItemRB.Text = "&Square";
            triangleItemRB.Text = "&Triangle";
            rectangleItemRB.Text = "&Rectangle";
            circleItemRB.Text = "&Circle";

            //select item
            MenuItem selectItem = new MenuItem();
            selectItem.Text = "&Select";

            mainMenu.MenuItems.Add(fileItem);
            fileItem.MenuItems.Add(newItem);
            fileItem.MenuItems.Add(exitItem);

            mainMenu.MenuItems.Add(createItem);
            createItem.MenuItems.Add(mouseClickItem);
            mouseClickItem.MenuItems.Add(squareItemMC);
            mouseClickItem.MenuItems.Add(triangleItemMC);
            mouseClickItem.MenuItems.Add(rectangleItemMC);
            mouseClickItem.MenuItems.Add(circleItemMC);

            createItem.MenuItems.Add(rubberBandingItem);
            rubberBandingItem.MenuItems.Add(squareItemRB);
            rubberBandingItem.MenuItems.Add(triangleItemRB);
            rubberBandingItem.MenuItems.Add(rectangleItemRB);
            rubberBandingItem.MenuItems.Add(circleItemRB);
            mainMenu.MenuItems.Add(selectItem);

            newItem.Click += new EventHandler(selectNew);
            exitItem.Click += new EventHandler(selectExit);

            squareItemMC.Click += new EventHandler(selectSquareMC);
            triangleItemMC.Click += new EventHandler(selectTriangleMC);
            rectangleItemMC.Click += new EventHandler(selectRectangleMC);
            circleItemMC.Click += new EventHandler(selectCircleMC);

            squareItemRB.Click += new EventHandler(selectSquareRB);
            triangleItemRB.Click += new EventHandler(selectTriangleRB);
            rectangleItemRB.Click += new EventHandler(selectRectangleRB);
            circleItemRB.Click += new EventHandler(selectCircleRB);
            selectItem.Click += new EventHandler(selectShape);

            return mainMenu;
        }

        private void selectNew(object sender, EventArgs e)
        {
            //close all mouse click
            selectMouseClickStatus = false;
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close all rubber-banding
            selectRubberBandingStatus = false;
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close select, move and rotate
            moveShapeStatus = false;
            rotateShapeStatus = false;
            selectShapeStatus = false;

            //clear the list
            shapeList = new List<Shape>();

            //clear current graphics using white color
            CreateGraphics().Clear(Form.ActiveForm.BackColor);

            //get current graphics and store into bitmap
            bitmap = new Bitmap(Width, Height);

            //push bitmap into graphics background
            gB = Graphics.FromImage(bitmap);
            gB.Clear(this.BackColor);

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            //repaint
            Refresh();
        }

        private void selectShape(object sender, EventArgs e)
        {
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            selectShapeStatus = true;
            isSelecting = true;
            moveShapeStatus = false;
            rotateShapeStatus = false;
            selectRubberBandingStatus = false;
            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
            }

            if (selectedShape != null)
            {
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);
                selectedShape = null;
            }
        }

        private void selectExit(object sender, EventArgs e)
        {
            //terminate application
            this.Close();
        }

        private void selectSquareMC(object sender, EventArgs e)
        {
            //close all rubber-banding
            selectRubberBandingStatus = false;
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close other mouse click
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = false;

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //draw all the shape again
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - remove centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);

                //set null for the selected shape
                selectedShape = null;
            }

            //open mouse click and square
            selectMouseClickStatus = true;
            selectSquareMCStatus = true;
            this.Cursor = Cursors.Cross;

        }

        private void selectTriangleMC(object sender, EventArgs e)
        {
            //close all rubber-banding
            selectRubberBandingStatus = false;
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close other mouse click
            selectSquareMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = false;

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //draw all shape again
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - remove centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);

                //set null for the selected shape
                selectedShape = null;
            }

            //open mouse click and triangle
            selectMouseClickStatus = true;
            selectTriangleMCStatus = true;
            this.Cursor = Cursors.Cross;
        }

        private void selectRectangleMC(object sender, EventArgs e)
        {
            //close all rubber-banding
            selectRubberBandingStatus = false;
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close other mouse click
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectCircleMCStatus = false;

            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = false;

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //draw all shape again
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - remove centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);

                //set null for the selected shape
                selectedShape = null;
            }

            //open mouse click and rectangle
            selectMouseClickStatus = true;
            selectRectangleMCStatus = true;
            this.Cursor = Cursors.Cross;
        }

        private void selectCircleMC(object sender, EventArgs e)
        {
            //close all rubber-banding
            selectRubberBandingStatus = false;
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close other mouse click
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;

            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = false;

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //draw all the shape again
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - remove centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);

                //set null for the selected shape
                selectedShape = null;
            }

            //open mouse click and circle
            selectMouseClickStatus = true;
            selectCircleMCStatus = true;
            this.Cursor = Cursors.Cross;
        }

        private void selectSquareRB(object sender, EventArgs e)
        {
            //close all mouse click
            selectMouseClickStatus = false;
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close other rubber-banding
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = false;

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //draw all shape again
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - remove centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);

                //set null for the selected shape
                selectedShape = null;
            }

            //open rubber-banding and square
            selectRubberBandingStatus = true;
            selectSquareRBStatus = true;
            this.Cursor = Cursors.Cross;

        }

        private void selectTriangleRB(object sender, EventArgs e)
        {
            //close all mouse click
            selectMouseClickStatus = false;
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close other rubber-banding
            selectSquareRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = false;

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //draw shape again
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - remove centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);

                //set null for the selected shape
                selectedShape = null;
            }

            //open rubber-banding and triangle
            selectRubberBandingStatus = true;
            selectTriangleRBStatus = true;
            this.Cursor = Cursors.Cross;
        }

        private void selectRectangleRB(object sender, EventArgs e)
        {
            //close all mouse click
            selectMouseClickStatus = false;
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close other rubber-banding
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectCircleRBStatus = false;

            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = false;

            //remove menu item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //draw all shape again 
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - remove centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);

                //set null for the selected shape
                selectedShape = null;
            }

            //open rubber-banding and rectangle
            selectRubberBandingStatus = true;
            selectRectangleRBStatus = true;
            this.Cursor = Cursors.Cross;
        }

        private void selectCircleRB(object sender, EventArgs e)
        {
            //close all mouse click
            selectMouseClickStatus = false;
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close other rubber-banding
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;

            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = false;

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //draw all shape again
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - remove centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);

                //set null for the selected shape
                selectedShape = null;
            }

            //open rubber-banding and circle
            selectRubberBandingStatus = true;
            selectCircleRBStatus = true;
            this.Cursor = Cursors.Cross;
        }

        private void moveShape(object sender, EventArgs e)
        {
            //close all mouse click
            selectMouseClickStatus = false;
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close all rubber-banding
            selectRubberBandingStatus = false;
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close rotate
            selectShapeStatus = false;
            rotateShapeStatus = false;

            Graphics g = CreateGraphics();
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 4;
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //draw centre point
                selectedShape.DrawCentrePoint(g, whitePen, whiteBrush);
            }

            //open move shape status
            moveShapeStatus = true;
            isRefresh = true;
        }

        private void deleteShape(object sender, EventArgs e)
        {
            //close all mouse click
            selectMouseClickStatus = false;
            selectSquareMCStatus = false;
            selectTriangleMCStatus = false;
            selectRectangleMCStatus = false;
            selectCircleMCStatus = false;

            //close all rubber-banding
            selectRubberBandingStatus = false;
            selectSquareRBStatus = false;
            selectTriangleRBStatus = false;
            selectRectangleRBStatus = false;
            selectCircleRBStatus = false;

            //close move, select, rotate
            moveShapeStatus = false;
            selectShapeStatus = false;
            rotateShapeStatus = false;

            //remove shape from list
            shapeList.Remove(selectedShape);

            //clear graphics
            CreateGraphics().Clear(Form.ActiveForm.BackColor);

            //get current graphics and store into bitmap
            //push bitmap to the graphics background
            bitmap = new Bitmap(Width, Height);
            gB = Graphics.FromImage(bitmap);
            gB.Clear(this.BackColor);

            //remove move item from menu
            if (mainMenu.MenuItems.Contains(moveItem))
            {
                mainMenu.MenuItems.Remove(moveItem);
            }

            //remove rotate item from menu
            if (mainMenu.MenuItems.Contains(rotateItem))
            {
                mainMenu.MenuItems.Remove(rotateItem);
            }

            //remove delete item from menu
            if (mainMenu.MenuItems.Contains(deleteItem))
            {
                mainMenu.MenuItems.Remove(deleteItem);
            }

            Graphics g = CreateGraphics();

            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;

            //draw all shape again
            foreach (var shape in shapeList)
            {
                shape.Draw(g, blackPen);
                shape.Draw(gB, blackPen);
            }

        }

        private void rotateShape(object sender, EventArgs e)
        {
            //close select, move, rotate
            selectShapeStatus = false;
            moveShapeStatus = false;
            rotateShapeStatus = true;

            Graphics g = CreateGraphics();
            SolidBrush b = new SolidBrush(Color.LightGreen);
            Pen redPen = new Pen(Color.Red);

            //check selected shape whether is null
            if (selectedShape != null)
            {
                //not null - draw centre point
                selectedShape.DrawCentrePoint(g, redPen, b);
            }

        }

        private void Main_Load(object sender, EventArgs e)
        {
            //get current graphics and store into bitmap
            bitmap = new Bitmap(Width, Height);

            //push bitmap into graphics background
            gB = Graphics.FromImage(bitmap);
            gB.Clear(this.BackColor);
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
            Pen blackPen = new Pen(Color.Black);
            blackPen.Width = 3;

            //set smoothing mode
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Graphics g = e.Graphics;
            //using g to draw bitmap
            g.DrawImage(bitmap, 0, 0);

            //pen when drawing
            Pen drawPen = new Pen(Color.LightGreen);
            drawPen.Width = 3;
            drawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            //pen when moving
            Pen movePen = new Pen(Color.Red);
            movePen.Width = 3;

            //light green brush
            SolidBrush b = new SolidBrush(Color.LightGreen);

            //refresh
            if (isRefresh)
            {
                foreach (var shape in shapeList)
                {
                    shape.Draw(g, blackPen);
                    shape.Draw(gB, blackPen);
                }

                isRefresh = false;
            }

            //draw new shape
            if (isNew)
            {
                if (selectMouseClickStatus)
                {
                    if (selectSquareMCStatus)
                    {
                        selectMouseClickStatus = false;
                        selectSquareMCStatus = false;

                        //square
                        MySquare aSquare = new MySquare(one, two);

                        //find x and y difference
                        double xDiff = two.X - one.X;
                        double yDiff = two.Y - one.Y;

                        //find midpoint
                        double xMid = (two.X + one.X) / 2;
                        double yMid = (two.Y + one.Y) / 2;

                        //set second point and thirdpoint
                        aSquare.setSecondPt((int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
                        aSquare.setThirdPt((int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));

                        //draw square
                        aSquare.Draw(g, blackPen);

                        //add square into list
                        shapeList.Add(aSquare);

                        //refresh
                        isRefresh = true;
                        isNew = false;
                        this.Cursor = Cursors.Default;
                    }

                    if (selectTriangleMCStatus)
                    {
                        selectMouseClickStatus = false;
                        selectTriangleMCStatus = false;

                        //triangle
                        MyTriangle aTriangle = new MyTriangle(one, two);

                        //find midpoint of the triangle
                        double xMid = (two.X + one.X) / 2;

                        //set second point and third point
                        aTriangle.setSecondPt(one.X, two.Y);
                        aTriangle.setThirdPt((int)xMid, one.Y);

                        //draw triangle
                        aTriangle.Draw(g, blackPen);

                        //add triangle into the list
                        shapeList.Add(aTriangle);

                        isRefresh = true;
                        isNew = false;
                        this.Cursor = Cursors.Default;
                    }

                    if (selectRectangleMCStatus)
                    {
                        selectMouseClickStatus = false;
                        selectRectangleMCStatus = false;

                        //rectangle
                        MyRectangle aRectangle = new MyRectangle(one, two);

                        //set second point and third point
                        aRectangle.setSecondPt(two.X, one.Y);
                        aRectangle.setThirdPt(one.X, two.Y);

                        //draw rectangle
                        aRectangle.Draw(g, blackPen);

                        //add rectangle into list
                        shapeList.Add(aRectangle);

                        isRefresh = true;
                        isNew = false;
                        this.Cursor = Cursors.Default;
                    }

                    if (selectCircleMCStatus)
                    {
                        selectMouseClickStatus = false;
                        selectCircleMCStatus = false;

                        //circle
                        MyCircle aCircle = new MyCircle(one, two);

                        //draw circle
                        aCircle.Draw(g, blackPen);

                        //add circle into list
                        shapeList.Add(aCircle);

                        isRefresh = true;
                        isNew = false;
                        this.Cursor = Cursors.Default;
                    }

                }

                if (selectRubberBandingStatus)
                {
                    if (selectSquareRBStatus)
                    {
                        //get current mouse location
                        EndPoint = PointToClient(MousePosition);

                        //square
                        MySquare aSquare = new MySquare(StartPoint, EndPoint);

                        //find x and y difference
                        double xDiff = EndPoint.X - StartPoint.X;
                        double yDiff = EndPoint.Y - StartPoint.Y;

                        //find midpoint
                        double xMid = (EndPoint.X + StartPoint.X) / 2;
                        double yMid = (EndPoint.Y + StartPoint.Y) / 2;

                        //set second and third point
                        aSquare.setSecondPt((int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
                        aSquare.setThirdPt((int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));

                        //draw foreground
                        aSquare.Draw(g, drawPen);
                    }

                    if (selectTriangleRBStatus)
                    {
                        //get current mouse location
                        EndPoint = PointToClient(MousePosition);

                        //triangle
                        MyTriangle aTriangle = new MyTriangle(StartPoint, EndPoint);

                        //find midpoint
                        double xMid = (EndPoint.X + StartPoint.X) / 2;

                        //set second and third point
                        aTriangle.setSecondPt(StartPoint.X, EndPoint.Y);
                        aTriangle.setThirdPt((int)xMid, StartPoint.Y);

                        //draw foreground
                        aTriangle.Draw(g, drawPen);
                    }

                    if (selectRectangleRBStatus)
                    {
                        //get current mouse location
                        EndPoint = PointToClient(MousePosition);

                        //rectangle
                        MyRectangle aRectangle = new MyRectangle(StartPoint, EndPoint);

                        //set second and third point
                        aRectangle.setSecondPt(EndPoint.X, StartPoint.Y);
                        aRectangle.setThirdPt(StartPoint.X, EndPoint.Y);

                        //draw foreground
                        aRectangle.Draw(g, drawPen);
                    }

                    if (selectCircleRBStatus)
                    {
                        //get current mouse location
                        EndPoint = PointToClient(MousePosition);

                        //circle
                        MyCircle aCircle = new MyCircle(StartPoint, EndPoint);

                        //draw foreground
                        aCircle.Draw(g, blackPen);
                    }
                }
            }

            //move selected shape
            if (isMove)
            {
                bitmap = new Bitmap(Width, Height);
                gB = Graphics.FromImage(bitmap);
                gB.Clear(this.BackColor);

                foreach (var shape in shapeList)
                {
                    if (!shape.Equals(selectedShape))
                    {
                        shape.Draw(g, blackPen);
                        shape.Draw(gB, blackPen);
                    }
                }

                if (selectedShape != null)
                {
                    selectedShape.Draw(g, movePen);
                }

            }

            //rotate the shape
            if (isRotate)
            {
                bitmap = new Bitmap(Width, Height);
                gB = Graphics.FromImage(bitmap);
                gB.Clear(this.BackColor);

                foreach (var shape in shapeList)
                {
                    if (!shape.Equals(selectedShape))
                    {
                        shape.Draw(g, blackPen);
                        shape.Draw(gB, blackPen);
                    }
                }

                if (selectedShape != null)
                {
                    selectedShape.Draw(g, movePen);
                    selectedShape.DrawCentrePoint(g, movePen, b);
                }


            }
        }

        //performed function when mouse is clicked
        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            //mouse click to draw shape
            if (e.Button == MouseButtons.Left && selectMouseClickStatus)
            {
                //when click first time
                if (clickNumber == 0)
                {
                    one = new Point(e.X, e.Y);
                    clickNumber = 1;
                }
                //when click second time
                else if (clickNumber == 1)
                {
                    two = new Point(e.X, e.Y);
                    clickNumber = 0;
                    isNew = true;
                }
            }

            //rubber-banding to draw shape
            if (e.Button == MouseButtons.Left && selectRubberBandingStatus)
            {
                //set current mouse location to start point and end point
                StartPoint = EndPoint = e.Location;
                isNew = true;
            }

            //select shape
            if (e.Button == MouseButtons.Left && selectShapeStatus && isSelectingShape != null)
            {
                selectedShape = isSelectingShape;
                isSelectingShape = null;
                isSelecting = false;
                selectShapeStatus = false;

                this.Cursor = Cursors.Default;

                //set sub menu for the selectedshape
                mainMenu.MenuItems.Add(3, moveItem);
                mainMenu.MenuItems.Add(4, rotateItem);
                mainMenu.MenuItems.Add(5, deleteItem);
                moveItem.Click += new EventHandler(moveShape);
                rotateItem.Click += new EventHandler(rotateShape);
                deleteItem.Click += new EventHandler(deleteShape);
            }

            //move shape
            if (e.Button == MouseButtons.Left && moveShapeStatus)
            {
                if (selectedShape != null)
                {
                    StartShapePoint = selectedShape.getFirstPt();
                    EndShapePoint = selectedShape.getFourthPt();
                    StartMovePoint = e.Location;
                    isMove = true;
                }
            }

            //rotate shape
            if (e.Button == MouseButtons.Left && rotateShapeStatus)
            {
                if (selectedShape != null)
                {
                    StartRotatePoint = e.Location;
                    isRotate = true;
                    initialAngle = angle;

                }
            }
        }

        //performed function when mouse is moving
        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            //if draw new
            if (isNew)
            {
                if (e.Button == MouseButtons.Left && selectRubberBandingStatus)
                {
                    //repaint()
                    Invalidate();
                }
            }

            //change the cursor when moving 
            if (moveShapeStatus)
            {
                if (isMove)
                {
                    this.Cursor = Cursors.SizeAll;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }

            }

            //change the cursor when rotate
            if (rotateShapeStatus)
            {
                if (isRotate)
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }

            }

            //when shape is moving
            if (isMove)
            {
                if (selectedShape != null)
                {
                    //square
                    if (selectedShape.getType().Equals(Type.Square.ToString()))
                    {
                        //refer by 
                        selectedShape.setFirstPt((int)(StartShapePoint.X + e.X - StartMovePoint.X), (int)(StartShapePoint.Y + e.Y - StartMovePoint.Y));
                        selectedShape.setFourthPt((int)(EndShapePoint.X + e.X - StartMovePoint.X), (int)(EndShapePoint.Y + e.Y - StartMovePoint.Y));
                        double xDiff = selectedShape.getFourthPt().X - selectedShape.getFirstPt().X;
                        double yDiff = selectedShape.getFourthPt().Y - selectedShape.getFirstPt().Y;
                        double xMid = (selectedShape.getFourthPt().X + selectedShape.getFirstPt().X) / 2;
                        double yMid = (selectedShape.getFourthPt().Y + selectedShape.getFirstPt().Y) / 2;

                        selectedShape.setSecondPt((int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
                        selectedShape.setThirdPt((int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
                    }
                    //triangle
                    else if (selectedShape.getType().Equals(Type.Triangle.ToString()))
                    {
                        selectedShape.setFirstPt((int)(StartShapePoint.X + e.X - StartMovePoint.X), (int)(StartShapePoint.Y + e.Y - StartMovePoint.Y));
                        selectedShape.setFourthPt((int)(EndShapePoint.X + e.X - StartMovePoint.X), (int)(EndShapePoint.Y + e.Y - StartMovePoint.Y));
                        double xMid = (selectedShape.getFourthPt().X + selectedShape.getFirstPt().X) / 2;
                        selectedShape.setSecondPt(selectedShape.getFirstPt().X, selectedShape.getFourthPt().Y);
                        selectedShape.setThirdPt((int)xMid, selectedShape.getFirstPt().Y);
                    }
                    //rectangle
                    else if (selectedShape.getType().Equals(Type.Rectangle.ToString()))
                    {
                        selectedShape.setFirstPt((int)(StartShapePoint.X + e.X - StartMovePoint.X), (int)(StartShapePoint.Y + e.Y - StartMovePoint.Y));
                        selectedShape.setFourthPt((int)(EndShapePoint.X + e.X - StartMovePoint.X), (int)(EndShapePoint.Y + e.Y - StartMovePoint.Y));
                        selectedShape.setSecondPt(selectedShape.getFourthPt().X, selectedShape.getFirstPt().Y);
                        selectedShape.setThirdPt(selectedShape.getFirstPt().X, selectedShape.getFourthPt().Y);
                    }
                    //circle
                    else if (selectedShape.getType().Equals(Type.Circle.ToString()))
                    {
                        selectedShape.setFirstPt((int)(StartShapePoint.X + e.X - StartMovePoint.X), (int)(StartShapePoint.Y + e.Y - StartMovePoint.Y));
                        selectedShape.setFourthPt((int)(EndShapePoint.X + e.X - StartMovePoint.X), (int)(EndShapePoint.Y + e.Y - StartMovePoint.Y));
                    }
                }
                //repaint
                Invalidate();
            }

            //select shape
            if (selectShapeStatus)
            {
                //selecting the shape
                if (isSelecting)
                {
                    Graphics g = CreateGraphics();
                    Pen hoverPen = new Pen(Color.Red);
                    hoverPen.Width = 3;

                    Pen blackPen = new Pen(Color.Black);
                    blackPen.Width = 3;

                    if (isSelectingShape == null)
                    {
                        foreach (var shape in shapeList)
                        {

                            if (shape.Contains(e.Location))
                            {
                                isSelectingShape = shape;

                            }
                        }
                    }
                    else
                    {
                        isSelectingShape.Draw(g, hoverPen);
                        this.Cursor = Cursors.Hand;

                        if (!isSelectingShape.Contains(e.Location))
                        {
                            CreateGraphics().Clear(Form.ActiveForm.BackColor);
                            bitmap = new Bitmap(Width, Height);
                            gB = Graphics.FromImage(bitmap);
                            gB.Clear(this.BackColor);

                            foreach (var shape in shapeList)
                            {
                                shape.Draw(g, blackPen);
                                shape.Draw(gB, blackPen);
                            }

                            this.Cursor = Cursors.Default;
                            isSelectingShape = null;
                        }

                    }
                }
            }

            //when shape is rotate
            if (isRotate)
            {
                if (selectedShape != null)
                {
                    //get angle
                    angle = selectedShape.GetAngle(StartRotatePoint, e.Location, selectedShape.getMidPoint());
                    angle = (float)(-angle * 180.0 / Math.PI);
                    angle = selectedShape.EditAngle(initialAngle, angle);

                    //square
                    if (selectedShape.getType().Equals(Type.Square.ToString()))
                    {
                        selectedShape.setFirstPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getFirstPt())));
                        selectedShape.setFourthPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getFourthPt())));
                        double xDiff = selectedShape.getFourthPt().X - selectedShape.getFirstPt().X;
                        double yDiff = selectedShape.getFourthPt().Y - selectedShape.getFirstPt().Y;
                        double xMid = (selectedShape.getFourthPt().X + selectedShape.getFirstPt().X) / 2;
                        double yMid = (selectedShape.getFourthPt().Y + selectedShape.getFirstPt().Y) / 2;

                        selectedShape.setSecondPt((int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
                        selectedShape.setThirdPt((int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
                    }
                    //triangle
                    else if (selectedShape.getType().Equals(Type.Triangle.ToString()))
                    {
                        selectedShape.setSecondPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getSecondPt())));
                        selectedShape.setThirdPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getThirdPt())));
                        selectedShape.setFourthPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getFourthPt())));
                    }
                    //rectangle
                    else if (selectedShape.getType().Equals(Type.Rectangle.ToString()))
                    {
                        selectedShape.setFirstPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getFirstPt())));
                        selectedShape.setSecondPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getSecondPt())));
                        selectedShape.setThirdPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getThirdPt())));
                        selectedShape.setFourthPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getFourthPt())));
                    }
                    //circle
                    else if (selectedShape.getType().Equals(Type.Circle.ToString()))
                    {
                        selectedShape.setFirstPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getFirstPt())));
                        selectedShape.setFourthPt(Point.Ceiling(selectedShape.RotatePoint((float)angle, selectedShape.getFourthPt())));
                    }

                    //repaint
                    Invalidate();
                }
            }
        }

        //performed function when mouse is release
        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            Pen blackpen = new Pen(Color.Black);
            blackpen.Width = 3;

            //if drawn new
            if (isNew)
            {
                //square
                if (selectSquareRBStatus)
                {
                    //close rubber-banding and square
                    selectSquareRBStatus = false;
                    selectRubberBandingStatus = false;

                    //set isNew to false
                    isNew = false;

                    //create aSquare
                    MySquare aSquare = new MySquare(StartPoint, EndPoint);
                    double xDiff = EndPoint.X - StartPoint.X;
                    double yDiff = EndPoint.Y - StartPoint.Y;
                    double xMid = (EndPoint.X + StartPoint.X) / 2;
                    double yMid = (EndPoint.Y + StartPoint.Y) / 2;

                    aSquare.setSecondPt((int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
                    aSquare.setThirdPt((int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));

                    //add into the list
                    shapeList.Add(aSquare);

                    //refresh in Main Paint
                    isRefresh = true;

                    //set cursor to default
                    this.Cursor = Cursors.Default;
                }

                //triangle
                if (selectTriangleRBStatus)
                {
                    //close rubber-banding and triangle
                    selectTriangleRBStatus = false;
                    selectRubberBandingStatus = false;

                    //set isNew to false
                    isNew = false;

                    //create aTriangle
                    MyTriangle aTriangle = new MyTriangle(StartPoint, EndPoint);
                    double xMid = (EndPoint.X + StartPoint.X) / 2;
                    aTriangle.setSecondPt(StartPoint.X, EndPoint.Y);
                    aTriangle.setThirdPt((int)xMid, StartPoint.Y);

                    //add aTriangle into list
                    shapeList.Add(aTriangle);

                    //refresh in Main Paint
                    isRefresh = true;

                    //set cursor to default
                    this.Cursor = Cursors.Default;
                }

                //rectangle
                if (selectRectangleRBStatus)
                {
                    //close rubber-banding and rectangle
                    selectRectangleRBStatus = false;
                    selectRubberBandingStatus = false;

                    //set isNew to false
                    isNew = false;

                    //create aRectangle
                    MyRectangle aRectangle = new MyRectangle(StartPoint, EndPoint);
                    aRectangle.setSecondPt(EndPoint.X, StartPoint.Y);
                    aRectangle.setThirdPt(StartPoint.X, EndPoint.Y);

                    //add aRectangle into the list
                    shapeList.Add(aRectangle);

                    //refresh in Main Paint
                    isRefresh = true;

                    //set cursor to default
                    this.Cursor = Cursors.Default;
                }

                //circle
                if (selectCircleRBStatus)
                {
                    //close rubber-banding and circle
                    selectCircleRBStatus = false;
                    selectRubberBandingStatus = false;

                    //set isNew to false
                    isNew = false;

                    //create aCircle
                    MyCircle aCircle = new MyCircle(StartPoint, EndPoint);

                    //add aCircle to list
                    shapeList.Add(aCircle);

                    //refresh in MainPaint
                    isRefresh = true;

                    //set cursor to default
                    this.Cursor = Cursors.Default;
                }

                //repaint
                Invalidate();

            }

            //set isMove to false when mouse up
            if (isMove)
            {
                isMove = false;
            }

            //set isRotate to false when mouse up
            if (isRotate)
            {
                isRotate = false;
            }

        }
    }
}
