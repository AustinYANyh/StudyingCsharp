@[toc]

## 三维坐标转换为二维坐标

**矩阵变换什么的最烦了...我先写下来以防以后忘记...**

```
mixtrl是一个矩阵
[1 0 0
0 1 0 
0 0 1]

下面这个函数是我用来画三维坐标系的
```

    ```csharp
public POINT2D Transform3DTo2D(POINT3D pt3d)
{
    POINT2D result = new POINT2D();

    double x = pt3d.x;
    double y = pt3d.y;
    double z = pt3d.z;

    double xTOx = (mixtrl[0] * x + mixtrl[1] * y + mixtrl[2] * z) * needXReverxe;
    double yTox = (mixtrl[3] * x + mixtrl[4] * y + mixtrl[5] * z) * needYReverxe;
    double zTox = (mixtrl[6] * x + mixtrl[7] * y + mixtrl[8] * z) * needZReverxe;

    double xx = -1 * Math.Cos(150 * 0.017453) * xTOx - Math.Cos(150 * 0.017453) * yTox;
    double yy = Math.Sin(150 * 0.017453) * xTOx - Math.Sin(150 * 0.017453) * yTox - zTox;

    result.x = orginPoint.X + xx;
    result.y = orginPoint.Y + yy;

    return result;
}
```

## 三维坐标(或二维坐标)的旋转

**以某个轴为旋转轴,实际上,只在垂直于坐标轴的平面做二维旋转**

- 二维
```
x = xcosα - ysinα
y = xsinα + ycosα
```
- 三维
```
绕x轴旋转
x1 = x;
y1 = ycosα - zsinα;
z1 = zcosα + ysinα;

绕y轴旋转
x1 = xcosα - zsinα;
y1 = y;
z1 = zcosα + xsinα;

绕z轴旋转
x1 = xcosα - ysinα;
y1 = ycosα + xsinα;
z1 = z;
```

**角度自己设定,我自己设置每次转20°**

**C#中的三角函数计算传入的参数为弧度...**

```csharp
public POINT3D TransRoll(POINT3D pt3d,int RollFlag,double angle)
{
    POINT3D result = new POINT3D();

    double x = pt3d.x;
    double y = pt3d.y;
    double z = pt3d.z;

    double cos = Math.Cos(angle * (Math.PI / 180));
    double sin = Math.Sin(angle * (Math.PI / 180));

    //绕x轴
    if (RollFlag == 0)
    {
	double x1 = x;
	double y1 = y * cos - z * sin;
	double z1 = z * cos + y * sin;

	result.x = x1;
	result.y = y1;
	result.z = z1;
    }
    else if(RollFlag == 1)
    {
	double x1 = x * cos - z * sin;
	double y1 = y;
	double z1 = z * cos + x * sin;

	result.x = x1;
	result.y = y1;
	result.z = z1;
    }
    else
    {
	double x1 = x * cos - y * sin;
	double y1 = y * cos + x * sin;
	double z1 = z;

	result.x = x1;
	result.y = y1;
	result.z = z1;
    }
    return result;
}
```
## 绘制椎体
    ```
public void paintCone(POINT3D pt3d)
{
    POINT2D pt2d = new POINT2D();

    Graphics graphics = this.CreateGraphics();
    Pen pen = new Pen(Color.Yellow, 2);

    for (double i = 1; i <= 360; ++i)
    {
	pt2d = Transform3DTo2D(TransRoll(pt3d, 2, i));
	graphics.DrawLine(pen, orginPoint, new Point((int)pt2d.x, (int)pt2d.y));
    }

    pen = new Pen(Color.Red, 2);
    POINT3D circlePT3D = new POINT3D(0, pt3d.y, pt3d.z);
    POINT2D circlePT2D = new POINT2D();

    circlePT2D = Transform3DTo2D(circlePT3D);

    for (double i = 1; i <= 360; ++i)
    {
	pt2d = Transform3DTo2D(TransRoll(circlePT3D, 2, i));
	graphics.DrawLine(pen, new Point((int)circlePT2D.x, (int)circlePT2D.y), 
		new Point((int)pt2d.x, (int)pt2d.y));
    }

    graphics.Dispose();
}
```

## 最终效果
**x，y轴反向**

![在这里插入图片描述](https://img-blog.csdnimg.cn/2020031022103085.gif)
**绕z轴旋转20°和绕z轴旋转-20°**

![在这里插入图片描述](https://img-blog.csdnimg.cn/20200310221048320.gif)

**z轴反向（带椎体）**

