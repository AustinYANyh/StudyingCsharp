<Grid.RowDefinitions>
            <RowDefinition Height="1*"></RowDefinition>
            <RowDefinition Height="1*"></RowDefinition>
        </Grid.RowDefinitions>

        <Slider Grid.Row="1" x:Name="slider" x:FieldModifier="public" IsSnapToTickEnabled="True" Maximum="100" Value="30" TickFrequency="10" ValueChanged="slider_ValueChanged"/>
        <Label Grid.Row="0" x:Name="Header" x:FieldModifier="public" Content="进给倍率" FontSize="15" HorizontalAlignment="Left" VerticalAlignment="Center" />
        <Grid Grid.Row="0">
            <Grid.ColumnDefinitions>
                <ColumnDefinition></ColumnDefinition>
                <ColumnDefinition Width="60" ></ColumnDefinition>
                <ColumnDefinition></ColumnDefinition>
            </Grid.ColumnDefinitions>
            <TextBox Grid.Column="1" x:Name="label1" x:FieldModifier="public" Style="{StaticResource textBoxStyle1}" Text="{Binding Value, Converter={StaticResource dataConvert}, ElementName=slider}" TextWrapping="Wrap"/>
            <Button Grid.Column="0"  x:Name="button1" Content="&lt;" Style="{StaticResource btnClick}" HorizontalAlignment="Right" VerticalAlignment="Center" HorizontalContentAlignment="Right" RenderTransformOrigin="-0.182,0.348" Click="button1_Click"/>
            <Button Grid.Column="2"  x:Name="button2" Content="&gt;" Style="{StaticResource btnClick}" HorizontalAlignment="Left" VerticalAlignment="Center" HorizontalContentAlignment="Left" Click="button2_Click"/>
        </Grid>
    </Grid>
    
private void UserControl_Loaded(object sender, RoutedEventArgs e)
{
    BusinessLayerService.StatusDataChangedHandle += new Action(viewModel.RefreshData);
    CurrentSpeedBall.progressBar.Maximum = 100;
    SetBindings(CurrentSpeedBall.progressBar, viewModel.motionStatus, "currentSpeed", BindingMode.OneWay, ProgressBar.ValueProperty);
    SetBindings(FeedRateSlider.slider, viewModel.motionStatus, "feedRate", BindingMode.TwoWay, Slider.ValueProperty);
}

public void SetBindings(FrameworkElement Obj, Object Source, string PropertyPath, BindingMode Mode, DependencyProperty DepProperty)
{
    Binding binding = new Binding();
    binding.Source = Source;
    binding.Path = new PropertyPath(PropertyPath);
    binding.Mode = Mode;
    BindingOperations.SetBinding(Obj, DepProperty, binding);
}
