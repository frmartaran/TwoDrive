﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TwoDrive.Importer {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ImporterResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ImporterResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TwoDrive.Importer.ImporterResource", typeof(ImporterResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File Not Found.
        /// </summary>
        internal static string FileNotFound_Exception {
            get {
                return ResourceManager.GetString("FileNotFound_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong date format..
        /// </summary>
        internal static string Json_DateFormat_Exception {
            get {
                return ResourceManager.GetString("Json_DateFormat_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing or Wrong type especification on json. Supported types: &quot;File&quot; or &quot;Folder&quot;.
        /// </summary>
        internal static string Json_Type_Exception {
            get {
                return ResourceManager.GetString("Json_Type_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing Content Tag.
        /// </summary>
        internal static string NoContent_Exception {
            get {
                return ResourceManager.GetString("NoContent_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Each folder tag must have the name attribute.
        /// </summary>
        internal static string NoName_Exception {
            get {
                return ResourceManager.GetString("NoName_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This importer only imports trees that start from the root folder. Use another importer or add a new one..
        /// </summary>
        internal static string NoRoot_Exception {
            get {
                return ResourceManager.GetString("NoRoot_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing Type Attribute. .
        /// </summary>
        internal static string NoType_Exception {
            get {
                return ResourceManager.GetString("NoType_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to s.
        /// </summary>
        internal static string String1 {
            get {
                return ResourceManager.GetString("String1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported file type. Must be &apos;txt&apos; or &apos;html&apos;.
        /// </summary>
        internal static string Unsupported {
            get {
                return ResourceManager.GetString("Unsupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid date format. Please try: yyyy-mm-dd.
        /// </summary>
        internal static string WrongFormat_Exception {
            get {
                return ResourceManager.GetString("WrongFormat_Exception", resourceCulture);
            }
        }
    }
}
