   M S B A M L   `   `   `  ����B  >WpfApp3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp3WpfApp3  5   	       WpfApp3.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp3     5   	   -   5          � Title$  
MainWindow��5   	   $	��600��6   $	��500��6)   $	��500��65   $	��500��6D   .��5	      �� -   5        $��outside��5	      $��1��6   
����ح�}�. ._�5
   
   �� -   5         ��Name$ CanvasInPath��5
       J� RenderTransformOrigin$	 0,0&�6'   $	��500��6C   $	��500��6O   $
��Left=�6\   }�. z�5      V� 5      .~�5      ��  �� ScaleY$ -1Q�6%   �� 5      �� 5      T� 5       T� Y 6)   ��  �� ElementName$ CanvasInPath�� �� Path$ ActualHeight�5      .+�5      �� 
������ ��6   ��  �� RelativeSource ��  �� Mode$ TemplatedParent=�$ Width���5      ��  �� $ TemplatedParent=�$ Height�5   -   A� 5      -   5        	 A�Name$	 DutyCycleCurveDot1��5      $��15��62   $��15��6=   
��� ���
 �� Left$
 30��6W   
 �� Top$
 11.5��6h    K� ZIndex$ 1��6z   $y�Visible=�6�   5   I   A� 5      -   5        $	 DutyCycleCurveDot2��5      $��15��62   $��15��6=   
��� ���$	
 255��6W   $	 122��6i   $ 1��6z   $y�Visible=�6�   5    I   �� 5#      -   5         ��Name$ DutyCycleline1��5#      	 �� X1$ 0��6+   	 �� Y1$
 18.5��62   	 �� X2$
 37.5��6<   	 �� Y2$
 18.5��6F   
��� ���$��3��6`   $y�	Collapsed=�6t   6�   �� 5$      -   5        $ DutyCycleline2��5$      $
 37.5��6+   $
 18.5��65   $ 187.5��6?   $ 129.5��6J   
��� ���$��3��6e   $y�	Collapsed=�6y   6�   �� 5%      -   5        $ DutyCycleline3��5%      $ 187.5��6+   $ 129.5��66   $	 375��6A   $ 129.5��6J   
��� ���$��3��6e   $y�	Collapsed=�6y   6�   5&      5'      5(      