   M S B A M L   `   `   `  ����  GroupClient    GroupClient.winGroupList   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  券商列表��5   	   $	��300��6   $	��520��6#    � Icon$ Images\RighteousFury.ico��6/   .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgMain��5      $��1��6#      AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .     CanUserReorderColumns .     ClipboardCopyMode$ IncludeHeader=�6�      
IsReadOnly . 	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 名称��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *名称+6J   4  .System.Windows.Controls.DataGridCheckBoxColumn  5	      $
 启用��6)    65   �� *启用+6P     5
      $
 交易服务器��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6N    64   �� *交易服务器+6\     5      $
 IP��6%    61   �� *IP+6J     5      $

 Port��6%    63   �� *Port+6N     5      $
 	版本号��6%    62   �� *	版本号+6L     5      $
 营业部代码��6%    64   �� *营业部代码+6P   4  .System.Windows.Controls.DataGridTemplateColumn  5      $
 操作��6)      CellTemplate 5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6)   .+�5   "   �� -   5        $��2,0,2,0q�5   A    � DataContext 6)   �� .��6l   编辑6p   �� 5   "   -   5        $��2,0,2,0q�5   A    6)   �� .��6n   删除6r   5      5      5      5      5      5      