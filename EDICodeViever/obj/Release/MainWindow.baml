   M S B A M L   `   `   `  ����   EDICodeViever    EDICodeViever.MainWindow   V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-Namespace:EDICodeViever     -      MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � Title$ EDI Code Viever (v,2,0)��$	��450��$��872.386�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=�.���  � RowDefinitions �� $��38 ��� $��38 ���  � ColumnDefinitions �� �� $	��206 �.+��� -    ��Name$ tbxIn��$��	10,8,12,7q�$��1���� -   $ tbxOut��$��12,11,10,10q�$��2����. $��Visible=�$��Visible=��� -    ��Name$ label1��$��	Separator��$��1��$��2��$��28��$
��Left=�$��	10,32,0,0q�$	��Top=�$��64���� -   $ tbxSeparator��$��1��$��2��$��23��$
��Left=�$��	10,55,0,0q�$	��Top=�$��75��$��'��	 �� TextAlignment$	 Center=��� -   $ label2��$��After separator��$��28��$
��Left=�$��
101,32,0,0q�$	��Top=�$��1��$��2��$��94���� -   $ 	tbxAction��$
��Left=�$��
107,55,0,0q�$��75��$��1��$��2��$��\n��$	 Center=�$��23��$	��Top=��� -   $ label4��$��	Początek��$
��Left=�$��10,186,0,130q�$��1��$��2��$��59���� -	   $ tbxStart��$��23��$
��Left=�$��
10,209,0,0q�$	��UNH��$	 Center=�$	��Top=�$��75��$��1��$��2���� -
   $ tbxEnd��$��2��$��23��$
��Left=�$��122,209,0,0q�$	��UNT��$	 Center=�$	��Top=�$��73��$��1���� -   $ label5��$��Koniec��$��2��$��28��$
��Left=�$��118,185,0,0q�$	��Top=�$��1��$��46���� -   
 ��Name$
 
btnRatowac��$��Ratować��$��1��$��23��$
��Left=�$��10,8,0,0q�$	��Top=�$	��185��$��1���� -    ��Name$ chkbxCutEmpty��$��Wytnij puste pola��$��1��$��2��$��16��$
��Left=�$��
10,128,0,0q�$	��Top=�$	��122���� -   $ chkbxSepare��$��Separuj��$��16��$
��Left=�$��	10,11,0,0q�$	��Top=�$��1��$��2��$��65���� -   $ chkbxCutCenter��$��Wytnij środek dekumentu��$��16��$
��Left=�$��
10,164,0,0q�$	��Top=�$��1��$��2��$	��170���� -   $ chkbxCutCharacters��$��Wytnij niedzowolone znaki��$��16��$
��Left=�$��
10,106,0,0q�$	��Top=�$��1��$��2��$	��172���� -   $
 btnChangeFiles��$��Przerób pliki��$��23��$
��Left=�$��
10,241,0,0q�$	��Top=�$	��185��$��1��$��2���� $��EDI Code Viever (v.2.0)��$
��Left=�$��	12,10,0,0q�$	��Top=����	#00000000���	#DDAAAAAA�� -   $
 btnClose��$��X��$��1��$
��Left=�$��	146,5,0,0q�$	��Top=�$��50�� " MaterialDesignFlatAccentButton#��[ �� -   $
 btnMaximalise��$��^��$��1��$
��Left=�$��82,5,0,0q�$	��Top=�$��50��  MaterialDesignFlatButton#��[ �� -   $
 btnMinimalise��$��_��$��1��$
��Left=�$��19,5,0,0q�$	��Top=�$��50��#��[ � -    �Name$ pbarWorkingWithFiles��$��1��$
��Left=�$��23��$��
10,241,0,0q�$��2��$	��Top=�$	��185��