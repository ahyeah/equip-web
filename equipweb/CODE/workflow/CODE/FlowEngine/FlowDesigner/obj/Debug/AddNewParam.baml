   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   G CFlowDesigner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:FlowDesigner.ManagementFlowDesigner.Management 6?   +clr-namespace:FlowDesignerFlowDesigner 6x      FlowDesigner.AddNewParam   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   <
Management%clr-namespace:FlowDesigner.Management     6?   ,localclr-namespace:FlowDesigner     6x     � Title$  变量管理��5   	   $	��512��6   $	��310��6#    � WindowStartupLocation$ CenterOwner=�6/   .��5      � .+�5   
   �� $��变量名称：��6   $
��Left=�6    $��	33,18,0,0q�6;   $	��Top=�6N   �� 5   
   $��变量类型：��6   $
��Left=�6    $��	33,61,0,0q�6;   $	��Top=�6N   �� 5	   
   $��变量描述：��6   $
��Left=�6    $��
33,107,0,0q�6;   $	��Top=�6O   �� 5
   
   -   5         ��Name$
 name��5
      $
��Left=�6    $��23��6;   $��
119,18,0,0q�6G    �� TextWrapping$
 Wrap=�6[   $�� ��6o   $	��Top=�6w   $	��120��6�   �� 5   
   -   5         ��Name$
 type��5      $
��Left=�6!   $��23��6<   $��
119,61,0,0q�6H   $	��Top=�6\   $	��120��6t    �� SelectedIndex$ 0��6�   .@�5      ��
string��6$   ��5      int��6!   ��5      	float��6#   ��5      bool��6"   5      �� 5   
   -   5        $
 desc��5      $
��Left=�6    $��23��6;   $��119,107,0,0q�6G   $
 Wrap=�6\   $�� ��6p   $	��Top=�6x   $	��120��6�   �� 5   
   -   5         ��Name$ 
Event_List��5      $
��Left=�6'   $	��255��6B   $��
15,157,0,0q�6O   $	��Top=�6c   $	��272��6{    � 	Resources 5         CheckBox_Select
 � Tag	 d� 	IsChecked
 �� Mode  
ComBox_App �� SelectedItem��%	  &	        &	 ~     �� 5      .R�5      �� -   5         5      �� *Name+	 64   �� *
Selected+$
 TwoWay=�6`   5      �� 5      .R�5      �� -   5         5      �� *	App_res+ 67   �� *	App_res+5      5      5       �� View 5      �� 5      .P�5       �� $��选择��6%    �� CellTemplate# [  61    �� Width$ 30��6a   �� 5"      $��事件（Event）��6%   $	 150��6^    �� DisplayMemberBinding 68   �� *Name+6^   �� 5#      $��	App预留��6%   # [ 64   $	 100��6_   5$      5&      �� 5'   
   -   5         ��Name$ Ok��5'      $��确定��6   $
��Left=�6*   $��
59,430,0,0q�6E   $	��Top=�6Y   $��75��6q   $��31��6|   6�   �� 5(   
   -   5        $ Cancel��5(      $��取消��6!   $
��Left=�6.   $��177,430,0,0q�6I   $	��Top=�6^   $��75��6v   $��31��6�    J� RenderTransformOrigin$ 0.333,5.581&�6�   �� 5)   
   $��关联的事件��6   $
��Left=�6    $��
15,137,0,0q�6;   $	��Top=�6O   5*      5+      