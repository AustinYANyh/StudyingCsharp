   //控制缩略图显示的命令表
        public static Dictionary<string, Action> dic = new Dictionary<string, Action>
        {
            { "EnableSpeedPowerCurve",()=>{ LayerSetDialogViewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index] .EnableSpeedPowerCurve.Value = true;
            EventAggregatorRepository.EventAggregator.GetEvent<UpdateZoomPicEvent>().Publish(true);} },
            {"DisEnableSpeedPowerCurve",()=>{ LayerSetDialogViewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index] .EnableSpeedPowerCurve.Value = false;
            EventAggregatorRepository.EventAggregator.GetEvent<UpdateZoomPicEvent>().Publish(true);} },
            {"EnableSpeedFrequencyCurve" ,()=>{LayerSetDialogViewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index] .EnableSpeedFrequencyCurve.Value = true;
            EventAggregatorRepository.EventAggregator.GetEvent<UpdateZoomPicEvent>().Publish(true);} },
            {"DisEnableSpeedFrequencyCurve" ,() =>{ LayerSetDialogViewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index].EnableSpeedFrequencyCurve.Value = false;
            EventAggregatorRepository.EventAggregator.GetEvent<UpdateZoomPicEvent>().Publish(true);} },

            { "FilmCutting_EnableSpeedPowerCurve",()=>{ LayerSetDialogViewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index].FilmCutting_EnableSpeedPowerCurve.Value = true;
            EventAggregatorRepository.EventAggregator.GetEvent<UpdateZoomPicEvent>().Publish(true);} },
            {"FilmCutting_DisEnableSpeedPowerCurve",()=>{ LayerSetDialogViewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index] .FilmCutting_EnableSpeedPowerCurve.Value = false;
            EventAggregatorRepository.EventAggregator.GetEvent<UpdateZoomPicEvent>().Publish(true);} },
            {"FilmCutting_EnableSpeedFrequencyCurve" ,()=>{LayerSetDialogViewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index] .FilmCutting_EnableSpeedFrequencyCurve.Value = true;
            EventAggregatorRepository.EventAggregator.GetEvent<UpdateZoomPicEvent>().Publish(true);} },
            {"FilmCutting_DisEnableSpeedFrequencyCurve" ,() =>{ LayerSetDialogViewModel.Technic.TechnicParameterLayerList[LayerSetDialogViewModel.index].FilmCutting_EnableSpeedFrequencyCurve.Value = false;
            EventAggregatorRepository.EventAggregator.GetEvent<UpdateZoomPicEvent>().Publish(true);} },
        };

        private void PowerCurve_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            string key = box.Name;
            if (dic.ContainsKey(key))
                dic[key]();
        }

        private void PowerCurve_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            int index = box.Name.IndexOf("_");
            string key = string.Empty;
            if (index == -1)
                key = "Dis" + box.Name;
            else
                key = box.Name.Insert(index + 1, "Dis");
            if (dic.ContainsKey(key))
                dic[key]();
        }