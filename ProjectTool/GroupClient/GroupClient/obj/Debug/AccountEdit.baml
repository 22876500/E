   M S B A M L   `   `   `  ����  GroupClient    GroupClient.AccountEdit   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  密码管理��5   	   $	��220��6   $	��300��6#    � Icon$ Images\RighteousFury.ico��6/   .��5      �  � 	Resources 5   
    �� 
TargetType �� Property �� Value��%�   '��       �� 5      " ��6   .�5      ��  � ��$ Right=�6?   6O   �� 5	       � ��$ Center=�6=   6N   5
      5       � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��1��6   .��6   管理员：6#   �� 5   
   $��2��6   .��6   	密码：6"   �� 5   
   $��3��6   .��6   确认密码：6$   �� 5   
   -   5         ��Name$ 
txtAccount��5   p   $��1��6   $��1��6   $��Center=�6/   $
��Left=�6J   $��76��6e   6�   G� 5   
   -   5        	 G�Name$	 txtPassword��5   t   $��2��6   $��1��6#   $��Center=�63   $
��Left=�6N   $��76��6i   6�   G� 5    
   -   5        $	 txtPasswordAgain��5    t   $��3��6   $��1��6#   $��Center=�63   $
��Left=�6N   $��76��6i   6�   �� 5"   
   -   5        $��4��5"      $��72��6   $��26��6)   $��2��65   $��2��6I   .��6r   确定6v   5#      5$      