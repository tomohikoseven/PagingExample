   M S B A M L   `   `   `  ����  PagingExample    PagingExample.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$%  WPF DataGrid Paging and Sorting��5   	   $	��190��5   	   $	��350��5   	   .��5      �  � RowDefinitions 5   
   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5           AutoGenerateColumns .     
IsReadOnly . ��5      �� *
Products+ �� NotifyOnTargetUpdated .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 
PRODUCT ID��5         Width5  /System.Windows.Controls.DataGridLengthConverter$ * 5         SortDirection$ 	AscendingX�5      1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 5      �� *Id+5        5      $ PRODUCT NAME��5      $ * 5      	 5      �� *Name+5      5      �� 5   
   $��4q�5       $��1��5!      
 �� Orientation$
 
Horizontal=�5"      $��Center=�5#      .+�5$      �� $	��4,0q�5%      $��<<��5&      ��5'      �� *FirstCommand+�� 5(      $	��4,0q�5)      $��<��5*      ��5+      �� *PreviousCommand+�� 5,      $��Center=�5-      $
 
Horizontal=�5.      .+�5/      �� ��50      �� *Start+�� 51      $
�� to ��52      �� 53      ��54      �� *End+�� 55      $
�� of ��56      �� 57      ��58      �� *
TotalItems+59      �� 5:      $	��4,0q�5;      $��>��5<      ��5=      �� *NextCommand+�� 5>      $	��4,0q�5?      $��>>��5@      ��5A      �� *LastCommand+5B      5C      5D      