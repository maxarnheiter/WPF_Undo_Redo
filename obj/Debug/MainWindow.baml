   M S B A M L   `   `   `  ����  WPF_Undo_Redo    WPF_Undo_Redo.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Undo/Redo Text Editor��5   	   $	��390��6'   $	��750��64   .��5      � .+�5   
   �� -   5         ��Name$ MainTextBox��5      $
��Left=�6'   $	��320��6B   $��	20,20,0,0q�6O    �� TextWrapping$
 Wrap=�6b   $	��Top=�6v   $	��220��6�   $��<Enter Text Here>��6�   6�   �� 5   
   -   5         ��Name$ 
UndoButton��5      $
��Undo��6%   $
��Left=�64   $��
260,80,0,0q�6O   $	��Top=�6c   $��80��6{   $��60��6�    J� RenderTransformOrigin$ 0.52,-0.412&�6�   $��20(�6�   $
��Bold"�6�   {�.  6�   �� 5	   
   -   5        $ 
RedoButton��5	      $
��Redo��6%   $
��Left=�64   $��260,200,0,0q�6O   $	��Top=�6d   $��80��6|   $��60��6�   $ 0.52,-0.412&�6�   $��20(�6�   $
��Bold"�6�   {�.  6�   �� 5
   
   -   5         ��Name$ PastEventsTextBlock��5
      $
��Left=�61   $��
385,20,0,0q�6L   $
��Wrap=�6`   $��Past Events: 0��6t   $	��Top=�6�   $��20��6�   $	��110��6�   �� 5   
   -   5        $ FutureEventsTextBlock��5      $
��Left=�63   $��
585,20,0,0q�6N   $
��Wrap=�6b   $��Future Events: 0��6v   $	��Top=�6�   $��20��6�   $	��105��6�   �� 5   
   $
��Left=�6   $	��280��62   $��
386,60,0,0q�6?   $	��Top=�6S   $	��160��6k   $��0q�6w   $��Visible=�6�   $
��Auto=�6�   ��. .��5      �� -   5         ��Name$ PastEventsStackPanel��5      $
��Left=�67   $	��280��6R   $	��Top=�6_   $	��140��6w   $��0,0,2,0q�6�   5      �� 5   
   $
��Left=�6   $	��280��62   $��
572,60,0,0q�6?   $	��Top=�6S   $	��160��6k   $��Visible=�6w   $
��Auto=�6�   ��. .��5      �� -   5        $ FutureEventsStackPanel��5      $
��Left=�69   $	��280��6T   $	��Top=�6a   $	��140��6y   5      5      5      