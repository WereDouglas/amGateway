   M S B A M L   `   `   `  ����  	amGateway    amGateway.SettingPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  Daily weather records��5      $	��800��6    $	��500��6/    N� 
Background �	#FFDBE0F2 � 	Resources 5      ��%    5      .��5      � $��0,0,0,10q�6   .+�5   
   �� -   5         ��Name$ internet��5      $�� ��6   $
��Left=�6-   $��	883,9,0,0q�6H   $	��Top=�6[   $��85��6s   �� 5   
   -   5        $
 port��5      $
��info��6   $
��Left=�6-   $��	413,0,0,0q�6H   $	��Top=�6[    J� RenderTransformOrigin$ -3.644,0.889&�6s   $	��546��6�   $��34��6�   �� 5   
   $��NETWORK��6   $��Right=�6"   $��
0,47,313,0q�6>   $	��Top=�6R   �� 5   
   -   5         ��Name$ comms��5   .   $��Right=�6   $��32��6=   $��
0,45,379,0q�6I    �� TextWrapping$
 Wrap=�6]   $�� ��6q   $	��Top=�6y   $��67��6�   �� 5   
   $
��COMM��6   $��Right=�6   $��
0,46,453,0q�6;   $	��Top=�6O   �� 5   
   -   5        $ network��5   .   $��Right=�6   $��32��6?   $��
0,47,173,0q�6K   $
 Wrap=�6_   $�� ��6s   $	��Top=�6{   $	��130��6�   �� 5   
   -   5        $
 info��5      $
��info��6   $
��Left=�6-   $��10,8,0,0q�6H   $	��Top=�6Z   $ -3.644,0.889&�6r   $	��384��6�   $��80��6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dtGridNetwork��5      $��10,93,10,10q�6*      CanUserReorderColumns . 	   CanUserResizeColumns	 . 
   CanUserResizeRows
 .     CanUserSortColumns .    AutoGenerateColumns .     HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush �	#FFE7E8EC   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header 5      �� 5      -   5         ��Name$ chkSelectAll1��5   $   $��check��6;   6K      CellTemplate 5       �� 5"      .R�5$      �� $ chkDiscontinue��6'    d� 	IsChecked 6?   �� *values+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5&      5*      0  *System.Windows.Controls.DataGridTextColumn  5,      $ Network��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 66   �� *Names+  5-      $ COM port��6%    67   �� *Comm+  5.      $ Status��6%    65   �� *Status+  52      $ Prefix��6)    53      �� 54      .R�55      �� .+�56   "   �� -   5        $��Prefixes��56   D   57      58      5:        5;      $
 Edit��6)    5<      �� 5=      .R�5>      �� .+�5?   "   �� -	   5        $��Edit comm port��5?   F   5@      5A      5C        5D      $ Delete��6)    5E      �� 5F      .R�5G      �� .+�5H   "   �� -
   5        $��Delete��5H   D   5I      5J      5L      5Q      �� 5R   
   -   5        $��detect��5R         OtherButton#��[  6"   $��Right=�6G   $��	0,47,98,0q�6c   $	��Top=�6v   $��70��6�   $��32��6�   $ 0.108,-1.011&�6�   �� 5S   
   -   5         ��Name$
 save��5S   E     
SaveButton#��[ 6   $
��save��66   $��Right=�6S   $��	0,48,18,0q�6o   $	��Top=�6�   $��67��6�   $��32��6�   5T      5U      