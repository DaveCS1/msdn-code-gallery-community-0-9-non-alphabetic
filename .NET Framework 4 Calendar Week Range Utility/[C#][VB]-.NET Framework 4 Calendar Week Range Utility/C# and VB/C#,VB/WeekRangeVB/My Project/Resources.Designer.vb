﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.1
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("WeekRange.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &amp;Calculate.
        '''</summary>
        Friend ReadOnly Property ButtonText() As String
            Get
                Return ResourceManager.GetString("ButtonText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Ca&amp;lendar:.
        '''</summary>
        Friend ReadOnly Property CalendarLabel() As String
            Get
                Return ResourceManager.GetString("CalendarLabel", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Eras:.
        '''</summary>
        Friend ReadOnly Property EraLabel() As String
            Get
                Return ResourceManager.GetString("EraLabel", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to FirstDay.
        '''</summary>
        Friend ReadOnly Property FirstDay() As String
            Get
                Return ResourceManager.GetString("FirstDay", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &amp;First Day of Week:.
        '''</summary>
        Friend ReadOnly Property FirstDayOfWeekLabel() As String
            Get
                Return ResourceManager.GetString("FirstDayOfWeekLabel", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to FirstFourDayWeek.
        '''</summary>
        Friend ReadOnly Property FirstFourDayWeek() As String
            Get
                Return ResourceManager.GetString("FirstFourDayWeek", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to FirstFullWeek.
        '''</summary>
        Friend ReadOnly Property FirstFullWeek() As String
            Get
                Return ResourceManager.GetString("FirstFullWeek", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Heisei|Showa|Taisho|Meiji.
        '''</summary>
        Friend ReadOnly Property JapaneseEras() As String
            Get
                Return ResourceManager.GetString("JapaneseEras", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Adjusted date range to {0}/{1} to {2}/{3}.
        '''</summary>
        Friend ReadOnly Property MSG_AdjustedRange() As String
            Get
                Return ResourceManager.GetString("MSG_AdjustedRange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Exception:.
        '''</summary>
        Friend ReadOnly Property MSG_Exception() As String
            Get
                Return ResourceManager.GetString("MSG_Exception", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The year field cannot be empty..
        '''</summary>
        Friend ReadOnly Property MSG_YearEmpty() As String
            Get
                Return ResourceManager.GetString("MSG_YearEmpty", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The format of the year is invalid..
        '''</summary>
        Friend ReadOnly Property MSG_YearFormat() As String
            Get
                Return ResourceManager.GetString("MSG_YearFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to The year is out of range..
        '''</summary>
        Friend ReadOnly Property MSG_YearOutOfRange() As String
            Get
                Return ResourceManager.GetString("MSG_YearOutOfRange", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Week Ranges:.
        '''</summary>
        Friend ReadOnly Property ResultText() As String
            Get
                Return ResourceManager.GetString("ResultText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &amp;Rule.
        '''</summary>
        Friend ReadOnly Property RuleLabel() As String
            Get
                Return ResourceManager.GetString("RuleLabel", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Week Ranges:.
        '''</summary>
        Friend ReadOnly Property WeekRangeLabel() As String
            Get
                Return ResourceManager.GetString("WeekRangeLabel", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Calendar Week Ranges.
        '''</summary>
        Friend ReadOnly Property WindowTitle() As String
            Get
                Return ResourceManager.GetString("WindowTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &amp;Year:.
        '''</summary>
        Friend ReadOnly Property YearLabel() As String
            Get
                Return ResourceManager.GetString("YearLabel", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
