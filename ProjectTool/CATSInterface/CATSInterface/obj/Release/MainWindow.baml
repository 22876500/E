   M S B A M L   `   `   `  ���� H  DCATSInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:CATSInterface.ConverterCATSInterface.Converter      CATSInterface.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5cvt%clr-namespace:CATSInterface.Converter     -     � Title$  CATSInterface��$	��550��$
��1102��.���  � 	Resources /   )CATSInterface.Converter.BoolDataConverter   cvtBool��%   &	           � RowDefinitions �� $
��Auto ��� �� $
��Auto �.+�o� $��0��.@�m� $��
CATS配置��.@�m� -   $��路径配置��m� $��扫单��.@�m� -    m�Name$ miStartScan��$��开始扫单��{��� *	IsStarted+ �� 	Converter# [  m� -   $ 
miStopScan��$��停止扫单��{��� *	IsStarted+�� $��1��.@��� $��订单��.��&   System.Windows.Controls.DataGrid  -     Name$ dgOrder��   CanUserAddRows .     
IsReadOnly .    AutoGenerateColumns . 	   ClipboardCopyMode$	 IncludeHeader=�
   Columns
 �� $��下单指令��.��  -   $ dgInstruction�� .   . $	 IncludeHeader=��� -    ��Name$ tbException��$��已标记废单��.��  -   $ dgSignedErrorOrder�� .   .  .  $	 IncludeHeader=�
 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ ClientID��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
ClientID+  $ 委托编号�� �� *	OrderNo+  $ 委托状态�� �� *OrderStatus+  $ 证券代码�� �� *	StockCode+  $ 交易方向�� �� *	TradeSide+  $ 委托数量�� �� *
OrderQty+  $ 委托价格�� �� *
OrderPrice+  $ 成交数量�� �� *	FilledQty+  $ 成交均价�� �� *FilledPrice+  $ 撤单数量�� �� *	CancelQty+  $ 股票市场�� �� *Market+  $ 委托时间�� �� *	OrderTime+ �� StringFormat$ yyyy-MM-dd HH:mm:ss��  $ 错误信息��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 70  �� *ErrMsg+4  .System.Windows.Controls.DataGridTemplateColumn  $ 操作��$	 150    CellTemplate �� .R���  �� Orientation$ 
Horizontal=�$��Center=�.+��� -	   $��80�� � DataContext �� .��撤除废单标记� $��2��$��60��.+��� -
    ��Name$ rtbMainInfo�� �� 
IsReadOnly . 