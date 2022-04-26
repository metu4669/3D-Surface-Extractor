using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.VisualC;

namespace PetCoCel
{
    public partial class Form1 : Form
    {
        private Device device = null;

        private VertexBuffer vb = null;

        private IndexBuffer ib = null;

        string[] temppX = new string[0];
        string[] temppY = new string[0];
        string[] temppZ = new string[0];

        private static int terWidth = 250, terLength = 250;

        private float moveSpeed = 0.2f;
        private float turnSpeed = 0.05f;

        private float rotY = 0;
        private float tempY = 0;

        private float rotXZ = 0;
        private float tempXZ = 0;

        private float raisedConst = 0.05f;

        private static int vertCount = terWidth * terLength;
        private static int indCount = (terWidth - 1) * (terLength - 1) * 6;

        private Vector3 camPosition, camLookAt, camUp;

        CustomVertex.PositionColored[] verts = null;

        bool isMiddleMouseDown = false;
        bool isLeftMouseDown = false;

        private static int[] indices = null;

        private FillMode fillMode = FillMode.WireFrame;
        private Color backgroundColor = Color.Black;

        private bool invalidating = true;

        Bitmap heightMap = null;
        public Main formMain;

        public Form1(string[] x, string[] y, string[] z, bool generateRequest, int param, Main form)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            InitializeComponent();

            InitializeGraphics();
            InitializeEventHandler();

            button2.Left = 10;
            button2.Top = menuStrip1.Height + 5;
            button2.Update();

            button1.Update();

            formMain = form;

            if (generateRequest)
            {
                generateMap(x, y, z, param);
            }

        }

        private void InitializeGraphics()
        {
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;

            pp.EnableAutoDepthStencil = true;
            pp.AutoDepthStencilFormat = DepthFormat.D16;

            device = new Device(0, DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, pp);

            GenerateVertext();
            GenerateIndex();

            vb = new VertexBuffer(typeof(CustomVertex.PositionColored), vertCount, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            OnVertexBufferCreate(vb, null);


            ib = new IndexBuffer(typeof(int), indCount, device, Usage.WriteOnly, Pool.Default);
            OnIndexBufferCreate(ib, null);

            //Main Camera Initial Position
            camPosition = new Vector3(2, 4.5f, -3.5f);
            camLookAt = new Vector3(2, 3.5f, -2.5f);
            camUp = new Vector3(0, 1, 0);
        }

        private void InitializeEventHandler()
        {
            vb.Created += new EventHandler(this.OnVertexBufferCreate);
            ib.Created += new EventHandler(this.OnIndexBufferCreate);

            button2.KeyDown += new KeyEventHandler(OnKeyDown);
            this.MouseWheel += new MouseEventHandler(OnMouseScroll);

            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
        }

        private void OnIndexBufferCreate(object sender, EventArgs e)
        {
            IndexBuffer ibuffer = (IndexBuffer)sender;
            ib.SetData(indices, 0, LockFlags.None);
        }

        private void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;

            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void SetupCamera()
        {
            camLookAt.X = (float)Math.Sin(rotY) + camPosition.X + (float)Math.Sin(rotXZ) * (float)Math.Sin(rotY);
            camLookAt.Y = (float)Math.Sin(rotXZ) + camPosition.Y;
            camLookAt.Z = (float)Math.Cos(rotY) + camPosition.Z + (float)Math.Sin(rotXZ) * (float)Math.Cos(rotY);

            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1.0f, 1000.0f);
            device.Transform.View = Matrix.LookAtLH(camPosition, camLookAt, camUp);

            device.RenderState.Lighting = false;
            device.RenderState.FillMode = fillMode;
            device.RenderState.CullMode = Cull.CounterClockwise;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, backgroundColor, 1, 0);
            SetupCamera();

