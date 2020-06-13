private void Piercing_TextChanged(object sender, KeyEventArgs e)
        {
            double value = Convert.ToDouble((sender as TextBox).Text.Trim());
            TechnicsParameterStruct a = viewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index];
            Type t = a.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                object value1 = pi.GetValue(a, null);//用pi.GetValue获得值
                string name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                                      //获得属性的类型,进行判断然后进行以后的操作,例如判断获得的属性是整数
                if (name == (sender as TextBox).Name)
                {
                    //进行你想要的操作
                    if ((value1 as XmlDoubleParameter).MinValue > value
                        || (value1 as XmlDoubleParameter).MaxValue < value)
                    {
                        BodorThinkerMessageBox msgbox = new BodorThinkerMessageBox(
                        string.Format("正确的参数范围是[{0},{1}]",(value1 as XmlDoubleParameter).MinValue,
                        (value1 as XmlDoubleParameter).MaxValue),BodorThinkerMessageBox.MessageType.Error);
                        msgbox.ShowDialog();
                        (sender as TextBox).Text = (value1 as XmlDoubleParameter).Value.ToString();
                        break;
                    }
                }
            }
        }