﻿#pragma checksum "..\..\..\..\Modules\Staff\WorkDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "632A676CCF8F57033BDA1D5A811DC375"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace SuperMarketer {
    
    
    /// <summary>
    /// WorkDialog
    /// </summary>
    public partial class WorkDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbStaffID;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbStoreID;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtpHireDate;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtpHireDateTo;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbAwards;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbPunishment;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmit;
        
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
            System.Uri resourceLocater = new System.Uri("/SuperMarketer;component/modules/staff/workdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
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
            this.txbStaffID = ((System.Windows.Controls.TextBox)(target));
            
            #line 29 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
            this.txbStaffID.LostFocus += new System.Windows.RoutedEventHandler(this.txbStaffID_LostFocus);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txbStoreID = ((System.Windows.Controls.TextBox)(target));
            
            #line 30 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
            this.txbStoreID.LostFocus += new System.Windows.RoutedEventHandler(this.txbStoreID_LostFocus);
            
            #line default
            #line hidden
            return;
            case 3:
            this.dtpHireDate = ((System.Windows.Controls.DatePicker)(target));
            
            #line 31 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
            this.dtpHireDate.LostFocus += new System.Windows.RoutedEventHandler(this.dtpHireDate_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.dtpHireDateTo = ((System.Windows.Controls.DatePicker)(target));
            
            #line 32 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
            this.dtpHireDateTo.LostFocus += new System.Windows.RoutedEventHandler(this.dtpHireDateTo_LostFocus);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txbAwards = ((System.Windows.Controls.TextBox)(target));
            
            #line 33 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
            this.txbAwards.LostFocus += new System.Windows.RoutedEventHandler(this.txbAwards_LostFocus);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txbPunishment = ((System.Windows.Controls.TextBox)(target));
            
            #line 34 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
            this.txbPunishment.LostFocus += new System.Windows.RoutedEventHandler(this.txbPunishment_LostFocus);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnSubmit = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\..\Modules\Staff\WorkDialog.xaml"
            this.btnSubmit.Click += new System.Windows.RoutedEventHandler(this.btnSubmit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