            device.BeginScene();

            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.SetStreamSource(0, vb, 0);
            device.Indices = ib;

            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertCount, 0, indCount / 3);

            device.EndScene();
            device.Present();

            menuStrip1.Update();

            if (invalidating)
            {
                this.Invalidate();
            }
        }

        private void GenerateVertext()
        {
            verts = new CustomVertex.PositionColored[vertCount];

            int k = 0;

            for (int z = 0; z < terWidth; z++)
            {
                for (int x = 0; x < terLength; x++)
                {
                    verts[k].Position = new Vector3(x, 0, z);
                    verts[k].Color = Color.White.ToArgb();

                    k++;
                }
            }
        }

        private void GenerateIndex()
        {
            indices = new int[indCount];
            int k = 0;
            int l = 0;

            for (int i = 0; i < indCount; i += 6)
            {
                indices[i] = k;
                indices[i + 1] = k + terLength;
                indices[i + 2] = k + terLength + 1;

                indices[i + 3] = k;
                indices[i + 4] = k + terLength + 1;
                indices[i + 5] = k + 1;

                k++;
                l++;
                if (l == terLength - 1)
                {
                    l = 0;
                    k++;
                }
            }

        }

        float maxHeight = 0;

        private void LoadHeigtMap()
        {
            int k = 0;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                verts = new CustomVertex.PositionColored[vertCount];
                ofd.Title = "Load Heightmap";
                ofd.Filter = "JPG files (*jpg)|*jpg";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    heightMap = new Bitmap(ofd.FileName);
                    Color pixelColor;
                    int[] arrayColor;
                    int tt = 0;
                    int max = 0;
                    int min = 0;
                    if (heightMap.Size.Width * heightMap.Size.Height > terLength * terWidth)
                    {
                        tt = heightMap.Size.Width * heightMap.Size.Height;
                    }
                    else
                    {
                        tt = terLength * terWidth;
                    }

                    arrayColor = new int[tt];
                    max = heightMap.GetPixel(0, 0).ToArgb();
                    min = heightMap.GetPixel(0, 0).ToArgb();
                    for (int z = 0; z < terLength; z++)
                    {
                        for (int x = 0; x < terWidth; x++)
                        {
                            if (heightMap.Size.Width > x && heightMap.Size.Height > z)
                            {
                                arrayColor[k] = heightMap.GetPixel(x, z).ToArgb();
                            }
                            if (max < arrayColor[k])
                            {
                                max = arrayColor[k];
                            }
                            if (min > arrayColor[k])
                            {
                                min = arrayColor[k];
                            }
                            k++;
                        }
                    }
                    k = 0;
                    int minA = min - arrayColor[k];
                    int diffMinMax = min - max;
                    int lerpBrBlue = Color.Blue.ToArgb() - Color.Brown.ToArgb();


                    for (int z = 0; z < terLength; z++)
                    {
                        for (int x = 0; x < terWidth; x++)
                        {
                            if (heightMap.Size.Width > x && heightMap.Size.Height > z)
                            {
                                pixelColor = heightMap.GetPixel(x, z);
                                verts[k].Position = new Vector3(x, (float)pixelColor.B / 10 - 5, z);

                                minA = min - arrayColor[k];
                                float n = (float)minA / (float)diffMinMax;
                                Color newColor;
                                float limit = 0.6f;
                                if (n < limit && n > 0.2f)
                                {
                                    newColor = colorCreator(limit, 0.2f, Color.Brown, Color.Green, n);
                                }
                                else if (0.05f < n && n <= 0.2f)
                                {
                                    newColor = colorCreator(0.2f, 0.05f, Color.Green, Color.Blue, n);
                                }
                                else if (n <= 0.05f && n >= 0.0f)
                                {
                                    newColor = Color.Blue;
                                }
                                else
                                {
                                    newColor = Color.Brown;
                                }

                                int tempColor = newColor.ToArgb();

                                verts[k].Color = tempColor;
                            }
                            else
                            {
                                pixelColor = heightMap.GetPixel(x, z);
                                verts[k].Position = new Vector3(x, 0, z);
                                verts[k].Color = Color.White.ToArgb();
                            }

                            k++;
                        }
                    }

                    camPosition.Y = 50f;
                    camPosition.X = heightMap.Size.Width / 2;
                    camPosition.Z = heightMap.Size.Height / 2;
                    camLookAt = new Vector3(camPosition.X, 0, camPosition.Z);
                }
            }
        }

        private Color colorCreator(float upRat, float lowRat, Color topColor, Color botColor, float currentRat)
        {
            Color tempColor = Color.White;
            float b = (currentRat - lowRat) / (upRat - lowRat);

            Color colorTop = topColor;
            Color colorBottom = botColor;

            int R1 = colorTop.R;
            int R2 = colorBottom.R;

            int rDiff = R1 - R2;

            int G1 = colorTop.G;
            int G2 = colorBottom.G;

            int gDiff = G1 - G2;

            int B1 = colorTop.B;
            int B2 = colorBottom.B;

            int bDiff = B1 - B2;

            return tempColor = Color.FromArgb((int)(R2 + (b * rDiff)), (int)(G2 + (b * gDiff)), (int)(B2 + (b * bDiff)));
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (Keys.W):
                    {
                        camPosition.X += moveSpeed * (float)Math.Sin(rotY);
                        camPosition.Z += moveSpeed * (float)Math.Cos(rotY);
                        break;
                    }
                case (Keys.S):
                    {
                        camPosition.X -= moveSpeed * (float)Math.Sin(rotY);
                        camPosition.Z -= moveSpeed * (float)Math.Cos(rotY);
                        break;
                    }
                case (Keys.A):
                    {
                        camPosition.X -= moveSpeed * (float)Math.Sin(rotY + Math.PI / 2);
                        camPosition.Z -= moveSpeed * (float)Math.Cos(rotY + Math.PI / 2);
                        break;
                    }
                case (Keys.D):
                    {
                        camPosition.X += moveSpeed * (float)Math.Sin(rotY + Math.PI / 2);
                        camPosition.Z += moveSpeed * (float)Math.Cos(rotY + Math.PI / 2);
                        break;
                    }
                case (Keys.E):
                    {
                        rotY += turnSpeed;
                        break;
                    }
                case (Keys.Q):
                    {
                        rotY -= turnSpeed;
                        break;
                    }
                case (Keys.Up):
                    {
                        if (rotXZ < Math.PI / 2)
                        {
                            rotXZ += turnSpeed;
                        }
                        break;
                    }
                case (Keys.Down):
                    {
                        if (rotXZ > -Math.PI / 2)
                        {
                            rotXZ -= turnSpeed;
                        }

                        break;
                    }
                case (Keys.F):
                    {
                        this.WindowState = FormWindowState.Maximized;
                        break;
                    }
            }
        }

        private void OnMouseScroll(object sender, MouseEventArgs e)
        {
            camPosition.Y -= e.Delta * 0.003f;
        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fillMode = FillMode.Solid;
            solidToolStripMenuItem.Checked = true;
            wireframeToolStripMenuItem.Checked = false;
            pointToolStripMenuItem.Checked = false;
        }

        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fillMode = FillMode.WireFrame;
            solidToolStripMenuItem.Checked = false;
            wireframeToolStripMenuItem.Checked = true;
            pointToolStripMenuItem.Checked = false;
        }

        private void pointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fillMode = FillMode.Point;
            solidToolStripMenuItem.Checked = false;
            wireframeToolStripMenuItem.Checked = false;
            pointToolStripMenuItem.Checked = true;
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            invalidating = false;
            if (cd.ShowDialog(this) == DialogResult.OK)
            {
                backgroundColor = cd.Color;
            }
            invalidating = true;
            this.Invalidate();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateVertext();
            vb.SetData(verts, 0, LockFlags.None);
        }

        private void loadHeigthMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadHeigtMap();
            vb.SetData(verts, 0, LockFlags.None);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isMiddleMouseDown)
            {
                rotY = tempY + e.X * turnSpeed;
                float tmmp = tempXZ - e.Y * turnSpeed / 4;

                if (tmmp < Math.PI / 2 && tmmp > -Math.PI / 2)
                {
                    rotXZ = tmmp;
                }
            }

            if (isLeftMouseDown)
            {
                Point mouseMoveLocation = new Point(e.X, e.Y);
                PickingTriangle(mouseMoveLocation);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            button2.Left = 10;
            button2.Top = menuStrip1.Height + 5;
            button2.Update();

            button1.Update();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            this.Update();
            this.BackColor = Color.White;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int generatedNumber = 5;

            int[] xCoord = new int[generatedNumber];
            int[] yCoord = new int[generatedNumber];
            int[] elevation = new int[generatedNumber];

            Random rand = new Random();

            for (int i = 0; i < generatedNumber; i++)
            {
                xCoord[i] = rand.Next(12);
                yCoord[i] = rand.Next(12);
                elevation[i] = 10 + rand.Next(15);
            }

            int maxElevation = elevation[0];
            int minElevation = elevation[0];

            int maxX = xCoord[0];
            int minX = xCoord[0];
            int maxY = yCoord[0];
            int minY = yCoord[0];

            for (int i = 0; i < generatedNumber; i++)
            {
                if (maxX < xCoord[i])
                {
                    maxX = xCoord[i];
                }
                if (minX > xCoord[i])
                {
                    minX = xCoord[i];
                }
                if (maxY < yCoord[i])
                {
                    maxY = yCoord[i];
                }
                if (minY > yCoord[i])
                {
                    minY = yCoord[i];
                }

                if (maxElevation < elevation[i])
                {
                    maxElevation = elevation[i];
                }

                if (minElevation > elevation[i])
                {
                    minElevation = elevation[i];
                }
            }

            int division = generatedNumber * 30;

            float[] genXCoor = new float[division];
            float[] genYCoor = new float[division];

            float[] genElevation = new float[division * division];

            float xDiff = maxX - minX;
            float yDiff = maxY - minY;

            float xDiffRatio = xDiff / division;
            float yDiffRatio = yDiff / division;

            for (int i = 0; i < genXCoor.Length; i++)
            {
                genXCoor[i] = minX + i * xDiffRatio;
                genYCoor[i] = minY + i * yDiffRatio;
            }
            int t = 0;
            for (int z = 0; z < division; z++)
            {
                for (int x = 0; x < division; x++)
                {
                    float sum = 0, genWeight = 0, tempZ = 0;

                    for (int i = 0; i < generatedNumber; i++)
                    {
                        float xD = (float)(xCoord[i] - genXCoor[x]);
                        float yD = (float)(yCoord[i] - genYCoor[z]);

                        float Dist = (float)Math.Sqrt(xD * xD + yD * yD);
                        sum = sum + 1 / Dist;
                    }

                    for (int i = 0; i < generatedNumber; i++)
                    {
                        float xD = (float)(xCoord[i] - genXCoor[x]);
                        float yD = (float)(yCoord[i] - genYCoor[z]);

                        float Dist = (float)Math.Sqrt(xD * xD + yD * yD);
                        genWeight = 1 / (Dist * sum);

                        tempZ = tempZ + (float)(genWeight * elevation[i]);
                    }

                    genElevation[t] = tempZ;
                    t++;
                }
            }

            int n = 0;

            for (int z = 0; z < division; z++)
            {
                for (int x = 0; x < division; x++)
                {
                    float sum = 0;
                    float constant = 2.5f;
                    for (int i = 0; i < generatedNumber; i++)
                    {
                        float xD = (float)(genXCoor[x] - xCoord[i]);
                        float yD = (float)(genYCoor[z] - yCoord[i]);

                        float Dist = (float)Math.Sqrt(xD * xD + yD * yD);
                        sum = sum + 1 / (float)(Math.Pow(Dist, constant));
                    }

                    float[] genWeight = new float[generatedNumber];

                    float tempZ = 0;

                    for (int i = 0; i < generatedNumber; i++)
                    {
                        float xD = (float)(genXCoor[x] - xCoord[i]);
                        float yD = (float)(genYCoor[z] - yCoord[i]);

                        float Dist = (float)Math.Sqrt(xD * xD + yD * yD);
                        genWeight[i] = 1 / ((float)(Math.Pow(Dist, constant)) * sum);

                        tempZ = tempZ + (float)(genWeight[i] * elevation[i]);
                    }
                    genElevation[n] = tempZ;

                    n++;

                }
            }

            int k = 0;
            float elevDiff = minElevation - maxElevation;
            for (int z = 0; z < terLength; z++)
            {
                for (int x = 0; x < terWidth; x++)
                {
                    if (x < division - 1 && z < division - 1)
                    {
                        verts[k].Position = new Vector3(genXCoor[x], genElevation[x + z * division], genYCoor[z]);

                        float minA = minElevation - genElevation[x + z * division];
                        float vv = (float)minA / (float)(elevDiff);
                        Color newColor2;
                        float limit = 0.8f;
                        if (vv < limit && vv > 0.2f)
                        {
                            newColor2 = colorCreator(limit, 0.2f, Color.Brown, Color.Blue, vv);
                        }
                        else if (0.05f < vv && vv <= 0.2f)
                        {
                            newColor2 = colorCreator(0.2f, 0.05f, Color.Blue, Color.Green, vv);
                        }
                        else if (vv <= 0.05f && vv >= 0.0f)
                        {
                            newColor2 = Color.Green;
                        }
                        else
                        {
                            newColor2 = Color.Brown;
                        }

                        int tempColor = newColor2.ToArgb();

                        verts[k].Color = tempColor;
                    }
                    else
                    {
                        verts[k].Position = new Vector3(genXCoor[division - 1], 0, genYCoor[division - 1]);
                        verts[k].Color = Color.Black.ToArgb();
                    }
                    k++;
                }
            }
            vb.SetData(verts, 0, LockFlags.None);

            camPosition.Y = maxElevation + 10;
            camLookAt = new Vector3(camPosition.X, 0, camPosition.Z);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case (MouseButtons.Middle):
                    {
                        tempY = rotY - e.X * turnSpeed;

                        tempXZ = rotXZ + e.Y * turnSpeed / 4;

                        isMiddleMouseDown = true;
                        break;
                    }
                case (MouseButtons.Left):
                    {
                        Point mouseDownLocation = new Point(e.X, e.Y);
                        PickingTriangle(mouseDownLocation);
                        isLeftMouseDown = true;
                        break;
                    }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case (MouseButtons.Middle):
                    {
                        isMiddleMouseDown = false;
                        break;
                    }
                case (MouseButtons.Left):
                    {
                        isLeftMouseDown = false;
                        break;
                    }
            }

        }


        private void PickingTriangle(Point mouseLocation)
        {
            IntersectInformation hitLocation;
            Vector3 near, far, direction;

            near = new Vector3(mouseLocation.X, mouseLocation.Y, 0);
            far = new Vector3(mouseLocation.X, mouseLocation.Y, 100);

            near.Unproject(device.Viewport, device.Transform.Projection, device.Transform.View, device.Transform.World);
            far.Unproject(device.Viewport, device.Transform.Projection, device.Transform.View, device.Transform.World);

            direction = near - far;

            for (int i = 0; i < indCount; i += 3)
            {
                if (Geometry.IntersectTri(verts[indices[i]].Position, verts[indices[i + 1]].Position, verts[indices[i + 2]].Position, near, direction, out hitLocation))
                {
                    verts[indices[i]].Position += new Vector3(0, raisedConst, 0);
                    verts[indices[i + 1]].Position += new Vector3(0, raisedConst, 0);
                    verts[indices[i + 2]].Position += new Vector3(0, raisedConst, 0);

                    for (int t = 0; t < indCount; t++)
                    {
                        if (verts[indices[t]].Position.Y > maxHeight)
                        {
                            maxHeight = verts[indices[t]].Position.Y;
                        }
                    }

                    verts[indices[i]].Color = Color.Red.ToArgb() * (int)(255 * verts[indices[i]].Position.Y / maxHeight);
                    verts[indices[i + 1]].Color = Color.Red.ToArgb() * (int)verts[indices[i + 1]].Position.Y;
                    verts[indices[i + 2]].Color = Color.Red.ToArgb() * (int)verts[indices[i + 2]].Position.Y;


                    vb.SetData(verts, 0, LockFlags.None);
                }
            }
        }

        private void generateMap(string[] xx, string[] yy, string[] zz, int parameter)
        {
            int generatedNumber = xx.Length;

            int[] xCoord = new int[generatedNumber];
            int[] yCoord = new int[generatedNumber];
            int[] elevation = new int[generatedNumber];

            Random rand = new Random();

            for (int i = 0; i < generatedNumber; i++)
            {
                xCoord[i] = int.Parse(xx[i]);
                yCoord[i] = int.Parse(yy[i]);
                elevation[i] = int.Parse(zz[i]);
            }

            int maxElevation = elevation[0];
            int minElevation = elevation[0];

            int maxX = xCoord[0];
            int minX = xCoord[0];
            int maxY = yCoord[0];
            int minY = yCoord[0];

            for (int i = 0; i < generatedNumber; i++)
            {
                if (maxX < xCoord[i])
                {
                    maxX = xCoord[i];
                }
                if (minX > xCoord[i])
                {
                    minX = xCoord[i];
                }
                if (maxY < yCoord[i])
                {
                    maxY = yCoord[i];
                }
                if (minY > yCoord[i])
                {
                    minY = yCoord[i];
                }

                if (maxElevation < elevation[i])
                {
                    maxElevation = elevation[i];
                }

                if (minElevation > elevation[i])
                {
                    minElevation = elevation[i];
                }
            }

            int division = generatedNumber * 100;

            float[] genXCoor = new float[division];
            float[] genYCoor = new float[division];

            float[] genElevation = new float[division * division];

            float xDiff = maxX - minX;
            float yDiff = maxY - minY;

            float xDiffRatio = xDiff / division;
            float yDiffRatio = yDiff / division;

            for (int i = 0; i < genXCoor.Length; i++)
            {
                genXCoor[i] = minX + i * xDiffRatio;
                genYCoor[i] = minY + i * yDiffRatio;
            }
            int t = 0;
            for (int z = 0; z < division; z++)
            {
                for (int x = 0; x < division; x++)
                {
                    float sum = 0, genWeight = 0, tempZ = 0;

                    for (int i = 0; i < generatedNumber; i++)
                    {
                        float xD = (float)(xCoord[i] - genXCoor[x]);
                        float yD = (float)(yCoord[i] - genYCoor[z]);

                        float Dist = (float)Math.Sqrt(xD * xD + yD * yD);
                        sum = sum + 1 / Dist;
                    }

                    for (int i = 0; i < generatedNumber; i++)
                    {
                        float xD = (float)(xCoord[i] - genXCoor[x]);
                        float yD = (float)(yCoord[i] - genYCoor[z]);

                        float Dist = (float)Math.Sqrt(xD * xD + yD * yD);
                        genWeight = 1 / (Dist * sum);

                        tempZ = tempZ + (float)(genWeight * elevation[i]);
                    }

                    genElevation[t] = tempZ;
                    t++;
                }
            }

            int n = 0;

            for (int z = 0; z < division; z++)
            {
                for (int x = 0; x < division; x++)
                {
                    float sum = 0;
                    float constant = 2.5f;
                    for (int i = 0; i < generatedNumber; i++)
                    {
                        float xD = (float)(genXCoor[x] - xCoord[i]);
                        float yD = (float)(genYCoor[z] - yCoord[i]);

                        float Dist = (float)Math.Sqrt(xD * xD + yD * yD);
                        sum = sum + 1 / (float)(Math.Pow(Dist, constant));
                    }

                    float[] genWeight = new float[generatedNumber];

                    float tempZ = 0;

                    for (int i = 0; i < generatedNumber; i++)
                    {
                        float xD = (float)(genXCoor[x] - xCoord[i]);
                        float yD = (float)(genYCoor[z] - yCoord[i]);

                        float Dist = (float)Math.Sqrt(xD * xD + yD * yD);
                        genWeight[i] = 1 / ((float)(Math.Pow(Dist, constant)) * sum);

                        tempZ = tempZ + (float)(genWeight[i] * elevation[i]);
                    }
                    genElevation[n] = tempZ;

                    n++;

                }
            }

            int k = 0;
            float elevDiff = minElevation - maxElevation;
            for (int z = 0; z < terLength; z++)
            {
                for (int x = 0; x < terWidth; x++)
                {
                    if (x < division - 1 && z < division - 1)
                    {
                        verts[k].Position = new Vector3(genXCoor[x], genElevation[x + z * division], genYCoor[z]);

                        float minA = minElevation - genElevation[x + z * division];
                        float vv = (float)minA / (float)(elevDiff);
                        Color newColor2;
                        float limit = 0.8f;
                        if (vv < limit && vv > 0.2f)
                        {
                            newColor2 = colorCreator(limit, 0.2f, Color.Brown, Color.Blue, vv);
                        }
                        else if (0.05f < vv && vv <= 0.2f)
                        {
                            newColor2 = colorCreator(0.2f, 0.05f, Color.Blue, Color.Green, vv);
                        }
                        else if (vv <= 0.05f && vv >= 0.0f)
                        {
                            newColor2 = Color.Green;
                        }
                        else
                        {
                            newColor2 = Color.Brown;
                        }

                        int tempColor = newColor2.ToArgb();

                        verts[k].Color = tempColor;
                    }
                    else
                    {
                        verts[k].Position = new Vector3(genXCoor[division - 1], 0, genYCoor[division - 1]);
                        verts[k].Color = Color.Black.ToArgb();
                    }
                    k++;
                }
            }
            vb.SetData(verts, 0, LockFlags.None);

            camPosition.Y = maxElevation + 10;
            camLookAt = new Vector3(camPosition.X, 0, camPosition.Z);

            temppX = new string[genXCoor.Length];
            temppY = new string[genXCoor.Length];
            temppZ = new string[genXCoor.Length];

            for (int l = 0; l < genXCoor.Length; l++)
            {
                temppX[l] = genXCoor[l].ToString();
                temppY[l] = genYCoor[l].ToString();
                temppZ[l] = genElevation[l].ToString();
            }
            

            if (parameter == 0)
            {
                formMain.getporosityXResult = new string[temppX.Length];
                formMain.getporosityYResult = new string[temppY.Length];
                formMain.getporosityZResult = new string[temppZ.Length];

                formMain.getporosityXResult = temppX;
                formMain.getporosityYResult = temppY;
                formMain.getporosityZResult = temppZ;
            }
            else if (parameter == 1)
            {
                formMain.getpermeabilityXResult = new string[temppX.Length];
                formMain.getpermeabilityYResult = new string[temppY.Length];
                formMain.getpermeabilityZResult = new string[temppZ.Length];

                formMain.getpermeabilityXResult = temppX;
                formMain.getpermeabilityYResult = temppY;
                formMain.getpermeabilityZResult = temppZ;
            }
            else if (parameter == 2)
            {
                formMain.getSaturationXResult = new string[temppX.Length];
                formMain.getSaturationYResult = new string[temppY.Length];
                formMain.getSaturationZResult = new string[temppZ.Length];

                formMain.getSaturationXResult = temppX;
                formMain.getSaturationYResult = temppY;
                formMain.getSaturationZResult = temppZ;
            }
            else if (parameter == 3)
            {
                formMain.getThicknessXResult = new string[temppX.Length];
                formMain.getThicknessYResult = new string[temppY.Length];
                formMain.getThicknessZResult = new string[temppZ.Length];

                formMain.getThicknessXResult = temppX;
                formMain.getThicknessYResult = temppY;
                formMain.getThicknessZResult = temppZ;
            }
            else if (parameter == 4)
            {
                formMain.getwaterCutXResult = new string[temppX.Length];
                formMain.getwaterCutYResult = new string[temppY.Length];
                formMain.getwaterCutZResult = new string[temppZ.Length];

                formMain.getwaterCutXResult = temppX;
                formMain.getwaterCutYResult = temppY;
                formMain.getwaterCutZResult = temppZ;
            }

        }
    }
}
