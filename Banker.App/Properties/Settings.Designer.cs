﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Banker.App.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string GoogleSpreadsheetDocumentWriterRefreshToken {
            get {
                return ((string)(this["GoogleSpreadsheetDocumentWriterRefreshToken"]));
            }
            set {
                this["GoogleSpreadsheetDocumentWriterRefreshToken"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1jvBtXd06T28m1RrADbM1QShJ3_sdFe8um3-Zn7xQrhM")]
        public string GoogleSpreadsheetDocumentWriterSpreadsheetId {
            get {
                return ((string)(this["GoogleSpreadsheetDocumentWriterSpreadsheetId"]));
            }
            set {
                this["GoogleSpreadsheetDocumentWriterSpreadsheetId"] = value;
            }
        }
    }
}
