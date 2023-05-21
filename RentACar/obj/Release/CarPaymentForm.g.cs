﻿#pragma checksum "..\..\CarPaymentForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D5B52166640D114570390E975626C513B119F578595DF09C2FD0F804BD513CA8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using RentACar;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace RentACar {
    
    
    /// <summary>
    /// CarPaymentForm
    /// </summary>
    public partial class CarPaymentForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\CarPaymentForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtBlockOrderPrice;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\CarPaymentForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtBlockPaid;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\CarPaymentForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtBlockDebtAmount;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\CarPaymentForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPaymentDate;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\CarPaymentForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbPaymentType;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\CarPaymentForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbPaymentMethod;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\CarPaymentForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtDebtAmount;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/RentACar;component/carpaymentform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CarPaymentForm.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 14 "..\..\CarPaymentForm.xaml"
            ((RentACar.CarPaymentForm)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 15 "..\..\CarPaymentForm.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_mouseDown);
            
            #line default
            #line hidden
            
            #line 15 "..\..\CarPaymentForm.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_mouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 26 "..\..\CarPaymentForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_close);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtBlockOrderPrice = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.txtBlockPaid = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.txtBlockDebtAmount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.txtPaymentDate = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.cbPaymentType = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.cbPaymentMethod = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 10:
            this.txtDebtAmount = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            
            #line 77 "..\..\CarPaymentForm.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_CarPay_click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

