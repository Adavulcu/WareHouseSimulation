   M S B A M L   `   `   `  ����P  LNewEra.DepoSimulasyon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:NewEra.DepoSimulasyonNewEra.DepoSimulasyon  5   	   '    !NewEra.DepoSimulasyon.WindowHucre   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:NewEra.DepoSimulasyon     5   	     � WindowStyle$  
ToolWindow=�5   	    � Title$ WindowHucre��5	   	   $	��450��6   $	��800��6*   $	��450��66   $	��800��6F   $	��450��6U   $	��800��6e   .��5
      � -   5         �Name$ MainGrid��5
       � RowDefinitions 5   
   �� 5      $��1* �6   6*   �� 5      $��1* �6   6*   �� 5      $��7* �6   6*    � ColumnDefinitions 5   
   �� 5      $��1* �6   6,   �� 5      $��1* �6   6,   �� 5      $��1* �6   6,   .+�5   
   �� -   5         ��Name$ LblAdres��5      $��Stretch=�6"   $��Stretch=�6@   $��Center=�6\   $��Center=�6~   
�������$��25(�5   #   $
��Bold"�61   $��1-01-G-07-7-1��6C   $��0��6[   $��3��6k   $��0��6   6�   �� 5   
   -   5        $ 
LblTipIsim��5      $��Stretch=�6$   $��Stretch=�6B   $��Center=�6^   $��Center=�6�   
�������$��25(�5   '   $
��Bold"�65   $��BESLEME��6G   $��0��6Y   $��1��6j   6y   �� 5   
   -   5        $ 	LblMiktar��5      $��Stretch=�6#   $��Stretch=�6A   $��Center=�6]   $��Center=�6   
�������$��25(�5   (   $
��Bold"�66   $��5 / 30��6H   $��1��6Y   $��1��6j   6y   �� 5   
   -   5        $ LblOran��5      $��Stretch=�6!   $��Stretch=�6?   $��Center=�6[   $��Center=�6}   
�������$��25(�5   (   $
��Bold"�66   $	��%18��6H   $��2��6V   $��1��6g   6v   �� 5   
   $��0��6   $��3��6'   $��2��6;   $��Stretch=�6H   $
��Left=�6d   
�������$
��Auto=�6�   $
��Auto=�6�   .��5       � -   5        $ GridHucreBilgi��5       $
��auto��6,   $
��auto��69   $��Stretch=�6G   5"      5#      5%      5&      