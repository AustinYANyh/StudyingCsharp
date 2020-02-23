using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FriendLIst
{
    public class PicRichTextBox : RichTextBox
    {
        public PicRichTextBox()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0007) //焦点事件
            {
                foreach (Control _SubControl in base.Controls)
                {
                    _SubControl.Tag = "1";
                }

                GetRichTextObjRectangle();

                for (int i = 0; i != base.Controls.Count; i++)
                {
                    if (base.Controls[i].Tag.ToString() == "1")
                    {
                        base.Controls.RemoveAt(i);
                        i--;
                    }
                }
            }
            base.WndProc(ref m);
        }

        public void AddFile(string p_FileFullPath)
        {
            byte[] _FileBytes = File.ReadAllBytes(p_FileFullPath);
            Image _Image = Image.FromStream(new MemoryStream(_FileBytes));
            string _Guid = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", "");
            StringBuilder _RtfText = new StringBuilder(@"{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052{\fonttbl{\f0\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}\uc1\pard\lang2052\f0\fs18{\object\objemb{\*\objclass Paint.Picture}");
            int _Width = _Image.Width * 15;
            int _Height = _Image.Height * 15;
            _RtfText.Append(@"\objw" + _Width.ToString() + @"\objh" + _Height.ToString());
            _RtfText.AppendLine(@"{\*\objdata");
            _RtfText.AppendLine(@"010500000200000007000000504272757368000000000000000000" + BitConverter.ToString(BitConverter.GetBytes(_FileBytes.Length + 20)).Replace("-", ""));
            _RtfText.Append("7A676B65" + _Guid); //标记            
            _RtfText.AppendLine(BitConverter.ToString(_FileBytes).Replace("-", ""));
            _RtfText.AppendLine(@"0105000000000000}{\result{\pict\wmetafile0}}}}");
            base.SelectedRtf = _RtfText.ToString();
        }

        private void PointFile(string p_Rtf, Point p_StarPoint, int p_Width, int p_Height)
        {
            int _Index = p_Rtf.IndexOf(@"{\*\objdata");
            if (_Index == -1) return;
            _Index += 80;
            string _LengthText = p_Rtf.Substring(_Index, 8);

            int _Length = BitConverter.ToInt32(new byte[] { Convert.ToByte(_LengthText.Substring(0, 2), 16), Convert.ToByte(_LengthText.Substring(2, 2), 16), Convert.ToByte(_LengthText.Substring(4, 2), 16), Convert.ToByte(_LengthText.Substring(6, 2), 16) }, 0);
            _Index += 10;

            string _Head = p_Rtf.Substring(_Index, 8);
            if (_Head.ToUpper() != "7A676B65") return;   //如果不是标记出来的 就不生成PictureBox
            _Index += 8;

            string _Guid = p_Rtf.Substring(_Index, 32);

            Control _Controls = base.Controls[_Guid];
            if (_Controls == null)
            {
                PictureBox _PictureBox = new PictureBox();
                _PictureBox.Name = _Guid;
                _PictureBox.Tag = "0";
                _PictureBox.Location = p_StarPoint;
                _PictureBox.Size = new Size(p_Width, p_Height);

                _Index += 32;
                _Length -= 20;

                _PictureBox.Image = Image.FromStream(LoadMemoryStream(p_Rtf, ref _Index, _Length));

                base.Controls.Add(_PictureBox);
            }
            else
            {
                _Controls.Location = p_StarPoint;
                _Controls.Size = new Size(p_Width, p_Height);
                _Controls.Tag = "0";
            }
        }

        private MemoryStream LoadMemoryStream(string p_Text, ref int p_Index, int p_Count)
        {
            MemoryStream _File = new MemoryStream();
            char[] _Text = p_Text.ToCharArray();
            for (int i = 0; i != p_Count; i++)
            {
                if (_Text[p_Index] == '\r' && _Text[p_Index + 1] == '\n')
                {
                    i--;
                }
                else
                {
                    _File.WriteByte(Convert.ToByte(_Text[p_Index].ToString() + _Text[p_Index + 1].ToString(), 16));
                }
                p_Index += 2;
            }
            return _File;
        }

        private void GetRichTextObjRectangle()
        {
            RichTextBox _RichText = new RichTextBox();
            _RichText.Rtf = base.Rtf;
            int _Count = base.Text.Length;

            for (int i = 0; i != _Count; i++)
            {
                if (base.Text[i] == ' ')
                {
                    _RichText.Select(i, 1);

                    if (_RichText.SelectionType.ToString() == "Object")
                    {
                        Point _StarPoint = base.GetPositionFromCharIndex(i);

                        System.Text.RegularExpressions.Regex _RegexWidth = new System.Text.RegularExpressions.Regex(@"(?<=\\objw)[^\\]+");
                        System.Text.RegularExpressions.Regex _RegexHeight = new System.Text.RegularExpressions.Regex(@"(?<=\\objh)[^{]+");

                        int _Width = 0;
                        int _Height = 0;

                        if (int.TryParse(_RegexWidth.Match(_RichText.SelectedRtf).Value, out _Width) && int.TryParse(_RegexHeight.Match(_RichText.SelectedRtf).Value, out _Height))
                        {
                            _Width = _Width / 15;
                            _Height = _Height / 15;
                            PointFile(_RichText.SelectedRtf, _StarPoint, _Width, _Height);
                        }
                    }
                }
            }
            _RichText.Dispose();
        }

    }
}
