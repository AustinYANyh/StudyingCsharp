private void GetPic()
{
    //截取缩率图
    //System.Windows.Point ppoint = Mouse.GetPosition(CoordinateSystemBg);
    //System.Windows.Point point = (CoordinateSystemBg as FrameworkElement).PointToScreen(ppoint);
    System.Windows.Point point = CoordinateSystemBg.PointToScreen(new Point(0, 0));
    int dpointx = (int)point.X;
    int dpointy = (int)point.Y;
    System.Drawing.Size size = new System.Drawing.Size(575, 545);
    System.Drawing.Rectangle rc = new System.Drawing.Rectangle(new System.Drawing.Point(dpointx, dpointy),
        size);
    var bitmap = new System.Drawing.Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

    using (System.Drawing.Graphics memoryGrahics = System.Drawing.Graphics.FromImage(bitmap))
    {
        memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, System.Drawing.CopyPixelOperation.SourceCopy);
    }
    var window = new MainDialog(_title: "截图", _height: 185, _width: 375);
    Image Image = new Image();
    MemoryStream ms = new MemoryStream();
    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
    byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以
    ms.Close();
    //Convert it to BitmapImage
    BitmapImage image = new BitmapImage();
    image.BeginInit();
    image.StreamSource = new MemoryStream(bytes);
    image.EndInit();
    Image.Source = image;
    window.MainGrid.Children.Add(Image);
    window.Show();
}